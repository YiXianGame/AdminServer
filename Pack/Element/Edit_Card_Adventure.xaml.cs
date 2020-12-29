using Make;
using Make.MODEL;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Pack.Element
{

    /// <summary>
    /// Edit_Card.xaml 的交互逻辑
    /// </summary>
    public partial class Edit_Card_Adventure : UserControl
    {
        Custom_Card_Adventure Origin_Custom_Card;
        public Edit_Card_Adventure()
        {
            InitializeComponent();
            Custom_Card_Adventure.State.IsEnabled = true;
        }


        public void Open_Edit(Custom_Card_Adventure adventureCard)
        {
            Origin_Custom_Card = adventureCard;
            Custom_Card_Adventure.Adventure = adventureCard.Adventure;
            Custom_Card_Adventure.DataContext = adventureCard.Adventure;
            Visibility = Visibility.Visible;
        }


        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Custom_Card_Adventure.DataContext != null)
            {
                State state = (State)((ComboBox)sender).DataContext;
                state.Name = ((ComboBoxItem)((ComboBox)sender).SelectedItem).Content.ToString();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            States_Select.Visibility = Visibility.Visible;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Origin_Custom_Card.Adventure.Save();

            GeneralControl.Adventure_Date = DateTime.Now;
            this.Visibility = Visibility.Hidden;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Custom_Card_Adventure.Adventure.Effect_States.Remove((State)(((Button)sender).DataContext));
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            GeneralControl.Adventure_Date = DateTime.Now;
            XY.MainWindow.AdventurePanle.AdventurePanel.Children.Remove(Origin_Custom_Card);
            GeneralControl.Adventures.Remove(Origin_Custom_Card.Adventure);
            GeneralControl.Adventures_ID.Remove(Origin_Custom_Card.Adventure.ID);
            Origin_Custom_Card.Adventure.Delete();
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (Custom_Card_Adventure.Adventure.Effect_States.Count >= GeneralControl.MaxStates)
            {
                MessageBox.Show("状态数量已满");
                States_Select.Visibility = Visibility.Hidden;
                return;
            }
            State state = new State
            {
                Name = (sender as Button).Content.ToString(),
                Duration_Immediate = 10,
                Duration_Round = 1
            };
            Custom_Card_Adventure.Adventure.Effect_States.Add(state);
            States_Select.Visibility = Visibility.Hidden;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            XY.Send("奇遇上传#" + JsonConvert.SerializeObject(Origin_Custom_Card.Adventure));
        }
    }
}
