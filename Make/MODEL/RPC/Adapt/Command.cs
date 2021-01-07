using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Make.MODEL.RPC.Adapt
{
    public class Command
    {
        public static void AddHp(long num)
        {
            Debug.Write("客户端进行加血行为:" + num +"\n");
        }
    }
}
