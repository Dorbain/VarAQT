using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;


namespace Logger
{
    public class Log
    {
        private static string FileNameExtention
        {
            get
            {
                string fileName = "_" + DateTime.Now.Date.ToString("ddMMyyyy") + ".log";
                return fileName;
            }
        }
        private static string PathName = AppDomain.CurrentDomain.BaseDirectory + "\\logs\\";
        public static bool enableDebug { get; set; }
        public static bool enableInfo { get; set; }
        public static bool enableError = true;
        private static async Task WriteFileAsync(string message, string type, string _className)
        {
            string FileName = PathName + _className + FileNameExtention;
            using (StreamWriter logFile = new StreamWriter(FileName, append: true))
            {
                await logFile.WriteLineAsync(DateTime.Now + ":" + DateTime.Now.Millisecond + type + message);
            }
        }
        public static bool Info(string message, string _className)
        {
            if (enableInfo)
            {
                Task asyncTask = WriteFileAsync(message, " |INFO | ", _className);
                return true;
            }
            return false;
        }
        public static bool Info(string message)
        {
            if (enableInfo)
            {
                Task asyncTask = WriteFileAsync(message, " |INFO | ", String.Empty);
                return true;
            }
            return false;
        }
        public static bool Error(string message, string _className)
        {
            if (enableError)
            {
                Task asyncTask = WriteFileAsync(message, " |ERROR| ", _className);
                return true;
            }
            return false;
        }
        public static bool Error(string message)
        {
            if (enableError)
            {
                Task asyncTask = WriteFileAsync(message, " |ERROR| ", String.Empty);
                return true;
            }
            return false;
        }
        public static bool Debug(string message, string _className)
        {
            if (enableDebug)
            {
                if (!message.Contains("MoveNext"))
                {
                    Task asyncTask = WriteFileAsync(message, " |DEBUG| ", _className);
                }
                return true;
            }
            return false;
        }
        public static bool Debug(string message)
        {
            if (enableDebug)
            {
                if (!message.Contains("MoveNext"))
                {
                    Task asyncTask = WriteFileAsync(message, " |DEBUG| ", String.Empty);
                }
                return true;
            }
            return false;
        }
        public static bool Fault(string message, string _className)
        {
            if (enableDebug)
            {
                if (!message.Contains("MoveNext"))
                {
                    Task asyncTask = WriteFileAsync(message, " |FAULT| ", _className);
                }
                return true;
            }
            return false;
        }
        public static bool Fault(string message)
        {
            if (enableDebug)
            {
                if (!message.Contains("MoveNext"))
                {
                    Task asyncTask = WriteFileAsync(message, " |FAULT| ", String.Empty);
                }
                return true;
            }
            return false;
        }
    }
}
