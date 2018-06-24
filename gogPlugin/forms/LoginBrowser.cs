using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CefSharp;
using CefSharp.WinForms;

namespace gogPlugin.forms
{
    public partial class LoginBrowser : Form
    {
        public LoginBrowser(GogPluginAddGames plugin)
        {
            InitializeComponent();
            this.plugin = plugin;
            this.FormClosing += this.ShutdownBrowser;
            this.FormClosed += plugin.BrowserClosed;
            InitBrowser();
        }

        private ChromiumWebBrowser browser;
        private GogPluginAddGames plugin;
        private string loginCode;

        public void InitBrowser()
        { 
            browser = new ChromiumWebBrowser(address: "https://auth.gog.com/auth?client_id=46899977096215655&redirect_uri=https%3A%2F%2Fembed.gog.com%2Fon_login_success%3Forigin%3Dclient&response_type=code&layout=client2");
            this.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;

            browser.AddressChanged += this.CheckForLoginCode;
        }

        private void ShutdownBrowser(object sender, FormClosingEventArgs e)
        {
            browser.Dispose();
        }

        private void CheckForLoginCode(object sender, AddressChangedEventArgs e)
        {
            if (e.Address.Contains("code=")) {
                loginCode = e.Address.Split(new [] { "code=" }, StringSplitOptions.None)[1];                
                browser.AddressChanged -= this.CheckForLoginCode;
                this.FormClosed += (sender2, e2) => plugin.loginSuccess(sender2, e2, loginCode);
                this.Close();
            }
        }
    }
}
