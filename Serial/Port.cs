using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//added
using System.IO.Ports;
using System.Threading;
using System.Net;

namespace Serial
{
    public class CatClient
    {

        // *** Event Handlers *** //

        /// <summary>
        /// Notify the Recieved Data
        /// </summary>
        /// <param name="data">Recieved Data</param>
        public delegate void SerialDataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e);
        public event SerialDataReceivedEventHandler OnSerialDataRecievedEvent;

        /// <summary>
        /// Notify the Connection Status of Socket
        /// </summary>
        /// <param name="status">Connection Status</param>
        public delegate void OnConnectEventHandler(bool status);
        public event OnConnectEventHandler OnSerialConnectEvent;

        // *** Properties *** //

        // Connection Parameters

        private string PortName;
        private int BaudRate;
        private int DataBits;
        private StopBits StopBits;
        private Handshake Handshake;
        private Parity Parity;
        private int WriteTimeout;
        private int ReadTimeout;

        static SerialPort ComPortCat = new SerialPort();
        //internal delegate void SerialDataReceivedEventHandlerDelegate(object sender, SerialDataReceivedEventArgs e);
        //internal delegate void SerialPinChangedEventHandlerDelegate(object sender, SerialPinChangedEventArgs e);
        delegate void SetTextCallback(string text);
        string InputData = String.Empty;

        public CatClient()
        {
            PortName = "COM3";
            BaudRate = 38400;
            DataBits = 8;
            StopBits = StopBits.One;
            Handshake = Handshake.None;
            Parity = Parity.None;
            WriteTimeout = 100;
            ReadTimeout = 100;
            //ComPortCat.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived);
        }
        public void connect()
        {
            ComPortCat.PortName = "COM3";
            ComPortCat.BaudRate = 38400;
            ComPortCat.DataBits = 8;
            ComPortCat.StopBits = StopBits.One;
            ComPortCat.Handshake = Handshake.None;
            ComPortCat.Parity = Parity.None;
            ComPortCat.WriteTimeout = 100;
            ComPortCat.ReadTimeout = 100;
            ComPortCat.Open();
        }




        //private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    InputData = ComPortCat.ReadExisting();
        //    if (InputData != String.Empty)
        //    {
        //        //this.BeginInvoke(new SetTextCallback(SetText), new object[] { InputData });
        //    }
        //}


        public string ReadExisting(object sender, SerialDataReceivedEventArgs e)
        {
            return ComPortCat.ReadExisting();
        }

        public void Write(string text)
        {
            ComPortCat.Write(text);
        }




    }
}
