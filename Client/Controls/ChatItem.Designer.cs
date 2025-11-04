namespace Client.Controls
{
    partial class ChatItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ChatName = new System.Windows.Forms.Label();
            this.ChatPicture = new Client.Controls.RoundedPicture();
            this.SuspendLayout();
            // 
            // ChatName
            // 
            this.ChatName.AutoSize = true;
            this.ChatName.Font = new System.Drawing.Font("Segoe UI Emoji", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChatName.Location = new System.Drawing.Point(58, 12);
            this.ChatName.Name = "ChatName";
            this.ChatName.Size = new System.Drawing.Size(106, 26);
            this.ChatName.TabIndex = 1;
            this.ChatName.Text = "Chat Name";
            this.ChatName.Click += new System.EventHandler(this.ChatName_Click);
            // 
            // ChatPicture
            // 
            this.ChatPicture.BackgroundColor = System.Drawing.SystemColors.Control;
            this.ChatPicture.BorderColor = System.Drawing.SystemColors.Control;
            this.ChatPicture.BorderWidth = 1F;
            this.ChatPicture.Dock = System.Windows.Forms.DockStyle.Left;
            this.ChatPicture.Image = null;
            this.ChatPicture.Location = new System.Drawing.Point(0, 0);
            this.ChatPicture.Margin = new System.Windows.Forms.Padding(5);
            this.ChatPicture.Name = "ChatPicture";
            this.ChatPicture.Radius = 25F;
            this.ChatPicture.Size = new System.Drawing.Size(50, 50);
            this.ChatPicture.TabIndex = 0;
            this.ChatPicture.Click += new System.EventHandler(this.ChatPicture_Click);
            // 
            // ChatItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(170)))), ((int)(((byte)(200)))));
            this.Controls.Add(this.ChatName);
            this.Controls.Add(this.ChatPicture);
            this.Name = "ChatItem";
            this.Size = new System.Drawing.Size(250, 50);
            this.Click += new System.EventHandler(this.ChatItem_Click);
            this.Resize += new System.EventHandler(this.ChatItem_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RoundedPicture ChatPicture;
        private System.Windows.Forms.Label ChatName;
    }
}
