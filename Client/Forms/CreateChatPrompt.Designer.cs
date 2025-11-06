namespace Client.Forms
{
    partial class CreateChatPrompt
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ChatNameLabel = new System.Windows.Forms.Label();
            this.ChatNameField = new System.Windows.Forms.TextBox();
            this.CheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.Create = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ChatNameLabel
            // 
            this.ChatNameLabel.AutoSize = true;
            this.ChatNameLabel.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChatNameLabel.Location = new System.Drawing.Point(12, 15);
            this.ChatNameLabel.Name = "ChatNameLabel";
            this.ChatNameLabel.Size = new System.Drawing.Size(101, 21);
            this.ChatNameLabel.TabIndex = 0;
            this.ChatNameLabel.Text = "Chat Name:";
            // 
            // ChatNameField
            // 
            this.ChatNameField.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(202)))), ((int)(((byte)(230)))));
            this.ChatNameField.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChatNameField.Location = new System.Drawing.Point(120, 12);
            this.ChatNameField.Name = "ChatNameField";
            this.ChatNameField.Size = new System.Drawing.Size(160, 29);
            this.ChatNameField.TabIndex = 1;
            // 
            // CheckedListBox
            // 
            this.CheckedListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(202)))), ((int)(((byte)(230)))));
            this.CheckedListBox.FormattingEnabled = true;
            this.CheckedListBox.Location = new System.Drawing.Point(12, 50);
            this.CheckedListBox.Name = "CheckedListBox";
            this.CheckedListBox.Size = new System.Drawing.Size(270, 169);
            this.CheckedListBox.TabIndex = 2;
            // 
            // CreateButton
            // 
            this.Create.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Create.Font = new System.Drawing.Font("Segoe UI Emoji", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Create.Location = new System.Drawing.Point(12, 229);
            this.Create.Name = "CreateButton";
            this.Create.Size = new System.Drawing.Size(116, 30);
            this.Create.TabIndex = 3;
            this.Create.Text = "Create";
            this.Create.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Font = new System.Drawing.Font("Segoe UI Emoji", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel.Location = new System.Drawing.Point(166, 229);
            this.Cancel.Name = "CancelButton";
            this.Cancel.Size = new System.Drawing.Size(116, 30);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // CreateChatPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(158)))), ((int)(((byte)(188)))));
            this.ClientSize = new System.Drawing.Size(294, 271);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Create);
            this.Controls.Add(this.CheckedListBox);
            this.Controls.Add(this.ChatNameField);
            this.Controls.Add(this.ChatNameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(310, 310);
            this.MinimumSize = new System.Drawing.Size(310, 310);
            this.Name = "CreateChatPrompt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CreateChatPrompt";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ChatNameLabel;
        private System.Windows.Forms.TextBox ChatNameField;
        private System.Windows.Forms.CheckedListBox CheckedListBox;
        private System.Windows.Forms.Button Create;
        private System.Windows.Forms.Button Cancel;
    }
}