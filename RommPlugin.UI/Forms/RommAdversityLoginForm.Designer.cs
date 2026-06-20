namespace RommPlugin.UI.Forms
{
    partial class RommAdversityLoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.btnProceed = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.useConfiguredAccount = new System.Windows.Forms.CheckBox();
            this.saveAdminAccount = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnProceed
            // 
            this.btnProceed.Location = new System.Drawing.Point(234, 350);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(75, 23);
            this.btnProceed.TabIndex = 3;
            this.btnProceed.Text = "Ok";
            this.btnProceed.UseVisualStyleBackColor = true;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(21, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 29);
            this.label1.TabIndex = 4;
            this.label1.Text = "RomM Sensible Access";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(22, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Username:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(26, 109);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(246, 20);
            this.txtUsername.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(23, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(27, 179);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(246, 20);
            this.txtPassword.TabIndex = 8;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(22, 220);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(248, 72);
            this.label6.TabIndex = 13;
            this.label6.Text = "You need an editor account to perform this action. This information will be save" +
    "d in hard code. Consider carefully whether you want to save your admin credenti" +
    "als.";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Location = new System.Drawing.Point(153, 350);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // useConfiguredAccount
            // 
            this.useConfiguredAccount.AutoSize = true;
            this.useConfiguredAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.useConfiguredAccount.ForeColor = System.Drawing.Color.White;
            this.useConfiguredAccount.Location = new System.Drawing.Point(26, 295);
            this.useConfiguredAccount.Margin = new System.Windows.Forms.Padding(2);
            this.useConfiguredAccount.Name = "useConfiguredAccount";
            this.useConfiguredAccount.Size = new System.Drawing.Size(247, 19);
            this.useConfiguredAccount.TabIndex = 15;
            this.useConfiguredAccount.Text = "Use account configured in settings";
            this.useConfiguredAccount.UseVisualStyleBackColor = true;
            this.useConfiguredAccount.CheckedChanged += new System.EventHandler(this.UseConfiguredAccount_CheckedChanged);
            // 
            // saveAdminAccount
            // 
            this.saveAdminAccount.AutoSize = true;
            this.saveAdminAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveAdminAccount.ForeColor = System.Drawing.Color.White;
            this.saveAdminAccount.Location = new System.Drawing.Point(26, 320);
            this.saveAdminAccount.Margin = new System.Windows.Forms.Padding(2);
            this.saveAdminAccount.Name = "saveAdminAccount";
            this.saveAdminAccount.Size = new System.Drawing.Size(218, 19);
            this.saveAdminAccount.TabIndex = 16;
            this.saveAdminAccount.Text = "Save admin account for next time";
            this.saveAdminAccount.UseVisualStyleBackColor = true;
            this.saveAdminAccount.CheckedChanged += new System.EventHandler(this.SaveAdminAccount_CheckedChanged);
            // 
            // RommAdversityLoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(332, 395);
            this.Controls.Add(this.saveAdminAccount);
            this.Controls.Add(this.useConfiguredAccount);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnProceed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtUsername);
            this.Name = "RommAdversityLoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RommAdversityLoginForm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button btnProceed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox useConfiguredAccount;
        private System.Windows.Forms.CheckBox saveAdminAccount;
    }
}
