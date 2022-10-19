namespace VarAQT
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.connectToVaraButton = new System.Windows.Forms.Button();
            this.fromModemTextBox = new System.Windows.Forms.TextBox();
            this.disconnectFromVaraButton = new System.Windows.Forms.Button();
            this.sendBeaconTimerButton = new System.Windows.Forms.Button();
            this.sendBeaconNowButton = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel9 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel10 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.stopTxNowButton = new System.Windows.Forms.Button();
            this.beaconTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.callSignTextBox = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.sendTextButton = new System.Windows.Forms.Button();
            this.connectToStationButton = new System.Windows.Forms.Button();
            this.disconnectFromStationButton = new System.Windows.Forms.Button();
            this.abortConnectionButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.sendTextTextBox = new System.Windows.Forms.TextBox();
            this.fromMonitorTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button12 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.sendBufferProgressBar = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.recieveBufferProgressBar = new System.Windows.Forms.ProgressBar();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // connectToVaraButton
            // 
            this.connectToVaraButton.Location = new System.Drawing.Point(0, 32);
            this.connectToVaraButton.Name = "connectToVaraButton";
            this.connectToVaraButton.Size = new System.Drawing.Size(135, 20);
            this.connectToVaraButton.TabIndex = 0;
            this.connectToVaraButton.Text = "Connect to VARA";
            this.connectToVaraButton.UseVisualStyleBackColor = true;
            this.connectToVaraButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // fromModemTextBox
            // 
            this.fromModemTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.fromModemTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromModemTextBox.ForeColor = System.Drawing.Color.White;
            this.fromModemTextBox.Location = new System.Drawing.Point(144, 56);
            this.fromModemTextBox.Multiline = true;
            this.fromModemTextBox.Name = "fromModemTextBox";
            this.fromModemTextBox.ReadOnly = true;
            this.fromModemTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.fromModemTextBox.Size = new System.Drawing.Size(270, 75);
            this.fromModemTextBox.TabIndex = 1;
            // 
            // disconnectFromVaraButton
            // 
            this.disconnectFromVaraButton.Location = new System.Drawing.Point(0, 58);
            this.disconnectFromVaraButton.Name = "disconnectFromVaraButton";
            this.disconnectFromVaraButton.Size = new System.Drawing.Size(135, 20);
            this.disconnectFromVaraButton.TabIndex = 2;
            this.disconnectFromVaraButton.Text = "Disconnect from VARA";
            this.disconnectFromVaraButton.UseVisualStyleBackColor = true;
            this.disconnectFromVaraButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // sendBeaconTimerButton
            // 
            this.sendBeaconTimerButton.Location = new System.Drawing.Point(0, 84);
            this.sendBeaconTimerButton.Name = "sendBeaconTimerButton";
            this.sendBeaconTimerButton.Size = new System.Drawing.Size(135, 20);
            this.sendBeaconTimerButton.TabIndex = 3;
            this.sendBeaconTimerButton.Text = "Send Beacon 5 Minutes";
            this.sendBeaconTimerButton.UseVisualStyleBackColor = true;
            this.sendBeaconTimerButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // sendBeaconNowButton
            // 
            this.sendBeaconNowButton.Location = new System.Drawing.Point(0, 110);
            this.sendBeaconNowButton.Name = "sendBeaconNowButton";
            this.sendBeaconNowButton.Size = new System.Drawing.Size(135, 20);
            this.sendBeaconNowButton.TabIndex = 4;
            this.sendBeaconNowButton.Text = "Send Beacon NOW";
            this.sendBeaconNowButton.UseVisualStyleBackColor = true;
            this.sendBeaconNowButton.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(0, 162);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(135, 20);
            this.button5.TabIndex = 5;
            this.button5.Text = "Send CQ";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(0, 188);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(135, 20);
            this.button6.TabIndex = 6;
            this.button6.Text = "Enable VARA Monitor";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1157, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.fileToolStripMenuItem.Text = "Settings";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5,
            this.toolStripStatusLabel6,
            this.toolStripStatusLabel7,
            this.toolStripStatusLabel8,
            this.toolStripStatusLabel9,
            this.toolStripStatusLabel10});
            this.statusStrip1.Location = new System.Drawing.Point(0, 480);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1157, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(111, 17);
            this.toolStripStatusLabel1.Text = "Command Channel";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.toolStripStatusLabel2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(51, 17);
            this.toolStripStatusLabel2.Text = "Unkown";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(78, 17);
            this.toolStripStatusLabel3.Text = "Data Channel";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.ActiveLinkColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.toolStripStatusLabel4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(58, 17);
            this.toolStripStatusLabel4.Text = "Unknown";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(65, 17);
            this.toolStripStatusLabel5.Text = "Frequency:";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(51, 17);
            this.toolStripStatusLabel6.Text = "Unkown";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(54, 17);
            this.toolStripStatusLabel7.Text = "TimeUTC";
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Size = new System.Drawing.Size(21, 17);
            this.toolStripStatusLabel8.Text = "RX";
            // 
            // toolStripStatusLabel9
            // 
            this.toolStripStatusLabel9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripStatusLabel9.Name = "toolStripStatusLabel9";
            this.toolStripStatusLabel9.Size = new System.Drawing.Size(96, 17);
            this.toolStripStatusLabel9.Text = "Last beacon was:";
            this.toolStripStatusLabel9.Click += new System.EventHandler(this.toolStripStatusLabel9_Click);
            // 
            // toolStripStatusLabel10
            // 
            this.toolStripStatusLabel10.Name = "toolStripStatusLabel10";
            this.toolStripStatusLabel10.Size = new System.Drawing.Size(36, 17);
            this.toolStripStatusLabel10.Text = "Time.";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // stopTxNowButton
            // 
            this.stopTxNowButton.Location = new System.Drawing.Point(0, 136);
            this.stopTxNowButton.Name = "stopTxNowButton";
            this.stopTxNowButton.Size = new System.Drawing.Size(135, 20);
            this.stopTxNowButton.TabIndex = 9;
            this.stopTxNowButton.Text = "Stop Beacon and TX";
            this.stopTxNowButton.UseVisualStyleBackColor = true;
            this.stopTxNowButton.Click += new System.EventHandler(this.button7_Click);
            // 
            // beaconTimer
            // 
            this.beaconTimer.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(152, 352);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "S-Meter:";
            // 
            // callSignTextBox
            // 
            this.callSignTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.callSignTextBox.ForeColor = System.Drawing.Color.White;
            this.callSignTextBox.Location = new System.Drawing.Point(93, 417);
            this.callSignTextBox.Name = "callSignTextBox";
            this.callSignTextBox.Size = new System.Drawing.Size(75, 20);
            this.callSignTextBox.TabIndex = 11;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.richTextBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.White;
            this.richTextBox1.Location = new System.Drawing.Point(144, 137);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(636, 203);
            this.richTextBox1.TabIndex = 12;
            this.richTextBox1.Text = "";
            // 
            // sendTextButton
            // 
            this.sendTextButton.Location = new System.Drawing.Point(693, 444);
            this.sendTextButton.Name = "sendTextButton";
            this.sendTextButton.Size = new System.Drawing.Size(75, 23);
            this.sendTextButton.TabIndex = 13;
            this.sendTextButton.Text = "Send";
            this.sendTextButton.UseVisualStyleBackColor = true;
            this.sendTextButton.Click += new System.EventHandler(this.button8_Click);
            // 
            // connectToStationButton
            // 
            this.connectToStationButton.Location = new System.Drawing.Point(12, 416);
            this.connectToStationButton.Name = "connectToStationButton";
            this.connectToStationButton.Size = new System.Drawing.Size(75, 20);
            this.connectToStationButton.TabIndex = 14;
            this.connectToStationButton.Text = "Connect";
            this.connectToStationButton.UseVisualStyleBackColor = true;
            this.connectToStationButton.Click += new System.EventHandler(this.button9_Click);
            // 
            // disconnectFromStationButton
            // 
            this.disconnectFromStationButton.Location = new System.Drawing.Point(12, 444);
            this.disconnectFromStationButton.Name = "disconnectFromStationButton";
            this.disconnectFromStationButton.Size = new System.Drawing.Size(75, 20);
            this.disconnectFromStationButton.TabIndex = 15;
            this.disconnectFromStationButton.Text = "Disconnect";
            this.disconnectFromStationButton.UseVisualStyleBackColor = true;
            this.disconnectFromStationButton.Click += new System.EventHandler(this.button10_Click);
            // 
            // abortConnectionButton
            // 
            this.abortConnectionButton.Location = new System.Drawing.Point(93, 444);
            this.abortConnectionButton.Name = "abortConnectionButton";
            this.abortConnectionButton.Size = new System.Drawing.Size(75, 23);
            this.abortConnectionButton.TabIndex = 16;
            this.abortConnectionButton.Text = "Abort";
            this.abortConnectionButton.UseVisualStyleBackColor = true;
            this.abortConnectionButton.Click += new System.EventHandler(this.button11_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(141, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "VARA HF Modem channel:";
            // 
            // sendTextTextBox
            // 
            this.sendTextTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.sendTextTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sendTextTextBox.ForeColor = System.Drawing.Color.White;
            this.sendTextTextBox.Location = new System.Drawing.Point(174, 421);
            this.sendTextTextBox.Multiline = true;
            this.sendTextTextBox.Name = "sendTextTextBox";
            this.sendTextTextBox.Size = new System.Drawing.Size(513, 46);
            this.sendTextTextBox.TabIndex = 18;
            // 
            // fromMonitorTextBox
            // 
            this.fromMonitorTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.fromMonitorTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromMonitorTextBox.ForeColor = System.Drawing.Color.White;
            this.fromMonitorTextBox.Location = new System.Drawing.Point(420, 56);
            this.fromMonitorTextBox.Multiline = true;
            this.fromMonitorTextBox.Name = "fromMonitorTextBox";
            this.fromMonitorTextBox.ReadOnly = true;
            this.fromMonitorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.fromMonitorTextBox.Size = new System.Drawing.Size(360, 75);
            this.fromMonitorTextBox.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(417, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "VARA HF Monitor channel:";
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.comboBox1.ForeColor = System.Drawing.Color.White;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "<LHR>",
            "<LHE>",
            "<INFO>",
            "<VER>",
            "<SNRR>",
            "<FSR>",
            "<FSO>",
            "<DISC>"});
            this.comboBox1.Location = new System.Drawing.Point(566, 394);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 21;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(513, 402);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Macro\'s:";
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(0, 214);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(135, 20);
            this.button12.TabIndex = 23;
            this.button12.Text = "Retry Data channel";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(786, 56);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(358, 284);
            this.dataGridView1.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(260, 352);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "SNR:";
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.textBox5.Location = new System.Drawing.Point(205, 349);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(49, 20);
            this.textBox5.TabIndex = 26;
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.textBox6.Location = new System.Drawing.Point(296, 349);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(49, 20);
            this.textBox6.TabIndex = 27;
            // 
            // sendBufferProgressBar
            // 
            this.sendBufferProgressBar.Location = new System.Drawing.Point(351, 346);
            this.sendBufferProgressBar.Name = "sendBufferProgressBar";
            this.sendBufferProgressBar.Size = new System.Drawing.Size(429, 17);
            this.sendBufferProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.sendBufferProgressBar.TabIndex = 28;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(783, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Last heard station:";
            // 
            // recieveBufferProgressBar
            // 
            this.recieveBufferProgressBar.Location = new System.Drawing.Point(351, 369);
            this.recieveBufferProgressBar.Name = "recieveBufferProgressBar";
            this.recieveBufferProgressBar.Size = new System.Drawing.Size(429, 17);
            this.recieveBufferProgressBar.TabIndex = 30;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1157, 502);
            this.ControlBox = false;
            this.Controls.Add(this.recieveBufferProgressBar);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.sendBufferProgressBar);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.fromMonitorTextBox);
            this.Controls.Add(this.sendTextTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.abortConnectionButton);
            this.Controls.Add(this.disconnectFromStationButton);
            this.Controls.Add(this.connectToStationButton);
            this.Controls.Add(this.sendTextButton);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.callSignTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.stopTxNowButton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.sendBeaconNowButton);
            this.Controls.Add(this.sendBeaconTimerButton);
            this.Controls.Add(this.disconnectFromVaraButton);
            this.Controls.Add(this.fromModemTextBox);
            this.Controls.Add(this.connectToVaraButton);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "VarAQT - PD1AQT";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectToVaraButton;
        private System.Windows.Forms.TextBox fromModemTextBox;
        private System.Windows.Forms.Button disconnectFromVaraButton;
        private System.Windows.Forms.Button sendBeaconTimerButton;
        private System.Windows.Forms.Button sendBeaconNowButton;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.Button stopTxNowButton;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.Timer beaconTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox callSignTextBox;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button sendTextButton;
        private System.Windows.Forms.Button connectToStationButton;
        private System.Windows.Forms.Button disconnectFromStationButton;
        private System.Windows.Forms.Button abortConnectionButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox sendTextTextBox;
        private System.Windows.Forms.TextBox fromMonitorTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.ProgressBar sendBufferProgressBar;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel9;
        private System.Windows.Forms.ProgressBar recieveBufferProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel10;
    }
}

