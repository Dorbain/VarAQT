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
    public class VARADataClient
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

        private string ClassName = "VaraDataClient";

        // *** Methods *** //

        /// <summary>
        /// Create a TCP Asynchronous Client. This client is connect to the server and port with passed parameters.
        /// </summary>
        /// <param name="_ip">Server IP</param>
        /// <param name="_port">Server Port</param>
        public VARADataClient(string _ip, int _port)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            ipAddress = IPAddress.Parse(_ip);
            port = _port;
            Log.Info(_ip + ":" + port, ClassName);
        }

        public void VARADataClientConnect()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            try
            {
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
                AsyncCallback onconnect = new AsyncCallback(OnVARADataClientConnect);
                socket.BeginConnect(epServer, onconnect, socket);
            }
            catch (Exception ex)
            {
                OnConnectEvent(false);
                Log.Error(ex.ToString(), ClassName);
                Log.Error(ex.Message.ToString(), ClassName);
                //throw new Exception("Socket Connection Falied. Message : " + ex.ToString());
            }
        }

        /// <summary>
        /// Check connection status of the socket
        /// </summary>
        /// <returns>True or False based on status</returns>
        public bool IsVARADataClientConnected()
        {
            if (socket != null)
            {
                return socket.Connected;
            }
            return false;
        }

        // Setup Callbacks if Socket is Connected
        private void OnVARADataClientConnect(IAsyncResult ar)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            Socket _socket = (Socket)ar.AsyncState;
            try
            {
                if (_socket.Connected)
                {
                    SetupRecieveVARADataClientCallback(_socket);
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
                //throw ex;
            }
        }

        // Setup Recieve Callback for Async Listning
        private void SetupRecieveVARADataClientCallback(Socket _socket)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            try
            {
                AsyncCallback recieveData = new AsyncCallback(OnVARADataClientRecievedData);
                _socket.BeginReceive(readerBuffer, 0, readerBuffer.Length, SocketFlags.None, recieveData, _socket);
            }
            catch (Exception ex)
            {
                VARADataClientDispose();
                Log.Error(ex.ToString(), ClassName);
                Log.Error(ex.Message.ToString(), ClassName);
            }
        }

        // Recieve data from TCP
        private void OnVARADataClientRecievedData(IAsyncResult ar)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            Socket _socket = (Socket)ar.AsyncState;

            if (IsVARADataClientConnected())
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
                        SetupRecieveVARADataClientCallback(_socket);
                    }
                    else
                    {
                        VARADataClientDispose();
                    }
                }
                catch (Exception ex)
                {
                    VARADataClientDispose();
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
        public bool VARADataClientWrite(String _data)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            // Check Connection
            if (IsVARADataClientConnected())
            {
                string dataToSend = Convert.ToString(_data.Length) + " " + _data.ToString();
                Byte[] byteDateLine = Encoding.UTF8.GetBytes(dataToSend.ToCharArray()); // ASCII naar UTF8 gezet
                try
                {
                    //byte[] buffer = new byte[0];
                    //if (_data.encoding == "ASCII")
                    //    buffer = Encoding.ASCII.GetBytes(data);
                    //else if (this.encoding == "UTF8")
                    //    buffer = Encoding.UTF8.GetBytes(data);
                    socket.Send(byteDateLine, byteDateLine.Length, 0);
                    Log.Info(dataToSend.ToString(), ClassName);
                    return true;
                }
                catch (Exception ex)
                {
                    VARADataClientDispose();
                    Log.Error(ex.ToString(), ClassName);
                    Log.Error(ex.Message.ToString(), ClassName);
                    return false;
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
        public bool VARADataClientWrite(byte[] _data)
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name.ToString(), ClassName);
            // Check Connection
            if (IsVARADataClientConnected())
            {
                try
                {
                    socket.Send(_data, _data.Length, 0);
                    return true;
                }
                catch (Exception ex)
                {
                    VARADataClientDispose();
                    Log.Error(ex.ToString(), ClassName);
                    Log.Error(ex.Message.ToString(), ClassName);
                    //throw new Exception("Data Writing Operation Failed " + ex.ToString());
                    return false;
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
        private void VARADataClientDispose()
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
        public void VARADataClientDisconnect()
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
