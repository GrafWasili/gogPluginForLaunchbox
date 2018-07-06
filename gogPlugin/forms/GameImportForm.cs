using System;
using System.Windows.Forms;

namespace gogPlugin.forms
{
    public partial class GameImportForm : Form
    {
        private GogPluginAddGames plugin;

        public GameImportForm(GogPluginAddGames plugin)
        {
            InitializeComponent();
            this.plugin = plugin;
            this.FormClosed += plugin.ImportFormClosed; 
            this.importGamesBind.DataSource = plugin.gamesFound;
        }

        private void importButton_click(object sender, EventArgs e)
        {
            this.Close();
            plugin.startImport();
        }
    }
}
