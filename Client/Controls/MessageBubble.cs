using Client.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Controls
{
    public partial class MessageBubble : UserControl
    {
        public MessageBubble(string message, string senderName, string timestamp, bool isCurrentUser, int maxWidth)
        {
            InitializeComponent();
            MaximumSize = new Size(maxWidth, 0);

            BubblePanel.AutoSize = false;
            BubblePanel.BackColor = isCurrentUser ? Color.LightSkyBlue : Color.WhiteSmoke;
            BubblePanel.Padding = new Padding(14, 14, 14, 14);
            BubblePanel.MaximumSize = new Size(maxWidth, 0);
            BubblePanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            ContentPanel.FlowDirection = FlowDirection.LeftToRight;
            ContentPanel.WrapContents = true;
            ContentPanel.AutoSize = true;
            ContentPanel.BackColor = Color.Transparent;
            ContentPanel.MaximumSize = new Size(maxWidth - 20, 0);

            var parts = ParseMessage(message);

            foreach(var part in parts)
            {
                if (part.IsEmoji && ChatForm.EmojiMap.TryGetValue(part.Content, out Bitmap emoji))
                {
                    var pic = new PictureBox
                    {
                        Image = new Bitmap(emoji, new Size(23, 23)),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Size = new Size(25, 25),
                        Margin = new Padding(0, 2, 2, 2)
                    };
                    ContentPanel.Controls.Add(pic);
                }
                else
                {
                    var lbl = new Label
                    {
                        Text = part.Content,
                        AutoSize = true,
                        Margin = new Padding(0, 3, 0, 0),
                        MaximumSize = new Size(maxWidth - 20, 0),
                        // Add Font segoe emoji
                    };
                    ContentPanel.Controls.Add(lbl);
                }
            }

            BubblePanel.Controls.Add(ContentPanel);
            ContentPanel.Location = new Point(0, 0);

            int totalHeight = ContentPanel.Height;

            var metaLabel = new Label
            {
                Text = $"{senderName} • {timestamp}",
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI Emoji", 10),
                ForeColor = Color.DimGray,
                Location = new Point(0, ContentPanel.Height + 4)
            };
            BubblePanel.Controls.Add(metaLabel);
            totalHeight += metaLabel.Height + 4;

            this.Height = totalHeight + 16;
            this.Width = ContentPanel.Width + 20;
        }

        private class MessagePart
        {
            public string Content { get; set; }
            public bool IsEmoji { get; set; }
        }

        private List<MessagePart> ParseMessage(string message)
        {
            var result = new List<MessagePart>();
            int pos = 0;
            while (pos < message.Length)
            {
                // Find next emoji code
                int nextEmojiPos = -1;
                string foundEmoji = null;
                foreach (var code in ChatForm.EmojiMap.Keys)
                {
                    int idx = message.IndexOf(code, pos);
                    if (idx != -1 && (nextEmojiPos == -1 || idx < nextEmojiPos))
                    {
                        nextEmojiPos = idx;
                        foundEmoji = code;
                    }
                }
                if (nextEmojiPos == -1)
                {
                    result.Add(new MessagePart { Content = message.Substring(pos), IsEmoji = false });
                    break;
                }
                if (nextEmojiPos > pos)
                {
                    result.Add(new MessagePart { Content = message.Substring(pos, nextEmojiPos - pos), IsEmoji = false });
                }
                result.Add(new MessagePart { Content = foundEmoji, IsEmoji = true });
                pos = nextEmojiPos + foundEmoji.Length;
            }
            return result;
        }
    }
}
