using Make;
using Make.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pack.Element
{
    /// <summary>
    /// CardPanle.xaml 的交互逻辑
    /// </summary>
    public partial class AdventurePanle : UserControl
    {
        public int NowFirst = 0;
        Adventure Filter_Adventure = new Adventure();
        public AdventurePanle()
        {
            InitializeComponent();
            Filter_Bar.DataContext = Filter_Adventure;
            Filter_Adventure.State = -1;
            User.DataContext = new User();
        }
        public Custom_Card_Adventure Add_Adventure(Adventure adventure)
        {
            Custom_Card_Adventure card = new Custom_Card_Adventure(adventure);
            AdventurePanel.Children.Add(card);
            if (Make.GeneralControl.LazyLoad_SkillCards) if (AdventurePanel.Children.Count >= 96) card.Visibility = Visibility.Collapsed;
            card.EditButton.Click += EditButton_Click;
            card.AuthorButton.Click += AuthorButton_Click;
            return card;
        }

        private void AuthorButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject ptr = sender as DependencyObject;
            while (!(ptr is Custom_Card_Adventure)) ptr = VisualTreeHelper.GetParent(ptr);
            XY.Send("作者查询#" + (ptr as Custom_Card_Adventure).Adventure.UserName);
            User.Visibility = Visibility.Visible;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject ptr = sender as DependencyObject;
            while (!(ptr is Custom_Card_Adventure)) ptr = VisualTreeHelper.GetParent(ptr);
            EditCard.Open_Edit((Custom_Card_Adventure)ptr);
        }
        private void Fitler()
        {
            Filter_Adventure.SetName(Template_Adventure_Name.Text);
            IEnumerable<Adventure> array = Filter.Adventure(GeneralControl.Adventures, Filter_Adventure);
            foreach (Custom_Card_Adventure item in AdventurePanel.Children)
            {
                if (array != null && array.Where<Adventure>(x => item.Adventure.Equals(x)).Count() != 0)
                {
                    item.Visibility = Visibility.Visible;
                }
                else item.Visibility = Visibility.Collapsed;
            }
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Fitler();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Fitler();
        }

        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            Fitler();
        }

        private void CheckBox_Click_2(object sender, RoutedEventArgs e)
        {
            Fitler();
        }

        private void CheckBox_Click_3(object sender, RoutedEventArgs e)
        {
            Fitler();
        }

        private void CheckBox_Click_4(object sender, RoutedEventArgs e)
        {
            Fitler();
        }

        private void Rate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            Fitler();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Fitler();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Adventure adventure = new Adventure
            {
                Attack = 10,
                Direct_Mp = 10,
                Cure = 10,
                Self_Mp = 20,
                Probability = 30
            };
            adventure.UserName = GeneralControl.Menu_Person_Information_Class.Instance.User.UserName;
            adventure.Name = "新奇遇";
            adventure.Save();
            adventure.Add_To_General();
            Add_Adventure(adventure);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Fitler();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            (from Custom_Card_Adventure item in AdventurePanel.Children where item.Adventure.Cloud == "云端" select item).ToList().ForEach(delegate (Custom_Card_Adventure item)
            {
                item.Adventure.Delete();
                GeneralControl.Adventures.Remove(item.Adventure);
                GeneralControl.Skill_Card_Date = DateTime.Now;
                AdventurePanel.Children.Remove(item);
            });
            XY.Send("获取奇遇#" + GeneralControl.Adventure_Date);
        }
    }
}
