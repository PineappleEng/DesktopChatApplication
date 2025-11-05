using Client.Controls;
using Client.Properties;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Client.Forms
{
    public partial class ChatForm : Form
    {
        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set 
            { 
                _currentUser = value;
                RequestChats?.Invoke(CurrentUser);
            }
        }

        private Chat _currentChat;
        public Chat CurrentChat
        {
            get => _currentChat;
            set
            {
                _currentChat = value;
                ChatName.Text = CurrentChat.Name;
                UpdateControlVisibility();
            }
        }

        private List<Chat> _userChatList = new List<Chat>();
        public List<Chat> UserChatList
        {
            get => _userChatList;
            set => _userChatList = value ?? new List<Chat>();
        }

        public event EventHandler LogOutButtonClicked;
        public event EventHandler CreateChatButtonClicked;
        public event Action<User> RequestChats;

        private ChatItem _lastSelectedItem; // track previously selected item for color restoration

        public ChatForm()
        {
            InitializeComponent();
            UpdateControlVisibility();
            LogOut.ButtonClicked += OnLogOutClicked;
            CreateChat.ButtonClicked += OnCreateChatClicked;
            AddMember.ButtonClicked += OnAddMemberClicked;
        }

        private void UpdateControlVisibility()
        {
            bool hasChat = CurrentChat != null;

            ChatDetails.Visible = hasChat;
            ChatDetails.Enabled = hasChat;

            MessageList.Visible = hasChat;
            MessageList.Enabled = hasChat;

            ChatInput.Visible = hasChat;
            ChatInput.Enabled = hasChat;
        }

        private ChatItem CreateChatItem(Chat chat)
        {
            var chatItem = new ChatItem(chat)
            {
                Image = Resources.monday_emoji,
                Cursor = Cursors.Hand
            };
            chatItem.ChatItemClicked += OnChatItemClicked;
            return chatItem;
        }

        public void LoadChats()
        {
            ChatList.SuspendLayout(); // prevents flicker during control addition
            ChatList.Controls.Clear();

            foreach (Chat chat in UserChatList)
            {
                var chatItem = CreateChatItem(chat);
                ChatList.Controls.Add(chatItem);
            }

            ChatList.ResumeLayout();
        }

        // async retained for future async loading logic
        private void OnChatItemClicked(object sender, EventArgs e)
        {
            if (!(sender is ChatItem selectedItem))
                return;

            // restore previous selection’s color
            if (_lastSelectedItem != null)
                _lastSelectedItem.BackColor = selectedItem.BackColor;

            _lastSelectedItem = selectedItem;

            // visually indicate selection
            selectedItem.BackColor = DarkenColor(selectedItem.BackColor, 0.85f);

            // set current chat
            CurrentChat = selectedItem.Chat;

            // update UI fields
            ChatPicture.Image = selectedItem.Image;
            ChatName.Text = selectedItem.Chat.Name;

            // placeholder for loading messages asynchronously
            //await LoadCurrentChatMessages(selectedItem.Chat);
            // TODO: Add message list request here
        }

        private void LoadCurrentChatMessages()
        {

        }

        private static Color DarkenColor(Color color, float factor)
        {
            int r = (int)(color.R * factor);
            int g = (int)(color.G * factor);
            int b = (int)(color.B * factor);
            return Color.FromArgb(r, g, b);
        }

        private void OnLogOutClicked(object sender, EventArgs e)
        {
            LogOutButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnCreateChatClicked(object sender, EventArgs e)
        {
            CreateChatButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnAddMemberClicked(object sender, EventArgs e)
        {
            // TODO: Implement AddMember logic or raise event
        }

        private void EmojiMenu_Click(object sender, EventArgs e)
        {
            // TODO: Implement Emoji menu logic or raise event
        }

        private void SendMessage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MessageInput.Text))
                return;

            // TODO: Implement sending logic
            MessageInput.Clear();
        }

        private void MessageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage.PerformClick();
                e.SuppressKeyPress = true;
            }
        }
    }
}
