using Make.MODEL.TCP_Async_Event;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;

namespace Make.MODEL
{
    [JsonObject(MemberSerialization.OptOut)]
    public class User
    {
        #region --字段--
        private string username="";
        private long qQ = -1;
        private string nickname;
        private int upgrade_num=0;
        private int create_num=0;
        private int money = 0;
        private string information="";
        private string passwords = "";
        private Dictionary<string, Simple_SkillCard> repository_SkillCards = new Dictionary<string, Simple_SkillCard>();//技能卡仓库
        private Dictionary<string, Simple_SkillCard> battle_SkillCards = new Dictionary<string, Simple_SkillCard>();//备战的技能卡
        private int battle_Count;//战斗场次
        private int exp;//经验
        private int balances;//金钱
        private int lv = 1;//等级
        private string title = "炼气";//称号
        private Enums.User_Active active = Enums.User_Active.Leisure;//玩家当前游戏状态
        private int kills;//击杀数
        private int deaths;//死亡数
        private DateTime skillCards_Date;//技能卡版本
        private DateTime registration_date;//注册时间
        private Token token;
        #endregion

        #region --属性--
        [JsonIgnore]
        public Dictionary<string, Simple_SkillCard> Repository_SkillCards { get => repository_SkillCards; set => repository_SkillCards = value; }
        [JsonIgnore]
        public Dictionary<string, Simple_SkillCard> Battle_SkillCards { get => battle_SkillCards; set => battle_SkillCards = value; }
        public string UserName { get => username; set => username = value; }
        public string NickName { get => nickname; set => nickname = value; }
        public string Information { get => information; set => information = value; }
        public long QQ { get => qQ; set => qQ = value; }
        public int Upgrade_num { get => upgrade_num; set => upgrade_num = value; }
        public int Create_num { get => create_num; set => create_num = value; }
        public int Money { get => money; set => money = value; }
        public string Passwords { get => passwords; set => passwords = value; }
        public int Battle_Count { get => battle_Count; set => battle_Count = value; }
        public int Exp
        {
            get => exp;
            set
            {
                exp = value;
                if (exp < 10)
                {
                    Title = "炼气";
                    Lv = 1;
                }
                else if (exp >= 10)
                {
                    Title = "筑基";
                    Lv = 2;
                }
                else if (exp >= 100)
                {
                    Title = "金丹";
                    Lv = 3;
                }
                else if (exp >= 500)
                {
                    Title = "元婴";
                    Lv = 4;
                }
                else if (exp >= 1000)
                {
                    Title = "分神";
                    Lv = 5;
                }
                else if (exp >= 1500)
                {
                    Title = "洞虚";
                    Lv = 6;
                }
                else if (exp >= 2000)
                {
                    Title = "大乘";
                    Lv = 7;
                }
                else if (exp >= 3000)
                {
                    Lv = 8;
                    Title = "羽化";
                }
            }
        }
        public int Balances { get => balances; set => balances = value; }
        public int Lv { get => lv; set => lv = value; }
        public string Title { get => title; set => title = value; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.User_Active Active { get => active; set => active = value; }
        public int Kills { get => kills; set => kills = value; }
        public int Deaths { get => deaths; set => deaths = value; }
        [JsonIgnore]
        public DateTime SkillCards_Date { get => skillCards_Date; set => skillCards_Date = value; }
        [JsonIgnore]
        public DateTime Registration_date { get => registration_date; set => registration_date = value; }
        [JsonIgnore]
        public Token Token { get => token; set => token = value; }
        #endregion

        #region --方法--
        public void SendMessages(String message, string bound = null)
        {
            token.Send(Enums.Msg_Client_Type.Information, message, bound);
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(this);
            string filepath = GeneralControl.Directory + "\\用户\\" + UserName + ".json";
            File.WriteAllText(filepath, json);
        }
        public static User Load(string iD)
        {
            string filepath = GeneralControl.Directory + "\\用户\\" + iD + ".json";
            if (!File.Exists(filepath)) return null;
            string json = (File.ReadAllText(filepath));
            return JsonConvert.DeserializeObject<User>(json);
        }
        public void Delete()
        {
            string filepath = GeneralControl.Directory + "\\用户\\" + UserName + ".json";
            File.Delete(filepath);
        }
        
        #endregion
    }
}
