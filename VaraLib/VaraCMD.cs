using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaraLib
{
    public class VaraCMD
    {
        /// <summary>
        /// CONNECT Source Destination<cr> (VARA HF and (VARA SAT)
        /// CONNECT Source Destination via Digi1 Digi2<cr>(VARA FM)
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        public static string connect(string owncall, string connectto) { return "CONNECT " + owncall + " " + connectto + "\r"; }
        /// <summary>
        /// Incomming connections enabled.
        /// This command will cause a disconnection if it is received in the middle of a VARA connection
        /// </summary>
        public static string listenON { get { return "LISTEN ON\r"; } }
        /// <summary>
        /// Incomming connections disabled
        /// This command will cause a disconnection if it is received in the middle of a VARA connection
        /// </summary>
        public static string listenOFF { get { return "LISTEN OFF\r"; } }
        /// <summary>
        /// Incomming CQ enabled.
        /// This command will cause a disconnection if it is received in the middle of a VARA connection
        /// </summary>
        public static string listenCQ { get { return "LISTEN CQ\r"; } }
        /// <summary>
        /// MYCALL Call1 Call2 Call3 Call4 Call5<cr>
        /// Set current call sign(maximum 5 call signs). Legitimate call signs include from 3 to 7 ASCII
        /// characters(A-Z, 0-9) followed by an optional “-“ and an SSID of -1 to -15, -T, and -R.
        /// </summary>
        /// <param name="call"></param>
        /// <returns></returns>
        public static string myCall(string call) { return "MYCALL " + call + " " + call + "-T \r"; }
        /// <summary>
        /// VARADataClientDisconnect the link, once the TX buffer is empty.
        /// </summary>
        public static string disconnect { get { return "DISCONNECT\r"; } }
        /// <summary>
        /// VARADataClientDisconnect the link inmediately. (dirty disconnect)
        /// </summary>
        public static string abort { get { return "ABORT\r"; } }
        /// <summary>
        /// Compression disabled
        /// </summary>
        public static string compressionOFF { get { return "COMPRESSION OFF\r"; } }
        /// <summary>
        /// Huffman compression enabled, designed for type text information. Recommended for Winlink.
        /// </summary>
        public static string compressionText { get { return "COMPRESSION TEXT\r"; } }
        /// <summary>
        /// Compression designed for File transfers
        /// </summary>
        public static string compressionFiles { get { return "COMPRESSION FILES\r"; } }
        /// <summary>
        /// Set VARA HF to 500Hz Narrow mode
        /// </summary>
        public static string bw500 { get { return "BW500\r"; } }
        /// <summary>
        /// Set VARA HF to 2300Hz Standard mode
        /// </summary>
        public static string bw2300 { get { return "BW2300\r"; } }
        /// <summary>
        /// Set VARA HF to 2750Hz Tactical mode
        /// </summary>
        public static string bw2750 { get { return "BW2750\r"; } }
        /// <summary>
        /// Optimizes VARA timing for using with chat type apps like VARA Chat, VarAC, vARIM ....
        /// Listen then CQ Frames
        /// Support high latency to connect two FlexRadio: SDR<->SDR
        /// Infinite Idle loop.Allows both stations to be in sync forever, until the path dies
        /// Send the SN command for each data block received.
        /// Optimize the handover interchange for keyboard to keyboard.
        /// This command should not be used with Winlink or B2F protocol apps.
        /// Includes the LISTEN ON command
        /// </summary>
        public static string chatON { get { return "CHAT ON\r"; } }
        /// <summary>
        /// Optimize the handover interchange for Winlink, B2F protocol, BBS, etc...
        /// Limited Idle Loops.Avoid the stations stay connected forever in a loop.
        /// Latency limited according Trimode Scan time of 4 seconds.Only one Flexradio can be used in the link: SDR<->Analog Rig or Analog Rig<->SDR
        /// </summary>
        public static string chatOFF { get { return "CHAT OFF\r"; } }
        /// <summary>
        /// VARA SAT, Send a CQ frame. Useful for type chat apps.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string cqFrame(string source) { return "CQFRAME " + source + "\r"; }
        /// <summary>
        /// VARA HF, Send a CQ frame with 500hz Bandwidth. Useful for type chat apps.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string cqFrame500(string source) { return "CQFRAME " + source + " 500\r"; }
        /// <summary>
        /// VARA HF, Send a CQ frame with 2300hz Bandwidth. Useful for type chat apps.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string cqFrame2300(string source) { return "CQFRAME " + source + " 2300\r"; }
        /// <summary>
        /// VARA HF, Send a CQ frame with 2750hz Bandwidth. Useful for type chat apps.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string cqFrame2750(string source) { return "CQFRAME " + source + " 2750\r"; }
        /// <summary>
        /// VARA send retries following a 4.0 seconds cycle, necessary to connect with the RMS Gateways (DWELL time 4s)
        /// </summary>
        public static string winlinkSession { get { return "WINLINK SESSION\r"; } }
        /// <summary>
        /// Set the retrie cycle to 4.6 seconds to allow connecting two SDR's at maximum latency (worst case)
        /// This command must be used for P2P connections, not for Gateways connections.
        /// </summary>
        public static string p2pSession { get { return "P2P SESSION\r"; } }
    }
}
