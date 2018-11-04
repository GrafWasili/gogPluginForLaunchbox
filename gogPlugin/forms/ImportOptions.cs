using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gogPlugin.forms
{
    public partial class ImportOptions : Form
    {
        public bool skipImported { get; private set; }
        public bool skipByTitle { get; private set; }
        public GogPluginAddGames.Mode mode { get; private set; }
        public string galaxyPath { get; private set; }
        public int platformNo { get; private set; }

        private GogPluginAddGames plugin;
        public TaskCompletionSource<bool> ok { get; }

        public ImportOptions(GogPluginAddGames plugin)
        {
            InitializeComponent();
            this.plugin = plugin;

            Bitmap b = (Bitmap)plugin.IconImage;
            IntPtr pIcon = b.GetHicon();
            Icon i = Icon.FromHandle(pIcon);
            this.Icon = i;

            // use recent options
            this.skipImported = plugin.skipImported;
            this.checkBoxSkipImported.Checked = plugin.skipImported;

            this.skipByTitle = plugin.skipByTitle;
            this.checkBoxSkipTitle.Checked = plugin.skipByTitle;

            if (plugin.galaxyPath == null)
            {
                this.galaxyPathTextBox.Text = this.galaxyPathBowser.RootFolder.ToString();
                this.galaxyPath = this.galaxyPathBowser.RootFolder.ToString();
            } else
            {
                this.galaxyPathTextBox.Text = plugin.galaxyPath;
                this.galaxyPath = plugin.galaxyPath;
            }
           
            this.comboBoxPlatformSelect.Items.AddRange(plugin.platforms.Values.ToArray());
            this.comboBoxPlatformSelect.SelectedIndex = plugin.platformNo;

            this.comboBoxModeSelect.SelectedIndex = (int)plugin.mode;
            this.mode = plugin.mode;

            ok = new TaskCompletionSource<bool>();
        }

        private void checkBoxSkipImported_CheckedChanged(object sender, EventArgs e)
        {
            this.skipImported = this.checkBoxSkipImported.Checked;
        }

         private void checkBoxSkipTitle_CheckedChanged(object sender, EventArgs e)
        {
            this.skipByTitle = this.checkBoxSkipTitle.Checked;
        }        

        private void buttonOk_click(object sender, EventArgs e)
        {
            if (((GogPluginAddGames.Mode)this.mode).Equals(GogPluginAddGames.Mode.startWithGalaxy))
            {
                try
                {
                    if (File.Exists($@"{this.galaxyPath}\GalaxyClient.exe"))
                    {
                        this.ok.SetResult(true);
                    }
                    else
                    {
                        MessageBox.Show("GalaxyClient.exe not found.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + ":\n" + ex.StackTrace);
                }
            } else
            {
                this.ok.SetResult(true);
            }
        }

        private void buttonPathBrowser_click(object sender, EventArgs e)
        {
            DialogResult result = this.galaxyPathBowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.galaxyPathTextBox.Text = this.galaxyPathBowser.SelectedPath.ToString();
                this.galaxyPath = this.galaxyPathBowser.SelectedPath.ToString();
            }
        }

        private void comboBoxModeSelect_valueChanged(object sender, EventArgs e)
        {
            this.mode = (GogPluginAddGames.Mode) this.comboBoxModeSelect.SelectedIndex;
        }

        private void comboBoxPlatformSelect_valueChanged(object sender, EventArgs e)
        {
            this.platformNo = this.comboBoxPlatformSelect.SelectedIndex;
        }

        private void galaxyPathTextBox_textChanged(object sender, EventArgs e)
        {
            this.galaxyPathBowser.SelectedPath = this.galaxyPathTextBox.Text;
            this.galaxyPath = this.galaxyPathTextBox.Text;
        }
    }
}
