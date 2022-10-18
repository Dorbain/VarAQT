﻿using System;
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

namespace VarAQT
{
    public partial class Form1 : Form
    {
        #region Constants

        // For Display Data in Text Box and Info - UI Thread Invoke
        public delegate void AddLogDeligate(string data);
        public AddLogDeligate UpdateFromModemTextBoxDeligate;
        public AddLogDeligate UpdateSendToModemTextBoxDeligate;
        public AddLogDeligate UpdateCallSignTextBoxDeligate;
        public AddLogDeligate UpdateSendTextTextBoxDeligate;
        public AddLogDeligate UpdateRichTextBox;
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
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            UpdateFromModemTextBoxDeligate = new AddLogDeligate(updateRecievedFromModemTextBox);
            UpdateSendToModemTextBoxDeligate = new AddLogDeligate(updateSendToModemTextBox);
            UpdateCallSignTextBoxDeligate = new AddLogDeligate(updateCallSignTextBox);
            UpdateSendTextTextBoxDeligate = new AddLogDeligate(updateSendTextTextBox);
            UpdateRichTextBox = new AddLogDeligate(updateRichTextBox);
            UpdateDataGridDeligate = new AddLogDeligate(updateLastHeardDataGrid);
            Log.enableDebug = true;
            Log.enableInfo = true;
            //UpdateStatusIcons = new AddNotificationDelegate(UpdateSatusIcons);
            timer1.Interval = 100;
            timer2.Interval = 300000; //60000 = 1 minuut, 300000 = 5 minuten ;
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
            connectToVara();
            bw500();
            listenON();
            chatOn();
            listenCQ();
            myCall(Values.callSign);
            addLastHeardFileToDataGrid();
        }

        #endregion

