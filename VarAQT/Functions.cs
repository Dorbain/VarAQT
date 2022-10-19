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
using VarAQT.Models;
using System.Runtime.ConstrainedExecution;

namespace VarAQT
{
    public static class Functions
    {
        public static string Frequency(string text)
        {
            char[] remove = { ';', 'F', 'A', 'B' };
            text = text.Trim(remove);
            text = text.Insert(3, ".");
            text = text.Insert(7, ".");
            return text;
        }

        /// <summary>
        /// Removes the S,M and ; from the string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Smeter(string text)
        {
            char[] remove = { ';', 'S', 'M' };
            text = text.Trim(remove);
            return text;
        }

        /// <summary>
        /// Removes the S,N etc. from the string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Snmeter(string text)
        {
            char[] remove = { ' ', ',', '.', 'S', 'N' };
            text = text.Trim(remove);
            return text;
        }

        public static void WriteLastHeardXML<T>(List<T> list, string filename)
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


        public static List<T> ReadLastHeardXML<T>(string filename)
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

        public static void WriteStationDetailsXML(StationDetails stationDetails)
        {
            XmlSerializer writer = new XmlSerializer(typeof(StationDetails));
            var path = "StationDetails.xml";
            FileStream file = File.Create(path);
            writer.Serialize(file, stationDetails);
            file.Close();
        }

        public static StationDetails readStationDetailsXML()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString());
            StationDetails result = new StationDetails();
            XmlSerializer reader = new XmlSerializer(typeof(StationDetails));
            var filePath = "StationDetails.xml";

            if (!File.Exists(filePath))
            {
                return new StationDetails();
            }

            using (FileStream myFileStream = new FileStream(filePath, FileMode.Open))
            {
                result = (StationDetails)reader.Deserialize(myFileStream);
            }
            return result;
        }


    }
}
