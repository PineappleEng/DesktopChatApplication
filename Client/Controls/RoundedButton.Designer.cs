namespace Client.Controls
{
    partial class RoundedButton
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
            this.Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Button
            // 
            this.Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Button.Location = new System.Drawing.Point(0, 0);
            this.Button.Name = "Button";
            this.Button.Size = new System.Drawing.Size(75, 23);
            this.Button.TabIndex = 0;
            this.Button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Button.UseVisualStyleBackColor = true;
            this.Button.Click += new System.EventHandler(this.Button_Click);
            this.Button.Resize += new System.EventHandler(this.Button_Resize);
            // 
            // RoundedButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.Button);
            this.Name = "RoundedButton";
            this.Size = new System.Drawing.Size(75, 23);
            this.Click += new System.EventHandler(this.RoundedButton_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Button;
    }
}
