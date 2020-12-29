using Make;
using Make.MODEL;
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
    public partial class CardPanle : UserControl
    {
        public int NowFirst = 0;
        SkillCard Filter_Skill = new SkillCard();
        public CardPanle()
        {
            InitializeComponent();
            Filter_Skill.State = -1;
            User.DataContext = new User();
            Filter_Bar.DataContext = Filter_Skill;
        }

        public Custom_Card_SkillCard Add_Card(SkillCardsModel skillCards)
        {
            Custom_Card_SkillCard card = new Custom_Card_SkillCard(skillCards);
            CardsPanel.Children.Add(card);
            if (GeneralControl.LazyLoad_SkillCards) if (CardsPanel.Children.Count >= 96) card.Visibility = Visibility.Collapsed;
            card.EditButton.Click += EditButton_Click;
            card.AuthorButton.Click += AuthorButton_Click;
            card.Cloud.Content = skillCards.Cloud;
            return card;
        }

        private void AuthorButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject ptr = sender as DependencyObject;
            while (!(ptr is Custom_Card_SkillCard)) ptr = VisualTreeHelper.GetParent(ptr);
            XY.Send("作者查询#" + (ptr as Custom_Card_SkillCard).SkillCardsModel.UserName);
            User.Visibility = Visibility.Visible;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject ptr = sender as DependencyObject;
            while (!(ptr is Custom_Card_SkillCard)) ptr = VisualTreeHelper.GetParent(ptr);
            EditCard.Open_Edit((Custom_Card_SkillCard)ptr);
        }

        private void Fitler()
        {
            Filter_Skill.Name = Template_Skill_Name.Text;
            IEnumerable<SkillCardsModel> array = Filter.SkillCardsModel((from Custom_Card_SkillCard item in CardsPanel.Children select item.SkillCardsModel), Rate.Value - 1, Filter_Skill);
            foreach (Custom_Card_SkillCard item in CardsPanel.Children)
            {
                if (array != null && array.Where<SkillCardsModel>(x => item.SkillCardsModel == x).Count() != 0)
                {
                    item.Visibility = Visibility.Visible;
                    item.Rate.Value = Rate.Value;
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
            SkillCard[] skillCards = new SkillCard[5] { new SkillCard(), new SkillCard(), new SkillCard(), new SkillCard(), new SkillCard() };
            int cnt = 0;
            SkillCardsModel skillCardsModel = new SkillCardsModel();
            foreach (SkillCard item in skillCards)
            {
                ++cnt;
                item.Level = cnt;
                item.Name = "新技能" + cnt.ToString();
                item.Father_ID = skillCardsModel.ID;
            }
            skillCardsModel.SkillCards = skillCards;
            skillCardsModel.UserName = GeneralControl.Menu_Person_Information_Class.Instance.User.UserName;
            skillCardsModel.Add_To_General();
            skillCardsModel.Save();
            Add_Card(skillCardsModel);
        }

        private void Template_Skill_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            Fitler();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            XY.Send("获取技能卡#" + GeneralControl.Skill_Card_Date);
        }
    }
}
