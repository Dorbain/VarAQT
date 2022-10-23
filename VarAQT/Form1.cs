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
using VarAQT;
using Logger;
using System.Media;
using static System.Net.Mime.MediaTypeNames;
using VarAQT.Models;
using System.Xml.Serialization;
using System.Globalization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.Common;

namespace VarAQT
{
    public partial class varAQT : Form
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

        public Strings _strings = new Strings();



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
        public varAQT()
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
            beaconTimer.Interval = Properties.Settings.Default.BeaconInterval * 60000; // 300000; //60000 = 1 minuut, 300000 = 5 minuten ;
            Values.BaseChannelFrequency = Channel.Channel9;
            Values.BaseFrequencyBand = Band.MeterIs20;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            setColorsAndText();
            setDataGrid();
            connectToVaraModem();
            sendCommandToVaraModem(VaraCMD.bw500);
            sendCommandToVaraModem(VaraCMD.listenON);
            sendCommandToVaraModem(VaraCMD.listenCQ);
            sendCommandToVaraModem(VaraCMD.chatON);
            sendCommandToVaraModem(VaraCMD.myCall(Values.stationDetails.CallSign));
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
            sendCommandToVaraModem(VaraCMD.bw500);
            sendCommandToVaraModem(VaraCMD.listenON);
            sendCommandToVaraModem(VaraCMD.listenCQ);
            sendCommandToVaraModem(VaraCMD.chatON);
            sendCommandToVaraModem(VaraCMD.myCall(Values.stationDetails.CallSign));
            if (Values.VARAMonitorEnabled)
                connectToVaraMonitorModem();
        }
        // Disconnect to VARA modem Button
        private void button2_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            sendCommandToVaraModem(VaraCMD.chatOFF);
            sendCommandToVaraModem(VaraCMD.listenOFF);
            sendCommandToVaraModem(VaraCMD.bw2750);
            varaCmd.VARACommandClientDisconnect();
            varaData.VARADataClientDisconnect();
            if (Values.VARAMonitorEnabled)
                varaMonitorCmd.VARAMonitorCommandClientDisconnect();
            disableButtonsDisconnected();
        }
        // Start Beacon Timer Button
        private void button3_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            switch (Values.BeaconActive)
            {
                case true:
                    Values.BeaconActive = false;
                    beaconTimer.Stop();
                    sendBeaconTimerButton.BackColor = Color.DarkSeaGreen;
                    break;
                case false:
                    Values.BeaconActive = true;
                    beaconTimer.Start();
                    sendBeaconTimerButton.BackColor = Color.DarkRed;
                    break;
            }
        }
        // Send Beacon Now Button
        private void button4_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            if (!Values.Busy)
                if (!Values.StationConnected)
                {
                    lastBeaconTimeToolStripStatusLabel.Text = DateTime.UtcNow.ToString("HH:mm:ss");
                    sendCommandToVaraModem(VaraCMD.cqFrame500(Values.stationDetails.CallSign + "-9"));
                    Values.BeaconActive = true;
                    Values.SendCQ = false;
                }


        }
        // Call for CQ
        private void button5_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            if (!Values.Busy)
                if (!Values.StationConnected)
                {
                    sendCommandToVaraModem(VaraCMD.cqFrame500(Values.stationDetails.CallSign + Values.VaraSIDToUse));
                    Values.SendCQ = true;
                }
        }
        // Spare Button (Now saves the LastHeard list to a XML file)
        private void button6_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            connectToVaraMonitorModem();
            setMonitorbw500();
            setMonitorlistenON();

        }
        // Stop beacon and TXStr Button
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
            sendCommandToVaraModem(VaraCMD.connect(Values.stationDetails.CallSign, callToConnect));
        }
        // Disconnect from station Button
        private void button10_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            sendCommandToVaraModem(VaraCMD.disconnect);
        }
        // Abort the connection Button
        private void button11_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            sendCommandToVaraModem(VaraCMD.abort);
        }
        // Ping a station.
        private void button12_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            Values.Ping = true;
            string callToConnect = callSignTextBox.Text.ToString();
            Values.connectedStationDetails.CallSign = callToConnect;
            abortConnectionButton.Enabled = true;
            sendCommandToVaraModem(VaraCMD.connect(Values.stationDetails.CallSign, callToConnect + "-T"));
            Values.OutGoingConnection = true; // makes sure the welcome message is not send.
        }
        #endregion

        #region Timers
        // timer1 updates the clock on the status strip. can be used for recuring things.
        private void timer1_Tick(object sender, EventArgs e)
        {
            utcTimeToolStripStatusLabel.Text = "UTC: " + DateTime.UtcNow.ToString("HH:mm:ss");
            switch (Values.StationConnected)
            {
                case true:
                    sendBufferProgressBar.Maximum = Values.SendBufferMaxSize;
                    recieveBufferProgressBar.Maximum = Values.RecieveBufferMaxSize;
                    if (Values.SendBuffer >= 0)
                        sendBufferProgressBar.Value = Values.SendBuffer;
                    if (Values.RecieveBuffer >= 0)
                        recieveBufferProgressBar.Value = Values.RecieveBuffer;
                    label10.Text = Values.connectedStationDetails.CallSign;
                    label11.Text = Values.connectedStationDetails.Locator;
                    label12.Text = Values.connectedStationDetails.rcv;
                    break;
                case false:
                    sendBufferProgressBar.Value = 0;
                    recieveBufferProgressBar.Value = 0;
                    //label10.Text = String.Empty;
                    //label11.Text = String.Empty;
                    //label12.Text = String.Empty;
                    break;
            }
        }
        // send beacon timer.
        private void timer2_Tick(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            if (!Values.Busy)
                if (!Values.StationConnected)
                {
                    sendCommandToVaraModem(VaraCMD.cqFrame500(Values.stationDetails.CallSign + "-9"));
                    lastBeaconTimeToolStripStatusLabel.Text = DateTime.UtcNow.ToString("HH:mm:ss");
                    Values.BeaconActive = true;
                    Values.SendCQ = false;
                }
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
                    this.BeginInvoke((Action)(() => varaToolStripStatusLabel.Text = Strings.ConectedToVaraStr));
                    this.BeginInvoke((Action)(() => varaToolStripStatusLabel.BackColor = Color.Green));
                    break;
                case false:
                    this.BeginInvoke((Action)(() => varaToolStripStatusLabel.Text = Strings.DisconnectedFromVaraStr));
                    this.BeginInvoke((Action)(() => varaToolStripStatusLabel.BackColor = Color.Red));
                    break;
                default:
                    this.BeginInvoke((Action)(() => varaToolStripStatusLabel.Text = Strings.UnknownVaraStatusStr));
                    this.BeginInvoke((Action)(() => varaToolStripStatusLabel.BackColor = Color.Yellow));
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
                    this.BeginInvoke((Action)(() => varaToolStripStatusLabel.Text = Strings.ConectedToVaraStr));
                    this.BeginInvoke((Action)(() => varaToolStripStatusLabel.BackColor = Color.Green));
                    break;
                case false:
                    this.BeginInvoke((Action)(() => varaToolStripStatusLabel.Text = Strings.DisconnectedFromVaraStr));
                    this.BeginInvoke((Action)(() => varaToolStripStatusLabel.BackColor = Color.Red));
                    break;
                default:
                    this.BeginInvoke((Action)(() => varaToolStripStatusLabel.Text = Strings.UnknownVaraStatusStr));
                    this.BeginInvoke((Action)(() => varaToolStripStatusLabel.BackColor = Color.Yellow));
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
                            this.BeginInvoke((Action)(() => channelBusyToolStripStatusLabel.BackColor = Color.Green));
                            this.BeginInvoke((Action)(() => channelBusyToolStripStatusLabel.Text = Strings.FreeStr));
                            Values.Busy = false;
                            break;
                        case VARAResult.busyON:
                            Values.Busy = true;
                            this.BeginInvoke((Action)(() => channelBusyToolStripStatusLabel.BackColor = Color.Red));
                            this.BeginInvoke((Action)(() => channelBusyToolStripStatusLabel.Text = Strings.BusyStr));
                            break;
                        case VARAResult.IAmAlive:
                            Log.Debug(VARAResult.IAmAlive);
                            break;
                        case VARAResult.missingSoundCard:
                            updateFromModemTextBox(Values.Incomming + text);
                            break;
                        case VARAResult.ok:
                            updateFromModemTextBox(Values.Incomming + text);
                            break;
                        case VARAResult.wrong:
                            updateFromModemTextBox(Values.Incomming + text);
                            break;
                        default:
                            updateFromModemTextBox(Values.Incomming + text);
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
                if (!string.IsNullOrEmpty(text))
                    switch (text)
                    {
                        case VARAResult.pttOFF:
                            RigControl.disableTX().ConfigureAwait(false);
                            Values.Busy = false;
                            this.BeginInvoke((Action)(() => rxTxToolStripStatusLabel.BackColor = Color.Green));
                            this.BeginInvoke((Action)(() => rxTxToolStripStatusLabel.Text = Strings.RXStr));
                            break;
                        case VARAResult.pttON:
                            RigControl.enableTX().ConfigureAwait(false);
                            Values.Busy = true;
                            this.BeginInvoke((Action)(() => rxTxToolStripStatusLabel.BackColor = Color.Red));
                            this.BeginInvoke((Action)(() => rxTxToolStripStatusLabel.Text = Strings.TXStr));
                            break;
                        case VARAResult.busyOFF:
                            this.BeginInvoke((Action)(() => channelBusyToolStripStatusLabel.BackColor = Color.Green));
                            this.BeginInvoke((Action)(() => channelBusyToolStripStatusLabel.Text = Strings.FreeStr));
                            Values.Busy = false;
                            break;
                        case VARAResult.busyON:
                            Values.Busy = true;
                            this.BeginInvoke((Action)(() => channelBusyToolStripStatusLabel.BackColor = Color.Red));
                            this.BeginInvoke((Action)(() => channelBusyToolStripStatusLabel.Text = Strings.BusyStr));
                            break;
                        case VARAResult.disconnected:
                            Values.OutGoingConnection = false;
                            Values.Ping = false;
                            disableButtonsStationDisconnected();
                            updateFromModemTextBoxDeligated(Values.Incomming + text);
                            break;
                        case VARAResult.pending:
                            updateFromModemTextBoxDeligated(Values.Incomming + text);
                            if (!Values.OutGoingConnection)
                                if (!Values.BeaconActive)
                                    SystemSounds.Exclamation.Play();
                            // need to add incomming connection alert.
                            break;
                        case VARAResult.cancelPending:
                            updateFromModemTextBoxDeligated(Values.Incomming + text);
                            if (Values.SendCQ)
                            {
                                if (!Values.Busy)
                                {
                                    sendCommandToVaraModem(VaraCMD.cqFrame500(Values.stationDetails.CallSign + Values.VaraSIDToUse));
                                    Values.SendCQ = false;
                                    break;
                                }
                            }
                            if (Values.BeaconActive)
                                Values.BeaconActive = false;
                            break;
                        case string a when text.Contains(VARAResult.registered):
                            enableButtonsRegistered();
                            updateFromModemTextBoxDeligated(Values.Incomming + text);
                            break;
                        case VARAResult.linkRegistered:
                            enableButtonsStationConnected();
                            updateFromModemTextBox(Values.Incomming + text);
                            break;
                        case VARAResult.linkUnRegistered:
                            enableButtonsStationConnected();
                            updateFromModemTextBoxDeligated(Values.Incomming + text);
                            break;
                        case VARAResult.IAmAlive:
                            Log.Debug(Values.Incomming + VARAResult.IAmAlive);
                            break;
                        case VARAResult.missingSoundCard:
                            updateFromModemTextBoxDeligated(Values.Incomming + text);
                            break;
                        case VARAResult.ok:
                            updateFromModemTextBoxDeligated(Values.Incomming + text);
                            break;
                        case VARAResult.wrong:
                            updateFromModemTextBoxDeligated(Values.Incomming + text);
                            break;
                        case string a when text.Contains("SN "):
                            if (Values.StationConnected)
                            {
                                this.BeginInvoke((Action)(() => textBox5.Text = Functions.Smeter(Values.Smeter)));
                                this.BeginInvoke((Action)(() => textBox6.Text = Functions.Snmeter(text)));
                                fromModemTextBox.Invoke(UpdateFromModemTextBoxDeligate, Values.Incomming + text + " " + Functions.Smeter(Values.Smeter));
                                Values.SignalNoice = Functions.Snmeter(text);
                            }
                            else
                            {
                                fromModemTextBox.Invoke(UpdateFromModemTextBoxDeligate, Values.Incomming + text + " " + Functions.Smeter(Values.Smeter));
                                Values.SignalNoice = Functions.Snmeter(text);
                            }
                            break;
                        case string a when text.Contains(VARAResult.cqFrame):
                            updateFromModemTextBoxDeligated(Values.Incomming + text);
                            updateLastHeardAsync(text).ConfigureAwait(false); ;
                            break;
                        case string a when text.Contains("BUFFER "):
                            updateFromModemTextBoxDeligated(Values.Incomming + text);
                            string buffer = text.Remove(0, 7);
                            int bufferSize = int.Parse(buffer);
                            Values.SendBuffer = bufferSize;
                            break;
                        default:
                            if (text.Contains("CONNECTED "))
                            {
                                updateFromModemTextBoxDeligated(Values.Incomming + text);
                                enableButtonsStationConnected();
                                // get call signs from connected string.

                                string remove = text.Remove(0, 10);
                                string[] split = remove.Split(' ');

                                if (!split[0].Contains(Values.stationDetails.CallSign))
                                {
                                    Values.connectedStationDetails.CallSign = split[0].ToString();
                                }
                                if (!split[1].Contains(Values.stationDetails.CallSign))
                                {
                                    string[] splitID = split[1].ToString().Split('-');
                                    Values.connectedStationDetails.CallSign = splitID[0].ToString();
                                }


                                // detect incomming ping request:
                                if (text.Contains(Values.stationDetails.CallSign + "-T"))
                                {
                                    Values.Ping = true;
                                    updateRichTextBoxDeligated(Strings.IncommingPing + Environment.NewLine, Color.DarkMagenta);
                                    Log.Info(Strings.IncommingPing + text);
                                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, "PING de " +
                                        Values.stationDetails.CallSign + " " +
                                        SendTag.signal +
                                        Values.SignalNoice +
                                        SendTag.end +
                                        SendTag.locator +
                                        Values.stationDetails.Locator +
                                        SendTag.end + "\r\n");
                                    sendText();
                                    break;
                                }

                                // detect outgooing ping request:
                                if (text.Contains(Values.connectedStationDetails.CallSign + "-T"))
                                {
                                    Values.Ping = true;
                                    updateRichTextBoxDeligated(Strings.OutgoingPing + Environment.NewLine , Color.DarkMagenta);
                                    Log.Info(Strings.IncommingPing + text);
                                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, "PING de " +
                                        Values.stationDetails.CallSign + " " +
                                        SendTag.signal +
                                        Values.SignalNoice +
                                        SendTag.end +
                                        SendTag.locator +
                                        Values.stationDetails.Locator +
                                        SendTag.end + "\r\n");
                                    sendText();
                                    break;
                                }
                                // detect if the connection is a new incomming connection and send welcome message.
                                if (!Values.OutGoingConnection)
                                {
                                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, _strings.WelcomeStr);
                                    sendText();
                                    break;
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
            // check if there is a length of data given. If yes then set the buffer size. And remove those numbers
            if (!string.IsNullOrEmpty(data) && char.IsDigit(data[0]))
            {
                richTextBox1.Invoke(UpdateRichTextBoxDeligate, "\r\n", Color.DarkBlue);
                Values.RecieveBuffer = 0;
                string[] split = data.Split(' ');
                string buffer = split.FirstOrDefault();
                Values.RecieveBufferMaxSize = int.Parse(buffer) - buffer.Length;
                int firstsplit = buffer.Length +1;
                data = data.Remove(0, firstsplit);
                Values.RecieveBuffer = int.Parse(buffer);
            }
            Values.RecieveBuffer = Values.RecieveBuffer - data.Length;
            richTextBox1.Invoke(UpdateRichTextBoxDeligate, data, Color.DarkBlue);
            Log.Info(data);
            Values.VaraDataRecievedBufferText = Values.VaraDataRecievedBufferText + data.ToString();
            if (Values.RecieveBuffer <= 0)
            {
                string[] split = Values.VaraDataRecievedBufferText.Split('<');
                foreach (string _string in split)
                {
                    Log.Info(_string);
                    switch (_string)
                    {
                        case string a when _string.Contains("R-"): //  = "<R-";
                                string _newstringRmin = _string.Remove(0, 2);
                                string[] _newsubstringRmin = _newstringRmin.Split('>');
                                Values.connectedStationDetails.rcv = "-" + _newsubstringRmin[0];
                            break;
                        case string a when _string.Contains("R+"): //  = "<R+";
                                string _newstringRplus = _string.Remove(0, 2);
                                string[] _newsubstringRplus = _newstringRplus.Split('>');
                                Values.connectedStationDetails.rcv = "+" + _newsubstringRplus[0];

                            break;
                        case string a when _string.Contains("NAME:"): //  = "<NAME:";
                                string _newstringName = _string.Remove(0, 4);
                                string[] _newsubstringName = _newstringName.Split('>');
                                Values.connectedStationDetails.Name = _newsubstringName[0];
                            break;
                        case string a when _string.Contains("QTH:"): //  = "<QTH:";
                                string _newstringQth = _string.Remove(0, 4);
                                string[] _newsubstringQth = _newstringQth.Split('>');
                                Values.connectedStationDetails.qth = _newsubstringQth[0];
                            break; 
                        case string a when _string.Contains("LOC:"): //  = "<LOC:";
                                string _newstringLoc = _string.Remove(0, 4);
                                string _newsubstringLoc = _newstringLoc.Substring(0, 6);
                                Values.connectedStationDetails.Locator = _newsubstringLoc;
                            break;
                    }
                }
            }
            // recieved tag handeling
            switch (data)
            {
                case string a when data.Contains(RecievedTag.lastheard): //  = "<LHR>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, Strings.ErrorNotSupported);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.qsyu): //  = "<QSYU>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, Strings.ErrorNotSupported);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.qsyd): //  = "<QSYD>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, Strings.ErrorNotSupported);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.version): //  = "<VER>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, SendTag.version);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.info): //  = "<INFO>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, Strings.ErrorNotSupported);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.snrrequest): //  = "<SNRR>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, SendTag.signal + Values.SignalNoice + SendTag.end);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.away): //  = "<AWAY>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, _strings.AwayResponse);
                    sendText();
                    break;
                case string a when data.Contains(RecievedTag.disconnect): //  = "<DISC>";
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, _strings.GoodByeResponse);
                    sendText();
                    sendCommandToVaraModem(VaraCMD.disconnect);
                    break;
            }

        }

        #endregion



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
                string beaconSID = String.Empty;

                string removedCQframe = text.Remove(0, 8); // Remove CQ Frame
                string replacedBW500 = removedCQframe.Replace(" 500", ""); // Remove the BW 500
                string beaconCall = replacedBW500.Replace(" ", ""); // Remove spaces if any.
                // split the callsign from the ID if there is any ID.

                if (beaconCall.Contains("-"))
                {
                    string[] ids = beaconCall.Split('-');
                    beaconCallSign = ids[0];
                    beaconSID = "-" + ids[1];
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
                    dataGridView1.Rows.Add(DateTime.UtcNow.ToString("HH:mm:ss"), beaconCallSign, beaconSID, Functions.Snmeter(Values.SignalNoice), count, Functions.Smeter(Values.Smeter));

                    lastHeard.Add(new station { UTCTime = DateTime.UtcNow.ToString(), Call = beaconCallSign, SID = beaconSID, SNR = Functions.Snmeter(Values.SignalNoice), SM = Functions.Smeter(Values.Smeter), Cnt = count.ToString() });


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
            if (!Values.StationConnected)
            {
                connectToVaraButton.Invoke((MethodInvoker)delegate { connectToVaraButton.Enabled = false; });
                disconnectFromVaraButton.Invoke((MethodInvoker)delegate { disconnectFromVaraButton.Enabled = true; });
                sendBeaconTimerButton.Invoke((MethodInvoker)delegate { sendBeaconTimerButton.Enabled = true; });
                sendBeaconNowButton.Invoke((MethodInvoker)delegate { sendBeaconNowButton.Enabled = true; });
                sendCQButton.Invoke((MethodInvoker)delegate { sendCQButton.Enabled = true; });
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
            Values.StationConnected = true;
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
            Values.StationConnected = false;

        }
        private void disableButtonsDisconnected()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());

            this.BeginInvoke((Action)(() => connectToVaraButton.Enabled = true));
            this.BeginInvoke((Action)(() => disconnectFromVaraButton.Enabled = false));
            this.BeginInvoke((Action)(() => sendBeaconTimerButton.Enabled = false));
            this.BeginInvoke((Action)(() => sendBeaconNowButton.Enabled = false));
            this.BeginInvoke((Action)(() => sendCQButton.Enabled = false));
            this.BeginInvoke((Action)(() => button6.Enabled = false));
            this.BeginInvoke((Action)(() => stopTxNowButton.Enabled = true));
            this.BeginInvoke((Action)(() => sendTextButton.Enabled = false));
            this.BeginInvoke((Action)(() => connectToStationButton.Enabled = false));
            this.BeginInvoke((Action)(() => disconnectFromStationButton.Enabled = false));
            this.BeginInvoke((Action)(() => abortConnectionButton.Enabled = false));
            Values.StationConnected = false;


        }

        private void updateRichTextBoxDeligated(string _data, Color color)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            richTextBox1.Invoke(UpdateRichTextBoxDeligate, _data, color);
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
                updateFromModemTextBox(Strings.VaraCommandChannelErrorStr + ex.ToString());
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
                updateFromModemTextBox(Strings.VaraDataChannelErrorStr + ex.ToString());
            }
            Values.VaraConnected = true;
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
            Values.VaraConnected = true;
        }

        private void sendCommandToVaraModem(string command)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            if (command != null)
                if (varaCmd.VARACommandClientWrite(command))
                {
                    updateFromModemTextBoxDeligated(Values.Outgoing + command);
                    //updateFromModemTextBox(Values.Outgoing + command);
                    Log.Info(command);
                }
                else
                {
                    Log.Error(Strings.VaraCommandClientWriteFailedDisconnectedStr);
                    updateFromModemTextBoxDeligated(Strings.VaraCommandClientWriteFailedDisconnectedStr);
                }
            else
            {
                Log.Error(Strings.VaraCommandSendFailedNoCommandStr);
                updateFromModemTextBoxDeligated(Strings.VaraCommandSendFailedNoCommandStr);
            }
        }
        private void setMonitorlistenON()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaMonitorCmd.VARAMonitorCommandClientWrite(VaraCMD.listenON))
                {
                    updateFromModemTextBox(Values.Outgoing + VaraCMD.listenON);
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
                    updateFromModemTextBox(Values.Outgoing + VaraCMD.bw500);
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
                    Values.SendBufferMaxSize = textToSend.Length;
                    Values.SendBuffer = textToSend.Length;
                    richTextBox1.Invoke(UpdateRichTextBoxDeligate, "\r\n" + textToSend + Environment.NewLine, Color.DarkRed);
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, "");
                }
                else
                {
                    Log.Error(Strings.VaraDataClientWriteFailedDisconnectedStr);
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                Log.Error(Strings.VaraDataClientWriteFailedDisconnectedStr);
            }
        }
        // Send Text to Data Channel.
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());

            sendTextTextBox.Text = macroComboBox.SelectedItem.ToString();
        }
        private void setDataGrid()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = Strings.TimeStr;
            dataGridView1.Columns[1].Name = Strings.CallSignStr;
            dataGridView1.Columns[2].Name = Strings.SIDStr;
            dataGridView1.Columns[3].Name = Strings.SignalNoiseRatioStr;
            dataGridView1.Columns[4].Name = Strings.CallSignStr;
            dataGridView1.Columns[5].Name = Strings.SmeterStr;
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

            DateTime today = DateTime.UtcNow.Date;

            fromfile = Functions.ReadLastHeardXML<station>(FileNames.LastHeardFileName);

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

                if (dateTime.Date == today.Date)
                {
                    dataGridView1.Rows.Add(dateTime.ToString("HH:mm:ss"), station.Call, station.SID, station.SNR, station.Cnt, station.SM);
                    dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Descending);
                    dataGridView1.AutoResizeColumns();
                }

                var obj = lastHeard.FirstOrDefault<station>(x => x.Call == search);
                if (obj == null)
                    lastHeard.Add(new station { UTCTime = dateTime.ToString(), Call = station.Call, SID = station.SID, SNR = station.SNR, SM = station.SM, Cnt = station.Cnt });
                //if (obj != null)
                //    lastHeard.Add(new station { UTCTime = dateTime.ToString(), Call = station.Call, SID = station.SID, SNR = station.SNR, SM = station.SM, Cnt = station.Cnt });
            }
        }
        private void setColorsAndText()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            // From:
            this.BackColor = Color.FromArgb(255, 31, 31, 31);
            this.ForeColor = Color.White;
            this.Text = "VarAQT - " + Values.stationDetails.CallSign + " - " + System.Windows.Forms.Application.ProductVersion;

            // Buttons:
            connectToVaraButton.BackColor = SystemColors.ControlDark;
            connectToVaraButton.ForeColor = Color.White;
            connectToVaraButton.Text = Strings.ConnectToVaraButtonStr;

            disconnectFromVaraButton.BackColor = SystemColors.ControlDark;
            disconnectFromVaraButton.ForeColor = Color.White;
            disconnectFromVaraButton.Text = Strings.DisconnectFromVaraButtonStr;

            sendBeaconTimerButton.BackColor = SystemColors.ControlDark;
            sendBeaconTimerButton.ForeColor = Color.White;
            sendBeaconTimerButton.Text = Strings.SendBeaconTimerButtonStr;

            sendBeaconNowButton.BackColor = SystemColors.ControlDark;
            sendBeaconNowButton.ForeColor = Color.White;
            sendBeaconNowButton.Text = Strings.SendBeaconNowButtonStr;

            sendCQButton.BackColor = SystemColors.ControlDark;
            sendCQButton.ForeColor = Color.White;
            sendCQButton.Text = Strings.SendCQButtonStr;

            button6.BackColor = SystemColors.ControlDark;
            button6.ForeColor = Color.White;
            button6.Text = Strings.button6;

            stopTxNowButton.BackColor = SystemColors.ControlDark;
            stopTxNowButton.ForeColor = Color.White;
            stopTxNowButton.Text = Strings.StopTxNowButtonStr;

            sendTextButton.BackColor = SystemColors.ControlDark;
            sendTextButton.ForeColor = Color.White;
            sendTextButton.Text = Strings.SendTextButtonStr;

            connectToStationButton.BackColor = SystemColors.ControlDark;
            connectToStationButton.ForeColor = Color.White;
            connectToStationButton.Text = Strings.ConnectToStationButtonStr;

            disconnectFromStationButton.BackColor = SystemColors.ControlDark;
            disconnectFromStationButton.ForeColor = Color.White;
            disconnectFromStationButton.Text = Strings.DisconnectFromStationButtonStr;

            abortConnectionButton.BackColor = SystemColors.ControlDark;
            abortConnectionButton.ForeColor = Color.White;
            abortConnectionButton.Text = Strings.AbortConnectionButtonStr;

            pingAStationButton.BackColor = SystemColors.ControlDark;
            pingAStationButton.ForeColor = Color.White;
            pingAStationButton.Text = Strings.PingAStationButtonStr;

            // Tool Strips:
            varaToolStripStatusLabel.BackColor = Color.FromArgb(255, 31, 31, 31);
            varaToolStripStatusLabel.ForeColor = Color.White;
            channelToolStripStatusLabel.BackColor = Color.FromArgb(255, 31, 31, 31);
            channelToolStripStatusLabel.ForeColor = Color.White;
            channelBusyToolStripStatusLabel.BackColor = Color.FromArgb(255, 31, 31, 31);
            channelBusyToolStripStatusLabel.ForeColor = Color.White;
            utcTimeToolStripStatusLabel.BackColor = Color.FromArgb(255, 31, 31, 31);
            utcTimeToolStripStatusLabel.ForeColor = Color.White;
            rxTxToolStripStatusLabel.BackColor = Color.FromArgb(255, 31, 31, 31);
            rxTxToolStripStatusLabel.ForeColor = Color.White;
            lastBeaconTextToolStripStatusLabel.BackColor = Color.FromArgb(255, 31, 31, 31);
            lastBeaconTextToolStripStatusLabel.ForeColor = Color.White;

            // Labels:
            label1.BackColor = Color.FromArgb(255, 31, 31, 31);
            label1.ForeColor = Color.White;
            label2.BackColor = Color.FromArgb(255, 31, 31, 31);
            label2.ForeColor = Color.White;
            label3.BackColor = Color.FromArgb(255, 31, 31, 31);
            label3.ForeColor = Color.White;
            label4.BackColor = Color.FromArgb(255, 31, 31, 31);
            label4.ForeColor = Color.White;
            label5.BackColor = Color.FromArgb(255, 31, 31, 31);
            label5.ForeColor = Color.White;
            label6.BackColor = Color.FromArgb(255, 31, 31, 31);
            label6.ForeColor = Color.White;


            // Data Grid
            dataGridView1.DefaultCellStyle.ForeColor = Color.White;
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(255, 31, 31, 31);
            dataGridView1.DefaultCellStyle.Font = new Font("Consolas", 8);
            dataGridView1.Font = new Font("Consolas", 8);
            dataGridView1.ForeColor = Color.White;
            dataGridView1.BackgroundColor = Color.FromArgb(255, 31, 31, 31);

            // ComboBox

            txRxChannelComboBox.SelectedItem = Strings.Channel9Str;
            txRxChannelComboBox.BackColor = Color.FromArgb(255, 31, 31, 31);
            txRxChannelComboBox.ForeColor = Color.White;
            cqChannelComboBox.SelectedItem = Strings.Channel9Str;
            cqChannelComboBox.BackColor = Color.FromArgb(255, 31, 31, 31);
            cqChannelComboBox.ForeColor = Color.White;
            frequencyBandComboBox.SelectedItem = "20 Metres";
            frequencyBandComboBox.BackColor = Color.FromArgb(255, 31, 31, 31);
            frequencyBandComboBox.ForeColor = Color.White;
            macroComboBox.BackColor = Color.FromArgb(255, 31, 31, 31);
            macroComboBox.ForeColor = Color.White;


        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            sendCommandToVaraModem(VaraCMD.chatOFF);
            sendCommandToVaraModem(VaraCMD.listenOFF);
            sendCommandToVaraModem(VaraCMD.bw2750);
            varaCmd.VARACommandClientDisconnect();
            varaData.VARADataClientDisconnect();
            if (Values.VARAMonitorEnabled)
                varaMonitorCmd.VARAMonitorCommandClientDisconnect();
            Functions.WriteLastHeardXML<station>(lastHeard, FileNames.LastHeardFileName);
            Thread.Sleep(1000);
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

        private void updateFrequency()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            int frequency = int.Parse(Values.BaseFrequencyBand.ToString() + Values.BaseChannelFrequency.ToString());
            frequencyToolStripStatusLabel.Text = Functions.FrequencyStr(frequency.ToString("D9")) + " Mhz";
            RigControl.updateFrq(frequency);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            string toCheck = txRxChannelComboBox.Text;
            switch (toCheck)
            {
                case string a when toCheck.Equals(Strings.Channel1Str):
                    Values.BaseChannelFrequency = Channel.Channel1;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals(Strings.Channel2Str):
                    Values.BaseChannelFrequency = Channel.Channel2;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals(Strings.Channel3Str):
                    Values.BaseChannelFrequency = Channel.Channel3;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals(Strings.Channel4Str):
                    Values.BaseChannelFrequency = Channel.Channel4;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals(Strings.Channel5Str):
                    Values.BaseChannelFrequency = Channel.Channel5;
                    updateFrequency();
                    break;
                //case string a when toCheck.Equals("Channel 6"):
                //    textBox2.Text = "014105000";
                //    break;
                //case string a when toCheck.Equals("Channel 7"):
                //    textBox2.Text = "014105000";
                //    break;
                //case string a when toCheck.Equals("Channel 8"):
                //    textBox2.Text = "014105000";
                //    break;
                case string a when toCheck.Equals(Strings.Channel9Str):
                    Values.BaseChannelFrequency = Channel.Channel9;
                    updateFrequency();
                    break;
                //case string a when toCheck.Equals("Channel 10"):
                //    textBox2.Text = "014105000";
                //    break;
                case string a when toCheck.Equals(Strings.Channel11Str):
                    Values.BaseChannelFrequency = Channel.Channel11;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals(Strings.Channel12Str):
                    Values.BaseChannelFrequency = Channel.Channel12;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals(Strings.Channel13Str):
                    Values.BaseChannelFrequency = Channel.Channel13;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals(Strings.Channel14Str):
                    Values.BaseChannelFrequency = Channel.Channel14;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals(Strings.Channel15Str):
                    Values.BaseChannelFrequency = Channel.Channel15;
                    updateFrequency();
                    break;
            }
        }

        private void cqChannelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            string toCheck = cqChannelComboBox.Text;
            switch (toCheck)
            {
                case string a when toCheck.Equals(Strings.Channel1Str):
                    Values.VaraSIDToUse = "-1";
                    break;
                case string a when toCheck.Equals(Strings.Channel2Str):
                    Values.VaraSIDToUse = "-2";
                    break;
                case string a when toCheck.Equals(Strings.Channel3Str):
                    Values.VaraSIDToUse = "-3";
                    break;
                case string a when toCheck.Equals(Strings.Channel4Str):
                    Values.VaraSIDToUse = "-4";
                    break;
                case string a when toCheck.Equals(Strings.Channel5Str):
                    Values.VaraSIDToUse = "-5";
                    break;
                //case string a when toCheck.Equals("Channel 6"):
                //    textBox2.Text = "014105000";
                //    break;
                //case string a when toCheck.Equals("Channel 7"):
                //    textBox2.Text = "014105000";
                //    break;
                //case string a when toCheck.Equals("Channel 8"):
                //    textBox2.Text = "014105000";
                //    break;
                case string a when toCheck.Equals(Strings.Channel9Str):
                    Values.VaraSIDToUse = "-9";
                    break;
                //case string a when toCheck.Equals("Channel 10"):
                //    textBox2.Text = "014105000";
                //    break;
                case string a when toCheck.Equals(Strings.Channel11Str):
                    Values.VaraSIDToUse = "-11";
                    break;
                case string a when toCheck.Equals(Strings.Channel12Str):
                    Values.VaraSIDToUse = "-12";
                    break;
                case string a when toCheck.Equals(Strings.Channel13Str):
                    Values.VaraSIDToUse = "-13";
                    break;
                case string a when toCheck.Equals(Strings.Channel14Str):
                    Values.VaraSIDToUse = "-14";
                    break;
                case string a when toCheck.Equals(Strings.Channel15Str):
                    Values.VaraSIDToUse = "-15";
                    break;
            }
        }

        private void frequencyBandComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            string toCheck = frequencyBandComboBox.Text;
            switch (toCheck)
            {
                case string a when toCheck.Equals("10 Metres"):
                    Values.BaseFrequencyBand = Band.MeterIs10;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals("11 Metres"):
                    Values.BaseFrequencyBand = Band.MeterIs11;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals("12 Metres"):
                    Values.BaseFrequencyBand = Band.MeterIs12;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals("15 Metres"):
                    Values.BaseFrequencyBand = Band.MeterIs15;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals("17 Metres"):
                    Values.BaseFrequencyBand = Band.MeterIs17;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals("20 Metres"):
                    Values.BaseFrequencyBand = Band.MeterIs20;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals("40 Metres"):
                    Values.BaseFrequencyBand = Band.MeterIs40;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals("60 Metres"):
                    Values.BaseFrequencyBand = Band.MeterIs60;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals("80 Metres"):
                    Values.BaseFrequencyBand = Band.MeterIs80;
                    updateFrequency();
                    break;
                case string a when toCheck.Equals("160 Metres"):
                    Values.BaseFrequencyBand = Band.MeterIs160;
                    updateFrequency();
                    break;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            callSignTextBox.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
        }

    }
}


