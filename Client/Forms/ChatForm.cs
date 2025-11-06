using Client.Controls;
using Client.Properties;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Client.Forms
{
    public partial class ChatForm : System.Windows.Forms.Form
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
                RequestMessages?.Invoke(CurrentChat);
                UpdateControlVisibility();
            }
        }

        private List<Chat> _userChatList = new List<Chat>();
        public List<Chat> UserChatList
        {
            get => _userChatList;
            set => _userChatList = value ?? new List<Chat>();
        }

        private List<KeyValuePair<string, Common.Models.Message>> _currentChatMessages = new List<KeyValuePair<string, Common.Models.Message>>();
        public List<KeyValuePair<string, Common.Models.Message>> CurrentChatMessages
        {
            get => _currentChatMessages;
            set => _currentChatMessages = value ?? new List<KeyValuePair<string, Common.Models.Message>>();
        }

        public static Dictionary<string, Bitmap> EmojiMap = new Dictionary<string, Bitmap>
        {
            { ":monday:", Resources.monday_emoji },
            { ":tux:", Resources.tux_emoji },
            { ":laugh:", Resources.laugh_emoji },
            { ":smile:", Resources.smile_emoji },
            { ":thumbs:", Resources.thumbs_emoji }
        };

        public event EventHandler LogOutButtonClicked;
        public event EventHandler CreateChatButtonClicked;
        public event Action<int, int, string, string> SendMessageButtonClicked;
        public event Action<User> RequestChats;
        public event Action<Chat> RequestMessages;

        private ChatItem _lastSelectedItem;

        public ChatForm()
        {
            InitializeComponent();
            UpdateControlVisibility();
            LogOut.ButtonClicked += OnLogOutClicked;
            CreateChat.ButtonClicked += OnCreateChatClicked;
            AddMember.ButtonClicked += OnAddMemberClicked;
        }

        #region User Chat Listing Logic

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
            // TODO: Add message list request here?
        }

        private static Color DarkenColor(Color color, float factor)
        {
            int r = (int)(color.R * factor);
            int g = (int)(color.G * factor);
            int b = (int)(color.B * factor);
            return Color.FromArgb(r, g, b);
        }

        #endregion

        public void InsertMessageBubble(
            string senderName,
            string content,
            string timestamp,
            bool isCurrentUser)
        {
            int maxBubbleWidth = Math.Max(100, 2 * MessageList.ClientSize.Width / 5);
            var bubble = new MessageBubble(content, senderName, timestamp, isCurrentUser, maxBubbleWidth);
            var messageContainer = new Panel
            {
                Height = bubble.Height + 8,
                Width = MessageList.ClientSize.Width,
                Margin = new Padding(0, 2, 0, 2),
                BackColor = Color.Transparent
            };

            if (isCurrentUser)
                bubble.Location = new Point(messageContainer.Width - bubble.Width - 40, 4);
            else
                bubble.Location = new Point(5, 4);

            messageContainer.Controls.Add(bubble);
            MessageList.Controls.Add(messageContainer);
        }

        public void LoadMessages()
        {
            MessageList.SuspendLayout();
            MessageList.Controls.Clear();
            MessageList.AutoScroll = true;

            foreach (var message in CurrentChatMessages)
            {
                int senderId = message.Value.SenderId;
                string senderName = message.Key;
                string content = message.Value.Content;
                string timestamp = message.Value.Timestamp;
                bool isCurrentUser = (senderId == CurrentUser.Id);

                InsertMessageBubble(senderName, content, timestamp, isCurrentUser);
            }
            MessageList.ResumeLayout(true);
        }

        #region Handlers

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

            SendMessageButtonClicked?.Invoke(
                CurrentChat.Id,
                CurrentUser.Id,
                MessageInput.Text.Trim(),
                CurrentUser.Name);
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

        #endregion
    }
}
