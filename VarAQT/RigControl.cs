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
                        this.label1.Text = Functions.frequency(s);
                        break;
                    case string FB when s.Contains("FB"):
                        this.label2.Text = Functions.frequency(s);
                        break;
                    case string SM when s.Contains("SM"):
                        Values.sMeter = s;
                        this.label3.Text = Functions.sMeter(s);
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
                    default:
                        if (s != string.Empty)
                            this.textBox1.Text += "!: " + s + Environment.NewLine;
                        this.textBox1.SelectionStart = textBox1.Text.Length;
                        this.textBox1.ScrollToCaret();
                        break;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Task taskFA = new Task(() => ComPortCat.Write("FA;"));
            Task taskFB = new Task(() => ComPortCat.Write("FB;"));
            Task taskSM = new Task(() => ComPortCat.Write("SM0;"));
            Task taskTX = new Task(() => ComPortCat.Write("TX;"));
            taskFA.Start();
            taskFA.Wait();
            //Thread.Sleep(100);
            taskFB.Start();
            taskFB.Wait();
            //Thread.Sleep(100);
            taskSM.Start();
            taskSM.Wait();
            //Thread.Sleep(100);
            taskTX.Start();
            taskTX.Wait();
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
            string toCheck = listBox1.Text;

            switch (toCheck)
            {
                case string a when toCheck.Equals("Channel 1"):
                    textBox2.Text = "014104250";
                    break;
                case string a when toCheck.Equals("Channel 2"):
                    textBox2.Text = "014103500";
                    break;
                case string a when toCheck.Equals("Channel 3"):
                    textBox2.Text = "014102750";
                    break;
                case string a when toCheck.Equals("Channel 4"):
                    textBox2.Text = "014102000";
                    break;
                case string a when toCheck.Equals("Channel 5"):
                    textBox2.Text = "014101250";
                    break;
                case string a when toCheck.Equals("Channel 6"):
                    textBox2.Text = "014105000";
                    break;
                case string a when toCheck.Equals("Channel 7"):
                    textBox2.Text = "014105000";
                    break;
                case string a when toCheck.Equals("Channel 8"):
                    textBox2.Text = "014105000";
                    break;
                case string a when toCheck.Equals("Channel 9"):
                    textBox2.Text = "014105000";
                    break;
                case string a when toCheck.Equals("Channel 10"):
                    textBox2.Text = "014105000";
                    break;
                case string a when toCheck.Equals("Channel 11"):
                    textBox2.Text = "014105750";
                    break;
                case string a when toCheck.Equals("Channel 12"):
                    textBox2.Text = "014106500";
                    break;
                case string a when toCheck.Equals("Channel 13"):
                    textBox2.Text = "014107250";
                    break;
                case string a when toCheck.Equals("Channel 14"):
                    textBox2.Text = "014108000";
                    break;
                case string a when toCheck.Equals("Channel 15"):
                    textBox2.Text = "014108750";
                    break;
                default:
                    textBox2.Text = "014105000";
                    break;


            }



        }












        //private void updateRecievedFromModemTextBox(string _data)
        //{
        //    textBox1.Text = textBox1.Text + DateTime.Now.ToString("HH:mm:ss") + " " + _data + Environment.NewLine;
        //    textBox1.SelectionStart = textBox1.Text.Length;
        //    textBox1.ScrollToCaret();
        //}

    }
}
