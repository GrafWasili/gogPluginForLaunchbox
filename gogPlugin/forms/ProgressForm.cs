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
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();
            this.FormClosing += onClose;
        }

        public async Task ShowProgress(Action<Progress<int>> func, string msg)
        {
            this.progressBar1.Maximum = 100;
            this.progressBar1.Step = 1;
            this.Text = msg;

            var progress = new Progress<int>(v =>
            {
                progressBar1.Value = v;
            });

            await Task.Run(() => func(progress));  
        }

        private void onClose(object sender, EventArgs e)
        {
            //TODO
        }
    }
}
