using Make;
using Make.MODEL;
using System.Windows;

namespace Pack.Element
{
    /// <summary>
    /// Custom_Card.xaml 的交互逻辑
    /// </summary>
    public partial class Custom_Card_SkillCard
    {
        public SkillCardsModel SkillCardsModel;
        public Custom_Card_SkillCard(SkillCardsModel skillCards)
        {
            InitializeComponent();
            DataContext = skillCards.SkillCards[0];
            SkillCardsModel = skillCards;
            Cloud.Content = SkillCardsModel.Cloud;
        }
        public Custom_Card_SkillCard()
        {
            InitializeComponent();

        }
        private void Rate_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            if (SkillCardsModel != null)
            {
                DataContext = SkillCardsModel.SkillCards[e.NewValue - 1];
            }
        }

        private void AuthorButton_Click(object sender, RoutedEventArgs e)
        {
            GeneralControl.Token.Send(Enums.Msg_Client_Type.Information, "作者查询#" + SkillCardsModel.UserName);
        }
    }
}
