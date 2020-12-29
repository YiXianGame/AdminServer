using Make;
using Make.MODEL;
using Make.MODEL.TCP_Async_Event;
using Newtonsoft.Json;
using Pack.Element;
using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace Pack
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            /* StringBuilder 底层测试小代码
            StringBuilder sb = new StringBuilder(20);
            sb.Append("123456789");
            Debug.WriteLine(d+"----");
            sb.Length = 0;
            Debug.WriteLine(d + "----");
            sb.Append ("abc");
            Debug.WriteLine(d + "----");
            sb.Length = 12;
            Debug.WriteLine(d + "----");
            sb.Append("de");
            */
            ///*
            Make.BLL.Initialization initialization = new Make.BLL.Initialization();//游戏初始化入口
            XY.MainWindow = this;
            InitializeComponent();
            Init();
            //*/
        }



        private void Init()
        {
            TCP_Server socket_Server = new TCP_Server();//TCP初始化
            TCP_Event.Receive += XY.TCP_Event_Receive;
            Thread thread = new Thread(() => { socket_Server.Init(new string[] { "28015", "1000", }); });
            thread.Start();

            UI_Init();
            Menu_Adventure_Cards.DataContext = Make.GeneralControl.Menu_Adventure_Cards_Class.Instance;
            Menu_Command.DataContext = GeneralControl.Menu_Command_Class.Instance;
            Menu_GameControl.DataContext = GeneralControl.Menu_GameControl_Class.Instance;
            Menu_Lience.DataContext = GeneralControl.Menu_Lience_Class.Instance;
            Menu_Person_Information.DataContext = GeneralControl.Menu_Person_Information_Class.Instance;
            Menu_Version_Information.DataContext = GeneralControl.Menu_Version_Informations_Class.Instance;
            Menu_Skill_Cards.DataContext = GeneralControl.Menu_Skill_Cards_Class.Instance;
            Menu_Data_Monitor.DataContext = GeneralControl.Menu_Data_Monitor_Class.Instance;
        }



        private void SkillCardsPanle_Initialized(object sender, EventArgs e)
        {

        }

        private void SkillCardsPanle_Initialized_1(object sender, EventArgs e)
        {

        }

        private void Menu_Button_3_Click(object sender, RoutedEventArgs e)
        {


        }

        private void Menu_Button_4_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Menu_Button_1_Click(object sender, RoutedEventArgs e)
        {
            Main_TabControl.SelectedIndex = 0;
        }
        private void Menu_Button_1_Copy_Click(object sender, RoutedEventArgs e)
        {
            Main_TabControl.SelectedIndex = 1;
        }

        private void Menu_Button_1_Copy1_Click(object sender, RoutedEventArgs e)
        {
            Main_TabControl.SelectedIndex = 2;
        }

        private void Menu_Button_1_Copy2_Click(object sender, RoutedEventArgs e)
        {
            Main_TabControl.SelectedIndex = 3;
        }

        private void Menu_Button_1_Copy3_Click(object sender, RoutedEventArgs e)
        {
            Main_TabControl.SelectedIndex = 4;
        }

        private void Menu_Button_1_Copy4_Click(object sender, RoutedEventArgs e)
        {
            Main_TabControl.SelectedIndex = 5;
        }

        private void Menu_Button_1_Copy5_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Menu_Button_1_Copy5_Click_1(object sender, RoutedEventArgs e)
        {
            Main_TabControl.SelectedIndex = 7;
        }
        private void UI_Init()
        {
            //初始化技能面板
            foreach (SkillCardsModel skillCardsModel in GeneralControl.Skill_Cards)
            {
                CardPanle.Add_Card(skillCardsModel);
            }
            //初始化奇遇
            foreach (Adventure adventure in Make.GeneralControl.Adventures)
            {
                AdventurePanle.Add_Adventure(adventure);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(GeneralControl.directory + @"\游戏配置\Menu_GameControl_Class.json", JsonConvert.SerializeObject(GeneralControl.Menu_GameControl_Class.Instance));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(GeneralControl.directory + @"\游戏配置\Menu_Command_Class.json", JsonConvert.SerializeObject(GeneralControl.Menu_Command_Class.Instance));
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Material.Ini.Write("游戏配置", "MaxLevel", GeneralControl.MaxLevel.ToString(), GeneralControl.directory + @"\游戏配置\GeneralControl.ini");
            Material.Ini.Write("游戏配置", "MaxStates", GeneralControl.MaxStates.ToString(), GeneralControl.directory + @"\游戏配置\GeneralControl.ini");
            Material.Ini.Write("游戏配置", "LazyLoad_SkillCards", GeneralControl.LazyLoad_SkillCards.ToString(), GeneralControl.directory + @"\游戏配置\GeneralControl.ini");
            Material.Ini.Write("游戏配置", "Skill_Card_Date", GeneralControl.Skill_Card_Date.ToString(), GeneralControl.directory + @"\游戏配置\GeneralControl.ini");
            Material.Ini.Write("游戏配置", "Adventure_Date", GeneralControl.Adventure_Date.ToString(), GeneralControl.directory + @"\游戏配置\GeneralControl.ini");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(GeneralControl.directory + @"\游戏配置\Menu_Person_Informations_Class.json", JsonConvert.SerializeObject(GeneralControl.Menu_Person_Information_Class.Instance));
            GeneralControl.Menu_Person_Information_Class.Instance.User.Save();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            GeneralControl.Menu_Person_Information_Class.Instance.User = Make.MODEL.User.Load(GeneralControl.Menu_Person_Information_Class.Instance.User.UserName);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(GeneralControl.directory + @"\游戏配置\Menu_Person_Informations_Class.json", JsonConvert.SerializeObject(GeneralControl.Menu_Person_Information_Class.Instance));
            GeneralControl.Menu_Data_Monitor_Class.Instance.Save();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
