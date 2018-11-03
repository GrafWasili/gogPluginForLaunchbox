namespace gogPlugin.forms
{
    partial class ErrorForm
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
            this.messageBox = new System.Windows.Forms.RichTextBox();
            this.resultMessage = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // messageBox
            // 
            this.messageBox.Location = new System.Drawing.Point(29, 86);
            this.messageBox.Name = "messageBox";
            this.messageBox.ReadOnly = true;
            this.messageBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.messageBox.Size = new System.Drawing.Size(451, 348);
            this.messageBox.TabIndex = 0;
            this.messageBox.Text = "";
            // 
            // resultMessage
            // 
            this.resultMessage.AutoSize = true;
            this.resultMessage.Location = new System.Drawing.Point(32, 25);
            this.resultMessage.Name = "resultMessage";
            this.resultMessage.Size = new System.Drawing.Size(0, 13);
            this.resultMessage.TabIndex = 1;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(395, 453);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += this.okButton_click;
            // 
            // ErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 497);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.resultMessage);
            this.Controls.Add(this.messageBox);
            this.Name = "ErrorForm";
            this.Text = "Scan Results";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox messageBox;
        private System.Windows.Forms.Label resultMessage;
        private System.Windows.Forms.Button okButton;
    }
}