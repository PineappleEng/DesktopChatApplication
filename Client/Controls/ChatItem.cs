using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Controls
{
    public partial class ChatItem : UserControl
    {
        private Bitmap _image;
        public Bitmap Image
        {
            get { return _image; }
            set
            {
                _image = value;
                ChatPicture.Image = _image;
            }
        }

        private string _text;
        public override string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                ChatName.Text = _text;
            }
        }

        public event EventHandler ChatItemClicked;

        public ChatItem()
        {
            InitializeComponent();
            ChatPicture.PictureClicked += ChatItemClicked;
        }

        #region ChatItem Click Event Propagation
        private void ChatItem_Click(object sender, EventArgs e)
        {
            ChatItemClicked?.Invoke(this, EventArgs.Empty);
        }

        private void ChatPicture_Click(object sender, EventArgs e)
        {
            ChatItemClicked?.Invoke(this, EventArgs.Empty);
        }

        private void ChatName_Click(object sender, EventArgs e)
        {
            ChatItemClicked?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        private void ChatItem_Resize(object sender, EventArgs e)
        {
            // Picture Resize
            int min = Math.Min(Width, Height);
            ChatPicture.Size = new Size(min, min);
            // Label Resize & Relocation
            float fontSize = (Height - ChatName.Height) / 2.0f;
            ChatName.Font = new Font(ChatName.Font.FontFamily, fontSize, ChatName.Font.Style);
            int x = min + 8, y = (Height - ChatName.Height) / 2;
            ChatName.Location = new Point(x, y);
        }
    }
}
