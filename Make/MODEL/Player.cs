using Make.BLL;
using Make.MODEL.TCP_Async_Event;
using Material;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Make.MODEL
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Player
    {

        #region --字段--
        private string userName = "";//玩家ID也是QQ号（-1时为机器人）
        private string nickName;//玩家昵称
        private int hp;//血量
        private int mp;//能量
        private int hp_max;//血量上限
        private int mp_max;//仙气上限
        private string title;
        private Room room;//房间
        private Enums.Race race;//种族
        private int team;//所属队伍
        private List<Player> enemies = new List<Player>();//玩家锁定
        private List<Player> enemiesed = new List<Player>();
        private List<Player> friends = new List<Player>();//队友
        private List<Player> friendsed = new List<Player>();
        private List<State> states = new List<State>();//战斗状态
        private bool is_Death;//是否死亡
        private int kills;//击杀数
        private int deaths;//死亡数
        private int death_Round;//死亡回合
        private bool action;//是否出招
        private Dictionary<string, SkillCard> hand_SkillCards = new Dictionary<string, SkillCard>();//手中的技能卡
        private SkillCard action_Skill;//释放的技能
        private long from_Group=-1;//QQ群号
        private DateTime leisure;
        private Token token;
        private bool is_Robot=false;
        private Enums.Player_Active active;
        #endregion

        #region --属性--

        public int Hp 
        { 

            get => hp;
            set 
            { 
                hp = value; 
                if (hp <= 0)
                {                    
                    Is_Death = true;
                }
                else 
                { 
                    Is_Death = false;
                }
            }
        }
        public int Mp { get => mp; set => mp = value; }
        public Dictionary<string, SkillCard> Hand_SkillCards { get => hand_SkillCards; set => hand_SkillCards = value; }
        public Room Room { get => room; set => room = value; }
        public SkillCard Action_Skill { get => action_Skill; set => action_Skill = value; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.Race Race { get => race; set => race = value; }
        public bool Action { get => action; set => action = value; }
        public int Team { get => team; set => team = value; }
        public long From_Group { get => from_Group; set => from_Group = value; }
        public List<Player> Enemiesed { get => enemiesed; set => enemiesed = value; }
        public List<Player> Enemies { get => enemies; set => enemies = value; }
        public bool Is_Robot { get => is_Robot; set => is_Robot = value; }
        public DateTime Leisure { get => leisure; set => leisure = value; }
        public List<Player> Friends { get => friends; set => friends = value; }
        public List<Player> Friendsed { get => friendsed; set => friendsed = value; }
        public bool Is_Death { get => is_Death; set => is_Death = value; }
        public int Kills { get => kills; set => kills = value; }
        public int Deaths { get => deaths; set => deaths = value; }
        public int Death_Round { get => death_Round; set => death_Round = value; }
        public List<State> States { get => states; set => states = value; }
        public int Hp_max { get => hp_max; set => hp_max = value; }
        public int Mp_max { get => mp_max; set => mp_max = value; }
        public string Title { get => title; set => title = value; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Enums.Player_Active Active { get => active; set => active = value; }
        public string UserName { get => userName; set => userName = value; }
        public string NickName { get => nickName; set => nickName = value; }
            
        #endregion

    }
}
