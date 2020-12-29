using Make.MODEL;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

namespace Make.BLL
{
    public class Initialization
    {
        public Initialization()
        {
            MODEL.TCP_Async_Event.TCP_Client tcpClient = new MODEL.TCP_Async_Event.TCP_Client();
            Thread thread = new Thread(() => { tcpClient.Init(); });
            thread.Start();
            //XY.Send_To_Server("连接成功||" + GeneralControl.CQApi.GetLoginQQ());
            Skill_Cards_Load();
            Adventures_Load();
        }
        public void Skill_Cards_Load()
        {
            string filepath = GeneralControl.Directory + "\\技能卡";
            DirectoryInfo root = new DirectoryInfo(filepath);
            foreach (FileInfo file in root.GetFiles())
            {
                string json = File.ReadAllText(file.FullName);
                SkillCardsModel skillCardsModel = JsonConvert.DeserializeObject<SkillCardsModel>(json);
                if (skillCardsModel.Cloud == "非云端") skillCardsModel.Add_To_General();
            }
        }
        public void Adventures_Load()
        {
            string filepath = GeneralControl.Directory + "\\奇遇";
            DirectoryInfo root = new DirectoryInfo(filepath);
            foreach (FileInfo file in root.GetFiles())
            {
                string json = File.ReadAllText(file.FullName);
                Adventure adventure = JsonConvert.DeserializeObject<Adventure>(json);
                if (adventure.Cloud == "非云端") adventure.Add_To_General();
            }
        }
    }
}
