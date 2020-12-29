using System;
using System.IO;

namespace Make.BLL
{
    public class Event_Enable
    {
        public void AppEnable()
        {
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory() + "\\仙战";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Directory.CreateDirectory(path + "\\技能卡");
                    Directory.CreateDirectory(path + "\\游戏配置");
                    Directory.CreateDirectory(path + "\\奇遇");
                }
                Initialization init = new Initialization();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
    }
}
