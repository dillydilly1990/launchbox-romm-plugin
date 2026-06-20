using System;
using System.IO;
using System.Windows.Forms;
using RommPlugin.Core.Logging;
using RommPlugin.Core.Models;
using RommPlugin.Core.Storage;

namespace RommPlugin.UI.Forms
{
    public partial class RommSettingsForm : Form
    {
        public RommSettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            RommPluginSettings settings;

            try
            {
                settings = RommPluginStorage.Load();
            }
            catch
            {
                settings = new RommPluginSettings();
            }

            txtBaseUrl.Text = settings.RommBaseUrl;
            txtUsername.Text = settings.Username;
            txtPassword.Text = settings.Password;
            txtRomsPath.Text = settings.RomsPath;
            keepLocalData.Checked = settings.KeepLocalData;
            saveLogs.Checked = settings.SaveLogs;
            processPendingOnStartup.Checked = settings.ProcessPendingOnStartup;
        }

        private void RommSettingsForm_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBaseUrl.Text))
            {
                MessageBox.Show("Base URL is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRomsPath.Text) ||
                !Directory.Exists(txtRomsPath.Text))
            {
                MessageBox.Show("Please select a valid ROMs path.");
                return;
            }

            var settings = RommPluginStorage.Load();
            settings.RommBaseUrl = txtBaseUrl.Text.Trim();
            settings.Username = txtUsername.Text.Trim();
            settings.Password = txtPassword.Text;
            settings.RomsPath = txtRomsPath.Text;
            settings.KeepLocalData = keepLocalData.Checked;
            settings.SaveLogs = saveLogs.Checked;
            settings.ProcessPendingOnStartup = processPendingOnStartup.Checked;

            RommPluginStorage.Save(settings);
            RommLogger.Initialize(settings.SaveLogs);

            MessageBox.Show(
                "Settings saved successfully.",
                "RomM Plugin",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            Close();
        }

        private void btnBrowseRomsPath_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select the folder where ROMs will be stored";

                if (!string.IsNullOrWhiteSpace(txtRomsPath.Text) &&
                    Directory.Exists(txtRomsPath.Text))
                {
                    dialog.SelectedPath = txtRomsPath.Text;
                }

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtRomsPath.Text = dialog.SelectedPath;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
