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
 





    }
}
