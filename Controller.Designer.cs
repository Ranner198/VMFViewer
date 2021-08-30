namespace VMF_Viewer
{
    partial class Controller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Controller));
            this.OutputTypeDropdown = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PortNumberInputField = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.MulticastGroupAddressInputField = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DestinationURNDataGridView = new System.Windows.Forms.DataGridView();
            this.DestinationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DestinationURN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StopButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.LongitudeInputBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.LatitudeInputBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.OriginatorURNInputBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SendTimeInputBox = new System.Windows.Forms.TextBox();
            this.SendTimeLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ModeDropdown = new System.Windows.Forms.ComboBox();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DestinationURNDataGridView)).BeginInit();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // OutputTypeDropdown
            // 
            this.OutputTypeDropdown.FormattingEnabled = true;
            this.OutputTypeDropdown.Items.AddRange(new object[] {
            "K05.1",
            "K05.19",
            "K07.1"});
            this.OutputTypeDropdown.Location = new System.Drawing.Point(126, 28);
            this.OutputTypeDropdown.Name = "OutputTypeDropdown";
            this.OutputTypeDropdown.Size = new System.Drawing.Size(121, 21);
            this.OutputTypeDropdown.TabIndex = 0;
            this.OutputTypeDropdown.SelectedIndexChanged += new System.EventHandler(this.OutputType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Location = new System.Drawing.Point(24, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Output Type:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.DimGray;
            this.groupBox1.Controls.Add(this.PortNumberInputField);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.AddButton);
            this.groupBox1.Controls.Add(this.RemoveButton);
            this.groupBox1.Controls.Add(this.ClearButton);
            this.groupBox1.Controls.Add(this.MulticastGroupAddressInputField);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.DestinationURNDataGridView);
            this.groupBox1.Controls.Add(this.StopButton);
            this.groupBox1.Controls.Add(this.StartButton);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.LongitudeInputBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.LatitudeInputBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.OriginatorURNInputBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.SendTimeInputBox);
            this.groupBox1.Controls.Add(this.SendTimeLabel);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Location = new System.Drawing.Point(27, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(761, 410);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "VMF Message Data";
            // 
            // PortNumberInputField
            // 
            this.PortNumberInputField.Location = new System.Drawing.Point(627, 275);
            this.PortNumberInputField.Name = "PortNumberInputField";
            this.PortNumberInputField.Size = new System.Drawing.Size(101, 22);
            this.PortNumberInputField.TabIndex = 21;
            this.PortNumberInputField.Text = "1583";
            this.PortNumberInputField.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(517, 278);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 17);
            this.label9.TabIndex = 20;
            this.label9.Text = "Port Number:";
            // 
            // AddButton
            // 
            this.AddButton.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.AddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddButton.Location = new System.Drawing.Point(537, 216);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(48, 29);
            this.AddButton.TabIndex = 19;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = false;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.BackColor = System.Drawing.Color.DarkRed;
            this.RemoveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoveButton.Location = new System.Drawing.Point(591, 216);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(75, 29);
            this.RemoveButton.TabIndex = 18;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = false;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.BackColor = System.Drawing.Color.Orange;
            this.ClearButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearButton.Location = new System.Drawing.Point(670, 216);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(59, 29);
            this.ClearButton.TabIndex = 17;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = false;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // MulticastGroupAddressInputField
            // 
            this.MulticastGroupAddressInputField.Location = new System.Drawing.Point(627, 252);
            this.MulticastGroupAddressInputField.Name = "MulticastGroupAddressInputField";
            this.MulticastGroupAddressInputField.Size = new System.Drawing.Size(101, 22);
            this.MulticastGroupAddressInputField.TabIndex = 16;
            this.MulticastGroupAddressInputField.Text = "224.1.2.3";
            this.MulticastGroupAddressInputField.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(432, 254);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(191, 17);
            this.label8.TabIndex = 15;
            this.label8.Text = "Multicast Group Address:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(182, 271);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(142, 22);
            this.textBox1.TabIndex = 14;
            this.textBox1.Text = "123456789";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 273);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "CC Destination URN";
            // 
            // DestinationURNDataGridView
            // 
            this.DestinationURNDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DestinationURNDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DestinationURNDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DestinationURNDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DestinationName,
            this.DestinationURN});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DestinationURNDataGridView.DefaultCellStyle = dataGridViewCellStyle1;
            this.DestinationURNDataGridView.Location = new System.Drawing.Point(27, 66);
            this.DestinationURNDataGridView.Name = "DestinationURNDataGridView";
            this.DestinationURNDataGridView.RowHeadersWidth = 60;
            this.DestinationURNDataGridView.Size = new System.Drawing.Size(701, 150);
            this.DestinationURNDataGridView.TabIndex = 11;
            this.DestinationURNDataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.DestinationURNDataGridView_RowNumber);
            this.DestinationURNDataGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.DestinationURNDataGridView_RowNumber);
            // 
            // DestinationName
            // 
            this.DestinationName.HeaderText = "Destination Name";
            this.DestinationName.Name = "DestinationName";
            // 
            // DestinationURN
            // 
            this.DestinationURN.HeaderText = "DestinationURN";
            this.DestinationURN.Name = "DestinationURN";
            // 
            // StopButton
            // 
            this.StopButton.BackColor = System.Drawing.Color.Red;
            this.StopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StopButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.StopButton.Location = new System.Drawing.Point(550, 357);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(184, 36);
            this.StopButton.TabIndex = 10;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = false;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // StartButton
            // 
            this.StartButton.BackColor = System.Drawing.Color.LimeGreen;
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.StartButton.Location = new System.Drawing.Point(550, 308);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(184, 36);
            this.StartButton.TabIndex = 9;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = false;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(59, 358);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 17);
            this.label7.TabIndex = 8;
            this.label7.Text = "Longitude:";
            // 
            // LongitudeInputBox
            // 
            this.LongitudeInputBox.Location = new System.Drawing.Point(149, 355);
            this.LongitudeInputBox.Name = "LongitudeInputBox";
            this.LongitudeInputBox.Size = new System.Drawing.Size(108, 22);
            this.LongitudeInputBox.TabIndex = 7;
            this.LongitudeInputBox.Text = "-86.5861";
            this.LongitudeInputBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(59, 330);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 17);
            this.label6.TabIndex = 6;
            this.label6.Text = "Latitude:  ";
            // 
            // LatitudeInputBox
            // 
            this.LatitudeInputBox.Location = new System.Drawing.Point(149, 327);
            this.LatitudeInputBox.Name = "LatitudeInputBox";
            this.LatitudeInputBox.Size = new System.Drawing.Size(108, 22);
            this.LatitudeInputBox.TabIndex = 5;
            this.LatitudeInputBox.Text = "34.7304";
            this.LatitudeInputBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(24, 306);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "Location";
            // 
            // OriginatorURNInputBox
            // 
            this.OriginatorURNInputBox.Location = new System.Drawing.Point(159, 35);
            this.OriginatorURNInputBox.Name = "OriginatorURNInputBox";
            this.OriginatorURNInputBox.Size = new System.Drawing.Size(142, 22);
            this.OriginatorURNInputBox.TabIndex = 3;
            this.OriginatorURNInputBox.Text = "1600205";
            this.OriginatorURNInputBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(20, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Originator URN:";
            // 
            // SendTimeInputBox
            // 
            this.SendTimeInputBox.Location = new System.Drawing.Point(651, 36);
            this.SendTimeInputBox.Name = "SendTimeInputBox";
            this.SendTimeInputBox.Size = new System.Drawing.Size(78, 22);
            this.SendTimeInputBox.TabIndex = 1;
            this.SendTimeInputBox.Text = "180";
            this.SendTimeInputBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // SendTimeLabel
            // 
            this.SendTimeLabel.AutoSize = true;
            this.SendTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SendTimeLabel.Location = new System.Drawing.Point(476, 39);
            this.SendTimeLabel.Name = "SendTimeLabel";
            this.SendTimeLabel.Size = new System.Drawing.Size(169, 17);
            this.SendTimeLabel.TabIndex = 0;
            this.SendTimeLabel.Text = "Send Time (Seconds):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Location = new System.Drawing.Point(69, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Mode:";
            // 
            // ModeDropdown
            // 
            this.ModeDropdown.FormattingEnabled = true;
            this.ModeDropdown.Items.AddRange(new object[] {
            "Sender",
            "Viewer"});
            this.ModeDropdown.Location = new System.Drawing.Point(126, 60);
            this.ModeDropdown.Name = "ModeDropdown";
            this.ModeDropdown.Size = new System.Drawing.Size(121, 21);
            this.ModeDropdown.TabIndex = 3;
            this.ModeDropdown.SelectedIndexChanged += new System.EventHandler(this.ModeDropdown_SelectedIndexChanged);
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(812, 24);
            this.MenuStrip.TabIndex = 5;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consoleToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // consoleToolStripMenuItem
            // 
            this.consoleToolStripMenuItem.Name = "consoleToolStripMenuItem";
            this.consoleToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.consoleToolStripMenuItem.Text = "Console";
            this.consoleToolStripMenuItem.Click += new System.EventHandler(this.consoleToolStripMenuItem_Click_1);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // Controller
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(812, 509);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ModeDropdown);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OutputTypeDropdown);
            this.Controls.Add(this.MenuStrip);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "Controller";
            this.Text = "VMF Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnExit);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DestinationURNDataGridView)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox OutputTypeDropdown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ModeDropdown;
        private System.Windows.Forms.TextBox LatitudeInputBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox OriginatorURNInputBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SendTimeInputBox;
        private System.Windows.Forms.Label SendTimeLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox LongitudeInputBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView DestinationURNDataGridView;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox MulticastGroupAddressInputField;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn DestinationName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DestinationURN;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.TextBox PortNumberInputField;
        private System.Windows.Forms.Label label9;
    }
}

