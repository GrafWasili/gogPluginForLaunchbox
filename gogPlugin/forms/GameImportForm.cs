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
