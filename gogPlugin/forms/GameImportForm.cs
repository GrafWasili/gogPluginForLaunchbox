using System;
using System.Drawing;
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

            Bitmap b = (Bitmap)plugin.IconImage;
            IntPtr pIcon = b.GetHicon();
            Icon i = Icon.FromHandle(pIcon);
            this.Icon = i;
        }

        private void importButton_click(object sender, EventArgs e)
        {
            this.Close();
            plugin.startImport();
        }
    }
}
