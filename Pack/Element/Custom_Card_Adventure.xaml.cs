using Make.MODEL;

namespace Pack.Element
{
    /// <summary>
    /// Custom_Card.xaml 的交互逻辑
    /// </summary>
    public partial class Custom_Card_Adventure
    {
        public Adventure Adventure;
        public Custom_Card_Adventure(Adventure adventure)
        {
            InitializeComponent();
            DataContext = adventure;
            Adventure = adventure;
            InitializeComponent();
            Cloud.Content = adventure.Cloud;
        }
        public Custom_Card_Adventure()
        {
            InitializeComponent();
        }
    }
}
