using Client.Forms;
using Common.Models;
using Common.Network;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class MainForm : Form
    {
        private Form _activeForm;
        private LogInForm _logInForm;
        private SignUpForm _signUpForm;
        private ChatForm _chatForm;

        private TcpClient _client;
        private StreamWriter _writer;
        private StreamReader _reader;
        private bool _listening = false;
        private bool _loggedIn = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeApplication();
        }

        public void InitializeApplication()
        {
            ShowLogInForm();
        }

        #region Form Switching Logic

        private void LoadForm(Form f)
        {
            SuspendLayout();

            if (_activeForm != null)
            {
                ContentPanel.Controls.Remove(_activeForm);
                _activeForm.Dispose();
            }

            _activeForm = f;
            _activeForm.TopLevel = false;
            _activeForm.FormBorderStyle = FormBorderStyle.None;
            _activeForm.Dock = DockStyle.Fill;
            ContentPanel.Controls.Add(_activeForm);
            _activeForm.Show();

            ResumeLayout();
        }

        public void ShowLogInForm()
        {
            if (_logInForm == null || _logInForm.IsDisposed)
            {
                _logInForm = new LogInForm();
                _logInForm.LogInButtonClicked += OnLogInButtonClicked;
                _logInForm.SignUpInsteadClicked += OnSignUpInsteadClicked;
            }
            LoadForm(_logInForm);
        }

        public void ShowSignUpForm()
        {
            if (_signUpForm == null || _signUpForm.IsDisposed)
            {
                _signUpForm = new SignUpForm();
                _signUpForm.SignUpButtonClicked += OnSignUpButtonClicked;
                _signUpForm.LogInInsteadClicked += OnLogInInsteadClicked;
            }
            LoadForm(_signUpForm);
        }

        public void ShowChatForm(User user)
        {
            if (_chatForm == null || _chatForm.IsDisposed)
            {
                _chatForm = new ChatForm();
                _chatForm.LogOutButtonClicked += OnLogOutButtonClicked;
                _chatForm.CreateChatButtonClicked += OnCreateChatButtonClicked;
                _chatForm.RequestChats += OnRequestChats;
            }

            _chatForm.CurrentUser = user;
            LoadForm(_chatForm);
        }

        #endregion

        #region Application Flow Logic

        private async void OnSignUpButtonClicked(User user)
        {
            if (_client == null || !_client.Connected)
            {
                await ConnectToServer();
            }
            await SendMessageAsync(new NetworkMessage
            {
                MessageType = NetworkMessageType.Signup,
                Payload = JsonSerializer.Serialize(user)
            });
        }

        private void OnLogInInsteadClicked(object sender, EventArgs e)
        {
            ShowLogInForm();
        }

        private async void OnLogInButtonClicked(User user)
        {
            if (_client == null || !_client.Connected)
            {
                await ConnectToServer();
            }
            await SendMessageAsync(new NetworkMessage
            {
                MessageType = NetworkMessageType.Login,
                Payload = JsonSerializer.Serialize(user)
            });
        }

        private void OnSignUpInsteadClicked(object sender, EventArgs e)
        {
            ShowSignUpForm();
        }

        private async void OnLogOutButtonClicked(object sender, EventArgs e)
        {
            await SendMessageAsync(new NetworkMessage
            {
                MessageType = NetworkMessageType.Logout
            });
            _client = null;
        }

        private async void OnRequestChats(User user)
        {
            await SendMessageAsync(new NetworkMessage
            {
                MessageType = NetworkMessageType.ListChats,
                Payload = JsonSerializer.Serialize(user)
            });
        }

        private async void OnCreateChatButtonClicked(object sender, EventArgs e)
        {
            await SendMessageAsync(new NetworkMessage
            {
                MessageType = NetworkMessageType.ListUsers
            });
        }

        private void CreateChat(List<User> users)
        {
            // Defensive: ensure there's a valid chat form and user context
            if (_chatForm == null || _chatForm.CurrentUser == null)
            {
                MessageBox.Show("You must be logged in to create a chat.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var dlg = new CreateChatPrompt())
            {
                // Populate the checklist with all users except the current one
                foreach (var user in users)
                {
                    dlg.CheckedList.Items.Add(user, false);
                }

                // Show dialog and proceed only if user confirms
                DialogResult result = dlg.ShowDialog(this);
                if (result != DialogResult.OK)
                    return;

                string chatName = dlg.ChatNameTextbox.Text.Trim();

                if (string.IsNullOrEmpty(chatName))
                {
                    MessageBox.Show(
                        "Please enter a name for the chat.",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Collect selected users (iterate correctly over CheckedItems)
                var selectedUsers = new List<User>();
                foreach (User selectedItem in dlg.CheckedList.CheckedItems)
                {
                    selectedUsers.Add(selectedItem);
                }

                // Ensure at least one user selected
                if (selectedUsers.Count == 0)
                {
                    MessageBox.Show(
                        "Select at least one user to create a chat.",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Add the current user to the participant list
                selectedUsers.Add(_chatForm.CurrentUser);

                try
                {
                    // Create and send the chat creation request
                    var newChat = new Chat
                    {
                        Name = chatName,
                        AdminId = _chatForm.CurrentUser.Id
                    };

                    var payload = new List<object> { newChat, selectedUsers };

                    Task.Run(() => SendMessageAsync(new NetworkMessage
                    {
                        MessageType = NetworkMessageType.CreateChat,
                        Payload = JsonSerializer.Serialize(payload)
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to create chat:\n{ex.Message}",
                        "Network Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region TCP Client Logic

        private async Task ConnectToServer()
        {
            try
            {
                await ConnectToServerAsync("10.0.2.15", 6969);
                _ = Task.Run(ListenAsync);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to server:\n{ex.Message}",
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ConnectToServerAsync(string ip, int port)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(ip, port);

            var stream = _client.GetStream();
            _writer = new StreamWriter(stream) { AutoFlush = true };
            _reader = new StreamReader(stream);
            _listening = true;
        }

        private async Task ListenAsync()
        {
            try
            {
                string line;
                while (_listening && (line = await _reader.ReadLineAsync()) != null)
                {
                    var msg = JsonSerializer.Deserialize<NetworkMessage>(line);
                    if (msg != null)
                        HandleMessageSafe(msg);
                }
            }
            catch (IOException)
            {
                HandleMessageSafe(new NetworkMessage
                {
                    MessageType = NetworkMessageType.Error,
                    Payload = JsonSerializer.Serialize("Lost connection to server.")
                });
            }
            catch (Exception ex)
            {
                HandleMessageSafe(new NetworkMessage
                {
                    MessageType = NetworkMessageType.Error,
                    Payload = JsonSerializer.Serialize($"Listener error: {ex.Message}")
                });
            }
        }

        private async Task SendMessageAsync(NetworkMessage msg)
        {
            try
            {
                if (_writer == null)
                {
                    MessageBox.Show("Not connected to the server.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string json = JsonSerializer.Serialize(msg);
                await _writer.WriteLineAsync(json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send message:\n{ex.Message}",
                    "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleMessageSafe(NetworkMessage msg)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => HandleMessage(msg)));
            else
                HandleMessage(msg);
        }

        private void HandleMessage(NetworkMessage msg)
        {
            switch (msg.MessageType)
            {
                case NetworkMessageType.Error:
                    MessageBox.Show(msg.GetPayload<string>(),
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case NetworkMessageType.Signup:
                    MessageBox.Show("Signed up successfully!",
                        "Signup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowLogInForm();
                    break;

                case NetworkMessageType.Login:
                {
                    var user = msg.GetPayload<User>();
                    _loggedIn = true;
                    ShowChatForm(user);
                    break;
                }

                case NetworkMessageType.Logout:
                    _loggedIn = false;
                    ShowLogInForm();
                    break;

                case NetworkMessageType.ListChats:
                    _chatForm.UserChatList = msg.GetPayload<List<Chat>>();
                    _chatForm.LoadChats();
                    break;

                case NetworkMessageType.ListUsers:
                {
                    var allUsers = msg.GetPayload<List<User>>();
                    var users = new List<User>();
                    foreach (var user in allUsers)
                    {
                        if (user.Id == _chatForm.CurrentUser.Id)
                            continue;
                        users.Add(user);
                    }
                    if (users.Count == 0)
                    {
                        MessageBox.Show(
                            "There are no other users",
                            "No Users",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                        CreateChat(users);
                    break;
                }

                case NetworkMessageType.CreateChat:
                {
                    var chat = msg.GetPayload<Chat>();
                    _chatForm.CurrentChat = chat;
                    _chatForm.UserChatList.Add(chat);
                    _chatForm.LoadChats();
                    break;
                }

                default:
                    MessageBox.Show(
                        $"Unhandled message type: {msg.MessageType}",
                        "Debug", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                    break;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Task.Run(() => SendMessageAsync(new NetworkMessage
            {
                MessageType = NetworkMessageType.Logout,
                Payload = null
            }));
            _listening = false;

            try
            {
                _reader?.Dispose();
                _writer?.Dispose();
                _client?.Close();
            }
            catch { /* ignore */ }
        }

        #endregion
    }
}
