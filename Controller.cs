using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VMF_Viewer
{
    public partial class Controller : Form
    {
        #region Variables
        public enum OutputType { K051, K0519, K071 };
        public OutputType outputType = OutputType.K051;
        public enum Mode { Sender, Reciever };
        public Mode mode = Mode.Sender;
        public VMF vmf;        
        #endregion        
        public Controller()
        {
            InitializeComponent();
            // Initalize Varibales
            EnableStartButton(true);
            this.OutputTypeDropdown.SelectedIndex = 0;
            this.ModeDropdown.SelectedIndex = 0;
            this.SendTimeInputBox.Text = "180";
            this.DestinationURNDataGridView.CellEndEdit += ValidateDesinationURNInput;

            vmf = new VMF();         
        }

        public void OnExit(object sender, FormClosingEventArgs e)
        {
            // Stop VMF
            if (Multicast.instance != null)
            {
                Multicast.instance.StopMulticast();
            }

            VMF.instance = null;
        }

        #region Data Bindings
        private void ValidateDesinationURNInput(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            var name = this.DestinationURNDataGridView.Rows[rowIndex].Cells[0];
            var urn = this.DestinationURNDataGridView.Rows[rowIndex].Cells[1];
            if (name != null && urn.Value != null && urn.Value.ToString().Length > 0)
            {
                // Cells are validated
                Console.instance?.Write("Cells Written");
                this.OriginatorURNInputBox.Text = urn.Value.ToString();
            }
        }

        private void OutputType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.outputType = (OutputType)Enum.ToObject(typeof(OutputType), this.OutputTypeDropdown.SelectedIndex);
        }

        private void ModeDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.mode = (Mode)Enum.ToObject(typeof(Mode), this.ModeDropdown.SelectedIndex);
        }
        #endregion

        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #region GUI Buttons
        private void StartButton_Click(object sender, EventArgs e)
        {
            if (Multicast.instance == null)
            {
                new Multicast("192.168.1.66", this.MulticastGroupAddressInputField.Text, this.PortNumberInputField.Text, this.mode);
            }
            if (this.mode == Mode.Reciever)
            {
                if (Multicast.instance != null)
                {
                    Multicast.instance.StartMulticast();
                    EnableStartButton(false);
                }
            }
            else
            {
                List<string> destinationURNs = new List<string>();

                foreach (DataGridViewRow row in this.DestinationURNDataGridView.Rows)
                {
                    if (!row.IsNewRow)
                        destinationURNs.Add(row.Cells[1].Value.ToString());
                }
                Console.instance?.Write(destinationURNs.ToString());
                Multicast.instance?.SendVMFMessage(vmf.BuildVMF(this.OriginatorURNInputBox.Text, destinationURNs));
                vmf.DeleteVMFMessage();
            }
        }

        private void EnableStartButton(bool b)
        {
            this.StartButton.Enabled = b;
            this.StartButton.BackColor = b ? Color.LimeGreen : Color.Gray;
            this.StopButton.Enabled = !b;
            this.StopButton.BackColor = !b ? Color.Red : Color.Gray;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            // Stop VMF
            if (Multicast.instance != null)
            {
                Multicast.instance.StopMulticast();
                EnableStartButton(true);             
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void consoleToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (Console.instance == null)
            {
                Console console = new Console();
                console.Show();
            }
            else
            {
                Console.instance.Show();
            }
        }

        private void DestinationURNDataGridView_RowNumber(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateCellNumbers();
        }

        private void DestinationURNDataGridView_RowNumber(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateCellNumbers();
        }

        private void UpdateCellNumbers()
        {
            for (int i = 0; i < this.DestinationURNDataGridView.Rows.Count; i++)
            {
                DataGridViewRow row = this.DestinationURNDataGridView.Rows[i];
                long rowNumber = i + 1;
                row.HeaderCell.Value = rowNumber.ToString();
                row.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = (DataGridViewRow)this.DestinationURNDataGridView.Rows[0].Clone();
            this.DestinationURNDataGridView.Rows.Add(row);
            long newNo = this.DestinationURNDataGridView.Rows.Count;
            row.HeaderCell.Value = newNo.ToString();
            row.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            List<int> rowsToDelete = new List<int>();
            foreach (DataGridViewCell cell in this.DestinationURNDataGridView.SelectedCells)
            {
                if (!rowsToDelete.Contains(cell.RowIndex))
                    rowsToDelete.Add(cell.RowIndex);
            }

            for (int i = rowsToDelete.Count-1; i >= 0; i--)
            {
                if (!this.DestinationURNDataGridView.Rows[rowsToDelete[i]].IsNewRow)
                    this.DestinationURNDataGridView.Rows.RemoveAt(rowsToDelete[i]);
            }
                
            this.DestinationURNDataGridView.Refresh();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            this.DestinationURNDataGridView.Rows.Clear();
            this.DestinationURNDataGridView.Refresh();
        }

        #endregion
    }
}
