using System;
using System.Windows.Forms;
using System.Text.Json;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using Common.Models;
using Common.Network;
using Client.Forms;

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
                _chatForm = new ChatForm();

            _chatForm.CurrentUser = user;
            LoadForm(_chatForm);
        }

        #endregion

        #region Application Flow Logic

        private async void OnSignUpButtonClicked(User user)
        {
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

        #endregion

        #region TCP Client Logic

        private async void MainForm_Load(object sender, EventArgs e)
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
                    var user = msg.GetPayload<User>();
                    ShowChatForm(user);
                    break;

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
