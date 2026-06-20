using System;
using RommPlugin.Core.Logging;
using RommPlugin.Core.Storage;
using RommPlugin.Services;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace RommPlugin
{
    public class RommMenuPlugin : ISystemEventsPlugin
    {
        private RommProcessInstallUninstallService sync = new RommProcessInstallUninstallService();

        public async void OnEventRaised(string eventType)
        {
            if (eventType != SystemEventTypes.LaunchBoxStartupCompleted)
            {
                return;
            }

            var settings = RommPluginStorage.Load();

            RommLogger.Initialize(settings.SaveLogs);

            if (!settings.ProcessPendingOnStartup)
            {
                return;
            }

            try
            {
                await sync.ProcessInstallUninstallEvents(false);
            }
            catch (Exception ex)
            {
                RommLogger.LogError("[RommPlugin] Sync error: " + ex);
            }
        }
    }
}
