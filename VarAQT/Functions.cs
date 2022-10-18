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
using System.Xml.Serialization;

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

        public static void WriteXML<T>(List<T> list, string filename)
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


        public static List<T> ReadXML<T>(string filename)
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


    }
}
