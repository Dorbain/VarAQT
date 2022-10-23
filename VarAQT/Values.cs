using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VarAQT.Models;

namespace VarAQT
{
    public class Values
    {
        public static StationDetails stationDetails { get; set; }

        public static ConnectedStationDetails connectedStationDetails { get; set; }

        public static string InputData = String.Empty;

        public static bool KissEnabled = false;
        public static bool Busy { get; set; }
        public static string Smeter { get; set; }
        public static string SignalNoice { get; set; }
        public static readonly string VARACommandClientIP = Properties.Settings.Default.VaraCommandClientIP;
        public static readonly int VARACommandClientPort = Properties.Settings.Default.VaraCommandClientPort;
        public static readonly string VARADataClientIP = Properties.Settings.Default.VaraCommandClientIP;
        public static readonly int VARADataClientPort = VARACommandClientPort + 1;
        public static readonly string VARAKISSClientIP = Properties.Settings.Default.VaraCommandClientIP;
        public static readonly int VARAKISSClientPort = Properties.Settings.Default.VaraKissClientPort;
        public static readonly bool VARAMonitorEnabled = Properties.Settings.Default.VaraMonitorEnabled;
        public static readonly string VARAMonitorCommandClientIP = Properties.Settings.Default.VaraMonitorCommandClientIP;
        public static readonly int VARAMonitorCommandClientPort = Properties.Settings.Default.VaraMonitorCommandClientPort;
        public static readonly string VARAMonitorDataClientIP = Properties.Settings.Default.VaraMonitorCommandClientIP;
        public static readonly int VARAMonitorDataClientPort = VARAMonitorCommandClientPort + 1;
        public static readonly string VARAMonitorKISSClientIP = Properties.Settings.Default.VaraMonitorCommandClientIP;
        public static readonly int VARAMonitorKISSClientPort = Properties.Settings.Default.VaraMonitorKissClientPort;
        public static bool StationConnected { get; set; }
        public static bool VaraConnected { get; set; }
        public static bool OutGoingConnection { get; set; }
        public static bool BeaconActive { get; set; }

        public const string Incomming = " <- ";
        public const string Outgoing = " -> ";
        public static int RecieveBuffer { get; set; }
        public static int RecieveBufferMaxSize { get; set; }
        public static int SendBuffer { get; set; }
        public static int SendBufferMaxSize { get; set; }
        public static bool SendCQ { get; set; }
        public static int BaseFrequency { get; set; }
        public static int BaseFrequencyBand { get; set; }
        public static int BaseChannelFrequency { get; set; }
        public static int FrequencyToUse { get; set; }
        public static string VaraSIDToUse { get; set; }
        public static bool Ping { get; set; }


        // for new data recieved

        public static string VaraDataRecievedBufferText { get; set; }



    }

    public class Strings
    {
        // Private text
        
        public string WelcomeStr = "Welcome to my station. it is still a new work in progress... \r\nWant to know more just ask me.";
        public string AwayResponse = "I am sorry that you are away. Maybe next time.";
        public string GoodByeResponse = "73's de " + Values.stationDetails.CallSign + " - Good Bye." ;
        
        // Application text

        public const string ConectedToVaraStr = "Connected to VARA";
        public const string DisconnectedFromVaraStr = "Disconnected from VARA";
        public const string UnknownVaraStatusStr = "Unknown status from VARA";
        public const string FreeStr = "FREE";
        public const string BusyStr = "BUSY";
        public const string TXStr = "TX";
        public const string RXStr = "RX";
        public const string IncommingPing = "Incomming Ping!";
        public const string OutgoingPing = "Outgoing Ping!";
        public const string ErrorNotSupported = "<ERR> Not supported!";
        public const string VaraCommandChannelErrorStr = "\n VARA Command channel error : ";
        public const string VaraDataChannelErrorStr = "\n VARA Data channel error : ";
        public const string VaraCommandClientWriteFailedDisconnectedStr = "VARA Command Client Write (Failed) : Disconnected";
        public const string VaraDataClientWriteFailedDisconnectedStr = "VARA Data Client Write (Failed) : Disconnected";
        public const string VaraCommandSendFailedNoCommandStr = "Send Command to VARA moden failed : No command given.";
        public const string TimeStr = "Time";
        public const string CallSignStr = "CallSign";
        public const string SIDStr = "SID";
        public const string SignalNoiseRatioStr = "SNR";
        public const string CountStr = "Cnt";
        public const string SmeterStr = "SM";

        // Channel names

        public const string Channel1Str = "Channel 1";
        public const string Channel2Str = "Channel 2";
        public const string Channel3Str = "Channel 3";
        public const string Channel4Str = "Channel 4";
        public const string Channel5Str = "Channel 5";
        public const string Channel6Str = "Channel 6";
        public const string Channel7Str = "Channel 7";
        public const string Channel8Str = "Channel 8";
        public const string Channel9Str = "Channel 9 - CQ";
        public const string Channel10Str = "Channel 10";
        public const string Channel11Str = "Channel 11";
        public const string Channel12Str = "Channel 12";
        public const string Channel13Str = "Channel 13";
        public const string Channel14Str = "Channel 14";
        public const string Channel15Str = "Channel 15";
        public const string Channel16Str = "Channel 16";

        // Buttons

