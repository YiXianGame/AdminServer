﻿using Make.MODEL;
using Material;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Make.BLL
{
    public class Initialization
    {
        public Initialization()
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "\\仙战";//游戏数据文档路径
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Directory.CreateDirectory(path + "\\技能卡");
                Directory.CreateDirectory(path + "\\游戏配置");
                Directory.CreateDirectory(path + "\\奇遇");
                Directory.CreateDirectory(path + "\\游戏数据");
                Directory.CreateDirectory(path + "\\用户");
            }
            Skill_Cards_Load();
            Adventures_Load();
            /* 仙域地图版本延期
            XianYu_Map xianYu_Map = new XianYu_Map(GeneralControl.Menu_GameControl_Class.Instance.Map_Size, GeneralControl.Menu_GameControl_Class.Instance.Map_Size);
            GeneralControl.Map = xianYu_Map;
            xianYu_Map.Resource_Init();
            */
        }
        public void Skill_Cards_Load()
        {
            string filepath = GeneralControl.directory + "\\技能卡";
            DirectoryInfo root = new DirectoryInfo(filepath);
            foreach (FileInfo file in root.GetFiles())
            {
                string json = File.ReadAllText(file.FullName);
                SkillCardsModel skillCardsModel = JsonConvert.DeserializeObject<SkillCardsModel>(json);
                skillCardsModel.Add_To_General(); 
            }
        }
        public void Adventures_Load()
        {
            string filepath = GeneralControl.directory + "\\奇遇";
            DirectoryInfo root = new DirectoryInfo(filepath);
            foreach (FileInfo file in root.GetFiles())
            {
                string json = File.ReadAllText(file.FullName);
                Adventure adventure= JsonConvert.DeserializeObject<Adventure>(json);
                adventure.Add_To_General();
            }
        }
    }
}
