using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Client.Forms;

namespace Client
{
    public partial class MainForm : Form
    {
        #region Application Views

        private Form _activeForm;
        private LogInForm _logInForm;
        private SignUpForm _signUpForm;
        private ChatForm _chatForm;

        public MainForm()
        {
            InitializeComponent();
        }

        private void LoadForm(Form f)
        {
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
        }

        public void ShowLogInForm()
        {
            if (_logInForm == null || _logInForm.IsDisposed)
                _logInForm = new LogInForm();
            LoadForm(_logInForm);
        }

        public void ShowSignUpForm()
        {
            if (_signUpForm == null || _signUpForm.IsDisposed)
                _signUpForm = new SignUpForm();
            LoadForm(_signUpForm);
        }

        public void ShowChatForm()
        {
            if (_chatForm == null || _chatForm.IsDisposed)
                _chatForm = new ChatForm();
            LoadForm(_chatForm);
        }

        #endregion
    }
}
