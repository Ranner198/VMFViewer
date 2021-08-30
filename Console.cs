using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VMF_Viewer
{
    public partial class Console : Form
    {
        public static Console instance;
        public BackgroundWorker UIWorker;
        public Console()
        {
            InitializeComponent();
            UIWorker = new BackgroundWorker();
            UIWorker.DoWork += new DoWorkEventHandler(UpdateUIFromThread);
            instance = this;
        }

        public void Write(params string[] s)
        {
            foreach (string str in s)
            {
                try
                {
                    this.ConsoleOutput.Invoke((Action)(() => this.ConsoleOutput.AppendText(str + Environment.NewLine)));
                }
                catch (Exception e) { /* Dont do anything */ }
            }
        }

        private void UpdateUIFromThread(object sender, DoWorkEventArgs e)
        {
            this.Write(e.Argument as string);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            this.ConsoleOutput.Text = String.Empty;
        }

        private void SaveLog_Click(object sender, EventArgs e)
        {
            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text File|*.txt";
            saveFileDialog.Title = "Save Logs to File";
            saveFileDialog.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                byte[] logs = Encoding.ASCII.GetBytes(this.ConsoleOutput.Text);
                fs.Write(logs, 0, logs.Length);

                fs.Close();
            }
        }
    }
}
