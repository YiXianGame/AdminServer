using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make.MODEL
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Simple_SkillCard
    {
        #region --字段--
        private string name;
        private int level;
        private int amount;
        #endregion

        #region --属性--
        public string Name { get => name; set => name = value; }
        public int Level { get => level; set => level = value; }
        public int Amount { get => amount; set => amount = value; }
        #endregion

        #region --方法--
        public Simple_SkillCard(string name,int level,int amount)
        {
            this.name = name;
            this.level = level;
            this.amount = amount;
        }
        #endregion
    }
}
