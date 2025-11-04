using System;
using System.Windows.Forms;

using Common.Models;

namespace Client.Forms
{
    public partial class LogInForm : Form
    {
        public event Action<User> LogInButtonClicked;
        public event EventHandler SignUpInsteadClicked;

        public LogInForm()
        {
            InitializeComponent();
        }

        private void UsernameField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                PasswordField.Focus();
        }

        private void PasswordField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LogInButton.PerformClick();
        }

        private void LogInButton_Click(object sender, EventArgs e)
        {
            LogInButtonClicked?.Invoke(new User
            {
                Name = UsernameField.Text.Trim(),
                HashedPassword = PasswordField.Text.Trim()
            });
        }

        private void SignUpInstead_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UsernameField.Clear();
            PasswordField.Clear();
            SignUpInsteadClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
