using System;

namespace gogPlugin.forms
{
    partial class ImportOptions
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
            this.checkBoxSkipImported = new System.Windows.Forms.CheckBox();
            this.checkBoxSkipTitle = new System.Windows.Forms.CheckBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.comboBoxModeSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.galaxyPathTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonPathBrowser = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxPlatformSelect = new System.Windows.Forms.ComboBox();
            this.galaxyPathBowser = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // checkBoxSkipImported
            // 
            this.checkBoxSkipImported.AutoSize = true;
            this.checkBoxSkipImported.Checked = true;
            this.checkBoxSkipImported.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSkipImported.Location = new System.Drawing.Point(30, 25);
            this.checkBoxSkipImported.Name = "checkBoxSkipImported";
            this.checkBoxSkipImported.Size = new System.Drawing.Size(207, 17);
            this.checkBoxSkipImported.TabIndex = 0;
            this.checkBoxSkipImported.Text = "Skip existing games imported by plugin";
            this.checkBoxSkipImported.UseVisualStyleBackColor = true;
            this.checkBoxSkipImported.CheckedChanged += new System.EventHandler(this.checkBoxSkipImported_CheckedChanged);
            // 
            // checkBoxSkipTitle
            // 
            this.checkBoxSkipTitle.AutoSize = true;
            this.checkBoxSkipTitle.Location = new System.Drawing.Point(30, 48);
            this.checkBoxSkipTitle.Name = "checkBoxSkipTitle";
            this.checkBoxSkipTitle.Size = new System.Drawing.Size(188, 17);
            this.checkBoxSkipTitle.TabIndex = 1;
            this.checkBoxSkipTitle.Text = "Skip existing games with same title";
            this.checkBoxSkipTitle.UseVisualStyleBackColor = true;
            this.checkBoxSkipTitle.CheckedChanged += new System.EventHandler(this.checkBoxSkipTitle_CheckedChanged);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(107, 233);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_click);
            // 
            // comboBoxModeSelect
            // 
            this.comboBoxModeSelect.FormattingEnabled = true;
            this.comboBoxModeSelect.Items.AddRange(new object[] {
            "Start with Galaxy",
            "Download Installer"});
            this.comboBoxModeSelect.Location = new System.Drawing.Point(30, 145);
            this.comboBoxModeSelect.Name = "comboBoxModeSelect";
            this.comboBoxModeSelect.Size = new System.Drawing.Size(207, 21);
            this.comboBoxModeSelect.TabIndex = 3;
            this.comboBoxModeSelect.SelectedValueChanged += new System.EventHandler(this.comboBoxModeSelect_valueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Mode";
            // 
            // galaxyPathTextBox
            // 
            this.galaxyPathTextBox.Location = new System.Drawing.Point(30, 193);
            this.galaxyPathTextBox.Name = "galaxyPathTextBox";
            this.galaxyPathTextBox.Size = new System.Drawing.Size(180, 20);
            this.galaxyPathTextBox.TabIndex = 5;
            this.galaxyPathTextBox.TextChanged += new System.EventHandler(this.galaxyPathTextBox_textChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "gogGalaxy Path";
            // 
            // buttonPathBrowser
            // 
            this.buttonPathBrowser.Location = new System.Drawing.Point(212, 192);
            this.buttonPathBrowser.Name = "buttonPathBrowser";
            this.buttonPathBrowser.Size = new System.Drawing.Size(25, 22);
            this.buttonPathBrowser.TabIndex = 7;
            this.buttonPathBrowser.Text = "...";
            this.buttonPathBrowser.UseVisualStyleBackColor = true;
            this.buttonPathBrowser.Click += new System.EventHandler(this.buttonPathBrowser_click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Platform";
            // 
            // comboBoxPlatformSelect
            // 
            this.comboBoxPlatformSelect.FormattingEnabled = true;
            this.comboBoxPlatformSelect.Location = new System.Drawing.Point(30, 96);
            this.comboBoxPlatformSelect.Name = "comboBoxPlatformSelect";
            this.comboBoxPlatformSelect.Size = new System.Drawing.Size(207, 21);
            this.comboBoxPlatformSelect.TabIndex = 8;
            this.comboBoxPlatformSelect.SelectedValueChanged += new System.EventHandler(this.comboBoxPlatformSelect_valueChanged);
            // 
            // ImportOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 268);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxPlatformSelect);
            this.Controls.Add(this.buttonPathBrowser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.galaxyPathTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxModeSelect);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.checkBoxSkipTitle);
            this.Controls.Add(this.checkBoxSkipImported);
            this.Name = "ImportOptions";
            this.Text = "Import Games from gog.com";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxSkipImported;
        private System.Windows.Forms.CheckBox checkBoxSkipTitle;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.ComboBox comboBoxModeSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox galaxyPathTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonPathBrowser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxPlatformSelect;
        private System.Windows.Forms.FolderBrowserDialog galaxyPathBowser;
    }
}