using Logger;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

// Added
using System.Net;
using System.Net.Sockets;
using System.IO;

//using VaraCommand;
using VaraLib;

namespace VarAQT
{
    public static class Functions
    {
        public static string frequency(string text)
        {
            char[] remove = { ';', 'F', 'A', 'B' };
            text = text.Trim(remove);
            text = text.Insert(3, ".");
            text = text.Insert(7, ".");
            return text;
        }
        public static string sMeter(string text)
        {
            char[] remove = { ';', 'S', 'M' };
            text = text.Trim(remove);
            return text;
        }

        public static string sNMeter(string text)
        {
            char[] remove = { ' ', ',', '.', 'S', 'N', ',' };
            text = text.Trim(remove);
            return text;
        }


    }
}
