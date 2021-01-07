using Make.MODEL.RPC;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Make.MODEL.TCP_Async_Event
{
    delegate void ProcessData(SocketAsyncEventArgs args);

    /// <summary>
    /// Token for use with SocketAsyncEventArgs.
    /// </summary>
    public sealed class Token
    {
        //下面两部分只负责接收部分，发包构造部分并没有使用，修改时请注意！
        //下面这部分用于拆包分析   
        const int headsize = 32;//头包长度
        const int bodysize = 4;//数据大小长度
        const int patternsize = 1;//消息类型长度
        const int futuresize = 27;//后期看情况加
        //下面这部分的byte用于接收数据
        private byte[] head = new byte[headsize + 1];
        private byte[] pattern = new byte[patternsize];
        private byte[] future = new byte[futuresize];
        ConcurrentDictionary<int, ClientRequestModel> tasks = new ConcurrentDictionary<int, ClientRequestModel>();
        Random random = new Random();
        string hostname;
        string port;
        private Socket connection;

        private StringBuilder sb;

        private int needRemain;


        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="connection">Socket to accept incoming data.</param>
        /// <param name="capacity">Buffer size for accepted data.</param>
        public Token(Socket connection, string hostname, string port,Int32 capacity)
        {
            this.hostname = hostname;
            this.port = port;
            this.connection = connection;
            this.sb = new StringBuilder(capacity);
        }

        /// <summary>
        /// Accept socket.
        /// </summary>
        internal Socket Connection
        {
            get { return this.connection; }
            set { connection = value; }
        }


        /// <summary>
        /// Process data received from the client.
        /// </summary>
        /// <param name="args">SocketAsyncEventArgs used in the operation.</param>
        internal string ProcessData(SocketAsyncEventArgs args)
        {
            // Get the message received from the client.
            String received = this.sb.ToString();
            //TODO Use message received to perform a specific operation.
            // Clear StringBuffer, so it can receive more data from a keep-alive connection client.
            sb.Length = 0;
            needRemain = 0;
            return received;
        }

        /// <summary>
        /// Set data received from the client.
        /// </summary>
        /// <param name="args">SocketAsyncEventArgs used in the operation.</param>
        internal void SetData(SocketAsyncEventArgs args)
        {
            int count = args.BytesTransferred;
            int pos = 0;
            while (pos < count)
            {
                //存在断包
                if (needRemain != 0)
                {
                    //如果接收数据满足整条量
                    if (needRemain <= count - pos)
                    {
                        string data = Encoding.UTF8.GetString(args.Buffer, pos, needRemain);
                        //客户端会有两种可能，一种是自己发出的请求信息得到反馈，另一种是服务端下达的请求指令
                        //0 - Request , 1 - Response
                        if (pattern[0] == 0)
                        {
                            ServerRequestModel request = JsonConvert.DeserializeObject<ServerRequestModel>(data);
#if DEBUG
                            Console.WriteLine("---------------------------------------------------------");
                            Console.WriteLine($"{DateTime.Now}::{hostname}:{port}::[服-指令]\n{request.ToString()}");
                            Console.WriteLine("---------------------------------------------------------"); 
#endif
                            RPCAdaptFactory.services.TryGetValue(new Tuple<string, string, string>(request.Service, hostname, port), out RPCAdaptProxy proxy);
                            proxy.methods.TryGetValue(request.Methodid, out MethodInfo method);
                            method.Invoke(null, request.Params);
                        }
                        else
                        {
                            ClientResponseModel response = JsonConvert.DeserializeObject<ClientResponseModel>(data);
#if DEBUG
                            Console.WriteLine("---------------------------------------------------------");
                            Console.WriteLine($"{DateTime.Now}::{hostname}:{port}::[服-返回]\n{response.ToString()}");
                            Console.WriteLine("---------------------------------------------------------"); 
#endif
                            if (int.TryParse(response.Id, out int id) && tasks.TryGetValue(id, out ClientRequestModel request))
                            {
                                request.set(response.Result);
                            }
                        }
                        pos = needRemain + pos;
                        needRemain = 0;
                        continue;
                    }
                    else
                    {
                        sb.Append(Encoding.UTF8.GetString(args.Buffer, pos, count - pos));
                        needRemain = needRemain - count + pos;
                        break;
                    }
                }
                else
                {
                    //头包凑不齐，直接返回等待下一次数据
                    if (count - pos < headsize - head[0])
                    {
                        Buffer.BlockCopy(args.Buffer, pos, head, head[0] + 1, count - pos);
                        head[0] += (byte)(count - pos);
                        break;
                    }
                    else
                    {
                        Buffer.BlockCopy(args.Buffer, pos, head, head[0] + 1, headsize - head[0]);
                        pos += headsize - head[0];
                        head[0] = 0;
                        //收到头包，开始对头包拆分
                        //4个字节的数据大小
                        needRemain = BitConverter.ToInt32(head, 1);
                        //1个字节的接收方式
                        pattern[0] = head[bodysize + 1];
                        //接收剩下的27个不用的字节
                        Buffer.BlockCopy(head, bodysize + patternsize + 1, future, 0, futuresize);
                        continue;
                    }
                }
            }
        }

        //比较过栈、自增随机数、二次随机数，另外并没有出现网上那种随机数极短时间内出现两次相同值的情况
        //一般收发数量也不会很多，1e4以内随机数要比栈快很多.
        public void Send(ClientRequestModel request)
        {
            if (Connection != null && Connection.Connected)
            {
#if DEBUG
                Console.WriteLine("---------------------------------------------------------");
                Console.WriteLine($"{DateTime.Now}::{hostname}:{port}::[客-请求]\n{request.ToString()}");
                Console.WriteLine("---------------------------------------------------------"); 
#endif
                int id = random.Next();
                while (tasks.TryGetValue(id, out ClientRequestModel value))
                {
                    id = random.Next();
                }
                request.ID = id.ToString();
                tasks.TryAdd(id, request);
                //构造data数据
                byte[] bodyBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
                //构造表头数据，固定4个字节的长度，表示内容的长度
                byte[] headerBytes = BitConverter.GetBytes(bodyBytes.Length);
                //构造消息类型 0 为Respond,1 为Request
                byte[] pattern = { 0 };
                //预备未来的一些数据
                byte[] future = new byte[27];
                //总计需要
                byte[] sendBuffer = new byte[headerBytes.Length + pattern.Length + future.Length + bodyBytes.Length];
                ///拷贝到同一个byte[]数组中
                Buffer.BlockCopy(headerBytes, 0, sendBuffer, 0, headerBytes.Length);
                Buffer.BlockCopy(pattern, 0, sendBuffer, headerBytes.Length, pattern.Length);
                Buffer.BlockCopy(future, 0, sendBuffer, headerBytes.Length + pattern.Length, future.Length);
                Buffer.BlockCopy(bodyBytes, 0, sendBuffer, headerBytes.Length + pattern.Length + future.Length, bodyBytes.Length);
                SocketAsyncEventArgs sendEventArgs = new SocketAsyncEventArgs();
                sendEventArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
                Connection.SendAsync(sendEventArgs);
            }
        }
        public void SendVoid(ClientRequestModel request)
        {
            if (Connection!=null && Connection.Connected)
            {
#if DEBUG
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine($"{DateTime.Now}::{hostname}:{port}::[客-指令]\n{request.ToString()}");
                Console.WriteLine("--------------------------------------------------"); 
#endif
                //构造data数据
                byte[] bodyBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
                //构造表头数据，固定4个字节的长度，表示内容的长度
                byte[] headerBytes = BitConverter.GetBytes(bodyBytes.Length);
                //构造消息类型 0 为Respond,1 为VoidRespond
                byte[] pattern = { 1 };
                //预备未来的一些数据
                byte[] future = new byte[27];
                //总计需要
                byte[] sendBuffer = new byte[headerBytes.Length + pattern.Length + future.Length + bodyBytes.Length];
                ///拷贝到同一个byte[]数组中
                Buffer.BlockCopy(headerBytes, 0, sendBuffer, 0, headerBytes.Length);
                Buffer.BlockCopy(pattern, 0, sendBuffer, headerBytes.Length, pattern.Length);
                Buffer.BlockCopy(future, 0, sendBuffer, headerBytes.Length + pattern.Length, future.Length);
                Buffer.BlockCopy(bodyBytes, 0, sendBuffer, headerBytes.Length + pattern.Length + future.Length, bodyBytes.Length);
                SocketAsyncEventArgs sendEventArgs = new SocketAsyncEventArgs();
                sendEventArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
                Connection.SendAsync(sendEventArgs);
            }
        }
    }
}
