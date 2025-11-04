using System;
using System.Windows.Forms;

using Common.Models;
using Common.Network;
using Client.Forms;
using Client.Network;
using System.Text.Json;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Client
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeApplication();
        }

        public void InitializeApplication()
        {
            ShowLogInForm();
        }

        #region Form Switch Logic
        private Form _activeForm;
        private LogInForm _logInForm;
        private SignUpForm _signUpForm;
        private ChatForm _chatForm;

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
            await SendMessage(new NetworkMessage
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
            await SendMessage(new NetworkMessage
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

        #region Tcp Client Logic
        private TcpClient _client;
        private StreamWriter _writer;
        private StreamReader _reader;

        private async void MainForm_Load(object sender, EventArgs e)
        {
            string serverIp = "10.0.2.15";
            int port = 6969;
            _client = new TcpClient();
            await _client.ConnectAsync(serverIp, port);

            var stream = _client.GetStream();
            _writer = new StreamWriter(stream) { AutoFlush = true };
            _reader = new StreamReader(stream);

            _ = Task.Run(Listen);
        }

        private async Task Listen()
        {
            string line;
            while ((line = await _reader.ReadLineAsync()) != null)
            {
                var msg = JsonSerializer.Deserialize<NetworkMessage>(line);
                HandleMessage(msg);
            }
        }

        private async Task SendMessage(NetworkMessage msg)
        {
            string json = JsonSerializer.Serialize(msg);
            await _writer.WriteLineAsync(json);
        }

        private void HandleMessage(NetworkMessage msg)
        {
            switch (msg.MessageType)
            {
                case NetworkMessageType.Error:
                    string errorMsg = msg.GetPayload<string>();
                    MessageBox.Show(
                        errorMsg, 
                        "Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                    break;
                case NetworkMessageType.Signup:
                    MessageBox.Show(
                        "Signed up successfuly",
                        "Signup",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    ShowLogInForm();
                    break;
                case NetworkMessageType.Login:
                    ShowChatForm(msg.GetPayload<User>());
                    break;
                //case NetworkMessageType.Login:
                //    break;
                //case NetworkMessageType.Login:
                //    break;
                //case NetworkMessageType.Login:
                //    break;
                //case NetworkMessageType.Login:
                //    break;
            }
        }
        #endregion
    }
}
