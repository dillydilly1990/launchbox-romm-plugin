using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using RommPlugin.Core.Logging;
using RommPlugin.Core.Models;
using RommPlugin.Core.Storage;
using RommPlugin.UI.Helpers;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace RommPlugin.Services
{
    public class RommProcessInstallUninstallService
    {
        public async Task ProcessInstallUninstallEvents(bool showEmptyMessage = true)
        {
            await ProgressRunner.RunAsync(
                "Processing installations events",
                async progress =>
                {
                    var flagPath = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "..",
                        "Plugins",
                        "RomM LaunchBox Integration",
                        "romm.sync"
                    );

                    if (!File.Exists(flagPath))
                    {
                        if (showEmptyMessage)
                        {
                            MessageBox.Show("RomM does not have any pending install");
                        }
                        return;
                    }

                    const string mutexName = "Global\\RommPluginSync";

                    RommSyncFile file;

                    using (var mutex = new Mutex(false, mutexName))
                    {
                        try
                        {
                            mutex.WaitOne();
                        }
                        catch (AbandonedMutexException)
                        {
                        }

                        try
                        {
                            file = JsonConvert.DeserializeObject<RommSyncFile>(File.ReadAllText(flagPath));
                        }
                        finally
                        {
                            mutex.ReleaseMutex();
                        }
                    }

                    if (file?.Events == null || file.Events.Count == 0)
                    {
                        if (showEmptyMessage)
                        {
                            MessageBox.Show("RomM does not have any pending install");
                        }
                        return;
                    }

                    var settings = RommPluginStorage.Load();
                    var dataManager = PluginHelper.DataManager;

                    var rommGamesOnly = dataManager.GetAllGames()
                        .Where(g => g.Platform != null && g.Platform.StartsWith("RomM | "))
                        .ToList();

                    var gamesById = new Dictionary<int, IGame>();

                    foreach (var game in rommGamesOnly)
                    {
                        if (TryGetRommId(game, out var id))
                        {
                            gamesById[id] = game;
                        }
                    }

                    var totalEvents = file.Events.Count;
                    var completedEvents = 0;
                    var processedIds = new HashSet<int>();

                    foreach (var evt in file.Events.ToList())
                    {
                        progress.SetStatus($"Processing: {completedEvents} of {totalEvents}");

                        try
                        {
                            if (!gamesById.TryGetValue(evt.RommGameId, out var game))
                            {
                                continue;
                            }

                            if (evt.Action == "install")
                            {
                                var fields = game.GetAllCustomFields().ToDictionary(f => f.Name, f => f.Value);

                                fields.TryGetValue(GameCustomFields.RemotePath, out var remotePath);
                                fields.TryGetValue(GameCustomFields.FileName, out var fileName);
                                fields.TryGetValue(GameCustomFields.IsFolderGame, out var folderValue);

                                var isFolderGame = folderValue == bool.TrueString;

                                var localFile = Path.Combine(
                                    settings.RomsPath,
                                    "romm",
                                    remotePath.Replace("/", "\\"),
                                    fileName
                                );

                                var zipPath = localFile;
                                var extractDir = Path.Combine(
                                    Path.GetDirectoryName(zipPath),
                                    Path.GetFileNameWithoutExtension(zipPath)
                                );

                                if (isFolderGame)
                                {
                                    UnzipAndDelete(zipPath, extractDir);

                                    var jsonPath = Path.Combine(extractDir, "_launchbox.json");

                                    if (File.Exists(jsonPath))
                                    {
                                        ConfigureLaunchBoxGame(game, extractDir, jsonPath);
                                    }

                                    localFile = extractDir;
                                }
                                else
                                {
                                    UnzipAndFlatten(zipPath);

                                    game.ApplicationPath = File.Exists(localFile) ? localFile : null;
                                }

                                game.Installed = isFolderGame ? Directory.Exists(localFile) : File.Exists(localFile);

                                processedIds.Add(evt.RommGameId);
                                completedEvents++;
                            }
                            else if (evt.Action == "uninstall")
                            {
                                ClearGameAdditionalApplications(game);
                                game.ApplicationPath = null;
                                game.Installed = false;

                                processedIds.Add(evt.RommGameId);
                                completedEvents++;
                            }
                            else
                            {
                                RommLogger.LogError($"[RommPlugin] Unknown action '{evt.Action}' for game {game.Title} (ID: {evt.RommGameId}), skipping");
                                processedIds.Add(evt.RommGameId);
                                completedEvents++;
                            }
                        }
                        catch (Exception ex)
                        {
                            RommLogger.LogError($"[RommPlugin] Error processing event '{evt.Action}' for game ID {evt.RommGameId}: {ex.Message}");
                            RommLogger.LogException(ex);
                        }
                    }

                    using (var mutex = new Mutex(false, mutexName))
                    {
                        try
                        {
                            mutex.WaitOne();
                        }
                        catch (AbandonedMutexException)
                        {
                        }

                        try
                        {
                            dataManager.Save();

                            var diskFile = JsonConvert.DeserializeObject<RommSyncFile>(File.ReadAllText(flagPath));

                            if (diskFile?.Events != null)
                            {
                                diskFile.Events.RemoveAll(e => processedIds.Contains(e.RommGameId));

                                if (diskFile.Events.Count == 0)
                                {
                                    File.Delete(flagPath);
                                }
                                else
                                {
                                    File.WriteAllText(
                                        flagPath,
                                        JsonConvert.SerializeObject(diskFile, Formatting.Indented)
                                    );
                                }
                            }
                        }
                        finally
                        {
                            mutex.ReleaseMutex();
                        }
                    }

                    RommLogger.Log("Pending installs processed successfully");

                    if (showEmptyMessage)
                    {
                        MessageBox.Show("RomM finish all pending install");
                    }
                }
            );
        }

        private bool TryGetRommId(IGame game, out int rommId)
        {
            rommId = 0;

            var value = game.GetAllCustomFields().FirstOrDefault(f => f.Name == GameCustomFields.GameId)?.Value;

            return int.TryParse(value, out rommId);
        }

        private void ConfigureLaunchBoxGame(IGame game, string baseFolder, string jsonPath)
        {
            var config = JsonConvert.DeserializeObject<LaunchBoxFolderGameConfig>(File.ReadAllText(jsonPath));

            if (config == null)
            {
                MessageBox.Show("RomM error while get game folder configuration");
                return;
            }

            ClearGameAdditionalApplications(game);

            if (!string.IsNullOrWhiteSpace(config.DefaultFileName))
            {
                game.ApplicationPath = Path.GetFullPath(Path.Combine(baseFolder, config.DefaultFileName));
            }

            if (config.AdditionalApplications != null)
            {
                foreach (var app in config.AdditionalApplications)
                {
                    var add = game.AddNewAdditionalApplication();
                    add.Name = app.Name;
                    add.ApplicationPath = ResolvePath(baseFolder, app.Path, false);
                    add.CommandLine = app.CommandLine;
                }
            }

            if (config.PreLoaders != null)
            {
                foreach (var loader in config.PreLoaders)
                {
                    var add = game.AddNewAdditionalApplication();
                    add.Name = loader.Name;
                    add.ApplicationPath = ResolvePath(baseFolder, loader.Path, loader.FromLaunchBoxRoot ?? false);
                    add.CommandLine = loader.CommandLine;
                    add.AutoRunBefore = true;
                    add.WaitForExit = loader.WaitForExit ?? false;
                }
            }

            if (config.PosLoaders != null)
            {
                foreach (var loader in config.PosLoaders)
                {
                    var add = game.AddNewAdditionalApplication();
                    add.Name = loader.Name;
                    add.ApplicationPath = ResolvePath(baseFolder, loader.Path, loader.FromLaunchBoxRoot ?? false);
                    add.CommandLine = loader.CommandLine;
                    add.AutoRunAfter = true;
                }
            }

            if (config.HasDLC == true)
            {
                var dlcFolder = Path.Combine(baseFolder, "_DLCs");

                if (Directory.Exists(dlcFolder))
                {
                    var files = Directory.GetFiles(dlcFolder);

                    int index = 1;
                    foreach (var file in files)
                    {
                        var add = game.AddNewAdditionalApplication();
                        add.Name = $"DLC {index}";
                        add.ApplicationPath = file;
                        index++;
                    }
                }
            }
        }

        private string ResolvePath(string baseFolder, string path, bool fromLaunchBoxRoot)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }

            if (fromLaunchBoxRoot)
            {
                return path;
            }

            return Path.GetFullPath(Path.Combine(baseFolder, path));
        }

        private void ClearGameAdditionalApplications(IGame game)
        {
            var applications = game.GetAllAdditionalApplications()
                .Where(a => !a.Name.Contains("(RomM)"))
                .ToList();

            foreach (var app in applications)
            {
                game.TryRemoveAdditionalApplication(app);
            }
        }

        private void UnzipAndDelete(string zipPath, string extractDir)
        {
            var rootFolder = Path.GetFileNameWithoutExtension(zipPath);

            using (var archive = ZipFile.OpenRead(zipPath))
            {
                foreach (var entry in archive.Entries)
                {
                    if (string.IsNullOrWhiteSpace(entry.Name))
                    {
                        continue;
                    }

                    var parts = entry.FullName
                        .Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries)
                        .SkipWhile(p => p != rootFolder)
                        .Skip(1)
                        .ToArray();

                    if (parts.Length == 0)
                    {
                        parts = entry.FullName
                            .Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries)
                            .ToArray();
                    }

                    var relativePath = Path.Combine(parts);

                    var destinationPath = Path.Combine(extractDir, relativePath);

                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

                    entry.ExtractToFile(destinationPath, true);
                }
            }

            File.Delete(zipPath);
        }

        private void UnzipAndFlatten(string zipPath)
        {
            if (File.Exists(zipPath) && Path.GetExtension(zipPath).Equals(".zip", StringComparison.OrdinalIgnoreCase))
            {
                var tempExtract = Path.Combine(
                    Path.GetDirectoryName(zipPath),
                    "__temp_extract"
                );

                if (Directory.Exists(tempExtract))
                {
                    Directory.Delete(tempExtract, true);
                }    

                Directory.CreateDirectory(tempExtract);

                ZipFile.ExtractToDirectory(zipPath, tempExtract);

                var innerFile = Directory.GetFiles(tempExtract, "*.zip", SearchOption.AllDirectories)
                    .FirstOrDefault();

                if (innerFile != null)
                {
                    File.Copy(innerFile, zipPath, true);
                }

                Directory.Delete(tempExtract, true);
            }
        }
    }
}