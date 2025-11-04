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
    public partial class CreateChatPrompt : Form
    {
        public CheckedListBox CheckedList
        {
            get { return CheckedListBox; }
        }

        public TextBox ChatNameTextbox
        {
            get { return ChatNameField; }
        }

        public CreateChatPrompt()
        {
            InitializeComponent();
        }
    }
}
