using System;

namespace Make.MODEL.TCP_Async_Event
{
    public class TCP_Event
    {
        public delegate void ReceiveDelegate(Token token, Msg_Server msg_Client);
        public static event ReceiveDelegate Receive;
        public static void OnReceive(Token token, Msg_Server receiveStr)
        {
            Console.WriteLine(receiveStr.ToString());
            Receive(token, receiveStr);
        }
    }
}
