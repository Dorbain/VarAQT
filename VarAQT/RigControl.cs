using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//added
//using HRDTranciever;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms.VisualStyles;
using VaraLib;
using Logger;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace VarAQT
{
    public partial class RigControl : Form
    {
        //// For Display Data in Text Box and Info - UI Thread Invoke
        //public delegate void AddLogDeligate(string data);
        //public AddLogDeligate UpdateTextBox;
        //public delegate void AddNotificationDelegate(int type, bool status);
        //public AddNotificationDelegate UpdateStatusIcons;
        //// Client Object
        //HRDClient hrdClient;

        static SerialPort ComPortCat = new SerialPort();
        static SerialPort ComPortTX = new SerialPort();
        internal delegate void SerialDataReceivedEventHandlerDelegate(object sender, SerialDataReceivedEventArgs e);
        internal delegate void SerialPinChangedEventHandlerDelegate(object sender, SerialPinChangedEventArgs e);
        //private SerialPinChangedEventHandler SerialPinChangedEventHandler1;
        delegate void SetTextCallback(string text);

        private static string ClassName = "RigControl";

        public RigControl()
        {
            InitializeComponent();
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            //SerialPinChangedEventHandler1 = new SerialPinChangedEventHandler(PinChanged);
            ComPortCat.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived_1);

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            setColorsAndText();

            timer1.Interval = 100;
            ComPortCat.PortName = "COM3";
            ComPortCat.BaudRate = 38400;
            ComPortCat.DataBits = 8;
            ComPortCat.StopBits = StopBits.One;
            ComPortCat.Handshake = Handshake.None;
            ComPortCat.Parity = Parity.None;
            ComPortCat.WriteTimeout = 100;
            ComPortCat.ReadTimeout = 100;
            ComPortCat.Open();

            ComPortTX.PortName = "COM4";
            ComPortTX.BaudRate = 38400;
            ComPortTX.DataBits = 8;
            ComPortTX.StopBits = StopBits.One;
            ComPortTX.Handshake = Handshake.None;
            ComPortTX.Parity = Parity.None;
            ComPortTX.WriteTimeout = 100;
            ComPortTX.ReadTimeout = 100;
            ComPortTX.Open();
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            disableTX().ConfigureAwait(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            //timer1.Stop();
            ComPortCat.Write("FA014105000;");
            ComPortCat.Write("FB014105000;");
        }

        private void port_DataReceived_1(object sender, SerialDataReceivedEventArgs e)
        {
            //Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            Values.InputData = ComPortCat.ReadExisting();
            if (Values.InputData != String.Empty)
            {
                this.BeginInvoke(new SetTextCallback(SetText), new object[] { Values.InputData });
            }
        }

        private void SetText(string text)
        {
            //Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            string[] textSplit = text.Split(';');
            foreach (string s in textSplit)
            {
                switch (s)
                {
                    case string FA when s.Contains("FA"):
                        this.label1.Text = Functions.FrequencyStr(s);
                        //Values.BaseFrequency = Functions.FrequencyInt(s);   
                        break;
                    case string FB when s.Contains("FB"):
                        this.label2.Text = Functions.FrequencyStr(s);
                        break;
                    case string SM when s.Contains("SM"):
                        Values.Smeter = s;
                        this.label3.Text = Functions.Smeter(s);
                        break;
                    case string TX0 when s.Contains("TX0"):
                        this.label4.Text = "RX";
                        this.label4.BackColor = Color.Green;
                        break;
                    case string TX1 when s.Contains("TX1"):
                        this.label4.Text = "TX";
                        this.label4.BackColor = Color.Red;
                        break;
                    case string TX2 when s.Contains("TX2"):
                        this.label4.Text = "TX";
                        this.label4.BackColor = Color.Red;
                        break;
                    case string unknown when s.Contains('?'):
                        textBox1.Text = s;
                        Log.Error(s, ClassName);
                        break;

                        
                }
                textBox1.Text = s;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Task taskFA = new Task(() => ComPortCat.Write("FA;"));
            Task taskFB = new Task(() => ComPortCat.Write("FB;"));
            Task taskSM = new Task(() => ComPortCat.Write("SM0;"));
            Task taskTX = new Task(() => ComPortCat.Write("TX;"));
            Task taskRM = new Task(() => ComPortCat.Write("RM0;"));
            taskFA.Start();
            taskFA.Wait();
            taskFB.Start();
            taskFB.Wait();
            taskSM.Start();
            taskSM.Wait();
            taskTX.Start();
            taskTX.Wait();
            taskRM.Start();
            taskRM.Wait();

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private async void button3_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            await enableTX();
            Thread.Sleep(500);
            await disableTX().ConfigureAwait(false);
        }
        public static async Task enableTX()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            await Task.Run(() =>
            {
                ComPortTX.RtsEnable = true;
            });
        }
        public static async Task disableTX()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            await Task.Run(() =>
            {
                ComPortTX.RtsEnable = false;
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            ComPortCat.Write("FA" + textBox2.Text + ";");

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            string toCheck = listBox1.Text;
            switch (toCheck)
            {
                case string a when toCheck.Equals("Channel 1"):
                    Values.FrequencyToUse = (Values.BaseFrequency - 750);
                    updateFrq();
                    break;
                case string a when toCheck.Equals("Channel 2"):
                    Values.FrequencyToUse = (Values.BaseFrequency - 1500);
                    updateFrq();
                    break;
                case string a when toCheck.Equals("Channel 3"):
                    Values.FrequencyToUse = (Values.BaseFrequency - 2250);
                    updateFrq();
                    break;
                case string a when toCheck.Equals("Channel 4"):
                    Values.FrequencyToUse = (Values.BaseFrequency - 3000);
                    updateFrq();
                    break;
                case string a when toCheck.Equals("Channel 5"):
                    Values.FrequencyToUse = (Values.BaseFrequency - 3750);
                    updateFrq();
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
                case string a when toCheck.Equals("Channel 9"):
                    Values.FrequencyToUse = 014105000;
                    Values.BaseFrequency = 014105000;
                    //textBox2.Text = (Values.BaseFrequency).ToString("D9");
                    updateFrq();
                    break;
                //case string a when toCheck.Equals("Channel 10"):
                //    textBox2.Text = "014105000";
                //    break;
                case string a when toCheck.Equals("Channel 11"):
                    Values.FrequencyToUse = (Values.BaseFrequency + 750);
                    updateFrq();
                    break;
                case string a when toCheck.Equals("Channel 12"):
                    Values.FrequencyToUse = (Values.BaseFrequency + 1500);
                    updateFrq();
                    break;
                case string a when toCheck.Equals("Channel 13"):
                    Values.FrequencyToUse = (Values.BaseFrequency + 2250); // Channel 9 + 2250
                    updateFrq();
                    break;
                case string a when toCheck.Equals("Channel 14"):
                    Values.FrequencyToUse = (Values.BaseFrequency + 3000); // Channel 9 + 3000
                    updateFrq();
                    break;
                case string a when toCheck.Equals("Channel 15"):
                    Values.FrequencyToUse = (Values.BaseFrequency + 3750); // Channel 9 + 3750
                    updateFrq();
                    break;
            }


        }
        public static void updateFrq()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            ComPortCat.Write("FA" + Values.FrequencyToUse.ToString("D9") + ";");
            Log.Info("FA" + Values.FrequencyToUse.ToString("D9") + ";");
        }

        public static void updateFrq(int frequency)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            ComPortCat.Write("FA" + frequency.ToString("D9") + ";");
            Log.Info("FA" + frequency.ToString("D9") + ";");
        }

        private void setColorsAndText()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            // Form:

            this.BackColor = Color.FromArgb(255, 31, 31, 31);
            this.ForeColor = Color.White;

            // Labels:
            label1.BackColor = Color.FromArgb(255, 31, 31, 31);
            label1.ForeColor = Color.Yellow;
            label2.BackColor = Color.FromArgb(255, 31, 31, 31);
            label2.ForeColor = Color.Yellow;
            label3.BackColor = Color.FromArgb(255, 31, 31, 31);
            label3.ForeColor = Color.Yellow;
            label4.BackColor = Color.FromArgb(255, 31, 31, 31);
            label4.ForeColor = Color.White;
            label5.BackColor = Color.FromArgb(255, 31, 31, 31);
            label5.ForeColor = Color.White;
            label6.BackColor = Color.FromArgb(255, 31, 31, 31);
            label6.ForeColor = Color.White;

        }











    }
}
