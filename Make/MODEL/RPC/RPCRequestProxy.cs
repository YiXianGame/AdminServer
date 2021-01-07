using Make.MODEL.TCP_Async_Event;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Make.MODEL.RPC
{
    public class RPCRequestProxy<T> : DispatchProxy
    {
        public Random random = new Random();
        private string servicename;
        private Tuple<string,string> key;
        public static T Create(string servicename, Tuple<string, string> clientkey)
        {
            RPCRequestProxy<T> proxy = (RPCRequestProxy<T>)(Create<T, RPCRequestProxy<T>>() as object);
            proxy.key = clientkey;
            proxy.servicename = servicename;
            return (T)(proxy as object);
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            try
            {
                //这里要连接字符串，发现StringBuilder效率高一些.
                StringBuilder methodid = new StringBuilder(targetMethod.Name);
                for (int i = 0; i < args.Length; i++)
                {
                    methodid.Append("-");
                    methodid.Append(args[i].GetType().Name);
                }
                //这里装盒，空下一个空的object位置，服务器到时候会用来装Token
                object[] obj = new Object[args.Length + 1];
                args.CopyTo(obj, 1);
                obj[0] = null;
                ClientRequestModel request = new ClientRequestModel("2.0", servicename, methodid.ToString(), obj);
                if (targetMethod.ReturnType == typeof(void))
                {
                    RPCClientFactory.GetClient(key).Token.SendVoid(request);
                    return null;
                }
                else
                {
                    RPCClientFactory.GetClient(key).Token.Send(request);
                    return request.get();
                }
            }
            catch(SocketException e)
            {
                RPCClientFactory.GetClient(key).Reconnect();
            }
            return null;
        }
    }
}
        