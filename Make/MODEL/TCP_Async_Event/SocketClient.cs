using Make.MODEL.RPC;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Make.MODEL.TCP_Async_Event
{

    public sealed class SocketClient : IDisposable
    {
        /// <summary>
        /// 远程服务器地址
        /// </summary>
        private IPEndPoint hostEndPoint;
        /// <summary>
        /// 信号量,控制线程进行连接等待
        /// </summary>
        private static AutoResetEvent autoConnectEvent = new AutoResetEvent(false);
        /// <summary>
        /// 客户端配置
        /// </summary>
        private const int DEFAULT_PORT = 28015, DEFAULT_NUM_CONNECTIONS = 1000, DEFAULT_BUFFER_SIZE = 1024;
        private string hostname;
        private string port;
        /// <summary>
        /// Token
        /// </summary>
        public Token Token { get; set; } 

        public SocketClient(string hostname,string port)
        {
            this.hostname = hostname;
            this.port = port;
            // Get host related information.
            IPAddress[] addressList = Dns.GetHostEntry(hostname).AddressList;
            // Get endpoint for the listener.
            IPEndPoint localEndPoint = new IPEndPoint(addressList[addressList.Length - 1],int.Parse(port));
            // Instantiates the endpoint and socket.
            this.hostEndPoint = new IPEndPoint(addressList[addressList.Length - 1], int.Parse(port));
            Token = new Token(null,hostname,port, DEFAULT_BUFFER_SIZE);
            Token.Connection = new Socket(this.hostEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            Socket clientSocket = Token.Connection;
            SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs();
            connectArgs.UserToken = clientSocket;
            connectArgs.RemoteEndPoint = this.hostEndPoint;
            connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnect);
            connectArgs.AcceptSocket = clientSocket;
            try
            {
                clientSocket.ConnectAsync(connectArgs);
                autoConnectEvent.WaitOne();
                SocketError errorCode = connectArgs.SocketError;
                if (errorCode == SocketError.Success)
                {
                    Token.Connection = clientSocket;
                    SocketAsyncEventArgs readEventArgs = new SocketAsyncEventArgs();
                    readEventArgs.UserToken = Token;
                    readEventArgs.SetBuffer(new byte[DEFAULT_BUFFER_SIZE],0,DEFAULT_BUFFER_SIZE);
                    readEventArgs.Completed += OnReceiveCompleted;
                    if (!clientSocket.ReceiveAsync(readEventArgs))
                    {
                        ProcessReceive(readEventArgs);
                    }
                }
                else
                {
                    Reconnect();
                }
            }
            catch(SocketException err)
            {
                Reconnect();
            }
        }

        private void OnConnect(object sender, SocketAsyncEventArgs e)
        {
            autoConnectEvent.Set();
        }

        public void Disconnect()
        {
            Token.Connection.Disconnect(false);
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            this.ProcessReceive(e);
        }
        private void ProcessReceive(SocketAsyncEventArgs e)
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
                        this.ProcessReceive(e);
                    }
                }
                else
                {
                    Reconnect();
                }
            }
            else
            {
                Reconnect();
            }
        }
        public void Reconnect()
        {
            Debug.WriteLine("与服务器连接异常,开始尝试重连！");
            Socket clientSocket = Token.Connection;
            for (int i = 1; i <= 10; i++)
            {
                clientSocket = Token.Connection;
                if (clientSocket != null)
                {
                    Debug.WriteLine("开始销毁历史Socket");
                    clientSocket.Close();
                    clientSocket.Dispose();
                    Debug.WriteLine("历史Socket销毁完成！");
                }
                Debug.WriteLine($"开始进行第{i}次尝试");
                clientSocket = new Socket(this.hostEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs();
                connectArgs.UserToken = clientSocket;
                connectArgs.RemoteEndPoint = this.hostEndPoint;
                connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnect);
                connectArgs.AcceptSocket = clientSocket;
                try
                {
                    clientSocket.ConnectAsync(connectArgs);
                    autoConnectEvent.WaitOne();
                    SocketError errorCode = connectArgs.SocketError;

                    if (errorCode == SocketError.Success)
                    {
                        Token = new Token(null, hostname, port, DEFAULT_BUFFER_SIZE);
                        Token.Connection = clientSocket;
                        SocketAsyncEventArgs readEventArgs = new SocketAsyncEventArgs();
                        readEventArgs.SetBuffer(new byte[DEFAULT_BUFFER_SIZE], 0, DEFAULT_BUFFER_SIZE);
                        readEventArgs.Completed += OnReceiveCompleted;
                        readEventArgs.UserToken = Token;
                        if (!clientSocket.ReceiveAsync(readEventArgs))
                        {
                            ProcessReceive(readEventArgs);
                        }
                        Debug.WriteLine("重连成功！");
                        break;
                    }
                    else
                    {
                        Debug.WriteLine("重连失败，5秒后重试！");
                        Thread.Sleep(5000);
                    }
                }
                catch (SocketException err)
                {
                    Debug.WriteLine("重连失败，5秒后重试！");
                    Thread.Sleep(5000);
                }
            }
            if(!clientSocket.Connected) Debug.WriteLine($"重连失败！"); 
        }
        #region IDisposable Members
        bool isDipose = false;

        ~SocketClient()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            Socket clientSocket = Token.Connection;
            if (isDipose) return;
            if (disposing)
            {
                hostEndPoint = null;
                autoConnectEvent.Close();
                autoConnectEvent = null;
            }
            //处理非托管资源
            try
            {
                clientSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception)
            {
                // Throw if client has closed, so it is not necessary to catch.
            }
            finally
            {
                clientSocket.Close();
                clientSocket = null;
            }
            isDipose = true;
        }
        #endregion
    }
}
