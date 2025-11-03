using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Controls
{
    public partial class RoundedPicture : UserControl
    {
        #region Control Border Rounding Logic
        private float radius = 0f;
        [Category("Appearance")]
        public float Radius
        {
            get => radius;
            set { radius = value; Invalidate(); }
        }

        private Color background_color = SystemColors.Control;
        private SolidBrush background_brush;
        [Category("Appearance")]
        public Color BackgroundColor
        {
            get => background_color;
            set
            {
                background_color = value;
                background_brush?.Dispose();
                background_brush = new SolidBrush(value);
                Invalidate();
            }
        }

        private Color border_color = SystemColors.Control;
        private float border_width = 1f;
        private Pen border_pen;
        [Category("Appearance")]
        public Color BorderColor
        {
            get => border_color;
            set
            {
                border_color = value;
                border_pen?.Dispose();
                border_pen = new Pen(ControlPaint.Dark(border_color), border_width);
                Invalidate();
            }
        }

        [Category("Appearance")]
        public float BorderWidth
        {
            get => border_width;
            set
            {
                border_width = value;
                border_pen?.Dispose();
                border_pen = new Pen(ControlPaint.Dark(border_color), border_width);
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Width <= 0 || Height <= 0) return;

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            try
            {
                float offset = Math.Max(0, border_width / 2f);
                float width = Math.Max(1, Width - border_width);
                float height = Math.Max(1, Height - border_width);

                RectangleF rect = new RectangleF(offset, offset, width, height);
                float effectiveRadius = Math.Max(0, Math.Min(radius, Math.Min(rect.Width, rect.Height) / 2));

                using (GraphicsPath path = new GraphicsPath())
                {
                    if (effectiveRadius > 0)
                    {
                        path.AddArc(rect.X, rect.Y, effectiveRadius * 2, effectiveRadius * 2, 180, 90);
                        path.AddArc(rect.Right - effectiveRadius * 2, rect.Y, effectiveRadius * 2, effectiveRadius * 2, 270, 90);
                        path.AddArc(rect.Right - effectiveRadius * 2, rect.Bottom - effectiveRadius * 2, effectiveRadius * 2, effectiveRadius * 2, 0, 90);
                        path.AddArc(rect.X, rect.Bottom - effectiveRadius * 2, effectiveRadius * 2, effectiveRadius * 2, 90, 90);
                        path.CloseFigure();
                    }
                    else
                    {
                        path.AddRectangle(rect);
                    }

                    Region = new Region(path);
                    g.FillPath(background_brush, path);
                    if (border_width > 0) g.DrawPath(border_pen, path);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Paint error: {ex.Message}");
            }
        }
        #endregion

        private Bitmap _image;
        public Bitmap Image
        {
            get { return _image; }
            set
            {
                _image = value;
                PictureBox.Image = _image;
                PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        public event EventHandler PictureClicked;

        public RoundedPicture()
        {
            InitializeComponent();
            RoundedPicture_Resize(this, EventArgs.Empty);

            // Initialize resources
            background_brush = new SolidBrush(background_color);
            border_pen = new Pen(ControlPaint.Dark(border_color), border_width);
        }

        private void RoundedPicture_Click(object sender, EventArgs e)
        {
            PictureClicked?.Invoke(this, EventArgs.Empty);
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureClicked?.Invoke(this, EventArgs.Empty);
        }

        private void RoundedPicture_Resize(object sender, EventArgs e)
        {
            Radius = Math.Min(Width, Height) / 2.0f;
        }
    }
}
