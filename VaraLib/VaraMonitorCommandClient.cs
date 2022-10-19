using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Logger;
using System.Reflection;

namespace VaraLib
{
    public class VARAMonitorCommandClient
    {
        // *** Event Handlers *** //

        /// <summary>
        /// Notify the Recieved Data
        /// </summary>
        /// <param name="data">Recieved Data</param>
        public delegate void DataReceivedEventHandler(String data);
        public event DataReceivedEventHandler OnDataRecievedEvent;

        /// <summary>
        /// Notify the Connection Status of Socket
        /// </summary>
        /// <param name="status">Connection Status</param>
        public delegate void OnConnectEventHandler(bool status);
        public event OnConnectEventHandler OnConnectEvent;

        // *** Properties *** //

        // Connection Parameters
        private IPAddress ipAddress;
        private int port;

        // Socket Parameters
        private Socket socket;
        private byte[] readerBuffer = new byte[256];

        private string ClassName = "VaraMonitorCommandClient";

        // *** Methods *** //

        /// <summary>
        /// Create a TCP Asynchronous Client. This client is connect to the server and port with passed parameters.
        /// </summary>
        /// <param name="_ip">Server IP</param>
        /// <param name="_port">Server Port</param>
        public VARAMonitorCommandClient(string _ip, int _port)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            ipAddress = IPAddress.Parse(_ip);
            port = _port;
            Log.Info(_ip + ":" + port, ClassName);
        }
        /// <summary>
        /// VARADataClientConnect to the server
        /// </summary>
        public void VARAMonitorCommandConnect()
        {
            try
            {
                Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
                // Close the socket if open
                if (socket != null && socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    System.Threading.Thread.Sleep(10);
                    socket.Close();
                }

                // Create the socket object
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Define the Server address and port
                IPEndPoint epServer = new IPEndPoint(ipAddress, port);

                // VARADataClientConnect to server non-Blocking method
                socket.Blocking = false;
                AsyncCallback onconnect = new AsyncCallback(OnVARAMonitorCommandClientConnect);
                socket.BeginConnect(epServer, onconnect, socket);
                Log.Info("Connected", ClassName);
            }
            catch (Exception ex)
            {
                OnConnectEvent(false);
                Log.Error(ex.ToString(), ClassName);
                Log.Error(ex.Message.ToString(), ClassName);
                //throw new Exception("Socket Connection Failed. Message : " + ex.ToString());
            }
        }

        /// <summary>
        /// Check connection status of the socket
        /// </summary>
        /// <returns>True or False based on status</returns>
        public bool IsVARAMonitorCommandClientConnected()
        {
            if (socket != null)
            {
                return socket.Connected;
            }
            return false;
        }

        // Setup Callbacks if Socket is Connected
        private void OnVARAMonitorCommandClientConnect(IAsyncResult ar)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            Socket _socket = (Socket)ar.AsyncState;

            try
            {
                if (_socket.Connected)
                {
                    SetupRecieveVARAMonitorCommandClientCallback(_socket);
                    OnConnectEvent(true);
                    Log.Info("Socket Connection established", ClassName);
                }
                else
                {
                    OnConnectEvent(false);
                    Log.Error("Cannot Establish the Socket Connection", ClassName);  
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString(), ClassName);
                Log.Error(ex.Message.ToString(), ClassName);
            }
        }

        // Setup Recieve Callback for Async Listning
        private void SetupRecieveVARAMonitorCommandClientCallback(Socket _socket)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            try
            {
                AsyncCallback recieveData = new AsyncCallback(OnVARAMonitorCommandClientRecieved);
                _socket.BeginReceive(readerBuffer, 0, readerBuffer.Length, SocketFlags.None, recieveData, _socket);
            }
            catch (Exception ex)
            {
                VARAMonitorCommandClientDispose();
                Log.Error(ex.ToString(), ClassName);
                Log.Error(ex.Message.ToString(), ClassName);
            }
        }

        // Recieve data from TCP
        private void OnVARAMonitorCommandClientRecieved(IAsyncResult ar)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            Socket _socket = (Socket)ar.AsyncState;
            if (IsVARAMonitorCommandClientConnected())
            {
                try
                {
                    // Check data is available
                    int nBytesRec = _socket.EndReceive(ar);
                    if (nBytesRec > 0)
                    {
                        string sRecieved = "";
                        for (int i = 0; i < nBytesRec; i++)
                        {
                            sRecieved += (char)readerBuffer[i];
                        }
                        // Fire Data Recieved Event
                        OnDataRecievedEvent(sRecieved);
                        Log.Info(sRecieved.ToString(), ClassName);
                        // If the Connection is Still Usable Restablish the Callback
                        SetupRecieveVARAMonitorCommandClientCallback(_socket);
                    }
                    else
                    {
                        VARAMonitorCommandClientDispose();
                    }
                }
                catch (Exception ex)
                {
                    VARAMonitorCommandClientDispose();
                    Log.Error(ex.ToString(), ClassName);
                    Log.Error(ex.Message.ToString(), ClassName);
                    //throw new Exception("Recieve Operation Failed " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// VARACommandClientWrite Data to Socket
        /// </summary>
        /// <param name="_data">Data to be written</param>
        /// <returns>Success status as Boolean Value</returns>
        public bool VARAMonitorCommandClientWrite(String _data)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            // Check Connection
            if (IsVARAMonitorCommandClientConnected())
            {
                try
                {
                    Byte[] byteDateLine = Encoding.ASCII.GetBytes(_data.ToCharArray());
                    socket.Send(byteDateLine, byteDateLine.Length, 0);
                    Log.Info(_data.ToString(), ClassName);
                    return true;
                }
                catch (Exception ex)
                {
                    VARAMonitorCommandClientDispose();
                    Log.Error(ex.ToString(), ClassName);
                    Log.Error(ex.Message.ToString(), ClassName);
                    return false;
                    //throw new Exception("Data Writing Operation Failed " + ex.ToString());
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// VARACommandClientWrite Data to Socket
        /// </summary>
        /// <param name="_data">Data to be written</param>
        /// <returns>Success status as Boolean Value</returns>
        public bool VARAMonitorCommandClientWrite(byte[] _data)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            // Check Connection
            if (IsVARAMonitorCommandClientConnected())
            {
                try
                {
                    socket.Send(_data, _data.Length, 0);
                    Log.Info(_data.ToString(), ClassName);
                    return true;
                }
                catch (Exception ex)
                {
                    VARAMonitorCommandClientDispose();
                    Log.Error(ex.ToString(), ClassName);
                    Log.Error(ex.Message.ToString(), ClassName);
                    return false;
                    //throw new Exception("Data Writing Operation Failed " + ex.ToString());
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Close the Socket Connection
        /// </summary>
        private void VARAMonitorCommandClientDispose()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            if (socket != null && socket.Connected)
            {
                OnConnectEvent(false);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }

        /// <summary>
        /// VARADataClientDisconnect the socket
        /// </summary>
        public void VARAMonitorCommandClientDisconnect()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            if (socket != null && socket.Connected)
            {
                OnConnectEvent(false);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }


}
