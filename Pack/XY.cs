using Make.MODEL;
using Make.MODEL.TCP_Async_Event;
using System;

namespace Pack
{
    //主流工作区
    public static class XY
    {
        public static MainWindow MainWindow;
        public static void TCP_Event_Receive(Token token, Msg_Client msg_Client)
        {

        }
        public static void Send(string msg, object bound = null)
        {
            Make.GeneralControl.Token.Send(Enums.Msg_Client_Type.Information, msg, bound);
        }
        public static void Console_Write(object msg)
        {
            XY.MainWindow.Dispatcher.Invoke((Action)delegate ()
            {
                if (XY.MainWindow.Menu_Data.Data_Console.Text.Length >= 10000)
                {
                    XY.MainWindow.Menu_Data.Data_Console.Text = "";
                }
                XY.MainWindow.Menu_Data.Data_Console.Text += DateTime.Now + "=>" + msg.ToString() + "\n";
            });
        }
    }
}
