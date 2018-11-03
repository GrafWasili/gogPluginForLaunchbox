using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gogPlugin.forms
{
    public partial class ProgressForm : Form
    {
        public ProgressForm(String title)
        {
            InitializeComponent();
            this.FormClosing += onClose;
            this.Text = title;
        }

        public async Task ShowProgress(Action<Progress<int>, Progress<String>> func)
        {
            this.progressBar1.Maximum = 100;
            this.progressBar1.Step = 1;

            var progressBar = new Progress<int>(i  =>
            {
                progressBar1.Value = i;
            });

            var progressMsg = new Progress<String>(s =>
            {
                this.infoText.Text = s;
            });

            await Task.Run(() => func(progressBar, progressMsg));  
        }

        private void onClose(object sender, EventArgs e)
        {
            //TODO
        }
    }
}
