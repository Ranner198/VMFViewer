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
            this.label3 = new System.Windows.Forms.Label();
            this.DestinationURNDataGridView = new System.Windows.Forms.DataGridView();
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
            this.DestinationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DestinationURN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.MulticastGroupAddressInputField = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DestinationURNDataGridView)).BeginInit();
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
            this.groupBox1.Location = new System.Drawing.Point(27, 123);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(761, 374);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "VMF Message Data";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(42, 243);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "CC Destination URN";
            // 
            // DestinationURNDataGridView
            // 
            this.DestinationURNDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.DestinationURNDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DestinationURNDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DestinationName,
            this.DestinationURN});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DestinationURNDataGridView.DefaultCellStyle = dataGridViewCellStyle1;
            this.DestinationURNDataGridView.Location = new System.Drawing.Point(46, 71);
            this.DestinationURNDataGridView.Name = "DestinationURNDataGridView";
            this.DestinationURNDataGridView.RowHeadersWidth = 40;
            this.DestinationURNDataGridView.Size = new System.Drawing.Size(641, 150);
            this.DestinationURNDataGridView.TabIndex = 11;
            // 
            // StopButton
            // 
            this.StopButton.BackColor = System.Drawing.Color.Red;
            this.StopButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StopButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.StopButton.Location = new System.Drawing.Point(503, 325);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(184, 36);
            this.StopButton.TabIndex = 10;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = false;
            // 
            // StartButton
            // 
            this.StartButton.BackColor = System.Drawing.Color.LimeGreen;
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.StartButton.Location = new System.Drawing.Point(503, 276);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(184, 36);
            this.StartButton.TabIndex = 9;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(77, 328);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 17);
            this.label7.TabIndex = 8;
            this.label7.Text = "Longitude:";
            // 
            // LongitudeInputBox
            // 
            this.LongitudeInputBox.Location = new System.Drawing.Point(167, 325);
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
            this.label6.Location = new System.Drawing.Point(77, 300);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 17);
            this.label6.TabIndex = 6;
            this.label6.Text = "Latitude:  ";
            // 
            // LatitudeInputBox
            // 
            this.LatitudeInputBox.Location = new System.Drawing.Point(167, 297);
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
            this.label5.Location = new System.Drawing.Point(42, 276);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "Location";
            // 
            // OriginatorURNInputBox
            // 
            this.OriginatorURNInputBox.Location = new System.Drawing.Point(182, 32);
            this.OriginatorURNInputBox.Name = "OriginatorURNInputBox";
            this.OriginatorURNInputBox.Size = new System.Drawing.Size(142, 22);
            this.OriginatorURNInputBox.TabIndex = 3;
            this.OriginatorURNInputBox.Text = "123456789";
            this.OriginatorURNInputBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(43, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Originator URN:";
            // 
            // SendTimeInputBox
            // 
            this.SendTimeInputBox.Location = new System.Drawing.Point(609, 31);
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
            this.SendTimeLabel.Location = new System.Drawing.Point(434, 34);
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
            // DestinationName
            // 
            this.DestinationName.HeaderText = "Destination Name";
            this.DestinationName.Name = "DestinationName";
            this.DestinationName.Width = 140;
            // 
            // DestinationURN
            // 
            this.DestinationURN.HeaderText = "DestinationURN";
            this.DestinationURN.Name = "DestinationURN";
            this.DestinationURN.Width = 130;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(200, 241);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(142, 22);
            this.textBox1.TabIndex = 14;
            this.textBox1.Text = "123456789";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // MulticastGroupAddressInputField
            // 
            this.MulticastGroupAddressInputField.Location = new System.Drawing.Point(586, 238);
            this.MulticastGroupAddressInputField.Name = "MulticastGroupAddressInputField";
            this.MulticastGroupAddressInputField.Size = new System.Drawing.Size(101, 22);
            this.MulticastGroupAddressInputField.TabIndex = 16;
            this.MulticastGroupAddressInputField.Text = "254.22.15.24";
            this.MulticastGroupAddressInputField.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(391, 240);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(191, 17);
            this.label8.TabIndex = 15;
            this.label8.Text = "Multicast Group Address:";
            // 
            // Controller
            // 
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Controller";
            this.Text = "VMF Viewer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DestinationURNDataGridView)).EndInit();
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
        private System.Windows.Forms.DataGridViewTextBoxColumn DestinationName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DestinationURN;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox MulticastGroupAddressInputField;
        private System.Windows.Forms.Label label8;
    }
}

