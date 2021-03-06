﻿using System.Collections.Generic;

namespace gogPlugin.forms
{
    partial class GameImportForm2
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
            this.components = new System.ComponentModel.Container();
            this.importGamesView = new System.Windows.Forms.DataGridView();
            this.ImportGamesViewTitleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImportGamesViewImportColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ImportGamesViewDownloadColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.gameDownloadBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.importGamesBind = new System.Windows.Forms.BindingSource(this.components);
            this.buttonImport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.importGamesView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameDownloadBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.importGamesBind)).BeginInit();
            this.SuspendLayout();
            // 
            // importGamesView
            // 
            this.importGamesView.AllowUserToAddRows = false;
            this.importGamesView.AutoGenerateColumns = false;
            this.importGamesView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.importGamesView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ImportGamesViewTitleColumn,
            this.ImportGamesViewImportColumn,
            this.ImportGamesViewDownloadColumn});
            this.importGamesView.DataSource = this.importGamesBind;
            this.importGamesView.Dock = System.Windows.Forms.DockStyle.Top;
            this.importGamesView.Location = new System.Drawing.Point(0, 0);
            this.importGamesView.Name = "importGamesView";
            this.importGamesView.Size = new System.Drawing.Size(800, 400);
            this.importGamesView.TabIndex = 0;
            // 
            // ImportGamesViewTitleColumn
            // 
            this.ImportGamesViewTitleColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ImportGamesViewTitleColumn.DataPropertyName = "title";
            this.ImportGamesViewTitleColumn.FillWeight = 60F;
            this.ImportGamesViewTitleColumn.HeaderText = "Title";
            this.ImportGamesViewTitleColumn.MaxInputLength = 200;
            this.ImportGamesViewTitleColumn.MinimumWidth = 200;
            this.ImportGamesViewTitleColumn.Name = "ImportGamesViewTitleColumn";
            this.ImportGamesViewTitleColumn.Width = 350;
            // 
            // ImportGamesViewImportColumn
            // 
            this.ImportGamesViewImportColumn.DataPropertyName = "import";
            this.ImportGamesViewImportColumn.HeaderText = "Import";
            this.ImportGamesViewImportColumn.MinimumWidth = 60;
            this.ImportGamesViewImportColumn.Name = "ImportGamesViewImportColumn";
            this.ImportGamesViewImportColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ImportGamesViewImportColumn.Width = 60;
            // 
            // ImportGamesViewDownloadColumn
            // 
            this.ImportGamesViewDownloadColumn.DataPropertyName = "selectedDownload";
            this.ImportGamesViewDownloadColumn.DataSource = this.gameDownloadBindingSource;
            this.ImportGamesViewDownloadColumn.DisplayMember = "display";
            this.ImportGamesViewDownloadColumn.HeaderText = "Download";
            this.ImportGamesViewDownloadColumn.MinimumWidth = 100;
            this.ImportGamesViewDownloadColumn.Name = "ImportGamesViewDownloadColumn";
            this.ImportGamesViewDownloadColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ImportGamesViewDownloadColumn.ValueMember = "self";
            this.ImportGamesViewDownloadColumn.Width = 300;
            // 
            // gameDownloadBindingSource
            // 
            this.gameDownloadBindingSource.AllowNew = false;
            this.gameDownloadBindingSource.DataSource = typeof(gogPlugin.GogPluginAddGames.GameDownload);
            // 
            // importGamesBind
            // 
            this.importGamesBind.AllowNew = false;
            this.importGamesBind.DataSource = typeof(gogPlugin.GogPluginAddGames.GameImport);
            this.importGamesBind.Sort = "title";
            // 
            // buttonImport
            // 
            this.buttonImport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonImport.Location = new System.Drawing.Point(31, 415);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(75, 23);
            this.buttonImport.TabIndex = 1;
            this.buttonImport.Text = "Start Import";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.importButton_click);
            // 
            // GameImportForm2
            // 
            this.AcceptButton = this.buttonImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.importGamesView);
            this.Name = "GameImportForm2";
            this.Text = "Import Games from gog.com";
            ((System.ComponentModel.ISupportInitialize)(this.importGamesView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gameDownloadBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.importGamesBind)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView importGamesView;
        private System.Windows.Forms.BindingSource importGamesBind;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.BindingSource gameDownloadBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImportGamesViewTitleColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ImportGamesViewImportColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn ImportGamesViewDownloadColumn;
    }
}