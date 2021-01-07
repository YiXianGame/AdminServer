using System;
using System.Collections.Generic;
using System.Text;

namespace Make.MODEL.RPC
{
    public class Error
    {
        int Code { get; set; }
        string Message { get; set; }
        string Data { get; set; }   
    }
}
