using Make.MODEL.TCP_Async_Event;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Make.MODEL.RPC
{
    /// <summary>
    /// 客户端工厂
    /// </summary>
    public class RPCClientFactory 
    {
        private static ConcurrentDictionary<Tuple<string, string>, SocketClient> socketclients { get; } = new ConcurrentDictionary<Tuple<string, string>, SocketClient>();
        
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <param name="serverIp">远程服务IP</param>
        /// <param name="port">远程服务端口</param>
        /// <returns>客户端</returns>
        public static SocketClient GetClient(Tuple<string, string> key)
        {
            SocketClient socketclient;
            socketclients.TryGetValue(key, out socketclient);
            if (socketclient == null)
            {
                socketclient = new SocketClient(key.Item1, key.Item2);
                socketclients[key] = socketclient;
                socketclient.Connect();
            }
            return socketclient;
        }

        public static void Destory(Tuple<string, string> key)
        {
            socketclients.TryRemove(key, out SocketClient socket);
            socket.Dispose();
        }
    }
}
