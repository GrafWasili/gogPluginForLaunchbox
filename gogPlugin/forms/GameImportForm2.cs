using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gogPlugin.forms
{
    public partial class GameImportForm2 : Form
    {
        private GogPluginAddGames plugin;

        public GameImportForm2(GogPluginAddGames plugin)
        {
            InitializeComponent();
            this.plugin = plugin;
            this.FormClosed += plugin.ImportFormClosed;
            this.Shown += setDownloadValues;
            this.importGamesBind.DataSource = plugin.gamesFound;
        }

        private void setDownloadValues(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.importGamesView.Rows)
            {
                DataGridViewComboBoxCell comboBox = (DataGridViewComboBoxCell)row.Cells["ImportGamesViewDownloadColumn"];
                GogPluginAddGames.GameImport gi = ((GogPluginAddGames.GameImport)row.DataBoundItem);
                comboBox.DataSource = gi.downloads;         
            }
        }

        private void importButton_click(object sender, EventArgs e)
        {
            this.Close();
            plugin.startImport();
        }
    }
}
