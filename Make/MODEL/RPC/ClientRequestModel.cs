using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Make.MODEL.RPC
{
    public class ClientRequestModel
    {
        [JsonIgnore]
        private AutoResetEvent sign = new AutoResetEvent(false);
        [JsonIgnore]
        public object Result;
        public readonly string Jsonrpc;
        public readonly string Methodid;
        public readonly object[] Params;
        public string ID;
        public readonly string Service;

        public ClientRequestModel(string jsonrpc,string service,string methodid, object[] @params)
        {
            Jsonrpc = jsonrpc;
            Methodid = methodid;
            Params = @params;
            Service = service;
        }

        public void set(object result)
        {
            Result = result;
            sign.Set();
        }
        public object get()
        {
            //暂停当前进程，等待返回.
            while (Result == null)
            {
                sign.WaitOne();
            }
            return Result;
        }
        public override string ToString()
        {
            return "Jsonrpc:" + Jsonrpc + "\n"
                + "Service:" + Service + "\n"
                + "Methodid:" + Methodid + "\n"
                + "Params:" + JsonConvert.SerializeObject(Params);
        }
    }
}
