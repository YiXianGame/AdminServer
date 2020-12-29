using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Make.BLL;
using Material;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Make.MODEL
{
    [JsonObject(MemberSerialization.OptOut)]
    public class SkillCard
    {
        private string name="";//技能卡名称
        private int level;//技能卡等级
        private string father_ID;//父卡类
        private string description="";//技能介绍
        private int need_Mp;//所需能量
        private int probability;//概率
        private int attack;//攻击力
        private int cure;//治疗量
        private int self_Mp;//自我能量
        private int direct_Mp;//指向能量
        private ObservableCollection<State> effect_States=new ObservableCollection<State>();//状态
        private List<Player> enemies = new List<Player>();//指向
        private List<Player> friends = new List<Player>();//指向
        private string owner;
        private bool is_Magic;//是否魔法
        private bool is_Physics;//是否物理
        private bool is_Self;//是否释放于自己
        private bool is_Cure;//是否治疗
        private bool is_Attack;//是否攻击
        private bool is_Eternal;//是否永恒
        private bool is_Basic;//是否基础卡组
        private int state=1;//技能卡状态(0 禁用 1 开启 2售卖)
        private string messages="";//技能反馈
        private int amount=1;//技能卡数量
        private DateTime date_Latest;
        private int attack_Number = 1;
        private int auxiliary_Number = 1;
        private string iD;

        public string Name 
        { 
            get => name;
            set
            {
                if (!int.TryParse(value, out int result))
                {
                    if (GeneralControl.Skill_Card_Name_Skllcard.ContainsKey(name) && GeneralControl.Skill_Card_ID_Skllcard.ContainsKey(ID))
                    {
                        while ((from string item in GeneralControl.Skill_Card_Name_Skllcard.Keys where item == value && item != Name select item).Any()) value += "-副本";
                        GeneralControl.Skill_Card_Name_Skllcard.Remove(name);
                        GeneralControl.Skill_Card_Name_Skllcard.Add(value, this);
                    }
                    name = value;
                }
                else return;
            }
        }
        public bool Is_Basic { get => is_Basic; set => is_Basic = value; }
        public int Level { get => level; set => level = value; }
        public int Amount { get => amount; set => amount = value; }
        public int Need_Mp { get => need_Mp; set => need_Mp = value; }
        public int Probability { get => probability; set => probability = value; }
        public int Attack { get => attack; set => attack = value; }
        public int Cure { get => cure; set => cure = value; }
        public int Self_Mp { get => self_Mp; set => self_Mp = value; }
        public int Direct_Mp { get => direct_Mp; set => direct_Mp = value; }
        public ObservableCollection<State> Effect_States { get => effect_States; set => effect_States = value;}
        public bool Is_Magic { get => is_Magic; set => is_Magic = value; }
        public string Messages { get => messages; set => messages = value; }
        public int State { get => state; set => state = value; }
        public string Description { get => description; set => description = value; }
        public int Attack_Number { get => attack_Number; set => attack_Number = value; }
        public int Auxiliary_Number { get => auxiliary_Number; set => auxiliary_Number = value; }
        public bool Is_Self { get => is_Self; set => is_Self = value; }
        [JsonIgnore]
        public List<Player> Enemies { get => enemies; set => enemies = value; }
        public string Owner { get => owner; set => owner = value; }
        public bool Is_Cure { get => is_Cure; set => is_Cure = value; }
        public bool Is_Attack { get => is_Attack; set => is_Attack = value; }
        public bool Is_Eternal { get => is_Eternal; set => is_Eternal = value; }
        public bool Is_Physics { get => is_Physics; set => is_Physics = value; }
        public string Father_ID { get => father_ID; set => father_ID = value; }
        [JsonIgnore]
        public DateTime Date_Latest { get => date_Latest; set => date_Latest = value; }
        public string ID { get => iD; set => iD = value; }
        [JsonIgnore]
        public List<Player> Friends { get => friends; set => friends = value; }


        public  SkillCard()
        {
            string temp_id;
            do
            {
                temp_id = Guid.NewGuid().ToString();
            }
            while (File.Exists(GeneralControl.directory + "\\技能卡\\" + temp_id + ".json"));
            ID = temp_id;
        }
        public void Assign(SkillCard skillCard)
        {
            foreach (PropertyInfo sP in skillCard.GetType().GetProperties())
            {
                foreach (PropertyInfo dP in GetType().GetProperties())
                {
                    if (dP.Name == sP.Name)
                    {
                        dP.SetValue(this, sP.GetValue(skillCard));
                    }
                }
            }
            /*
            Attack = skillCard.Attack;
            level = skillCard.Level; ;//技能卡等级
            father_ID = skillCard.Father_ID;//父卡类
            description = skillCard.Description;//技能介绍
            need_Mp = skillCard.Need_Mp;//所需能量
            probability = skillCard.Probability;//概率
            attack = skillCard.Attack;//攻击力
            cure = skillCard.Cure;//治疗量
            self_Mp = skillCard.Self_Mp;//自我能量
            direct_Mp = skillCard.Direct_Mp;//指向能量
            effect_States = new List<State>();//状态
            foreach(State item in skillCard.Effect_States)
            {
                effect_States.Add(item.Clone());
            }
            directs = new List<Player>(skillCard.Directs.ToArray());
            owner = skillCard.owner;
            is_Magic = skillCard.Is_Magic;//是否魔法
            is_Physics = skillCard.Is_Physics;//是否物理
            is_Self = skillCard.Is_Self;//是否释放于自己
            is_Cure = skillCard.Is_Cure;//是否治疗
            is_Attack = skillCard.Is_Attack;//是否攻击
            is_Eternal = skillCard.Is_Eternal;//是否永恒
            state = skillCard.State;//技能卡状态
            messages = skillCard.Messages;//技能反馈
            amount = skillCard.Amount;//技能卡数量
            date_Latest = skillCard.Date_Latest;
            attack_Number = skillCard.Attack_Number;
            */
         }
        /// <summary>
        /// 克隆一个卡牌，指定技能卡的主人
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public SkillCard Clone(Player player)
        {
            SkillCard skillCard= (SkillCard)MemberwiseClone();
            skillCard.effect_States = new ObservableCollection<State>(effect_States.ToArray());
            skillCard.owner = player.UserName;
            skillCard.enemies = new List<Player>(enemies.ToArray());
            skillCard.friends = new List<Player>(friends.ToArray());
            return skillCard;
        }
        /// <summary>
        /// 克隆一个卡牌，指定技能卡的主人
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public SkillCard Clone(string id)
        {
            SkillCard skillCard = (SkillCard)MemberwiseClone();
            skillCard.effect_States = new ObservableCollection<State>(effect_States.ToArray());
            skillCard.owner = id;
            skillCard.enemies = new List<Player>(enemies.ToArray());
            skillCard.friends = new List<Player>(friends.ToArray());
            return skillCard;
        }
        /// <summary>
        /// 克隆一个卡牌，没有指定技能卡的主人
        /// </summary>
        /// <returns></returns>
        public SkillCard Clone()
        {
            SkillCard skillCard = (SkillCard)MemberwiseClone();
            skillCard.Effect_States = new ObservableCollection<State>();
            skillCard.enemies = new List<Player>();
            skillCard.friends = new List<Player>();
            foreach (State item in Effect_States)
            {
                skillCard.Effect_States.Add(item.Clone());
            }
            skillCard.enemies = new List<Player>(enemies.ToArray());
            skillCard.friends = new List<Player>(friends.ToArray());
            return skillCard;
        }
        /// <summary>
        /// 返回这张卡的父亲级SkillCard_Model
        /// </summary>
        /// <returns></returns>
        public SkillCardsModel Find_Father()
        {
            return (from SkillCardsModel item in GeneralControl.Skill_Cards where item.ID == Father_ID select item).FirstOrDefault();
        }
    }
}
