using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VarAQT.Models;

namespace VarAQT
{
    public static class Values
    {
        public static StationDetails stationDetails { get; set; }

        public static string InputData = String.Empty;

        public static bool kissEnabled = false;
        public static bool busy { get; set; }
        public static string sMeter { get; set; }
        public static string signalNoice { get; set; }
        //public static string callSign = Values.stationDetails.CallSign;
        //public static string name = Properties.Settings.Default.Name;
        //public static string QTH = Properties.Settings.Default.QTH;
        //public static string rig = Properties.Settings.Default.Rig;
        //public static string grid = Properties.Settings.Default.GridLocator;

        public static string VARACommandClientIP = Properties.Settings.Default.VaraCommandClientIP;
        public static int VARACommandClientPort = Properties.Settings.Default.VaraCommandClientPort;

        public static string VARADataClientIP = Properties.Settings.Default.VaraCommandClientIP;
        public static int VARADataClientPort = VARACommandClientPort + 1;

        public static string VARAKISSClientIP = Properties.Settings.Default.VaraCommandClientIP;
        public static int VARAKISSClientPort = Properties.Settings.Default.VaraKissClientPort;

        public static bool VARAMonitorEnabled = Properties.Settings.Default.VaraMonitorEnabled;
        
        public static string VARAMonitorCommandClientIP = Properties.Settings.Default.VaraMonitorCommandClientIP;
        public static int VARAMonitorCommandClientPort = Properties.Settings.Default.VaraMonitorCommandClientPort;

        public static string VARAMonitorDataClientIP = Properties.Settings.Default.VaraMonitorCommandClientIP;
        public static int VARAMonitorDataClientPort = VARAMonitorCommandClientPort + 1;

        public static string VARAMonitorKISSClientIP = Properties.Settings.Default.VaraMonitorCommandClientIP;
        public static int VARAMonitorKISSClientPort = Properties.Settings.Default.VaraMonitorKissClientPort;

        public static bool stationConnected { get; set; }
        public static bool varaConnected { get; set; }
        public static bool outGoingConnection { get; set; }

        public static bool beaconActive { get; set; }

        public static string incomming = " <- ";
        public static string outgoing = " -> ";

        public static int recieveBuffer { get; set; }
        public static int recieveBufferSize { get; set; }
        public static int sendBuffer { get; set; }
        public static int sendBufferSize { get; set; }

        public static bool sendCQ { get; set; }



    }

    public static class RecievedTag
    {
        // Recieved during a connection with information:
        public static string signal = "<R"; // signal recievd from connected station.
        // public static string fullcall = "<FC:"; // its full call sign?
        public static string name = "<NAME:"; // name recieved from connected station.
        public static string qth = "<QTH:"; // City recieved from connected station.
        public static string locator = "<LOC:"; // Locator recieved from connected station.

        // Tags that need to send requested information:
        public static string info = "<INFO>"; // Asks for detailed station information.
        public static string snrrequest = "<SNRR>"; // Asks for signal report.
        public static string version = "<VER>"; // Asks for the VARA version.
        public static string lastheard = "<LHR>"; // Asks for last heard station.

        // Tags that should trigger an event:
        public static string away = "<AWAY>";
        public static string disconnect = "<DISC>"; // Should do a bye bye and disconnect.

        public static string qsyu = "<QSYU>";
        public static string qsyd = "<QSYD>";

        
        
        
        //public static string qsyfstart = "<QSYF>";
        //public static string qsyfend = "</QSYF>";


    }
    public static class SendTag
    {
        public static string signal = "<R";
        public static string fullcall = "<FC:";
        public static string name = "<NAME:";
        public static string qth = "<QTH:";
        public static string locator = "<LOC:";
        public static string lastheardempty = "<LHE>";
        public static string lastheardreject = "<LHJ>";
        public static string away = "<AWAY>";
        public static string disconnect = "<DISC>";
        public static string version = "Another VARA Chat App. v 1.0.0.0";
        public static string end = ">";
    }

    public static class Tags
    {
        public static string callsign = "<CALL>";
        public static string namerequest = "<NAME>";
        public static string qthrequest = "<QTH>";
        public static string locatorrequest = "<LOC>";
        public static string rig = "<RIG>";
        public static string antenna = "<ANT>";
    }
}
