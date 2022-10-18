using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Added
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Reflection;

//using VaraCommand;
using VaraLib;
using Logger;
using System.Media;
using static System.Net.Mime.MediaTypeNames;
using VarAQT.Models;
using System.Xml.Serialization;
using System.Globalization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Xml.Linq;

namespace VarAQT
{
    public partial class Form1 : Form
    {
        #region Constants

        // For Display Data in Text Box and Info - UI Thread Invoke
        public delegate void AddLogDeligate(string data);
        
        public AddLogDeligate UpdateFromModemTextBoxDeligate;
        //public AddLogDeligate UpdateSendToModemTextBoxDeligate;
        public AddLogDeligate UpdateCallSignTextBoxDeligate;
        public AddLogDeligate UpdateSendTextTextBoxDeligate;
        
        public delegate void AddRichDeligate(string data, Color color);
        public AddRichDeligate UpdateRichTextBoxDeligate;
        
        public AddLogDeligate UpdateDataGridDeligate;
        
        public delegate void AddNotificationDelegate(int type, bool status);
        public AddNotificationDelegate UpdateStatusIcons;
        // Client Object
        VARACommandClient varaCmd;
        VARADataClient varaData;
        // VARAKISSClient varaKISS;

        public List<stations> lastHeard = new List<stations>();

        #endregion

        #region Form
        public Form1()
        {
            InitializeComponent();
            Log.enableDebug = true;
            Log.enableInfo = true;
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            UpdateFromModemTextBoxDeligate = new AddLogDeligate(updateRecievedFromModemTextBox);
            //UpdateSendToModemTextBoxDeligate = new AddLogDeligate(updateSendToModemTextBox);
            UpdateCallSignTextBoxDeligate = new AddLogDeligate(updateCallSignTextBox);
            UpdateSendTextTextBoxDeligate = new AddLogDeligate(updateSendTextTextBox);
            UpdateRichTextBoxDeligate = new AddRichDeligate(updateRichTextBox);
            UpdateDataGridDeligate = new AddLogDeligate(updateLastHeardDataGrid);
            //UpdateStatusIcons = new AddNotificationDelegate(UpdateSatusIcons);
            timer1.Interval = 100;
            beaconTimer.Interval = 300000; //60000 = 1 minuut, 300000 = 5 minuten ;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            RigControl rigControlForm = new RigControl();
            rigControlForm.Show();
            setColors();
            setDataGrid();
            disableButtonsDisconnected();
            timer1.Start();
            connectToVaraModem();
            bw500();
            listenON();
            chatOn();
            listenCQ();
            myCall(Values.callSign);
            addLastHeardFileToDataGrid();
        }

        #endregion

