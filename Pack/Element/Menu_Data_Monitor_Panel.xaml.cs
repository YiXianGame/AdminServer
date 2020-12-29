using Make.MODEL;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Pack.Element
{
    /// <summary>
    /// Menu_Data_Monitor.xaml 的交互逻辑
    /// </summary>
    public partial class Menu_Data_Monitor_Panel : UserControl
    {
        Adventure origin_Adventure, new_Adventure;
        SkillCardsModel origin_SkillcardsModel, new_SkillCardsModel;
        public Menu_Data_Monitor_Panel()
        {
            InitializeComponent();
        }
    }
}
