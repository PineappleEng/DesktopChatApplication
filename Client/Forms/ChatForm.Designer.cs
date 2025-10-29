namespace Client.Forms
{
    partial class ChatForm
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
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.FillPanel = new System.Windows.Forms.Panel();
            this.ChatDetails = new System.Windows.Forms.Panel();
            this.UserPanel = new System.Windows.Forms.Panel();
            this.ChatList = new System.Windows.Forms.FlowLayoutPanel();
            this.ChatInput = new System.Windows.Forms.Panel();
            this.MessageList = new System.Windows.Forms.FlowLayoutPanel();
            this.roundedButton1 = new Client.Controls.RoundedButton();
            this.UserIcon = new Client.Controls.RoundedPicture();
            this.LeftPanel.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.UserPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // LeftPanel
            // 
            this.LeftPanel.Controls.Add(this.ChatList);
            this.LeftPanel.Controls.Add(this.UserPanel);
            this.LeftPanel.Controls.Add(this.FillPanel);
            this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftPanel.Name = "LeftPanel";
            this.LeftPanel.Size = new System.Drawing.Size(265, 561);
            this.LeftPanel.TabIndex = 0;
            // 
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.MessageList);
            this.RightPanel.Controls.Add(this.ChatInput);
            this.RightPanel.Controls.Add(this.ChatDetails);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightPanel.Location = new System.Drawing.Point(265, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(619, 561);
            this.RightPanel.TabIndex = 1;
            // 
            // FillPanel
            // 
            this.FillPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(158)))), ((int)(((byte)(188)))));
            this.FillPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.FillPanel.Location = new System.Drawing.Point(0, 0);
            this.FillPanel.Name = "FillPanel";
            this.FillPanel.Size = new System.Drawing.Size(265, 65);
            this.FillPanel.TabIndex = 0;
            // 
            // ChatDetails
            // 
            this.ChatDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(158)))), ((int)(((byte)(188)))));
            this.ChatDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChatDetails.Location = new System.Drawing.Point(0, 0);
            this.ChatDetails.Name = "ChatDetails";
            this.ChatDetails.Size = new System.Drawing.Size(619, 65);
            this.ChatDetails.TabIndex = 0;
            // 
            // UserPanel
            // 
            this.UserPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(162)))), ((int)(((byte)(190)))));
            this.UserPanel.Controls.Add(this.roundedButton1);
            this.UserPanel.Controls.Add(this.UserIcon);
            this.UserPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.UserPanel.Location = new System.Drawing.Point(0, 65);
            this.UserPanel.Name = "UserPanel";
            this.UserPanel.Size = new System.Drawing.Size(48, 496);
            this.UserPanel.TabIndex = 1;
            // 
            // ChatList
            // 
            this.ChatList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(182)))), ((int)(((byte)(210)))));
            this.ChatList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChatList.Location = new System.Drawing.Point(48, 65);
            this.ChatList.Name = "ChatList";
            this.ChatList.Size = new System.Drawing.Size(217, 496);
            this.ChatList.TabIndex = 2;
            // 
            // ChatInput
            // 
            this.ChatInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ChatInput.Location = new System.Drawing.Point(0, 516);
            this.ChatInput.Name = "ChatInput";
            this.ChatInput.Size = new System.Drawing.Size(619, 45);
            this.ChatInput.TabIndex = 1;
            // 
            // MessageList
            // 
            this.MessageList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(202)))), ((int)(((byte)(230)))));
            this.MessageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageList.Location = new System.Drawing.Point(0, 65);
            this.MessageList.Name = "MessageList";
            this.MessageList.Size = new System.Drawing.Size(619, 451);
            this.MessageList.TabIndex = 2;
            // 
            // roundedButton1
            // 
            this.roundedButton1.AutoSize = true;
            this.roundedButton1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.roundedButton1.BorderColor = System.Drawing.SystemColors.Control;
            this.roundedButton1.BorderWidth = 1F;
            this.roundedButton1.Image = global::Client.Properties.Resources.logout;
            this.roundedButton1.Location = new System.Drawing.Point(0, 400);
            this.roundedButton1.Name = "roundedButton1";
            this.roundedButton1.Radius = 24F;
            this.roundedButton1.Size = new System.Drawing.Size(48, 48);
            this.roundedButton1.TabIndex = 1;
            // 
            // UserIcon
            // 
            this.UserIcon.BackgroundColor = System.Drawing.SystemColors.Control;
            this.UserIcon.BorderColor = System.Drawing.SystemColors.Control;
            this.UserIcon.BorderWidth = 1F;
            this.UserIcon.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.UserIcon.Image = global::Client.Properties.Resources.user;
            this.UserIcon.Location = new System.Drawing.Point(0, 448);
            this.UserIcon.Name = "UserIcon";
            this.UserIcon.Radius = 24F;
            this.UserIcon.Size = new System.Drawing.Size(48, 48);
            this.UserIcon.TabIndex = 0;
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.LeftPanel);
            this.Name = "ChatForm";
            this.Text = "ChatForm";
            this.LeftPanel.ResumeLayout(false);
            this.RightPanel.ResumeLayout(false);
            this.UserPanel.ResumeLayout(false);
            this.UserPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Panel FillPanel;
        private System.Windows.Forms.Panel ChatDetails;
        private System.Windows.Forms.Panel UserPanel;
        private System.Windows.Forms.FlowLayoutPanel ChatList;
        private System.Windows.Forms.Panel ChatInput;
        private System.Windows.Forms.FlowLayoutPanel MessageList;
        private Controls.RoundedPicture UserIcon;
        private Controls.RoundedButton roundedButton1;
    }
}