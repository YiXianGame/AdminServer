using System.Threading;
using System.Threading.Tasks;

namespace Make.MODEL.RPC
{
    public class ClientResponseModel
    {
        public readonly string Jsonrpc = null;
        public readonly object Result = null;
        public readonly Error Error = null;
        public readonly string Id = null;
        public ClientResponseModel(string jsonrpc, object result, Error error, string id)
        {
            Jsonrpc = jsonrpc;
            Result = result;
            Error = error;
            Id = id;
        }
        public override string ToString()
        {

            return  "Jsonrpc:" + Jsonrpc + "\n"
                + "Id:" + Id + "\n"
                + "Result:" + Result + "\n"
                + "Error:" + Error.ToString();
        }
    }
}
