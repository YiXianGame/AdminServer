using System;

namespace Make.MODEL.TCP_Async_Event
{
    public class TCP_Client
    {
        public void Init()
        {
            try
            {
                string host = "10.163.211.150";
                //string ip = "10.128.143.71";
                Int32 port = Convert.ToInt32("28015");
                SocketClient sa = new SocketClient(host, port);
                sa.Connect();
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Usage: SocketAsyncClient <host> <port> [iterations]");
            }
            catch (FormatException)
            {
                Console.WriteLine("Usage: SocketAsyncClient <host> <port> [iterations]." +
                    "\r\n\t<host> Name of the host to connect." +
                    "\r\n\t<port> Numeric value for the host listening TCP port." +
                    "\r\n\t[iterations] Number of iterations to the host.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }
    }
}
