using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VarAQT
{
    public static class Values
    {
        public static string InputData = String.Empty;

        public static bool kissEnabled = false;
        public static bool busy { get; set; }
        public static string sMeter { get; set; }
        public static string signalNoice { get; set; }
        public static string callSign = Properties.Settings.Default.CallSign;
        public static string name = Properties.Settings.Default.Name;
        public static string QTH = Properties.Settings.Default.QTH;
        public static string rig = Properties.Settings.Default.Rig;
        public static string grid = Properties.Settings.Default.GridLocator;

        public static string VARACommandClientIP = Properties.Settings.Default.VaraCommandClientIP;
        public static int VARACommandClientPort = Properties.Settings.Default.VaraCommandClientPort;

        public static string VARADataClientIP = Properties.Settings.Default.VaraCommandClientIP;
        public static int VARADataClientPort = VARACommandClientPort + 1;

        public static string VARAKISSClientIP = Properties.Settings.Default.VaraCommandClientIP;
        public static int VARAKISSClientPort = Properties.Settings.Default.VaraKissClientPort;

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






    }

    public static class RecievedTag
    {
        public static string signal = "<R";
        public static string fullcall = "<FC:";
        public static string name = "<NAME:";
        public static string qth = "<QTH:";
        public static string locator = "<LOC:";
        public static string callsign = "<CALL>";
        public static string namerequest = "<NAME>";
        public static string qthrequest = "<QTH>";
        public static string locatorrequest = "<LOC>";
        public static string rig = "<RIG>";
        public static string antenna = "<ANT>";
        public static string hcall = "<HCALL>";
        public static string hname = "<HNAME>";
        public static string hqth = "<HQTH>";
        public static string hlocator = "<HLOC>";
        public static string lastheard = "<LHR>";
        public static string qsyu = "<QSYU>";
        public static string qsyd = "<QSYD>";
        public static string version = "<VER>";
        public static string info = "<INFO>";
        public static string snrrequest = "<SNRR>";
        public static string away = "<AWAY>";
        public static string disconnect = "<DISC>";
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
    }
}
