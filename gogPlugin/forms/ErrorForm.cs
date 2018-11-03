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
    public partial class ErrorForm : Form
    {
        public TaskCompletionSource<bool> ok { get; }

        public ErrorForm(string resultMessage, Dictionary<string,string> errorMessages)
        {
            InitializeComponent();
            this.resultMessage.Text = resultMessage;

            StringBuilder sbErrors = new StringBuilder();
            foreach (string game in errorMessages.Keys)
            {
                sbErrors.Append(game + ": " + errorMessages[game] + "\n\n");
            }
            this.messageBox.Text = sbErrors.ToString();
            ok = new TaskCompletionSource<bool>();
        }

        private void okButton_click(object sender, EventArgs e)
        {
            this.Close();
            this.ok.SetResult(true);
        }

    }
}
