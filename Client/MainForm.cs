using System;
using System.Windows.Forms;

using Common.Models;
using Common.Network;
using Client.Forms;
using Client.Network;
using System.Text.Json;

namespace Client
{
    public partial class MainForm : Form
    {
        private ChatClient _client;

        public MainForm()
        {
            InitializeComponent();
            ShowLogInForm();
        }

        public void InitializeApplication()
        {
            ShowLogInForm();
            _client = new ChatClient();
            _client.MessageReceived += OnMessageReceived;
            _client.Disconnected += OnDisconnected;
        }

        private void OnMessageReceived(NetworkMessage msg)
        {

        }

        private void OnDisconnected()
        {
            _client.Disconnect();
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

        public void ShowChatForm()
        {
            if (_chatForm == null || _chatForm.IsDisposed)
                _chatForm = new ChatForm();
            LoadForm(_chatForm);
        }
        #endregion

        #region Application Flow
        private async void OnLogInButtonClicked(string username, string rawPassword)
        {
            var networkMessage = new NetworkMessage
            {
                MessageType = NetworkMessageType.Login,
                Payload = JsonSerializer.Serialize(new User
                {
                    Name = username,
                    HashedPassword = rawPassword
                })
            };
            await _client.SendMessage(networkMessage);
            // TODO: Wait for a response
            ShowChatForm();
        }

        private void OnSignUpInsteadClicked(object sender, EventArgs e)
        {
            ShowSignUpForm();
        }

        private void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            // TODO: Implement user registration
            ShowLogInForm();
        }

        private void OnLogInInsteadClicked(object sender, EventArgs e)
        {
            ShowLogInForm();
        }
        #endregion

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await _client.Connect("", 0);
        }
    }
}
