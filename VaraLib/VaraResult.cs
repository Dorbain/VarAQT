using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaraLib
{
    public class VARAResult
    {
        public const string connected = "CONNECTED";
        public const string disconnected = "DISCONNECTED";
        public const string pttOFF = "PTT OFF";
        public const string pttON = "PTT ON";
        public static string buffer(string Bytes) { return "BUFFER " + Bytes + "\r"; }
        public const string pending = "PENDING";
        public const string cancelPending = "CANCELPENDING";
        public const string busyOFF = "BUSY OFF";
        public const string busyON = "BUSY ON";
        public const string registered = "REGISTERED";
        public const string linkRegistered = "LINK REGISTERED";
        public const string linkUnRegistered = "LINK UNREGISTERED";
        public const string IAmAlive = "IAMALIVE";
        public const string missingSoundCard = "MISSING SOUNDCARD";
        public const string cqFrame = "CQFRAME ";
        /// <summary>
        /// VARA HF, A CQ frame have been decoded at 500hz bandwidth. Useful for type chat apps.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string cqFrame500(string source) { return cqFrame + source + " 500\r"; }
        /// <summary>
        /// VARA HF, A CQ frame have been decoded at 2300hz bandwidth. Useful for type chat apps.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string cqFrame2300(string source) { return cqFrame + source + " 2300\r"; }
        /// <summary>
        /// VARA HF, A CQ frame have been decoded at 2750hz bandwidth. Useful for type chat apps.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string cqFrame2750(string source) { return cqFrame + source + " 2750\r"; }
        public static string sn(string Bytes) { return "SN " + Bytes + "\r"; }
        public const string ok = "OK";
        public const string wrong = "WRONG";
    }
}