        #region Buttons
        // Connect to VARA modem Button
        private void button1_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            connectToVaraModem();
            bw500();
            listenON();
            listenCQ();
            chatOn();
            myCall(Values.callSign);
        }
        // Disconnect to VARA modem Button
        private void button2_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            varaCmd.VARACommandClientDisconnect();
            varaData.VARADataClientDisconnect();
            disableButtonsDisconnected();
        }
        // Start Beacon Timer Button
        private void button3_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            switch (Values.beaconActive)
            {
                case true:
                    Values.beaconActive = false;
                    beaconTimer.Stop();
                    sendBeaconTimerButton.BackColor = Color.DarkSeaGreen;
                    break;
                case false:
                    Values.beaconActive = true;
                    beaconTimer.Start();
                    sendBeaconTimerButton.BackColor = Color.DarkRed;
                    break;
            }
        }
        // Send Beacon Now Button
        private void button4_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            sendBeacon();
        }
        // Set BandWidth to 2300 Button (Will be used for something else)
        private void button5_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            bw2300();

        }
        // Spare Button (Now saves the LastHeard list to a XML file)
        private void button6_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            addLastHeardFileToDataGrid();

        }
        // Stop beacon and TX Button
        private void button7_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            RigControl.disableTX().ConfigureAwait(false);
            beaconTimer.Stop();
        }
        // Send Text to Modem on Data channel Button
        private void button8_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            sendText();

        }
        // connect to station Button
        private void button9_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            string callToConnect = callSignTextBox.Text.ToString();
            connectToStation(callToConnect);
        }
        // Disconnect from station Button
        private void button10_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            disconnectFromStation();
        }
        // Abort the connection Button
        private void button11_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            abortConnectedStation();
        }
        private void button12_Click(object sender, EventArgs e)
        {
            Functions.WriteXML<stations>(lastHeard, "LastHeard.xml");
        }
        #endregion

        #region Timers
        // timer1 updates the clock on the status strip. can be used for recuring things.
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel7.Text = "UTC: " + DateTime.UtcNow.ToString("HH:mm:ss");
            sendBufferProgressBar.Maximum = Values.sendBufferSize;
            sendBufferProgressBar.Value = Values.sendBuffer;
            if (!Values.stationConnected)
                sendBufferProgressBar.Value = 0;
            recieveBufferProgressBar.Maximum = Values.recieveBufferSize;
            recieveBufferProgressBar.Value = Values.recieveBuffer;
            if (!Values.stationConnected)
                recieveBufferProgressBar.Value = 0;
        }
        // send beacon timer.
        private void timer2_Tick(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            sendBeacon();
        }
        #endregion

        // Connection Status Listner
        private void OnCommandConnect(bool status)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            switch (status)
            {
                case true:
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.Text = "Connected"));
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.BackColor = Color.Green));
                    break;
                case false:
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.Text = "Disconnected"));
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.BackColor = Color.Red));
                    break;
                default:
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.Text = "Unknown"));
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.BackColor = Color.Yellow));
                    break;
            }
        }
        // VARA Command channel Recieved Listner
        public async void OnCommandRecieved(string data)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            string[] dataRecieved = data.Split('\r');
            foreach (string text in dataRecieved)
            {
                if (text != String.Empty)
                    switch (text)
                    {
                        case VARAResult.pttOFF:
                            await RigControl.disableTX().ConfigureAwait(false);
                            Values.busy = false;
                            this.BeginInvoke((Action)(() => toolStripStatusLabel8.BackColor = Color.Green));
                            this.BeginInvoke((Action)(() => toolStripStatusLabel8.Text = "RX"));
                            break;
                        case VARAResult.pttON:
                            await RigControl.enableTX().ConfigureAwait(false);
                            Values.busy = true;
                            this.BeginInvoke((Action)(() => toolStripStatusLabel8.BackColor = Color.Red));
                            this.BeginInvoke((Action)(() => toolStripStatusLabel8.Text = "TX"));
                            break;
                        case VARAResult.busyOFF:
                            this.BeginInvoke((Action)(() => toolStripStatusLabel6.BackColor = Color.Green));
                            this.BeginInvoke((Action)(() => toolStripStatusLabel6.Text = "FREE"));
                            Values.busy = false;
                            break;
                        case VARAResult.busyON:
                            Values.busy = true;
                            this.BeginInvoke((Action)(() => toolStripStatusLabel6.BackColor = Color.Red));
                            this.BeginInvoke((Action)(() => toolStripStatusLabel6.Text = "BUSY"));
                            break;
                        case VARAResult.disconnected:
                            Values.outGoingConnection = false;
                            disableButtonsStationDisconnected();
                            await updateFromModemTextBoxAsync(Values.incomming + text);
                            break;
                        case VARAResult.pending:
                            await updateFromModemTextBoxAsync(Values.incomming + text);
                            break;
                        case VARAResult.cancelPending:
                            await updateFromModemTextBoxAsync(Values.incomming + text);
                            break;
                        case string a when text.Contains(VARAResult.registered):
                            await updateFromModemTextBoxAsync(Values.incomming + text);
                            enableButtonsRegistered();
                            break;
                        case VARAResult.registered:
                            await updateFromModemTextBoxAsync(Values.incomming + text);
                            enableButtonsRegistered();
                            break;
                        case VARAResult.linkRegistered:
                            enableButtonsStationConnected();
                            await updateFromModemTextBoxAsync(Values.incomming + text);
                            break;
                        case VARAResult.linkUnRegistered:
                            enableButtonsStationConnected();
                            await updateFromModemTextBoxAsync(Values.incomming + text);
                            break;
                        case VARAResult.IAmAlive:
                            Log.Debug(VARAResult.IAmAlive);
                            break;
                        case VARAResult.missingSoundCard:
                            await updateFromModemTextBoxAsync(Values.incomming + text);
                            break;
                        case VARAResult.ok:
                            await updateFromModemTextBoxAsync(Values.incomming + text);
                            break;
                        case VARAResult.wrong:
                            await updateFromModemTextBoxAsync(Values.incomming + text);
                            break;

                        case string a when text.Contains("SN "):
                            if (Values.stationConnected)
                            {
                                this.BeginInvoke((Action)(() => textBox5.Text = Functions.sMeter(Values.sMeter)));
                                this.BeginInvoke((Action)(() => textBox6.Text = Functions.sNMeter(text)));
                                fromModemTextBox.Invoke(UpdateFromModemTextBoxDeligate, Values.incomming + text + " " + Functions.sMeter(Values.sMeter));
                                Values.signalNoice = text;
                            }
                            else
                            {
                                fromModemTextBox.Invoke(UpdateFromModemTextBoxDeligate, Values.incomming + text + " " + Functions.sMeter(Values.sMeter));
                                Values.signalNoice = text;
                            }
                            break;
                        case string a when text.Contains(VARAResult.cqFrame):
                            await updateFromModemTextBoxAsync(Values.incomming + text);
                            await updateLastHeardAsync(text);
                            break;
                        default:
                            if (text.Contains("BUFFER "))
                            {
                                await updateFromModemTextBoxAsync(Values.incomming + text);
                                string buffer = text.Remove(0, 7);
                                int bufferSize = int.Parse(buffer);
                                Values.sendBuffer = bufferSize;
                                break;
                            }
                            else if (text.Contains("CONNECTED "))
                            {
                                await updateFromModemTextBoxAsync(Values.incomming + text);
                                enableButtonsStationConnected();
                                if (!Values.outGoingConnection)
                                {
                                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, "Welcome to my station. it is still a work in progress... \r\n<LOC:JO22OI>");
                                    sendText();
                                }
                                break;
                            }
                            richTextBox1.Invoke(UpdateRichTextBoxDeligate, text + "\r\n" , Color.DarkRed);

                            break;
                    }
            }
        }
        private async Task updateFromModemTextBoxAsync(string text)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            await Task.Run(() =>
            {
                fromModemTextBox.Invoke(UpdateFromModemTextBoxDeligate, text);
            });
        }
        private void updateRecievedFromModemTextBox(string _data)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            fromModemTextBox.AppendText(DateTime.UtcNow.ToString("HH:mm:ss") + " " + _data + Environment.NewLine);
            fromModemTextBox.SelectionStart = fromModemTextBox.Text.Length;
            fromModemTextBox.ScrollToCaret();
            Log.Info(_data);
        }
        private async Task updateCallSignTextBoxAsync(string text)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            await Task.Run(() =>
            {
                callSignTextBox.Invoke(UpdateCallSignTextBoxDeligate, text);
            });
        }
        private void updateCallSignTextBox(string _data)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            callSignTextBox.AppendText(DateTime.UtcNow.ToString("HH:mm:ss") + " " + _data + Environment.NewLine);
            callSignTextBox.SelectionStart = callSignTextBox.Text.Length;
            callSignTextBox.ScrollToCaret();
        }
        private async Task updateSendTextTextBoxAsync(string text)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            await Task.Run(() =>
            {
                sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, text);
            });
        }
        private void updateSendTextTextBox(string _data)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            sendTextTextBox.Text = _data;
        }
        //private void updateSendToModemTextBox(string _data)
        //{
        //    Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
        //    sendToModemTextBox.AppendText(DateTime.UtcNow.ToString("HH:mm:ss") + " <- " + _data + Environment.NewLine);
        //    sendToModemTextBox.SelectionStart = sendToModemTextBox.Text.Length;
        //    sendToModemTextBox.ScrollToCaret();
        //}
        private async Task updateLastHeardAsync(string text)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            await Task.Run(() =>
            {
                string remove = text.Remove(0, 8); // Remove CQ Frame
                string replace = remove.Replace(" 500", ""); // Remove the BW 500
                string beacon = replace.Replace(" ", ""); // Remove spaces if any.
                // split the callsign from the ID
                string[] ids = beacon.Split('-');
                string beaconCallSign = ids[0];
                string beaconSign = ids[1];

                dataGridView1.Invoke(new Action(delegate ()
                {
                    string search = beaconCallSign;
                    int rowIndex = -1;
                    int count = 1;
                    foreach (DataGridViewRow rows in dataGridView1.Rows)
                    {
                        if (rows.Cells[1].Value.ToString().Equals(search))
                        {
                            rowIndex = rows.Index;
                            string counter = rows.Cells[4].Value.ToString();
                            count = int.Parse(counter);
                            count++;
                            dataGridView1.Rows.RemoveAt(rowIndex);
                            break;
                        }
                    }
                    dataGridView1.Rows.Add(DateTime.UtcNow.ToString("HH:mm:ss"), beaconCallSign, beaconSign , Functions.sNMeter(Values.signalNoice), count, Functions.sMeter(Values.sMeter));

                    lastHeard.Add(new stations { UTCTime = DateTime.UtcNow.ToString(), Call = beaconCallSign, SID = beaconSign, SNR = Functions.sNMeter(Values.signalNoice), SM = Functions.sMeter(Values.sMeter), Cnt = count.ToString() });


                    dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    for (int i = 0; i <= dataGridView1.Columns.Count - 1; i++)
                    {
                        // Store Auto Sized Widths:
                        int colw = dataGridView1.Columns[i].Width;

                        // Remove AutoSizing:
                        dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                        // Set Width to calculated AutoSize value:
                        dataGridView1.Columns[i].Width = colw;
                    }
                    dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Descending);
                    dataGridView1.AutoResizeColumns();
                    SystemSounds.Hand.Play();
                }));


            });
        }
        private void updateLastHeardDataGrid(string _data)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            sendTextTextBox.Text = _data;
        }
        private void enableButtonsRegistered()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            if (!Values.stationConnected)
            {
                connectToVaraButton.Invoke((MethodInvoker)delegate { connectToVaraButton.Enabled = false; });
                disconnectFromVaraButton.Invoke((MethodInvoker)delegate { disconnectFromVaraButton.Enabled = true; });
                sendBeaconTimerButton.Invoke((MethodInvoker)delegate { sendBeaconTimerButton.Enabled = true; });
                sendBeaconNowButton.Invoke((MethodInvoker)delegate { sendBeaconNowButton.Enabled = true; });
                button5.Invoke((MethodInvoker)delegate { button5.Enabled = true; });
                button6.Invoke((MethodInvoker)delegate { button6.Enabled = true; });
                stopTxNowButton.Invoke((MethodInvoker)delegate { stopTxNowButton.Enabled = true; });
                sendTextButton.Invoke((MethodInvoker)delegate { sendTextButton.Enabled = false; });
                connectToStationButton.Invoke((MethodInvoker)delegate { connectToStationButton.Enabled = true; });
                disconnectFromStationButton.Invoke((MethodInvoker)delegate { disconnectFromStationButton.Enabled = false; });
                abortConnectionButton.Invoke((MethodInvoker)delegate { abortConnectionButton.Enabled = false; });
            }
        }
        private void enableButtonsStationConnected()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            Values.stationConnected = true;
            stopTxNowButton.Invoke((MethodInvoker)delegate { stopTxNowButton.Enabled = true; });
            sendTextButton.Invoke((MethodInvoker)delegate { sendTextButton.Enabled = true; });
            connectToStationButton.Invoke((MethodInvoker)delegate { connectToStationButton.Enabled = false; });
            disconnectFromStationButton.Invoke((MethodInvoker)delegate { disconnectFromStationButton.Enabled = true; });
            abortConnectionButton.Invoke((MethodInvoker)delegate { abortConnectionButton.Enabled = true; });
        }
        private void disableButtonsStationDisconnected()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            stopTxNowButton.Invoke((MethodInvoker)delegate { stopTxNowButton.Enabled = true; });
            sendTextButton.Invoke((MethodInvoker)delegate { sendTextButton.Enabled = false; });
            connectToStationButton.Invoke((MethodInvoker)delegate { connectToStationButton.Enabled = true; });
            disconnectFromStationButton.Invoke((MethodInvoker)delegate { disconnectFromStationButton.Enabled = false; });
            abortConnectionButton.Invoke((MethodInvoker)delegate { abortConnectionButton.Enabled = false; });
            Values.stationConnected = false;
        }
        private void disableButtonsDisconnected()
        {
            connectToVaraButton.Enabled = true;
            disconnectFromVaraButton.Enabled = false;
            sendBeaconTimerButton.Enabled = false;
            sendBeaconNowButton.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            stopTxNowButton.Enabled = true;
            sendTextButton.Enabled = false;
            connectToStationButton.Enabled = false;
            disconnectFromStationButton.Enabled = false;
            abortConnectionButton.Enabled = false;
            Values.stationConnected = false;
        }
        // VARA DATA Channel connected.
        private void OnDataConnect(bool status)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            switch (status)
            {
                case true:
                    this.BeginInvoke((Action)(() => toolStripStatusLabel4.Text = "Connected"));
                    this.BeginInvoke((Action)(() => toolStripStatusLabel4.BackColor = Color.Green));
                    break;
                case false:
                    this.BeginInvoke((Action)(() => toolStripStatusLabel4.Text = "Disconnected"));
                    this.BeginInvoke((Action)(() => toolStripStatusLabel4.BackColor = Color.Red));
                    break;
                default:
                    this.BeginInvoke((Action)(() => toolStripStatusLabel4.Text = "Unknown"));
                    this.BeginInvoke((Action)(() => toolStripStatusLabel4.BackColor = Color.Yellow));
                    break;
            }
        }
        // VARA DATA channel Recieved Listner
        private void OnDataRecieved(string data)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            richTextBox1.Invoke(UpdateRichTextBoxDeligate, data, Color.DarkBlue);
            string error = "<ERR> Not supported!";
            // recieved tag handeling
            switch (data)
            {
                case string a when data.Contains(RecievedTag.signal): //  = "<R";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, "<R" + Values.signalNoice + ">");
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.fullcall): //  = "<FC:";
                    //sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, error);
                    //sendText();
                    break;
                case string a when data.Contains(RecievedTag.name): //  = "<NAME:";
                    //sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, error);
                    //sendText();
                    break;
                case string a when data.Contains(RecievedTag.qth): //  = "<QTH:";
                    //sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, error);
                    //sendText();
                    break;
                case string a when data.Contains(RecievedTag.locator): //  = "<LOC:";
                    //sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, error);
                    //sendText();
                    break;
                case string a when data.Contains(RecievedTag.callsign): // = "<CALL>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, Values.callSign);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.namerequest): // = "<NAME>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, "<NAME:" + Values.name + ">");
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.qthrequest): //  = "<QTH>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, "<QTH:" + Values.QTH + ">");
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.locatorrequest): //  = "<LOC>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, "<LOC:" + Values.grid + ">");
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.rig): //  = "<RIG>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, Values.rig);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.antenna): //  = "<ANT>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, error);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.hcall): //  = "<HCALL>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, error);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.hname): //  = "<HNAME>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, error);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.hqth): //  = "<HQTH>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, error);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.hlocator): //  = "<HLOC>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, error);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.lastheard): //  = "<LHR>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, error);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.qsyu): //  = "<QSYU>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, error);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.qsyd): //  = "<QSYD>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, error);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.version): //  = "<VER>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, SendTag.version);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.info): //  = "<INFO>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, error);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.snrrequest): //  = "<SNRR>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, error);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.away): //  = "<AWAY>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, "I am sorry you are away.");
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.disconnect): //  = "<DISC>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, SendTag.disconnect);
                    sendText();
                    break;
            }

        }
        private void updateRichTextBox(string _data, Color color)
        {
            Log.Info(_data.ToString());
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = color;
            richTextBox1.AppendText(_data);
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
        private void UpdateSatusIcons(int type, bool status)
        {
            //Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            if (type == 1)
            {
                this.BeginInvoke((Action)(() => toolStripStatusLabel6.BackColor = Color.Red));
                this.BeginInvoke((Action)(() => toolStripStatusLabel6.Text = "BUSY"));
            }
            if (type == 0)
            {
                this.BeginInvoke((Action)(() => toolStripStatusLabel6.BackColor = Color.Green));
                this.BeginInvoke((Action)(() => toolStripStatusLabel6.Text = "FREE"));
            }
            if (type == 2)
            {
                this.BeginInvoke((Action)(() => toolStripStatusLabel7.BackColor = Color.Yellow));
                //Thread.Sleep(500);
                this.BeginInvoke((Action)(() => toolStripStatusLabel7.BackColor = SystemColors.Control));
            }
        }
        private void connectToVaraModem()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            // Opening Command TCP client

            try
            {
                varaCmd = new VARACommandClient(Values.VARACommandClientIP, Values.VARACommandClientPort);
                varaCmd.OnConnectEvent += new VARACommandClient.OnConnectEventHandler(OnCommandConnect);
                varaCmd.OnDataRecievedEvent += new VARACommandClient.DataReceivedEventHandler(OnCommandRecieved);
                varaCmd.CommandConnect();
                //updateRecievedFromModemTextBox("\n Vara command channel is connected...\n");
            }
            catch (Exception ex)
            {
                // Catch errors in Connection and Recieve Callbacks
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateRecievedFromModemTextBox("\n Vara command channel error : " + ex.ToString());
            }
            // Opening Data TCP Client
            try
            {
                varaData = new VARADataClient(Values.VARADataClientIP, Values.VARADataClientPort);
                varaData.OnConnectEvent += new VARADataClient.OnConnectEventHandler(OnDataConnect);
                varaData.OnDataRecievedEvent += new VARADataClient.DataReceivedEventHandler(OnDataRecieved);
                varaData.VARADataClientConnect();
                //updateRecievedFromModemTextBox("\n Vara data channel is connected...\n");
            }
            catch (Exception ex)
            {
                // Catch errors in Connection and Recieve Callbacks
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateRecievedFromModemTextBox("\n Vara data channel error : " + ex.ToString());
            }
            Values.varaConnected = true;
        }
        private void chatOn()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());

            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.chatON))
                {
                    updateRecievedFromModemTextBox(Values.outgoing + VaraCMD.chatON);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateRecievedFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateRecievedFromModemTextBox("Error : " + ex.ToString());
            }

        }
        private void listenON()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.listenON))
                {
                    updateRecievedFromModemTextBox(Values.outgoing + VaraCMD.listenON);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateRecievedFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateRecievedFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void listenCQ()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.listenCQ))
                {
                    updateRecievedFromModemTextBox(Values.outgoing + VaraCMD.listenCQ);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateRecievedFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateRecievedFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void bw500()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.bw500))
                {
                    updateRecievedFromModemTextBox(Values.outgoing + VaraCMD.bw500);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateRecievedFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateRecievedFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void bw2300()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.bw2300))
                {
                    updateRecievedFromModemTextBox(Values.outgoing + VaraCMD.bw2300);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateRecievedFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateRecievedFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void bw2750()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.bw2750))
                {
                    updateRecievedFromModemTextBox(Values.outgoing + VaraCMD.bw2750);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateRecievedFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateRecievedFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void myCall(string call)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.myCall(call)))
                {
                    updateRecievedFromModemTextBox(Values.outgoing + VaraCMD.myCall(call));
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateRecievedFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateRecievedFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void sendBeacon()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            if (!Values.busy)
                try
                {
                    if (varaCmd.VARACommandClientWrite("CQFRAME " + Values.callSign + "-9 500\r"))
                    {
                        updateRecievedFromModemTextBox(Values.outgoing + "CQFRAME " + Values.callSign + "-9 500\r");
                        Log.Info("CQFRAME " + Values.callSign + "-9 500\r");
                    }
                    else
                    {
                        Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                        updateRecievedFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                    }
                }
                catch (Exception ex)
                {
                    // Catch errors in Sending Data
                    Log.Error(ex.ToString());
                    Log.Error(ex.Message.ToString());
                    updateRecievedFromModemTextBox("Error : " + ex.ToString());
                }
        }
        private void connectToStation(string callToConnect)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.connect(Values.callSign, callToConnect)))
                {
                    updateRecievedFromModemTextBox(Values.outgoing + VaraCMD.connect(Values.callSign, callToConnect));
                    Values.outGoingConnection = true;
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateRecievedFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateRecievedFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void disconnectFromStation()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.disconnect))
                {
                    updateRecievedFromModemTextBox(Values.outgoing + VaraCMD.disconnect);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateRecievedFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateRecievedFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void abortConnectedStation()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.abort))
                {
                    updateRecievedFromModemTextBox(Values.outgoing + VaraCMD.abort);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateRecievedFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateRecievedFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void sendText()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            string textToSend = sendTextTextBox.Text.ToString();
            try
            {
                if (varaData.VARADataClientWrite(textToSend + "\r\n"))
                {
                    Log.Info(textToSend);
                    Values.sendBufferSize = textToSend.Length;
                    Values.sendBuffer = textToSend.Length;
                    richTextBox1.Invoke(UpdateRichTextBoxDeligate, "\r\n" + textToSend, Color.DarkRed);
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, "");
                }
                else
                {
                    Log.Error("VARADataClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                Log.Error("VARADataClientWrite (Failed) : Disconnected");
            }
        }
        // Send Text to Data Channel.
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());

            sendTextTextBox.Text = comboBox1.SelectedItem.ToString();
        }
        private void setDataGrid()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "Time";
            dataGridView1.Columns[1].Name = "CallSign";
            dataGridView1.Columns[2].Name = "SID";
            dataGridView1.Columns[3].Name = "SNR";
            dataGridView1.Columns[4].Name = "Cnt";
            dataGridView1.Columns[5].Name = "SM";
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            for (int i = 0; i <= dataGridView1.Columns.Count - 1; i++)
            {
                // Store Auto Sized Widths:
                int colw = dataGridView1.Columns[i].Width;
                // Remove AutoSizing:
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                // Set Width to calculated AutoSize value:
                dataGridView1.Columns[i].Width = colw;
            }
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Descending);
            dataGridView1.AutoResizeColumns();
        }
        private void addLastHeardFileToDataGrid()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            List<stations> fromfile = new List<stations>();

            fromfile = Functions.ReadXML<stations>("LastHeard.xml");

            foreach (var station in fromfile)
            {
                DateTime dateTime = DateTime.ParseExact(station.UTCTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                string search = station.Call;
                int rowIndex = -1;
                foreach (DataGridViewRow rows in dataGridView1.Rows)
                {
                    if (rows.Cells[1].Value.ToString().Equals(search))
                    {
                        rowIndex = rows.Index;
                        dataGridView1.Rows.RemoveAt(rowIndex);
                        break;
                    }
                }
                lastHeard.Add(new stations { UTCTime = dateTime.ToString(), Call = station.Call, SID = station.SID, SNR = station.SNR, SM = station.SM, Cnt = station.Cnt });
                dataGridView1.Rows.Add(dateTime.ToString("HH:mm:ss"), station.Call, station.SID, station.SNR, station.Cnt, station.SM);
                dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Descending);
                dataGridView1.AutoResizeColumns();
            }


        }
        private void setColors()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            // From:
            this.BackColor = Color.FromArgb(255, 31, 31, 31);
            this.ForeColor = Color.White;

            // Buttons:
            connectToVaraButton.BackColor = SystemColors.ControlDark;
            connectToVaraButton.ForeColor = Color.White;
            disconnectFromVaraButton.BackColor = SystemColors.ControlDark;
            disconnectFromVaraButton.ForeColor = Color.White;
            sendBeaconTimerButton.BackColor = SystemColors.ControlDark;
            sendBeaconTimerButton.ForeColor = Color.White;
            sendBeaconNowButton.BackColor = SystemColors.ControlDark;
            sendBeaconNowButton.ForeColor = Color.White;
            button5.BackColor = SystemColors.ControlDark;
            button5.ForeColor = Color.White;
            button6.BackColor = SystemColors.ControlDark;
            button6.ForeColor = Color.White;
            stopTxNowButton.BackColor = SystemColors.ControlDark;
            stopTxNowButton.ForeColor = Color.White;
            sendTextButton.BackColor = SystemColors.ControlDark;
            sendTextButton.ForeColor = Color.White;
            connectToStationButton.BackColor = SystemColors.ControlDark;
            connectToStationButton.ForeColor = Color.White;
            disconnectFromStationButton.BackColor = SystemColors.ControlDark;
            disconnectFromStationButton.ForeColor = Color.White;
            abortConnectionButton.BackColor = SystemColors.ControlDark;
            abortConnectionButton.ForeColor = Color.White;
            button12.BackColor = SystemColors.ControlDark;
            button12.ForeColor = Color.White;

            // Tool Strips:
            toolStripStatusLabel1.BackColor = SystemColors.ControlDark;
            toolStripStatusLabel1.ForeColor = Color.White;
            toolStripStatusLabel2.BackColor = SystemColors.ControlDark;
            toolStripStatusLabel2.ForeColor = Color.White;
            toolStripStatusLabel3.BackColor = SystemColors.ControlDark;
            toolStripStatusLabel3.ForeColor = Color.White;
            toolStripStatusLabel4.BackColor = SystemColors.ControlDark;
            toolStripStatusLabel4.ForeColor = Color.White;
            toolStripStatusLabel5.BackColor = SystemColors.ControlDark;
            toolStripStatusLabel5.ForeColor = Color.White;
            toolStripStatusLabel6.BackColor = SystemColors.ControlDark;
            toolStripStatusLabel6.ForeColor = Color.White;
            toolStripStatusLabel7.BackColor = SystemColors.ControlDark;
            toolStripStatusLabel7.ForeColor = Color.White;
            toolStripStatusLabel8.BackColor = SystemColors.ControlDark;
            toolStripStatusLabel8.ForeColor = Color.White;
            toolStripStatusLabel9.BackColor = SystemColors.ControlDark;
            toolStripStatusLabel9.ForeColor = Color.White;

            // Labels:
            label1.BackColor = SystemColors.ControlDark;
            label1.ForeColor = Color.White;
            label2.BackColor = SystemColors.ControlDark;
            label2.ForeColor = Color.White;
            label3.BackColor = SystemColors.ControlDark;
            label3.ForeColor = Color.White;
            label4.BackColor = SystemColors.ControlDark;
            label4.ForeColor = Color.White;
            label5.BackColor = SystemColors.ControlDark;
            label5.ForeColor = Color.White;
            label6.BackColor = SystemColors.ControlDark;
            label6.ForeColor = Color.White;

            // Data Grid
            dataGridView1.DefaultCellStyle.ForeColor = Color.White;
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(255, 31, 31, 31);
            dataGridView1.DefaultCellStyle.Font = new Font("Consolas", 8);
            dataGridView1.Font = new Font("Consolas", 8);
            dataGridView1.ForeColor = Color.White;
            dataGridView1.BackgroundColor = Color.FromArgb(255, 31, 31, 31);



        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            Functions.WriteXML<stations>(lastHeard, "LastHeard.xml");
            //this.Close();
            System.Windows.Forms.Application.Exit();

        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settingsForm = new Settings();
            settingsForm.Show();
        }
    }
}


