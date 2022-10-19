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
        public delegate void AddFromModemDeligate(string data);
        public AddFromModemDeligate UpdateFromModemTextBoxDeligate;

        public delegate void AddSendTextDeligate(string data);
        public AddSendTextDeligate UpdateSendTextTextBoxDeligate;

        public delegate void AddRichDeligate(string data, Color color);
        public AddRichDeligate UpdateRichTextBoxDeligate;

        public delegate void AddDataGridDeligate(string data);
        public AddDataGridDeligate UpdateDataGridDeligate;

        //public AddLogDeligate UpdateSendToModemTextBoxDeligate;
        //public AddLogDeligate UpdateCallSignTextBoxDeligate;

        //public delegate void AddNotificationDelegate(int type, bool status);
        //public AddNotificationDelegate UpdateStatusIcons;
        // Client Object
        VARACommandClient varaCmd;
        VARADataClient varaData;
        VARAMonitorCommandClient varaMonitorCmd;
        // VARAKISSClient varaKISS;

        public List<station> lastHeard = new List<station>();

        #endregion

        #region Form
        public Form1()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            InitializeComponent();
            UpdateFromModemTextBoxDeligate = new AddFromModemDeligate(updateFromModemTextBox);
            UpdateSendTextTextBoxDeligate = new AddSendTextDeligate(updateSendTextTextBox);
            UpdateRichTextBoxDeligate = new AddRichDeligate(updateRichTextBox);
            UpdateDataGridDeligate = new AddDataGridDeligate(updateLastHeardDataGrid);


            //UpdateSendToModemTextBoxDeligate = new AddLogDeligate(updateSendToModemTextBox);
            //UpdateCallSignTextBoxDeligate = new AddLogDeligate(updateCallSignTextBox);
            //UpdateStatusIcons = new AddNotificationDelegate(UpdateSatusIcons);
            RigControl rigControlForm = new RigControl();
            rigControlForm.Show();
            timer1.Interval = 100;
            beaconTimer.Interval = 300000; //60000 = 1 minuut, 300000 = 5 minuten ;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            setColors();
            setDataGrid();
            connectToVaraModem();
            setbw500();
            setlistenON();
            setchatOn();
            setlistenCQ();
            setMyCall(Values.stationDetails.CallSign);
            disableButtonsDisconnected();
            addLastHeardFileToDataGrid();
            timer1.Start();

            if (Values.VARAMonitorEnabled)
            {
                connectToVaraMonitorModem();
                setMonitorbw500();
                setMonitorlistenON();

            }

        }

        #endregion

        #region Buttons
        // Connect to VARA modem Button
        private void button1_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            connectToVaraModem();
            setbw500();
            setlistenON();
            setlistenCQ();
            setchatOn();
            setMyCall(Values.stationDetails.CallSign);
            if (Values.VARAMonitorEnabled)
                connectToVaraMonitorModem();
        }
        // Disconnect to VARA modem Button
        private void button2_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            varaCmd.VARACommandClientDisconnect();
            varaData.VARADataClientDisconnect();
            if(Values.VARAMonitorEnabled)
                varaMonitorCmd.VARAMonitorCommandClientDisconnect();
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
            sendCQ();
            Values.sendCQ = true;
        }
        // Spare Button (Now saves the LastHeard list to a XML file)
        private void button6_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            connectToVaraMonitorModem();
            setMonitorbw500();
            setMonitorlistenON();

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
            abortConnectionButton.Enabled = true;
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
            // Opening Data TCP Client
            try
            {
                varaData = new VARADataClient(Values.VARADataClientIP, Values.VARADataClientPort);
                varaData.OnConnectEvent += new VARADataClient.OnConnectEventHandler(OnDataConnect);
                varaData.OnDataRecievedEvent += new VARADataClient.DataReceivedEventHandler(OnDataRecieved);
                varaData.VARADataClientConnect();
                //updateFromModemTextBox("\n Vara data channel is connected...\n");
            }
            catch (Exception ex)
            {
                // Catch errors in Connection and Recieve Callbacks
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("\n Vara data channel error : " + ex.ToString()); // Changed
            }

        }
        #endregion

        #region Timers
        // timer1 updates the clock on the status strip. can be used for recuring things.
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel7.Text = "UTC: " + DateTime.UtcNow.ToString("HH:mm:ss");
            sendBufferProgressBar.Maximum = 256; //Values.sendBufferSize;
            sendBufferProgressBar.Value = Values.sendBuffer;
            if (!Values.stationConnected)
                sendBufferProgressBar.Value = 0;
            recieveBufferProgressBar.Maximum = 256; // Values.recieveBufferSize;
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

        #region Connection Listeners
        // Connection Status Listner for the VARA Command channel
        private void OnCommandConnect(bool status)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            switch (status)
            {
                case true:
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.Text = "Vara Connected"));
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.BackColor = Color.Green));
                    break;
                case false:
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.Text = "Vara Disconnected"));
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.BackColor = Color.Red));
                    break;
                default:
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.Text = "Unknown"));
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.BackColor = Color.Yellow));
                    break;
            }
        }
        // Connection Status Listner for the VARA Data channel
        private void OnDataConnect(bool status)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            switch (status)
            {
                case true:
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.Text = "Vara Connected"));
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.BackColor = Color.Green));
                    break;
                case false:
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.Text = "Vara Disconnected"));
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.BackColor = Color.Red));
                    break;
                default:
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.Text = "Unknown"));
                    this.BeginInvoke((Action)(() => toolStripStatusLabel2.BackColor = Color.Yellow));
                    break;
            }
        }
        // Connection Status Listner for the VARA Monitor Command channel
        private void OnMonitorCommandConnect(bool status)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            //switch (status)
            //{
            //    case true:
            //        this.BeginInvoke((Action)(() => toolStripStatusLabel2.Text = "Connected"));
            //        this.BeginInvoke((Action)(() => toolStripStatusLabel2.BackColor = Color.Green));
            //        break;
            //    case false:
            //        this.BeginInvoke((Action)(() => toolStripStatusLabel2.Text = "Disconnected"));
            //        this.BeginInvoke((Action)(() => toolStripStatusLabel2.BackColor = Color.Red));
            //        break;
            //    default:
            //        this.BeginInvoke((Action)(() => toolStripStatusLabel2.Text = "Unknown"));
            //        this.BeginInvoke((Action)(() => toolStripStatusLabel2.BackColor = Color.Yellow));
            //        break;
            //}
        }
        #endregion

        // VARA Modem Command channel Recieved Listner
        public void OnMonitorCommandRecieved(string data)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            string[] dataRecieved = data.Split('\r');
            foreach (string text in dataRecieved)
            {
                if (text != String.Empty)
                {
                    switch (text)
                    {
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
                        case VARAResult.IAmAlive:
                            Log.Debug(VARAResult.IAmAlive);
                            break;
                        case VARAResult.missingSoundCard:
                            updateFromModemTextBox(Values.incomming + text);
                            break;
                        case VARAResult.ok:
                            updateFromModemTextBox(Values.incomming + text);
                            break;
                        case VARAResult.wrong:
                            updateFromModemTextBox(Values.incomming + text);
                            break;
                        default:
                            updateFromModemTextBox(Values.incomming + text);
                            break;
                    }
                }
            }
        }
        // VARA Command channel Recieved Listener
        public void OnCommandRecieved(string data)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            string[] dataRecieved = data.Split('\r');
            foreach (string text in dataRecieved)
            {
                if (text != String.Empty)
                    switch (text)
                    {
                        case VARAResult.pttOFF:
                            RigControl.disableTX().ConfigureAwait(false);
                            Values.busy = false;
                            this.BeginInvoke((Action)(() => toolStripStatusLabel8.BackColor = Color.Green));
                            this.BeginInvoke((Action)(() => toolStripStatusLabel8.Text = "RX"));
                            break;
                        case VARAResult.pttON:
                            RigControl.enableTX().ConfigureAwait(false);
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
                            updateFromModemTextBoxDeligated(Values.incomming + text);
                            break;
                        case VARAResult.pending:
                            updateFromModemTextBoxDeligated(Values.incomming + text);
                            break;
                        case VARAResult.cancelPending:
                            updateFromModemTextBoxDeligated(Values.incomming + text);
                            if (Values.sendCQ)
                                sendCQ();
                            break;
                        case string a when text.Contains(VARAResult.registered):
                            enableButtonsRegistered();
                            updateFromModemTextBoxDeligated(Values.incomming + text);
                            break;
                        case VARAResult.linkRegistered:
                            enableButtonsStationConnected();
                            updateFromModemTextBox(Values.incomming + text);
                            break;
                        case VARAResult.linkUnRegistered:
                            enableButtonsStationConnected();
                            updateFromModemTextBoxDeligated(Values.incomming + text);
                            break;
                        case VARAResult.IAmAlive:
                            Log.Debug(Values.incomming + VARAResult.IAmAlive);
                            break;
                        case VARAResult.missingSoundCard:
                            updateFromModemTextBoxDeligated(Values.incomming + text);
                            break;
                        case VARAResult.ok:
                            updateFromModemTextBoxDeligated(Values.incomming + text);
                            break;
                        case VARAResult.wrong:
                            updateFromModemTextBoxDeligated(Values.incomming + text);
                            break;
                        case string a when text.Contains("SN "):
                            if (Values.stationConnected)
                            {
                                this.BeginInvoke((Action)(() => textBox5.Text = Functions.Smeter(Values.sMeter)));
                                this.BeginInvoke((Action)(() => textBox6.Text = Functions.Snmeter(text)));
                                fromModemTextBox.Invoke(UpdateFromModemTextBoxDeligate, Values.incomming + text + " " + Functions.Smeter(Values.sMeter));
                                Values.signalNoice = Functions.Snmeter(text);
                            }
                            else
                            {
                                fromModemTextBox.Invoke(UpdateFromModemTextBoxDeligate, Values.incomming + text + " " + Functions.Smeter(Values.sMeter));
                                Values.signalNoice = Functions.Snmeter(text);
                            }
                            break;
                        case string a when text.Contains(VARAResult.cqFrame):
                            updateFromModemTextBoxDeligated(Values.incomming + text);
                            updateLastHeardAsync(text).ConfigureAwait(false); ;
                            break;
                        default:
                            if (text.Contains("BUFFER "))
                            {
                                updateFromModemTextBoxDeligated(Values.incomming + text);
                                string buffer = text.Remove(0, 7);
                                int bufferSize = int.Parse(buffer);
                                Values.sendBuffer = bufferSize;
                                break;
                            }
                            else if (text.Contains("CONNECTED "))
                            {
                                updateFromModemTextBoxDeligated(Values.incomming + text);
                                enableButtonsStationConnected();
                                if (!Values.outGoingConnection)
                                {
                                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, "Welcome to my station. it is still a work in progress... \r\n<LOC:JO22OI>");
                                    sendText();
                                }
                                break;
                            }
                            richTextBox1.Invoke(UpdateRichTextBoxDeligate, text + "\r\n", Color.DarkRed);
                            Log.Info(text);
                            break;
                    }
            }
        }
        // VARA Data channel Reciever Listener
        private void OnDataRecieved(string data)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            richTextBox1.Invoke(UpdateRichTextBoxDeligate, data, Color.DarkBlue);
            string error = "<ERR> Not supported!";
            // recieved tag handeling
            switch (data)
            {
                case string a when data.Contains(RecievedTag.signal): //  = "<R";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, SendTag.signal + Values.signalNoice + SendTag.end);
                    sendText();
                    break;
                //case string a when data.Contains(RecievedTag.fullcall): //  = "<FC:";
                //    //sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, error);
                //    //sendText();
                //    break;
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
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, SendTag.signal + Values.signalNoice + SendTag.end);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.away): //  = "<AWAY>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, "I am sorry that you are away. Maybe next time.");
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.disconnect): //  = "<DISC>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, SendTag.disconnect);
                    sendText();
                    break;
            }

        }

        private void updateFromModemTextBoxDeligated(string text)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            fromModemTextBox.Invoke(UpdateFromModemTextBoxDeligate, text);
        }
        private void updateFromModemTextBox(string text)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            fromModemTextBox.AppendText(DateTime.UtcNow.ToString("HH:mm:ss") + " " + text + Environment.NewLine);
            fromModemTextBox.SelectionStart = fromModemTextBox.Text.Length;
            fromModemTextBox.ScrollToCaret();
            Log.Info(text);
        }

        //private async Task updateCallSignTextBoxAsync(string text)
        //{
        //    Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
        //    await Task.Run(() =>
        //    {
        //        callSignTextBox.Invoke(UpdateCallSignTextBoxDeligate, text);
        //    });
        //}

        //private void updateCallSignTextBox(string _data)
        //{
        //    Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
        //    callSignTextBox.AppendText(DateTime.UtcNow.ToString("HH:mm:ss") + " " + _data + Environment.NewLine);
        //    callSignTextBox.SelectionStart = callSignTextBox.Text.Length;
        //    callSignTextBox.ScrollToCaret();
        //}
        //private async Task updateSendTextTextBoxAsync(string text)

        //{
        //    Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
        //    await Task.Run(() =>
        //    {
        //        sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, text);
        //    });
        //}

        private void updateSendTextTextBox(string text)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            sendTextTextBox.Text = text;
        }

        private async Task updateLastHeardAsync(string text)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            await Task.Run(() =>
            {
                // declare temp strings

                string beaconCallSign = String.Empty;
                string beaconSign = String.Empty;
                
                string removeCQframe = text.Remove(0, 8); // Remove CQ Frame
                string replaceBW500 = removeCQframe.Replace(" 500", ""); // Remove the BW 500
                string beaconCall = replaceBW500.Replace(" ", ""); // Remove spaces if any.
                // split the callsign from the ID if there is any ID.

                if (beaconCall.Contains("-"))
                {
                    string[] ids = beaconCall.Split('-');
                    beaconCallSign = ids[0];
                    beaconSign = ids[1];
                }
                else
                    beaconCallSign = beaconCall;

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
                    dataGridView1.Rows.Add(DateTime.UtcNow.ToString("HH:mm:ss"), beaconCallSign, beaconSign, Functions.Snmeter(Values.signalNoice), count, Functions.Smeter(Values.sMeter));

                    lastHeard.Add(new station { UTCTime = DateTime.UtcNow.ToString(), Call = beaconCallSign, SID = beaconSign, SNR = Functions.Snmeter(Values.signalNoice), SM = Functions.Smeter(Values.sMeter), Cnt = count.ToString() });


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
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());

                this.BeginInvoke((Action)(() => connectToVaraButton.Enabled = true));
                this.BeginInvoke((Action)(() => disconnectFromVaraButton.Enabled = false));
                this.BeginInvoke((Action)(() => sendBeaconTimerButton.Enabled = false));
                this.BeginInvoke((Action)(() => sendBeaconNowButton.Enabled = false));
                this.BeginInvoke((Action)(() => button5.Enabled = false));
                this.BeginInvoke((Action)(() => button6.Enabled = false));
                this.BeginInvoke((Action)(() => stopTxNowButton.Enabled = true));
                this.BeginInvoke((Action)(() => sendTextButton.Enabled = false));
                this.BeginInvoke((Action)(() => connectToStationButton.Enabled = false));
                this.BeginInvoke((Action)(() => disconnectFromStationButton.Enabled = false));
                this.BeginInvoke((Action)(() => abortConnectionButton.Enabled = false));
                Values.stationConnected = false;


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
        //private void UpdateSatusIcons(int type, bool status)
        //{
        //    //Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
        //    if (type == 1)
        //    {
        //        this.BeginInvoke((Action)(() => toolStripStatusLabel6.BackColor = Color.Red));
        //        this.BeginInvoke((Action)(() => toolStripStatusLabel6.Text = "BUSY"));
        //    }
        //    if (type == 0)
        //    {
        //        this.BeginInvoke((Action)(() => toolStripStatusLabel6.BackColor = Color.Green));
        //        this.BeginInvoke((Action)(() => toolStripStatusLabel6.Text = "FREE"));
        //    }
        //    if (type == 2)
        //    {
        //        this.BeginInvoke((Action)(() => toolStripStatusLabel7.BackColor = Color.Yellow));
        //        //Thread.Sleep(500);
        //        this.BeginInvoke((Action)(() => toolStripStatusLabel7.BackColor = SystemColors.Control));
        //    }
        //}
        private void connectToVaraModem()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            // Opening Command TCP client

            try
            {
                varaCmd = new VARACommandClient(Values.VARACommandClientIP, Values.VARACommandClientPort);
                varaCmd.OnConnectEvent += new VARACommandClient.OnConnectEventHandler(OnCommandConnect);
                varaCmd.OnDataRecievedEvent += new VARACommandClient.DataReceivedEventHandler(OnCommandRecieved);
                varaCmd.VARACommandConnect();
                //updateFromModemTextBox("\n Vara command channel is connected...\n");
            }
            catch (Exception ex)
            {
                // Catch errors in Connection and Recieve Callbacks
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("\n Vara command channel error : " + ex.ToString());
            }
            // Opening Data TCP Client
            try
            {
                varaData = new VARADataClient(Values.VARADataClientIP, Values.VARADataClientPort);
                varaData.OnConnectEvent += new VARADataClient.OnConnectEventHandler(OnDataConnect);
                varaData.OnDataRecievedEvent += new VARADataClient.DataReceivedEventHandler(OnDataRecieved);
                varaData.VARADataClientConnect();
                //updateFromModemTextBox("\n Vara data channel is connected...\n");
            }
            catch (Exception ex)
            {
                // Catch errors in Connection and Recieve Callbacks
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("\n Vara data channel error : " + ex.ToString());
            }
            Values.varaConnected = true;
        }
        private void connectToVaraMonitorModem()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            // Opening Command TCP client

            try
            {
                varaMonitorCmd = new VARAMonitorCommandClient(Values.VARAMonitorCommandClientIP, Values.VARAMonitorCommandClientPort);
                varaMonitorCmd.OnConnectEvent += new VARAMonitorCommandClient.OnConnectEventHandler(OnMonitorCommandConnect);
                varaMonitorCmd.OnDataRecievedEvent += new VARAMonitorCommandClient.DataReceivedEventHandler(OnMonitorCommandRecieved);
                varaMonitorCmd.VARAMonitorCommandConnect();
                //updateFromModemTextBox("\n Vara command channel is connected...\n");
            }
            catch (Exception ex)
            {
                // Catch errors in Connection and Recieve Callbacks
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("\n Vara command channel error : " + ex.ToString());
            }
            // Opening Data TCP Client
            //try
            //{
            //    varaData = new VARADataClient(Values.VARADataClientIP, Values.VARADataClientPort);
            //    varaData.OnConnectEvent += new VARADataClient.OnConnectEventHandler(OnDataConnect);
            //    varaData.OnDataRecievedEvent += new VARADataClient.DataReceivedEventHandler(OnDataRecieved);
            //    varaData.VARADataClientConnect();
            //    //updateFromModemTextBox("\n Vara data channel is connected...\n");
            //}
            //catch (Exception ex)
            //{
            //    // Catch errors in Connection and Recieve Callbacks
            //    Log.Error(ex.ToString());
            //    Log.Error(ex.Message.ToString());
            //    updateFromModemTextBox("\n Vara data channel error : " + ex.ToString());
            //}
            Values.varaConnected = true;
        }
        private void setchatOn()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());

            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.chatON))
                {
                    updateFromModemTextBox(Values.outgoing + VaraCMD.chatON);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("Error : " + ex.ToString());
            }

        }
        private void setlistenON()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.listenON))
                {
                    updateFromModemTextBox(Values.outgoing + VaraCMD.listenON);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void setMonitorlistenON()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaMonitorCmd.VARAMonitorCommandClientWrite(VaraCMD.listenON))
                {
                    updateFromModemTextBox(Values.outgoing + VaraCMD.listenON);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void setlistenCQ()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.listenCQ))
                {
                    updateFromModemTextBox(Values.outgoing + VaraCMD.listenCQ);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void setbw500()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.bw500))
                {
                    updateFromModemTextBox(Values.outgoing + VaraCMD.bw500);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void setMonitorbw500()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaMonitorCmd.VARAMonitorCommandClientWrite(VaraCMD.bw500))
                {
                    updateFromModemTextBox(Values.outgoing + VaraCMD.bw500);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void setbw2300()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.bw2300))
                {
                    updateFromModemTextBox(Values.outgoing + VaraCMD.bw2300);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void setbw2750()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.bw2750))
                {
                    updateFromModemTextBox(Values.outgoing + VaraCMD.bw2750);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void setMyCall(string call)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.myCall(call)))
                {
                    updateFromModemTextBox(Values.outgoing + VaraCMD.myCall(call));
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void sendBeacon()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            if (!Values.busy)
                try
                {
                    if (varaCmd.VARACommandClientWrite("CQFRAME " + Values.stationDetails.CallSign + "-9 500\r"))
                    {
                        updateFromModemTextBox(Values.outgoing + "CQFRAME " + Values.stationDetails.CallSign + "-9 500\r");
                        Log.Info("CQFRAME " + Values.stationDetails.CallSign + "-9 500\r");
                        this.BeginInvoke((Action)(() => toolStripStatusLabel10.Text = DateTime.UtcNow.ToString("HH:mm:ss")));
                        this.BeginInvoke((Action)(() => toolStripStatusLabel10.BackColor = Color.Green));

                    }
                    else
                    {
                        Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                        updateFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                        toolStripStatusLabel10.BackColor = Color.Yellow;
                    }
                }
                catch (Exception ex)
                {
                    // Catch errors in Sending Data
                    Log.Error(ex.ToString());
                    Log.Error(ex.Message.ToString());
                    updateFromModemTextBox("Error : " + ex.ToString());
                    toolStripStatusLabel10.BackColor = Color.Yellow;
                }
            else
            {
                updateFromModemTextBox("Beacon failed, Frequency busy.");
                Log.Info("Beacon failed, Frequency busy.");
                toolStripStatusLabel10.BackColor = Color.Red;
            }
            Values.sendCQ = false;
        }
        private void sendCQ()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            if (!Values.busy)
                try
                {
                    if (varaCmd.VARACommandClientWrite("CQFRAME " + Values.stationDetails.CallSign + "-9 500\r"))
                    {
                        fromModemTextBox.Invoke(UpdateFromModemTextBoxDeligate, Values.outgoing + "CQFRAME " + Values.stationDetails.CallSign + "-9 500\r");
                        Log.Info("CQFRAME " + Values.stationDetails.CallSign + "-9 500\r");
                        Values.sendCQ = true;
                    }
                    else
                    {
                        Values.sendCQ = false;
                        Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                        fromModemTextBox.Invoke(UpdateFromModemTextBoxDeligate, "VARACommandClientWrite (Failed) : Disconnected");
                    }
                }
                catch (Exception ex)
                {
                    Values.sendCQ = false;
                    // Catch errors in Sending Data
                    Log.Error(ex.ToString());
                    Log.Error(ex.Message.ToString());
                    fromModemTextBox.Invoke(UpdateFromModemTextBoxDeligate, "Error written in log file!");
                }
            else
            {
                Values.sendCQ = false;

                fromModemTextBox.Invoke(UpdateFromModemTextBoxDeligate, "CQ failed, Frequency busy.");
                Log.Info("CQ failed, Frequency busy.");
            }
            Values.sendCQ = false;
        }
        private void connectToStation(string callToConnect)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.connect(Values.stationDetails.CallSign, callToConnect)))
                {
                    updateFromModemTextBox(Values.outgoing + VaraCMD.connect(Values.stationDetails.CallSign, callToConnect));
                    Values.outGoingConnection = true;
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void disconnectFromStation()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.disconnect))
                {
                    updateFromModemTextBox(Values.outgoing + VaraCMD.disconnect);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("Error : " + ex.ToString());
            }
        }
        private void abortConnectedStation()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.abort))
                {
                    updateFromModemTextBox(Values.outgoing + VaraCMD.abort);
                }
                else
                {
                    Log.Error("VARACommandClientWrite (Failed) : Disconnected");
                    updateFromModemTextBox("VARACommandClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                updateFromModemTextBox("Error : " + ex.ToString());
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
            List<station> fromfile = new List<station>();

            fromfile = Functions.ReadLastHeardXML<station>("LastHeard.xml");

            foreach (var station in fromfile)
            {
                DateTime dateTime = DateTime.ParseExact(station.UTCTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                
                // check for duplicates in datagrid.
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
                
                dataGridView1.Rows.Add(dateTime.ToString("HH:mm:ss"), station.Call, station.SID, station.SNR, station.Cnt, station.SM);
                dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Descending);
                dataGridView1.AutoResizeColumns();

                var obj = lastHeard.FirstOrDefault<station>(x => x.Call == search);
                if (obj == null)
                    lastHeard.Add(new station { UTCTime = dateTime.ToString(), Call = station.Call, SID = station.SID, SNR = station.SNR, SM = station.SM, Cnt = station.Cnt });
                //if (obj != null)
                //    lastHeard.Add(new station { UTCTime = dateTime.ToString(), Call = station.Call, SID = station.SID, SNR = station.SNR, SM = station.SM, Cnt = station.Cnt });
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
            Functions.WriteLastHeardXML<station>(lastHeard, "LastHeard.xml");
            System.Windows.Forms.Application.Exit();
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settingsForm = new Settings();
            settingsForm.Show();
        }
        private void toolStripStatusLabel9_Click(object sender, EventArgs e)
        {

        }
    }
}


