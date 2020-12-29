using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Make.MODEL.TCP_Async_Event
{
    /// <summary>
    /// Implements the connection logic for the socket client.
    /// </summary>
    public sealed class SocketClient : IDisposable
    {
        /// <summary>
        /// Constants for socket operations.
        /// </summary>
        private const Int32 ReceiveOperation = 1, SendOperation = 0;

        /// <summary>
        /// The socket used to send/receive messages.
        /// </summary>
        private Socket clientSocket;

        /// <summary>
        /// Flag for connected socket.
        /// </summary>
        private Boolean connected = false;

        /// <summary>
        /// Listener endpoint.
        /// </summary>
        private IPEndPoint hostEndPoint;

        /// <summary>
        /// Signals a connection.
        /// </summary>
        private static AutoResetEvent autoConnectEvent = new AutoResetEvent(false);

        /// <summary>
        /// Signals the send/receive operation.
        /// </summary>
        private static AutoResetEvent[] autoSendReceiveEvents = new AutoResetEvent[]
        {
            new AutoResetEvent(false),
            new AutoResetEvent(false)
        };

        /// <summary>
        /// Create an uninitialized client instance.  
        /// To start the send/receive processing
        /// call the Connect method followed by SendReceive method.
        /// </summary>
        /// <param name="hostName">Name of the host where the listener is running.</param>
        /// <param name="port">Number of the TCP port from the listener.</param>
        internal SocketClient(String hostName, Int32 port)
        {
            // Get host related information.
            IPHostEntry host = Dns.GetHostEntry(hostName);

            // Addres of the host.
            IPAddress[] addressList = host.AddressList;

            // Instantiates the endpoint and socket.
            this.hostEndPoint = new IPEndPoint(addressList[addressList.Length - 1], port);
            this.clientSocket = new Socket(this.hostEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Connect to the host.
        /// </summary>
        /// <returns>True if connection has succeded, else false.</returns>
        internal void Connect()
        {
            SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs();

            connectArgs.UserToken = this.clientSocket;
            connectArgs.RemoteEndPoint = this.hostEndPoint;
            connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnect);
            connectArgs.AcceptSocket = clientSocket;
            clientSocket.ConnectAsync(connectArgs);
            autoConnectEvent.WaitOne();

            SocketError errorCode = connectArgs.SocketError;
            if (errorCode == SocketError.Success)
            {
                InitReceive();
            }
            else
            {
                throw new SocketException((Int32)errorCode);
            }
        }
        private void InitReceive()
        {
            byte[] receiveBuffer = new byte[1024];
            // Prepare arguments for send/receive operation.
            SocketAsyncEventArgs completeArgs = new SocketAsyncEventArgs();
            completeArgs.SetBuffer(receiveBuffer, 0, receiveBuffer.Length);
            completeArgs.UserToken = new Token(clientSocket, 1024);
            completeArgs.RemoteEndPoint = this.hostEndPoint;
            completeArgs.AcceptSocket = clientSocket;
            completeArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceive);
            GeneralControl.Token = completeArgs.UserToken as Token;
            clientSocket.ReceiveAsync(completeArgs);
        }
        /// <summary>
        /// Disconnect from the host.
        /// </summary>
        internal void Disconnect()
        {
            clientSocket.Disconnect(false);
        }

        private void OnConnect(object sender, SocketAsyncEventArgs e)
        {

            // Signals the end of connection.
            autoConnectEvent.Set();

            // Set the flag for socket connected.
            this.connected = (e.SocketError == SocketError.Success);

        }

        private void Receive(SocketAsyncEventArgs e)
        {
            // Check if the remote host closed the connection.
            if (e.BytesTransferred > 0)
            {
                if (e.SocketError == SocketError.Success)
                {
                    Token token = e.UserToken as Token;
                    token.SetData(e);

                    if (!token.Connection.ReceiveAsync(e))
                    {
                        // Read the next block of data sent by client.
                        this.Receive(e);
                    }
                }
                else
                {
                    this.ProcessError(e);
                }
            }
        }

        private void OnReceive(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                Receive(e);
            }
            else
            {
                this.ProcessError(e);
            }
        }

        /// <summary>
        /// Close socket in case of failure and throws a SockeException according to the SocketError.
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the failed operation.</param>
        private void ProcessError(SocketAsyncEventArgs e)
        {
            Socket s = (e.UserToken as Token).Connection;
            if (s.Connected)
            {
                // close the socket associated with the client
                try
                {
                    s.Shutdown(SocketShutdown.Both);
                }
                catch (Exception)
                {
                    // throws if client process has already closed
                }
                finally
                {
                    s.Close();
                }
            }
            int cnt = 0;
            while (true)
            {
                Debug.WriteLine("发生断开异常，正在进行重连.第" + (++cnt) + "次重连[最多重连10次]");
                if (cnt == 10)
                {
                    Debug.WriteLine("重连失败");
                    // Throw the SocketException
                    throw new SocketException((Int32)e.SocketError);
                }
                try
                {
                    this.clientSocket = new Socket(this.hostEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    clientSocket.Connect(s.RemoteEndPoint);
                    InitReceive();
                    Debug.WriteLine("客户端重连成功.");
                    return;
                }
                catch
                {
                    if (clientSocket.Connected) clientSocket.Close();
                    Thread.Sleep(5000);
                }
            }
        }

        /// <summary>
        /// 构造发送数据
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public byte[] Convert_SendMsg(string msg)
        {
            int length = msg.Length;
            //构造内容
            byte[] bodyBytes = Encoding.UTF8.GetBytes(msg);
            //构造表头数据，固定4个字节的长度，表示内容的长度
            byte[] headerBytes = BitConverter.GetBytes(bodyBytes.Length);

            byte[] tempBytes = new byte[headerBytes.Length + bodyBytes.Length];
            ///拷贝到同一个byte[]数组中，发送出去..
            Buffer.BlockCopy(headerBytes, 0, tempBytes, 0, headerBytes.Length);
            Buffer.BlockCopy(bodyBytes, 0, tempBytes, headerBytes.Length, bodyBytes.Length);
            return tempBytes;
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes the instance of SocketClient.
        /// </summary>
        public void Dispose()
        {
            autoConnectEvent.Close();
            autoSendReceiveEvents[SendOperation].Close();
            autoSendReceiveEvents[ReceiveOperation].Close();
            if (this.clientSocket.Connected)
            {
                this.clientSocket.Close();
            }
        }

        #endregion
    }
}
