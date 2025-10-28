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
    public partial class LogInForm : Form
    {
        public event EventHandler LogInButtonClicked;
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
            LogInButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void SignUpInstead_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UsernameField.Clear();
            PasswordField.Clear();
            SignUpInsteadClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
