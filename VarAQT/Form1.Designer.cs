namespace VarAQT
{
    partial class varAQT
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
            this.sendCQButton = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.varaToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.channelToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.channelBusyToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.utcTimeToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.rxTxToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lastBeaconTextToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lastBeaconTimeToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.frequencyTextToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.frequencyToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.macroComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pingAStationButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.sendBufferProgressBar = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.recieveBufferProgressBar = new System.Windows.Forms.ProgressBar();
            this.txRxChannelComboBox = new System.Windows.Forms.ComboBox();
            this.cqChannelComboBox = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.frequencyBandComboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // connectToVaraButton
            // 
            this.connectToVaraButton.Location = new System.Drawing.Point(0, 39);
            this.connectToVaraButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.connectToVaraButton.Name = "connectToVaraButton";
            this.connectToVaraButton.Size = new System.Drawing.Size(158, 25);
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
            this.fromModemTextBox.Location = new System.Drawing.Point(168, 69);
            this.fromModemTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.fromModemTextBox.Multiline = true;
            this.fromModemTextBox.Name = "fromModemTextBox";
            this.fromModemTextBox.ReadOnly = true;
            this.fromModemTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.fromModemTextBox.Size = new System.Drawing.Size(352, 91);
            this.fromModemTextBox.TabIndex = 1;
            // 
            // disconnectFromVaraButton
            // 
            this.disconnectFromVaraButton.Location = new System.Drawing.Point(0, 71);
            this.disconnectFromVaraButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.disconnectFromVaraButton.Name = "disconnectFromVaraButton";
            this.disconnectFromVaraButton.Size = new System.Drawing.Size(158, 25);
            this.disconnectFromVaraButton.TabIndex = 2;
            this.disconnectFromVaraButton.Text = "Disconnect from VARA";
            this.disconnectFromVaraButton.UseVisualStyleBackColor = true;
            this.disconnectFromVaraButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // sendBeaconTimerButton
            // 
            this.sendBeaconTimerButton.Location = new System.Drawing.Point(0, 103);
            this.sendBeaconTimerButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sendBeaconTimerButton.Name = "sendBeaconTimerButton";
            this.sendBeaconTimerButton.Size = new System.Drawing.Size(158, 25);
            this.sendBeaconTimerButton.TabIndex = 3;
            this.sendBeaconTimerButton.Text = "Send Beacon 5 Minutes";
            this.sendBeaconTimerButton.UseVisualStyleBackColor = true;
            this.sendBeaconTimerButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // sendBeaconNowButton
            // 
            this.sendBeaconNowButton.Location = new System.Drawing.Point(0, 135);
            this.sendBeaconNowButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sendBeaconNowButton.Name = "sendBeaconNowButton";
            this.sendBeaconNowButton.Size = new System.Drawing.Size(158, 25);
            this.sendBeaconNowButton.TabIndex = 4;
            this.sendBeaconNowButton.Text = "Send Beacon NOW";
            this.sendBeaconNowButton.UseVisualStyleBackColor = true;
            this.sendBeaconNowButton.Click += new System.EventHandler(this.button4_Click);
            // 
            // sendCQButton
            // 
            this.sendCQButton.Location = new System.Drawing.Point(0, 199);
            this.sendCQButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sendCQButton.Name = "sendCQButton";
            this.sendCQButton.Size = new System.Drawing.Size(158, 25);
            this.sendCQButton.TabIndex = 5;
            this.sendCQButton.Text = "Send CQ";
            this.sendCQButton.UseVisualStyleBackColor = true;
            this.sendCQButton.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(1002, 568);
            this.button6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(158, 25);
            this.button6.TabIndex = 6;
            this.button6.Text = "Enable VARA Monitor";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1350, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.fileToolStripMenuItem.Text = "Settings";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.varaToolStripStatusLabel,
            this.channelToolStripStatusLabel,
            this.channelBusyToolStripStatusLabel,
            this.utcTimeToolStripStatusLabel,
            this.rxTxToolStripStatusLabel,
            this.lastBeaconTextToolStripStatusLabel,
            this.lastBeaconTimeToolStripStatusLabel,
            this.frequencyTextToolStripStatusLabel,
            this.frequencyToolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 596);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 17, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1350, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // varaToolStripStatusLabel
            // 
            this.varaToolStripStatusLabel.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.varaToolStripStatusLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.varaToolStripStatusLabel.Name = "varaToolStripStatusLabel";
            this.varaToolStripStatusLabel.Size = new System.Drawing.Size(51, 17);
            this.varaToolStripStatusLabel.Text = "Unkown";
            // 
            // channelToolStripStatusLabel
            // 
            this.channelToolStripStatusLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.channelToolStripStatusLabel.Name = "channelToolStripStatusLabel";
            this.channelToolStripStatusLabel.Size = new System.Drawing.Size(54, 17);
            this.channelToolStripStatusLabel.Text = "Channel:";
            // 
            // channelBusyToolStripStatusLabel
            // 
            this.channelBusyToolStripStatusLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.channelBusyToolStripStatusLabel.Name = "channelBusyToolStripStatusLabel";
            this.channelBusyToolStripStatusLabel.Size = new System.Drawing.Size(51, 17);
            this.channelBusyToolStripStatusLabel.Text = "Unkown";
            // 
            // utcTimeToolStripStatusLabel
            // 
            this.utcTimeToolStripStatusLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.utcTimeToolStripStatusLabel.Name = "utcTimeToolStripStatusLabel";
            this.utcTimeToolStripStatusLabel.Size = new System.Drawing.Size(54, 17);
            this.utcTimeToolStripStatusLabel.Text = "TimeUTC";
            // 
            // rxTxToolStripStatusLabel
            // 
            this.rxTxToolStripStatusLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rxTxToolStripStatusLabel.Name = "rxTxToolStripStatusLabel";
            this.rxTxToolStripStatusLabel.Size = new System.Drawing.Size(21, 17);
            this.rxTxToolStripStatusLabel.Text = "RX";
            // 
            // lastBeaconTextToolStripStatusLabel
            // 
            this.lastBeaconTextToolStripStatusLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lastBeaconTextToolStripStatusLabel.Name = "lastBeaconTextToolStripStatusLabel";
            this.lastBeaconTextToolStripStatusLabel.Size = new System.Drawing.Size(96, 17);
            this.lastBeaconTextToolStripStatusLabel.Text = "Last beacon was:";
            this.lastBeaconTextToolStripStatusLabel.Click += new System.EventHandler(this.toolStripStatusLabel9_Click);
            // 
            // lastBeaconTimeToolStripStatusLabel
            // 
            this.lastBeaconTimeToolStripStatusLabel.Name = "lastBeaconTimeToolStripStatusLabel";
            this.lastBeaconTimeToolStripStatusLabel.Size = new System.Drawing.Size(36, 17);
            this.lastBeaconTimeToolStripStatusLabel.Text = "Time.";
            // 
            // frequencyTextToolStripStatusLabel
            // 
            this.frequencyTextToolStripStatusLabel.Name = "frequencyTextToolStripStatusLabel";
            this.frequencyTextToolStripStatusLabel.Size = new System.Drawing.Size(65, 17);
            this.frequencyTextToolStripStatusLabel.Text = "Frequency:";
            // 
            // frequencyToolStripStatusLabel
            // 
            this.frequencyToolStripStatusLabel.Name = "frequencyToolStripStatusLabel";
            this.frequencyToolStripStatusLabel.Size = new System.Drawing.Size(30, 17);
            this.frequencyToolStripStatusLabel.Text = "Mhz";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // stopTxNowButton
            // 
            this.stopTxNowButton.Location = new System.Drawing.Point(0, 167);
            this.stopTxNowButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.stopTxNowButton.Name = "stopTxNowButton";
            this.stopTxNowButton.Size = new System.Drawing.Size(158, 25);
            this.stopTxNowButton.TabIndex = 9;
            this.stopTxNowButton.Text = "Stop Beacon and TXStr";
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
            this.label1.Location = new System.Drawing.Point(178, 433);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "S-Meter:";
            // 
            // callSignTextBox
            // 
            this.callSignTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.callSignTextBox.ForeColor = System.Drawing.Color.White;
            this.callSignTextBox.Location = new System.Drawing.Point(99, 509);
            this.callSignTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.callSignTextBox.Name = "callSignTextBox";
            this.callSignTextBox.Size = new System.Drawing.Size(88, 22);
            this.callSignTextBox.TabIndex = 11;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.richTextBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.ForeColor = System.Drawing.Color.White;
            this.richTextBox1.Location = new System.Drawing.Point(168, 169);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(742, 249);
            this.richTextBox1.TabIndex = 12;
            this.richTextBox1.Text = "";
            // 
            // sendTextButton
            // 
            this.sendTextButton.Location = new System.Drawing.Point(808, 546);
            this.sendTextButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sendTextButton.Name = "sendTextButton";
            this.sendTextButton.Size = new System.Drawing.Size(88, 29);
            this.sendTextButton.TabIndex = 13;
            this.sendTextButton.Text = "Send";
            this.sendTextButton.UseVisualStyleBackColor = true;
            this.sendTextButton.Click += new System.EventHandler(this.button8_Click);
            // 
            // connectToStationButton
            // 
            this.connectToStationButton.Location = new System.Drawing.Point(3, 506);
            this.connectToStationButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.connectToStationButton.Name = "connectToStationButton";
            this.connectToStationButton.Size = new System.Drawing.Size(88, 25);
            this.connectToStationButton.TabIndex = 14;
            this.connectToStationButton.Text = "Connect";
            this.connectToStationButton.UseVisualStyleBackColor = true;
            this.connectToStationButton.Click += new System.EventHandler(this.button9_Click);
            // 
            // disconnectFromStationButton
            // 
            this.disconnectFromStationButton.Location = new System.Drawing.Point(3, 568);
            this.disconnectFromStationButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.disconnectFromStationButton.Name = "disconnectFromStationButton";
            this.disconnectFromStationButton.Size = new System.Drawing.Size(88, 25);
            this.disconnectFromStationButton.TabIndex = 15;
            this.disconnectFromStationButton.Text = "Disconnect";
            this.disconnectFromStationButton.UseVisualStyleBackColor = true;
            this.disconnectFromStationButton.Click += new System.EventHandler(this.button10_Click);
            // 
            // abortConnectionButton
            // 
            this.abortConnectionButton.Location = new System.Drawing.Point(99, 564);
            this.abortConnectionButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.abortConnectionButton.Name = "abortConnectionButton";
            this.abortConnectionButton.Size = new System.Drawing.Size(88, 29);
            this.abortConnectionButton.TabIndex = 16;
            this.abortConnectionButton.Text = "Abort";
            this.abortConnectionButton.UseVisualStyleBackColor = true;
            this.abortConnectionButton.Click += new System.EventHandler(this.button11_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(164, 39);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "VARA HF Modem channel:";
            // 
            // sendTextTextBox
            // 
            this.sendTextTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.sendTextTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sendTextTextBox.ForeColor = System.Drawing.Color.White;
            this.sendTextTextBox.Location = new System.Drawing.Point(203, 518);
            this.sendTextTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sendTextTextBox.Multiline = true;
            this.sendTextTextBox.Name = "sendTextTextBox";
            this.sendTextTextBox.Size = new System.Drawing.Size(598, 55);
            this.sendTextTextBox.TabIndex = 18;
            // 
            // fromMonitorTextBox
            // 
            this.fromMonitorTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.fromMonitorTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fromMonitorTextBox.ForeColor = System.Drawing.Color.White;
            this.fromMonitorTextBox.Location = new System.Drawing.Point(528, 69);
            this.fromMonitorTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.fromMonitorTextBox.Multiline = true;
            this.fromMonitorTextBox.Name = "fromMonitorTextBox";
            this.fromMonitorTextBox.ReadOnly = true;
            this.fromMonitorTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.fromMonitorTextBox.Size = new System.Drawing.Size(382, 91);
            this.fromMonitorTextBox.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(525, 39);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = "VARA HF Monitor channel:";
            // 
            // macroComboBox
            // 
            this.macroComboBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.macroComboBox.ForeColor = System.Drawing.Color.White;
            this.macroComboBox.FormattingEnabled = true;
            this.macroComboBox.Items.AddRange(new object[] {
            "<LHR>",
            "<LHE>",
            "<INFO>",
            "<VER>",
            "<SNRR>",
            "<FSR>",
            "<FSO>",
            "<DISC>"});
            this.macroComboBox.Location = new System.Drawing.Point(264, 488);
            this.macroComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.macroComboBox.Name = "macroComboBox";
            this.macroComboBox.Size = new System.Drawing.Size(219, 24);
            this.macroComboBox.TabIndex = 21;
            this.macroComboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label4.Location = new System.Drawing.Point(200, 491);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 16);
            this.label4.TabIndex = 22;
            this.label4.Text = "Macro\'s:";
            // 
            // pingAStationButton
            // 
            this.pingAStationButton.Location = new System.Drawing.Point(3, 537);
            this.pingAStationButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pingAStationButton.Name = "pingAStationButton";
            this.pingAStationButton.Size = new System.Drawing.Size(88, 25);
            this.pingAStationButton.TabIndex = 23;
            this.pingAStationButton.Text = "Ping";
            this.pingAStationButton.UseVisualStyleBackColor = true;
            this.pingAStationButton.Click += new System.EventHandler(this.button12_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(917, 69);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(417, 350);
            this.dataGridView1.TabIndex = 24;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(304, 433);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 16);
            this.label5.TabIndex = 25;
            this.label5.Text = "SNR:";
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.textBox5.Location = new System.Drawing.Point(239, 430);
            this.textBox5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(56, 22);
            this.textBox5.TabIndex = 26;
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.textBox6.Location = new System.Drawing.Point(346, 430);
            this.textBox6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(56, 22);
            this.textBox6.TabIndex = 27;
            // 
            // sendBufferProgressBar
            // 
            this.sendBufferProgressBar.Location = new System.Drawing.Point(410, 426);
            this.sendBufferProgressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.sendBufferProgressBar.Name = "sendBufferProgressBar";
            this.sendBufferProgressBar.Size = new System.Drawing.Size(500, 21);
            this.sendBufferProgressBar.Step = 1;
            this.sendBufferProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.sendBufferProgressBar.TabIndex = 28;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(914, 39);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 16);
            this.label6.TabIndex = 29;
            this.label6.Text = "Last heard stations today:";
            // 
            // recieveBufferProgressBar
            // 
            this.recieveBufferProgressBar.Location = new System.Drawing.Point(410, 454);
            this.recieveBufferProgressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.recieveBufferProgressBar.Name = "recieveBufferProgressBar";
            this.recieveBufferProgressBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.recieveBufferProgressBar.Size = new System.Drawing.Size(500, 21);
            this.recieveBufferProgressBar.Step = 1;
            this.recieveBufferProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.recieveBufferProgressBar.TabIndex = 30;
            // 
            // txRxChannelComboBox
            // 
            this.txRxChannelComboBox.FormattingEnabled = true;
            this.txRxChannelComboBox.Items.AddRange(new object[] {
            "Channel 5",
            "Channel 4",
            "Channel 3",
            "Channel 2",
            "Channel 1",
            "Channel 9 - CQ",
            "Channel 11",
            "Channel 12",
            "Channel 13",
            "Channel 14",
            "Channel 15"});
            this.txRxChannelComboBox.Location = new System.Drawing.Point(0, 293);
            this.txRxChannelComboBox.Name = "txRxChannelComboBox";
            this.txRxChannelComboBox.Size = new System.Drawing.Size(158, 24);
            this.txRxChannelComboBox.TabIndex = 31;
            this.txRxChannelComboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // cqChannelComboBox
            // 
            this.cqChannelComboBox.FormattingEnabled = true;
            this.cqChannelComboBox.Items.AddRange(new object[] {
            "Channel 5",
            "Channel 4",
            "Channel 3",
            "Channel 2",
            "Channel 1",
            "Channel 9 - CQ",
            "Channel 11",
            "Channel 12",
            "Channel 13",
            "Channel 14",
            "Channel 15"});
            this.cqChannelComboBox.Location = new System.Drawing.Point(0, 243);
            this.cqChannelComboBox.Name = "cqChannelComboBox";
            this.cqChannelComboBox.Size = new System.Drawing.Size(158, 24);
            this.cqChannelComboBox.TabIndex = 32;
            this.cqChannelComboBox.SelectedIndexChanged += new System.EventHandler(this.cqChannelComboBox_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 224);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 16);
            this.label7.TabIndex = 33;
            this.label7.Text = "CQ Channel:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 274);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 16);
            this.label8.TabIndex = 34;
            this.label8.Text = "TXStr/RX Channel:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(0, 324);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 16);
            this.label9.TabIndex = 35;
            this.label9.Text = "Frequency Band:";
            // 
            // frequencyBandComboBox
            // 
            this.frequencyBandComboBox.FormattingEnabled = true;
            this.frequencyBandComboBox.Items.AddRange(new object[] {
            "10 Metres",
            "12 Metres",
            "15 Metres",
            "17 Metres",
            "20 Metres",
            "30 Metres",
            "40 Metres",
            "60 Metres",
            "80 Metres",
            "160 Metres"});
            this.frequencyBandComboBox.Location = new System.Drawing.Point(0, 344);
            this.frequencyBandComboBox.Name = "frequencyBandComboBox";
            this.frequencyBandComboBox.Size = new System.Drawing.Size(158, 24);
            this.frequencyBandComboBox.TabIndex = 36;
            this.frequencyBandComboBox.SelectedIndexChanged += new System.EventHandler(this.frequencyBandComboBox_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(903, 494);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 18);
            this.label10.TabIndex = 37;
            this.label10.Text = "Who";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label11.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(903, 518);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 18);
            this.label11.TabIndex = 38;
            this.label11.Text = "His LOC";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label12.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(903, 541);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 18);
            this.label12.TabIndex = 39;
            this.label12.Text = "His Report";
            // 
            // varAQT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1350, 618);
            this.ControlBox = false;
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.frequencyBandComboBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cqChannelComboBox);
            this.Controls.Add(this.txRxChannelComboBox);
            this.Controls.Add(this.recieveBufferProgressBar);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.sendBufferProgressBar);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.pingAStationButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.macroComboBox);
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
            this.Controls.Add(this.sendCQButton);
            this.Controls.Add(this.sendBeaconNowButton);
            this.Controls.Add(this.sendBeaconTimerButton);
            this.Controls.Add(this.disconnectFromVaraButton);
            this.Controls.Add(this.fromModemTextBox);
            this.Controls.Add(this.connectToVaraButton);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "varAQT";
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
        private System.Windows.Forms.Button sendCQButton;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel varaToolStripStatusLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripStatusLabel channelToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel channelBusyToolStripStatusLabel;
        private System.Windows.Forms.Button stopTxNowButton;
        private System.Windows.Forms.ToolStripStatusLabel utcTimeToolStripStatusLabel;
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
        private System.Windows.Forms.ComboBox macroComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button pingAStationButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.ProgressBar sendBufferProgressBar;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel rxTxToolStripStatusLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripStatusLabel lastBeaconTextToolStripStatusLabel;
        private System.Windows.Forms.ProgressBar recieveBufferProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel lastBeaconTimeToolStripStatusLabel;
        private System.Windows.Forms.ComboBox txRxChannelComboBox;
        private System.Windows.Forms.ComboBox cqChannelComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox frequencyBandComboBox;
        private System.Windows.Forms.ToolStripStatusLabel frequencyTextToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel frequencyToolStripStatusLabel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
    }
}

