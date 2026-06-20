using System;
using System.Windows.Forms;
using RommPlugin.Core.Models;
using RommPlugin.Core.Storage;

namespace RommPlugin.UI.Forms
{
    public partial class RommAdversityLoginForm : Form
    {
        private readonly RommPluginSettings _settings;

        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool UseConfiguredAccount { get { return useConfiguredAccount.Checked; } }
        public bool SaveAdminAccount { get { return saveAdminAccount.Checked; } }

        public RommAdversityLoginForm(RommPluginSettings settings)
        {
            InitializeComponent();
            _settings = settings;

            if (_settings.LoginFormSaveAdminAccount)
            {
                var admin = RommAdminStorage.Load();
                if (admin != null)
                {
                    saveAdminAccount.Checked = true;
                    txtUsername.Text = admin.Username;
                    txtPassword.Text = admin.Password;
                }
            }
            else if (_settings.LoginFormUseConfiguredAccount &&
                     !string.IsNullOrWhiteSpace(_settings.Username))
            {
                useConfiguredAccount.Checked = true;
            }
        }

        private void UseConfiguredAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (useConfiguredAccount.Checked)
            {
                txtUsername.Text = _settings.Username;
                txtPassword.Text = _settings.Password;
                txtUsername.Enabled = false;
                txtPassword.Enabled = false;

                saveAdminAccount.CheckedChanged -= SaveAdminAccount_CheckedChanged;
                saveAdminAccount.Checked = false;
                saveAdminAccount.Enabled = false;
                saveAdminAccount.CheckedChanged += SaveAdminAccount_CheckedChanged;
            }
            else
            {
                txtUsername.Enabled = true;
                txtPassword.Enabled = true;
                txtUsername.Clear();
                txtPassword.Clear();

                saveAdminAccount.Enabled = true;
            }
        }

        private void SaveAdminAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (saveAdminAccount.Checked)
            {
                useConfiguredAccount.CheckedChanged -= UseConfiguredAccount_CheckedChanged;
                useConfiguredAccount.Checked = false;
                useConfiguredAccount.Enabled = false;
                useConfiguredAccount.CheckedChanged += UseConfiguredAccount_CheckedChanged;
            }
            else
            {
                useConfiguredAccount.Enabled = true;
            }
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            if (!useConfiguredAccount.Checked)
            {
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
            }

            _settings.LoginFormUseConfiguredAccount = useConfiguredAccount.Checked;
            _settings.LoginFormSaveAdminAccount = saveAdminAccount.Checked;
            RommPluginStorage.Save(_settings);

            Username = txtUsername.Text;
            Password = txtPassword.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