        #region Buttons
        // VARADataClientConnect Button
        private void button1_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            connectToVara();
            bw500();
            listenON();
            listenCQ();
            chatOn();
            myCall(Values.callSign);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            varaCmd.VARACommandClientDisconnect();
            varaData.VARADataClientDisconnect();
            disableButtonsDisconnected();
        }
        // Send Beacon Button
        private void button3_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            switch (Values.beaconActive)
            {
                case true:
                    Values.beaconActive = false;
                    timer2.Stop();
                    sendBeaconTimerButton.BackColor = Color.DarkSeaGreen;
                    break;
                case false:
                    Values.beaconActive = true;
                    timer2.Start();
                    sendBeaconTimerButton.BackColor = Color.DarkRed;
                    break;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            if (!Values.busy)
                try
                {
                    if (varaCmd.VARACommandClientWrite("CQFRAME " + Values.callSign + "-9 500\r"))
                    {
                        updateSendToModemTextBox("CQFRAME " + Values.callSign + "-9 500\r");
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
        private void button5_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            bw2300();

        }
        private void button6_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            addLastHeardFileToDataGrid();

        }
        private void button7_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            RigControl.disableTX().ConfigureAwait(false);
            timer2.Stop();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            sendText();

        }
        // connect to Call
        private void button9_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            string callToConnect = callSignTextBox.Text.ToString();
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.connect(Values.callSign, callToConnect)))
                {
                    updateSendToModemTextBox(VaraCMD.connect(Values.callSign, callToConnect));
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
        private void button10_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.disconnect))
                {
                    updateSendToModemTextBox(VaraCMD.disconnect);
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
        private void button11_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            try
            {
                if (varaCmd.VARACommandClientWrite(VaraCMD.abort))
                {
                    updateSendToModemTextBox(VaraCMD.abort);
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
        private void button12_Click(object sender, EventArgs e)
        {
            WriteXML<stations>(lastHeard, "LastHeard.xml");
        }
        #endregion

        #region Timers
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel7.Text = "UTC: " + DateTime.UtcNow.ToString("HH:mm:ss");
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            if (!Values.busy)
                try
                {
                    if (varaCmd.VARACommandClientWrite("CQFRAME " + Values.callSign + "-9 500\r"))
                    {
                        updateSendToModemTextBox("CQFRAME " + Values.callSign + "-9 500\r");
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
        // Data Recieved Listner
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
                            //await updateFromModemTextBoxAsync(text); 
                            break;
                        case VARAResult.pttON:
                            await RigControl.enableTX().ConfigureAwait(false);
                            Values.busy = true;
                            this.BeginInvoke((Action)(() => toolStripStatusLabel8.BackColor = Color.Red));
                            this.BeginInvoke((Action)(() => toolStripStatusLabel8.Text = "TX"));
                            //await updateFromModemTextBoxAsync(text); 
                            break;
                        case VARAResult.busyOFF:
                            this.BeginInvoke((Action)(() => toolStripStatusLabel6.BackColor = Color.Green));
                            this.BeginInvoke((Action)(() => toolStripStatusLabel6.Text = "FREE"));
                            Values.busy = false;
                            //UpdateSatusIcons(0, true);
                            break;
                        case VARAResult.busyON:
                            Values.busy = true;
                            this.BeginInvoke((Action)(() => toolStripStatusLabel6.BackColor = Color.Red));
                            this.BeginInvoke((Action)(() => toolStripStatusLabel6.Text = "BUSY"));
                            //UpdateSatusIcons(1, true);
                            break;
                        case VARAResult.disconnected:
                            Values.outGoingConnection = false;
                            disableButtonsStationDisconnected();
                            await updateFromModemTextBoxAsync(text);
                            break;
                        case VARAResult.pending:
                            await updateFromModemTextBoxAsync(text);
                            break;
                        case VARAResult.cancelPending:
                            await updateFromModemTextBoxAsync(text);
                            break;
                        case string a when text.Contains(VARAResult.registered):
                            await updateFromModemTextBoxAsync(text);
                            enableButtonsRegistered();
                            break;
                        case VARAResult.registered:
                            //textBox2.Invoke(UpdateTextBox, text);
                            enableButtonsRegistered();
                            break;
                        case VARAResult.linkRegistered:
                            enableButtonsStationConnected();
                            await updateFromModemTextBoxAsync(text);
                            break;
                        case VARAResult.linkUnRegistered:
                            enableButtonsStationConnected();
                            await updateFromModemTextBoxAsync(text);
                            break;
                        case VARAResult.IAmAlive:
                            //textBox2.Invoke(UpdateTextBox, text);
                            break;
                        case VARAResult.missingSoundCard:
                            await updateFromModemTextBoxAsync(text);
                            break;
                        case VARAResult.ok:
                            await updateFromModemTextBoxAsync(text);
                            break;
                        case VARAResult.wrong:
                            await updateFromModemTextBoxAsync(text);
                            break;

                        case string a when text.Contains("SN "):
                            if (Values.stationConnected)
                            {
                                this.BeginInvoke((Action)(() => textBox5.Text = Functions.sMeter(Values.sMeter)));
                                this.BeginInvoke((Action)(() => textBox6.Text = Functions.sNMeter(text)));
                                fromModemTextBox.Invoke(UpdateFromModemTextBoxDeligate, text + " " + Functions.sMeter(Values.sMeter));
                                Values.signalNoice = text;
                            }
                            else
                            {
                                fromModemTextBox.Invoke(UpdateFromModemTextBoxDeligate, text + " " + Functions.sMeter(Values.sMeter));
                                Values.signalNoice = text;
                            }
                            break;
                        case string a when text.Contains(VARAResult.cqFrame):
                            await updateFromModemTextBoxAsync(text);
                            await updateLastHeardAsync(text);
                            break;
                        default:
                            if (text.Contains("BUFFER"))
                            {
                                await updateFromModemTextBoxAsync(text);
                                break;
                            }
                            else if (text.Contains("CONNECTED"))
                            {
                                await updateFromModemTextBoxAsync(text);
                                enableButtonsStationConnected();
                                if (!Values.outGoingConnection)
                                {
                                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, "Welcome to my station. it is still a work in progress... \r\n<LOC:JO22OI>");
                                    sendText();
                                }
                                break;
                            }
                            richTextBox1.Invoke(UpdateRichTextBox, text);

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
        private void updateSendToModemTextBox(string _data)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            sendToModemTextBox.AppendText(DateTime.UtcNow.ToString("HH:mm:ss") + " " + _data + Environment.NewLine);
            sendToModemTextBox.SelectionStart = sendToModemTextBox.Text.Length;
            sendToModemTextBox.ScrollToCaret();
        }
        private async Task updateLastHeardAsync(string text)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            await Task.Run(() =>
            {
                string remove = text.Remove(0, 8);
                string replace = remove.Replace(" 500", "");
                string beacon = replace.Replace(" ", "");
                //dataGridView1.Rows.Add(LastTime, CallSign, Signal, HeardCount);
                //addDataGrid.Invoke(DateTime.NowUtc, callsign, "-1", 1);
                //sendTextTextBox.Invoke(UpdateDataGridDeligate, text);
                dataGridView1.Invoke(new Action(delegate ()
                {
                    string search = beacon;
                    int rowIndex = -1;
                    int count = 1;
                    foreach (DataGridViewRow rows in dataGridView1.Rows)
                    {
                        if (rows.Cells[1].Value.ToString().Equals(search))
                        {
                            rowIndex = rows.Index;
                            string counter = rows.Cells[3].Value.ToString();
                            count = int.Parse(counter);
                            count++;
                            dataGridView1.Rows.RemoveAt(rowIndex);
                            break;
                        }
                    }
                    dataGridView1.Rows.Add(DateTime.UtcNow.ToString("HH:mm:ss"), beacon, Functions.sNMeter(Values.signalNoice), count);

                    lastHeard.Add(new stations { UTCTime = DateTime.UtcNow.ToString(), Call = beacon, SNR = Functions.sNMeter(Values.signalNoice), Cnt = count.ToString() });


                    dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
        // Connection Status Listner
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
        // Data Recieved Listner
        private void OnDataRecieved(string data)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            richTextBox1.Invoke(UpdateRichTextBox, data);
        }
        private void updateRichTextBox(string _data)
        {
            Log.Info(_data.ToString());
            richTextBox1.AppendText(_data);
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
        // VARADataClientDisconnect Button
        private void connectToVara()
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
                    updateSendToModemTextBox(VaraCMD.chatON);
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
                    updateSendToModemTextBox(VaraCMD.listenON);
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
                    updateSendToModemTextBox(VaraCMD.listenCQ);
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
                    updateSendToModemTextBox(VaraCMD.bw500);
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
                    updateSendToModemTextBox(VaraCMD.bw2300);
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
                    updateSendToModemTextBox(VaraCMD.bw2750);
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
                    updateSendToModemTextBox(VaraCMD.myCall(call));
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
                    richTextBox1.Invoke(UpdateRichTextBox, textToSend);
                    sendTextTextBox.Invoke(UpdateSendTextTextBoxDeligate, "");
                }
                else
                {
                    Log.Error("VARADataClientWrite (Failed) : Disconnected");
                    //updateRecievedFromModemTextBox("VARADataClientWrite (Failed) : Disconnected");
                }
            }
            catch (Exception ex)
            {
                // Catch errors in Sending Data
                Log.Error(ex.ToString());
                Log.Error(ex.Message.ToString());
                Log.Error("VARADataClientWrite (Failed) : Disconnected");
                //updateRecievedFromModemTextBox("Error : " + ex.ToString());
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
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "Time";
            dataGridView1.Columns[1].Name = "CallSign";
            dataGridView1.Columns[2].Name = "SNR";
            dataGridView1.Columns[3].Name = "Cnt";
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

            fromfile = ReadXML<stations>("LastHeard.xml");

            foreach (var station in fromfile)
            {
                DateTime dateTime = DateTime.ParseExact(station.UTCTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                lastHeard.Add(new stations { UTCTime = dateTime.ToString(), Call = station.Call, SNR = station.SNR, Cnt = station.Cnt });
                dataGridView1.Rows.Add(dateTime.ToString("HH:mm:ss"), station.Call, station.SNR, station.Cnt);
                dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Descending);
            }


        }
        private void setColors()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());

            this.BackColor = Color.FromArgb(255, 31, 31, 31);
            this.ForeColor = Color.White;
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

            dataGridView1.DefaultCellStyle.ForeColor = Color.White;
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(255, 31, 31, 31);
            dataGridView1.DefaultCellStyle.Font = new Font("Consolas", 8);
            dataGridView1.Font = new Font("Consolas", 8);
            dataGridView1.ForeColor = Color.White;
            dataGridView1.BackgroundColor = Color.FromArgb(255, 31, 31, 31);



        }
        public void WriteXML<T>(List<T> list, string filename)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            string filePath = filename;
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (TextWriter tw = new StreamWriter(filePath, append: false))
            {
                serializer.Serialize(tw, list);
                tw.Close();
            }
        }
        public List<T> ReadXML<T>(string filename)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            string filePath = filename;
            List<T> result;
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            XmlSerializer ser = new XmlSerializer(typeof(List<T>));
            using (FileStream myFileStream = new FileStream(filePath, FileMode.Open))
            {
                result = (List<T>)ser.Deserialize(myFileStream);
            }
            return result;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            WriteXML<stations>(lastHeard, "LastHeard.xml");
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

