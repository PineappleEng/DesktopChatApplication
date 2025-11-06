namespace Client.Controls
{
    partial class MessageBubble
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BubblePanel = new System.Windows.Forms.Panel();
            this.ContentPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.BubblePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BubblePanel
            // 
            this.BubblePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BubblePanel.Controls.Add(this.ContentPanel);
            this.BubblePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BubblePanel.ForeColor = System.Drawing.SystemColors.Control;
            this.BubblePanel.Location = new System.Drawing.Point(0, 0);
            this.BubblePanel.Name = "BubblePanel";
            this.BubblePanel.Padding = new System.Windows.Forms.Padding(14);
            this.BubblePanel.Size = new System.Drawing.Size(305, 100);
            this.BubblePanel.TabIndex = 0;
            // 
            // ContentPanel
            // 
            this.ContentPanel.AutoSize = true;
            this.ContentPanel.BackColor = System.Drawing.Color.Transparent;
            this.ContentPanel.Font = new System.Drawing.Font("Segoe UI Emoji", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContentPanel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ContentPanel.Location = new System.Drawing.Point(3, 3);
            this.ContentPanel.Name = "ContentPanel";
            this.ContentPanel.Padding = new System.Windows.Forms.Padding(1);
            this.ContentPanel.Size = new System.Drawing.Size(299, 70);
            this.ContentPanel.TabIndex = 0;
            // 
            // MessageBubble
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BubblePanel);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "MessageBubble";
            this.Size = new System.Drawing.Size(305, 100);
            this.BubblePanel.ResumeLayout(false);
            this.BubblePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BubblePanel;
        private System.Windows.Forms.FlowLayoutPanel ContentPanel;
    }
}
