using Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Forms
{
    public partial class SignUpForm : Form
    {
        public event Action<User> SignUpButtonClicked;
        public event EventHandler LogInInsteadClicked;

        public SignUpForm()
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
                SignUpButton.PerformClick();
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            SignUpButtonClicked?.Invoke(new User
            {
                Name = UsernameField.Text.Trim(),
                HashedPassword = PasswordField.Text.Trim()
            });
        }

        private void LogInInstead_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UsernameField.Clear();
            PasswordField.Clear();
            LogInInsteadClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
