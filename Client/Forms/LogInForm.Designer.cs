namespace Client.Forms
{
    partial class LogInForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LogInPanel = new System.Windows.Forms.Panel();
            this.LogInLabel = new System.Windows.Forms.Label();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.UsernameField = new System.Windows.Forms.TextBox();
            this.PasswordField = new System.Windows.Forms.TextBox();
            this.LogInButton = new System.Windows.Forms.Button();
            this.SignUpInstead = new System.Windows.Forms.LinkLabel();
            this.LogInPicture = new System.Windows.Forms.PictureBox();
            this.LogInPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogInPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // LogInPanel
            // 
            this.LogInPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(202)))), ((int)(((byte)(230)))));
            this.LogInPanel.Controls.Add(this.LogInPicture);
            this.LogInPanel.Controls.Add(this.SignUpInstead);
            this.LogInPanel.Controls.Add(this.LogInButton);
            this.LogInPanel.Controls.Add(this.PasswordField);
            this.LogInPanel.Controls.Add(this.UsernameField);
            this.LogInPanel.Controls.Add(this.PasswordLabel);
            this.LogInPanel.Controls.Add(this.UsernameLabel);
            this.LogInPanel.Controls.Add(this.LogInLabel);
            this.LogInPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.LogInPanel.Location = new System.Drawing.Point(484, 0);
            this.LogInPanel.Name = "LogInPanel";
            this.LogInPanel.Size = new System.Drawing.Size(400, 561);
            this.LogInPanel.TabIndex = 0;
            // 
            // LogInLabel
            // 
            this.LogInLabel.AutoSize = true;
            this.LogInLabel.Font = new System.Drawing.Font("Segoe UI Emoji", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogInLabel.Location = new System.Drawing.Point(133, 190);
            this.LogInLabel.Name = "LogInLabel";
            this.LogInLabel.Size = new System.Drawing.Size(134, 53);
            this.LogInLabel.TabIndex = 0;
            this.LogInLabel.Text = "Log In";
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Font = new System.Drawing.Font("Segoe UI Emoji", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameLabel.Location = new System.Drawing.Point(62, 270);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(110, 28);
            this.UsernameLabel.TabIndex = 1;
            this.UsernameLabel.Text = "Username:";
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Font = new System.Drawing.Font("Segoe UI Emoji", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordLabel.Location = new System.Drawing.Point(68, 310);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(104, 28);
            this.PasswordLabel.TabIndex = 2;
            this.PasswordLabel.Text = "Password:";
            // 
            // UsernameField
            // 
            this.UsernameField.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameField.Location = new System.Drawing.Point(180, 270);
            this.UsernameField.Name = "UsernameField";
            this.UsernameField.Size = new System.Drawing.Size(158, 29);
            this.UsernameField.TabIndex = 3;
            this.UsernameField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UsernameField_KeyDown);
            // 
            // PasswordField
            // 
            this.PasswordField.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordField.Location = new System.Drawing.Point(180, 310);
            this.PasswordField.Name = "PasswordField";
            this.PasswordField.PasswordChar = '*';
            this.PasswordField.Size = new System.Drawing.Size(158, 29);
            this.PasswordField.TabIndex = 4;
            this.PasswordField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordField_KeyDown);
            // 
            // LogInButton
            // 
            this.LogInButton.AutoSize = true;
            this.LogInButton.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogInButton.Location = new System.Drawing.Point(150, 355);
            this.LogInButton.Name = "LogInButton";
            this.LogInButton.Size = new System.Drawing.Size(100, 31);
            this.LogInButton.TabIndex = 5;
            this.LogInButton.Text = "Log In";
            this.LogInButton.UseVisualStyleBackColor = true;
            this.LogInButton.Click += new System.EventHandler(this.LogInButton_Click);
            // 
            // SignUpInstead
            // 
            this.SignUpInstead.AutoSize = true;
            this.SignUpInstead.Font = new System.Drawing.Font("Segoe UI Emoji", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SignUpInstead.Location = new System.Drawing.Point(128, 400);
            this.SignUpInstead.Name = "SignUpInstead";
            this.SignUpInstead.Size = new System.Drawing.Size(143, 17);
            this.SignUpInstead.TabIndex = 6;
            this.SignUpInstead.TabStop = true;
            this.SignUpInstead.Text = "Don\'t have an account?";
            this.SignUpInstead.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SignUpInstead_LinkClicked);
            // 
            // LogInPicture
            // 
            this.LogInPicture.Image = global::Client.Properties.Resources.login;
            this.LogInPicture.Location = new System.Drawing.Point(155, 85);
            this.LogInPicture.Name = "LogInPicture";
            this.LogInPicture.Size = new System.Drawing.Size(90, 90);
            this.LogInPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LogInPicture.TabIndex = 7;
            this.LogInPicture.TabStop = false;
            // 
            // LogInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(158)))), ((int)(((byte)(188)))));
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.LogInPanel);
            this.Name = "LogInForm";
            this.Text = "LogInForm";
            this.LogInPanel.ResumeLayout(false);
            this.LogInPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogInPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel LogInPanel;
        private System.Windows.Forms.TextBox UsernameField;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Label LogInLabel;
        private System.Windows.Forms.TextBox PasswordField;
        private System.Windows.Forms.Button LogInButton;
        private System.Windows.Forms.LinkLabel SignUpInstead;
        private System.Windows.Forms.PictureBox LogInPicture;
    }
}