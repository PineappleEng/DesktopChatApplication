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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatForm));
            this.LeftPanel = new System.Windows.Forms.Panel();
            this.ChatList = new System.Windows.Forms.FlowLayoutPanel();
            this.UserPanel = new System.Windows.Forms.Panel();
            this.LogOut = new Client.Controls.RoundedButton();
            this.CreateChat = new Client.Controls.RoundedButton();
            this.UserPicture = new Client.Controls.RoundedPicture();
            this.FillPanel = new System.Windows.Forms.Panel();
            this.RightPanel = new System.Windows.Forms.Panel();
            this.MessageList = new System.Windows.Forms.FlowLayoutPanel();
            this.ChatInput = new System.Windows.Forms.Panel();
            this.MessageInput = new System.Windows.Forms.RichTextBox();
            this.SendMessage = new System.Windows.Forms.Button();
            this.EmojiMenu = new System.Windows.Forms.Button();
            this.ChatDetails = new System.Windows.Forms.Panel();
            this.ChatName = new System.Windows.Forms.Label();
            this.ChatPicture = new Client.Controls.RoundedPicture();
            this.AddMember = new Client.Controls.RoundedButton();
            this.LeftPanel.SuspendLayout();
            this.UserPanel.SuspendLayout();
            this.RightPanel.SuspendLayout();
            this.ChatInput.SuspendLayout();
            this.ChatDetails.SuspendLayout();
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
            // ChatList
            // 
            this.ChatList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(182)))), ((int)(((byte)(210)))));
            this.ChatList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChatList.Location = new System.Drawing.Point(48, 65);
            this.ChatList.Name = "ChatList";
            this.ChatList.Size = new System.Drawing.Size(217, 496);
            this.ChatList.TabIndex = 2;
            // 
            // UserPanel
            // 
            this.UserPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(162)))), ((int)(((byte)(190)))));
            this.UserPanel.Controls.Add(this.LogOut);
            this.UserPanel.Controls.Add(this.CreateChat);
            this.UserPanel.Controls.Add(this.UserPicture);
            this.UserPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.UserPanel.Location = new System.Drawing.Point(0, 65);
            this.UserPanel.Name = "UserPanel";
            this.UserPanel.Size = new System.Drawing.Size(48, 496);
            this.UserPanel.TabIndex = 1;
            // 
            // LogOut
            // 
            this.LogOut.AutoSize = true;
            this.LogOut.BackColor = System.Drawing.Color.Transparent;
            this.LogOut.BackgroundColor = System.Drawing.SystemColors.Control;
            this.LogOut.BorderColor = System.Drawing.SystemColors.Control;
            this.LogOut.BorderWidth = 1F;
            this.LogOut.Image = global::Client.Properties.Resources.logout;
            this.LogOut.Location = new System.Drawing.Point(0, 395);
            this.LogOut.Name = "LogOut";
            this.LogOut.Radius = 10F;
            this.LogOut.Size = new System.Drawing.Size(48, 48);
            this.LogOut.TabIndex = 1;
            // 
            // CreateChat
            // 
            this.CreateChat.AutoSize = true;
            this.CreateChat.BackColor = System.Drawing.Color.Transparent;
            this.CreateChat.BackgroundColor = System.Drawing.SystemColors.Control;
            this.CreateChat.BorderColor = System.Drawing.SystemColors.Control;
            this.CreateChat.BorderWidth = 1F;
            this.CreateChat.Image = global::Client.Properties.Resources.create_chat;
            this.CreateChat.Location = new System.Drawing.Point(0, 0);
            this.CreateChat.Name = "CreateChat";
            this.CreateChat.Radius = 10F;
            this.CreateChat.Size = new System.Drawing.Size(48, 48);
            this.CreateChat.TabIndex = 2;
            // 
            // UserPicture
            // 
            this.UserPicture.BackgroundColor = System.Drawing.SystemColors.Control;
            this.UserPicture.BorderColor = System.Drawing.SystemColors.Control;
            this.UserPicture.BorderWidth = 1F;
            this.UserPicture.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.UserPicture.Image = global::Client.Properties.Resources.user;
            this.UserPicture.Location = new System.Drawing.Point(0, 448);
            this.UserPicture.Name = "UserPicture";
            this.UserPicture.Radius = 24F;
            this.UserPicture.Size = new System.Drawing.Size(48, 48);
            this.UserPicture.TabIndex = 0;
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
            // RightPanel
            // 
            this.RightPanel.Controls.Add(this.ChatInput);
            this.RightPanel.Controls.Add(this.ChatDetails);
            this.RightPanel.Controls.Add(this.MessageList);
            this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightPanel.Location = new System.Drawing.Point(265, 0);
            this.RightPanel.Name = "RightPanel";
            this.RightPanel.Size = new System.Drawing.Size(619, 561);
            this.RightPanel.TabIndex = 1;
            // 
            // MessageList
            // 
            this.MessageList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(202)))), ((int)(((byte)(230)))));
            this.MessageList.Location = new System.Drawing.Point(0, 65);
            this.MessageList.Name = "MessageList";
            this.MessageList.Size = new System.Drawing.Size(654, 473);
            this.MessageList.TabIndex = 2;
            // 
            // ChatInput
            // 
            this.ChatInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(182)))), ((int)(((byte)(210)))));
            this.ChatInput.Controls.Add(this.MessageInput);
            this.ChatInput.Controls.Add(this.SendMessage);
            this.ChatInput.Controls.Add(this.EmojiMenu);
            this.ChatInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ChatInput.Location = new System.Drawing.Point(0, 516);
            this.ChatInput.Name = "ChatInput";
            this.ChatInput.Size = new System.Drawing.Size(619, 45);
            this.ChatInput.TabIndex = 1;
            // 
            // MessageInput
            // 
            this.MessageInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MessageInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageInput.Font = new System.Drawing.Font("Segoe UI Emoji", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MessageInput.Location = new System.Drawing.Point(75, 0);
            this.MessageInput.Multiline = false;
            this.MessageInput.Name = "MessageInput";
            this.MessageInput.Size = new System.Drawing.Size(469, 45);
            this.MessageInput.TabIndex = 2;
            this.MessageInput.Text = "";
            this.MessageInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MessageInput_KeyDown);
            // 
            // SendMessage
            // 
            this.SendMessage.Dock = System.Windows.Forms.DockStyle.Right;
            this.SendMessage.Location = new System.Drawing.Point(544, 0);
            this.SendMessage.Name = "SendMessage";
            this.SendMessage.Size = new System.Drawing.Size(75, 45);
            this.SendMessage.TabIndex = 1;
            this.SendMessage.UseVisualStyleBackColor = true;
            this.SendMessage.Click += new System.EventHandler(this.SendMessage_Click);
            // 
            // EmojiMenu
            // 
            this.EmojiMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.EmojiMenu.Location = new System.Drawing.Point(0, 0);
            this.EmojiMenu.Name = "EmojiMenu";
            this.EmojiMenu.Size = new System.Drawing.Size(75, 45);
            this.EmojiMenu.TabIndex = 0;
            this.EmojiMenu.UseVisualStyleBackColor = true;
            this.EmojiMenu.Click += new System.EventHandler(this.EmojiMenu_Click);
            // 
            // ChatDetails
            // 
            this.ChatDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(158)))), ((int)(((byte)(188)))));
            this.ChatDetails.Controls.Add(this.ChatName);
            this.ChatDetails.Controls.Add(this.ChatPicture);
            this.ChatDetails.Controls.Add(this.AddMember);
            this.ChatDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChatDetails.Location = new System.Drawing.Point(0, 0);
            this.ChatDetails.Name = "ChatDetails";
            this.ChatDetails.Size = new System.Drawing.Size(619, 65);
            this.ChatDetails.TabIndex = 0;
            // 
            // ChatName
            // 
            this.ChatName.AutoSize = true;
            this.ChatName.Font = new System.Drawing.Font("Segoe UI Emoji", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChatName.Location = new System.Drawing.Point(71, 16);
            this.ChatName.Name = "ChatName";
            this.ChatName.Size = new System.Drawing.Size(134, 32);
            this.ChatName.TabIndex = 1;
            this.ChatName.Text = "Chat Name";
            // 
            // ChatPicture
            // 
            this.ChatPicture.BackgroundColor = System.Drawing.SystemColors.Control;
            this.ChatPicture.BorderColor = System.Drawing.SystemColors.Control;
            this.ChatPicture.BorderWidth = 1F;
            this.ChatPicture.Dock = System.Windows.Forms.DockStyle.Left;
            this.ChatPicture.Image = global::Client.Properties.Resources.monday_emoji;
            this.ChatPicture.Location = new System.Drawing.Point(0, 0);
            this.ChatPicture.Name = "ChatPicture";
            this.ChatPicture.Radius = 32.5F;
            this.ChatPicture.Size = new System.Drawing.Size(65, 65);
            this.ChatPicture.TabIndex = 0;
            // 
            // AddMember
            // 
            this.AddMember.AutoSize = true;
            this.AddMember.BackColor = System.Drawing.Color.Transparent;
            this.AddMember.BackgroundColor = System.Drawing.SystemColors.Control;
            this.AddMember.BorderColor = System.Drawing.SystemColors.Control;
            this.AddMember.BorderWidth = 1F;
            this.AddMember.Image = ((System.Drawing.Bitmap)(resources.GetObject("AddMember.Image")));
            this.AddMember.Location = new System.Drawing.Point(561, 5);
            this.AddMember.Name = "AddMember";
            this.AddMember.Radius = 12F;
            this.AddMember.Size = new System.Drawing.Size(55, 55);
            this.AddMember.TabIndex = 2;
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(202)))), ((int)(((byte)(230)))));
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.RightPanel);
            this.Controls.Add(this.LeftPanel);
            this.Name = "ChatForm";
            this.Text = "ChatForm";
            this.LeftPanel.ResumeLayout(false);
            this.UserPanel.ResumeLayout(false);
            this.UserPanel.PerformLayout();
            this.RightPanel.ResumeLayout(false);
            this.ChatInput.ResumeLayout(false);
            this.ChatDetails.ResumeLayout(false);
            this.ChatDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel LeftPanel;
        private System.Windows.Forms.Panel RightPanel;
        private System.Windows.Forms.Panel FillPanel;
        private System.Windows.Forms.Panel ChatDetails;
        private System.Windows.Forms.Panel UserPanel;
        private System.Windows.Forms.Panel ChatInput;
        private Controls.RoundedPicture UserPicture;
        private Controls.RoundedButton LogOut;
        private Controls.RoundedPicture ChatPicture;
        private System.Windows.Forms.Label ChatName;
        private Controls.RoundedButton AddMember;
        private Controls.RoundedButton CreateChat;
        private System.Windows.Forms.RichTextBox MessageInput;
        private System.Windows.Forms.Button SendMessage;
        private System.Windows.Forms.Button EmojiMenu;
        public System.Windows.Forms.FlowLayoutPanel ChatList;
        public System.Windows.Forms.FlowLayoutPanel MessageList;
    }
}