        public const string ConnectToVaraButtonStr = "Connect to VARA";
        public const string DisconnectFromVaraButtonStr = "Disconnect from VARA";
        public const string SendBeaconTimerButtonStr = "Send Beacon 5 Minutes";
        public const string SendBeaconNowButtonStr = "Send Beacon NOW";
        public const string StopTxNowButtonStr = "Stop Beacon and TXStr";
        public const string SendCQButtonStr = "Send CQ";
        public const string ConnectToStationButtonStr = "Connect";
        public const string PingAStationButtonStr = "Ping";
        public const string DisconnectFromStationButtonStr = "Disconnect";
        public const string AbortConnectionButtonStr = "Abort";
        public const string SendTextButtonStr = "Send";
        public const string button6 = "Enable VARA Monitor";



    }

    public class FileNames
    {
        public const string LastHeardFileName = "LastHeard.xml";

    }


    public class RecievedTag
    {
        // Recieved during a connection with information:
        public const string signal = "<R"; // signal recievd from connected station.
        // public static string fullcall = "<FC:"; // its full call sign?
        public const string name = "<NAME:"; // name recieved from connected station.
        public const string qth = "<QTH:"; // City recieved from connected station.
        public const string locator = "<LOC:"; // Locator recieved from connected station.

        // Tags that need to send requested information:
        public const string info = "<INFO>"; // Asks for detailed station information.
        public const string snrrequest = "<SNRR>"; // Asks for signal report.
        public const string version = "<VER>"; // Asks for the VARA version.
        public const string lastheard = "<LHR>"; // Asks for last heard station.

        // Tags that should trigger an event:
        public const string away = "<AWAY>";
        public const string disconnect = "<DISC>"; // Should do a bye bye and disconnect.

        public const string qsyu = "<QSYU>";
        public const string qsyd = "<QSYD>";
    }
    public class SendTag
    {
        public const string signal = "<R";
        public const string fullcall = "<FC:";
        public const string name = "<NAME:";
        public const string qth = "<QTH:";
        public const string locator = "<LOC:";
        public const string lastheardempty = "<LHE>";
        public const string lastheardreject = "<LHJ>";
        public const string away = "<AWAY>";
        public const string disconnect = "<DISC>";
        public const string version = "Another VARA Chat App. v 1.0.0.0";
        public const string end = ">";
    }
    public class Tags
    {
        public const string callsign = "<CALL>";
        public const string namerequest = "<NAME>";
        public const string qthrequest = "<QTH>";
        public const string locatorrequest = "<LOC>";
        public const string rig = "<RIG>";
        public const string antenna = "<ANT>";
    }
    /// <summary>
    /// Frequency Band Selections.
    /// </summary>
    public class Band
    {
        /// <summary>
        /// 28–29.7 MHz - 10 Meter amateur radio band.
        /// </summary>
        public const int MeterIs10 = 28;
        /// <summary>
        /// 11 Meter amateur radio band 26,965 - 27,405 MHz devided in 40 channels at 10 KHZ
        /// </summary>
        public const int MeterIs11 = 27;
        /// <summary>
        /// 24.89–24.99 MHz - 12 Meter amateur radio band.
        /// </summary>
        public const int MeterIs12 = 24;
        /// <summary>
        /// 21–21.45 MHz - 15 Meter amateur radio band.
        /// </summary>
        public const int MeterIs15 = 21;
        /// <summary>
        /// 18.068–18.168 MHz - 17 Meter amateur radio band.
        /// </summary>
        public const int MeterIs17 = 18;
        /// <summary>
        /// 14.000–14.350 MHz - 20 Meter amateur radio band.
        /// </summary>
        public const int MeterIs20 = 14;
        /// <summary>
        /// 10.1–10.15 MHz - 30 Meter amateur radio band.
        /// </summary>
        public const int MeterIs30 = 10;
        /// <summary>
        /// 7.0–7.3 MHz - 40 Meter amateur radio band.
        /// </summary>
        public const int MeterIs40 = 7;
        /// <summary>
        /// 5 MHz region - 60 Meter amateur radio band.
        /// </summary>
        public const int MeterIs60 = 5;
        /// <summary>
        /// 3.5–4.0 MHz - 80 Meter amateur radio band.
        /// </summary>
        public const int MeterIs80 = 3;
        /// <summary>
        /// 160 Meter amateur radio band.
        /// </summary>
        public const int MeterIs160 = 1; // ??
    }
    /// <summary>
    /// Also known as frequency slot.
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// The centre frequency used for VARA communication as a starting point.
        /// </summary>
        private const int BaseChannel = 105000;
        public const int Channel1 = BaseChannel -750; // Base Channel ferquency - 750
        public const int Channel2 = BaseChannel - 1500; // Base Channel frequency - 1500
        public const int Channel3 = BaseChannel - 2250; // Base Channel frequency - 2250
        public const int Channel4 = BaseChannel - 3000; // Base Channel frequency - 3000
        public const int Channel5 = BaseChannel - 3750; // Base Channel frequency - 3750
        public const int Channel6 = BaseChannel - 0; // Base Channel frequency - 
        public const int Channel7 = BaseChannel - 0; // Base Channel frequency - 
        public const int Channel8 = BaseChannel - 0; // Base Channel frequency - 
        public const int Channel9 = BaseChannel; // Base Channel frequency - used for CQ
        public const int Channel10 = BaseChannel + 0; // Base Channel frequency + ?
        public const int Channel11 = BaseChannel + 750; // Base Channel frequency + 750
        public const int Channel12 = BaseChannel + 1500; // Base Channel frequency + 1500
        public const int Channel13 = BaseChannel + 2250; // Base Channel frequency + 2250
        public const int Channel14 = BaseChannel + 3000; // Base Channel frequency + 3000
        public const int Channel15 = BaseChannel + 3750; // Base Channel frequency + 3750
        public const int Channel16 = BaseChannel + 0; // Base Channel frequency - 
    }
}
