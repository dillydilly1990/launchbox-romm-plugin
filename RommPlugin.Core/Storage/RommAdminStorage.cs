using System;
using System.IO;
using Newtonsoft.Json;
using RommPlugin.Core.Models;

namespace RommPlugin.Core.Storage
{
    public static class RommAdminStorage
    {
        private static readonly string Folder =
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Plugins",
                "RomM LaunchBox Integration"
            );

        private static readonly string FilePath = Path.Combine(Folder, "admin.json");

        public static RommAdminAccount Load()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return null;
                }

                var json = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<RommAdminAccount>(json);
            }
            catch
            {
                return null;
            }
        }

        public static void Save(RommAdminAccount account)
        {
            Directory.CreateDirectory(Folder);
            var json = JsonConvert.SerializeObject(account, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        public static void Delete()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}
