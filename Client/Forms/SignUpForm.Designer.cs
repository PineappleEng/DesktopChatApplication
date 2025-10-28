namespace Client.Forms
{
    partial class SignUpForm
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
            this.SignUpPanel = new System.Windows.Forms.Panel();
            this.LogInInstead = new System.Windows.Forms.LinkLabel();
            this.SignUpButton = new System.Windows.Forms.Button();
            this.PasswordField = new System.Windows.Forms.TextBox();
            this.UsernameField = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.SignUpLabel = new System.Windows.Forms.Label();
            this.SignUpPicture = new System.Windows.Forms.PictureBox();
            this.SignUpPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SignUpPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // SignUpPanel
            // 
            this.SignUpPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(202)))), ((int)(((byte)(230)))));
            this.SignUpPanel.Controls.Add(this.SignUpPicture);
            this.SignUpPanel.Controls.Add(this.LogInInstead);
            this.SignUpPanel.Controls.Add(this.SignUpButton);
            this.SignUpPanel.Controls.Add(this.PasswordField);
            this.SignUpPanel.Controls.Add(this.UsernameField);
            this.SignUpPanel.Controls.Add(this.PasswordLabel);
            this.SignUpPanel.Controls.Add(this.UsernameLabel);
            this.SignUpPanel.Controls.Add(this.SignUpLabel);
            this.SignUpPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.SignUpPanel.Location = new System.Drawing.Point(0, 0);
            this.SignUpPanel.Name = "SignUpPanel";
            this.SignUpPanel.Size = new System.Drawing.Size(400, 561);
            this.SignUpPanel.TabIndex = 0;
            // 
            // LogInInstead
            // 
            this.LogInInstead.AutoSize = true;
            this.LogInInstead.Font = new System.Drawing.Font("Segoe UI Emoji", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogInInstead.Location = new System.Drawing.Point(122, 400);
            this.LogInInstead.Name = "LogInInstead";
            this.LogInInstead.Size = new System.Drawing.Size(156, 17);
            this.LogInInstead.TabIndex = 13;
            this.LogInInstead.TabStop = true;
            this.LogInInstead.Text = "Already have an account?";
            this.LogInInstead.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LogInInstead_LinkClicked);
            // 
            // SignUpButton
            // 
            this.SignUpButton.AutoSize = true;
            this.SignUpButton.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SignUpButton.Location = new System.Drawing.Point(150, 355);
            this.SignUpButton.Name = "SignUpButton";
            this.SignUpButton.Size = new System.Drawing.Size(100, 31);
            this.SignUpButton.TabIndex = 12;
            this.SignUpButton.Text = "Sign Up";
            this.SignUpButton.UseVisualStyleBackColor = true;
            this.SignUpButton.Click += new System.EventHandler(this.SignUpButton_Click);
            // 
            // PasswordField
            // 
            this.PasswordField.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordField.Location = new System.Drawing.Point(180, 310);
            this.PasswordField.Name = "PasswordField";
            this.PasswordField.PasswordChar = '*';
            this.PasswordField.Size = new System.Drawing.Size(158, 29);
            this.PasswordField.TabIndex = 11;
            this.PasswordField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordField_KeyDown);
            // 
            // UsernameField
            // 
            this.UsernameField.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameField.Location = new System.Drawing.Point(180, 270);
            this.UsernameField.Name = "UsernameField";
            this.UsernameField.Size = new System.Drawing.Size(158, 29);
            this.UsernameField.TabIndex = 10;
            this.UsernameField.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UsernameField_KeyDown);
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Font = new System.Drawing.Font("Segoe UI Emoji", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordLabel.Location = new System.Drawing.Point(68, 310);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(104, 28);
            this.PasswordLabel.TabIndex = 9;
            this.PasswordLabel.Text = "Password:";
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Font = new System.Drawing.Font("Segoe UI Emoji", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameLabel.Location = new System.Drawing.Point(62, 270);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(110, 28);
            this.UsernameLabel.TabIndex = 8;
            this.UsernameLabel.Text = "Username:";
            // 
            // SignUpLabel
            // 
            this.SignUpLabel.AutoSize = true;
            this.SignUpLabel.Font = new System.Drawing.Font("Segoe UI Emoji", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SignUpLabel.Location = new System.Drawing.Point(131, 190);
            this.SignUpLabel.Name = "SignUpLabel";
            this.SignUpLabel.Size = new System.Drawing.Size(163, 53);
            this.SignUpLabel.TabIndex = 7;
            this.SignUpLabel.Text = "Sign Up";
            // 
            // SignUpPicture
            // 
            this.SignUpPicture.Image = global::Client.Properties.Resources.signup;
            this.SignUpPicture.Location = new System.Drawing.Point(155, 85);
            this.SignUpPicture.Name = "SignUpPicture";
            this.SignUpPicture.Size = new System.Drawing.Size(90, 90);
            this.SignUpPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SignUpPicture.TabIndex = 14;
            this.SignUpPicture.TabStop = false;
            // 
            // SignUpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(158)))), ((int)(((byte)(188)))));
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.SignUpPanel);
            this.Name = "SignUpForm";
            this.Text = "SignUpForm";
            this.SignUpPanel.ResumeLayout(false);
            this.SignUpPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SignUpPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel SignUpPanel;
        private System.Windows.Forms.LinkLabel LogInInstead;
        private System.Windows.Forms.Button SignUpButton;
        private System.Windows.Forms.TextBox PasswordField;
        private System.Windows.Forms.TextBox UsernameField;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.Label SignUpLabel;
        private System.Windows.Forms.PictureBox SignUpPicture;
    }
}