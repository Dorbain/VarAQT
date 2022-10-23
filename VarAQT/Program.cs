using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using VaraLib;
using Logger;
using System.Reflection;

namespace VarAQT
{
    internal static class Program
    {

        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Log.enableDebug = true;
            Log.enableInfo = true;
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            Values.stationDetails = Functions.readStationDetailsXML();
            Values.connectedStationDetails = new Models.ConnectedStationDetails();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new varAQT());
            Functions.WriteStationDetailsXML(Values.stationDetails);
        }
    }
}
