using System.Windows.Controls;

namespace 如家棋牌室_TCP
{
    /// <summary>
    /// Lock.xaml 的交互逻辑
    /// </summary>
    public partial class Lock : Page
    {
        public Lock()
        {
            InitializeComponent();
        }

        private void Bt_1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Password.Password += "1";
        }

        private void Bt_2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Password.Password += "2";
        }

        private void Bt_3_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Password.Password += "3";
        }

        private void Bt_4_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Password.Password += "4";
        }

        private void Bt_5_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Password.Password += "5";
        }

        private void Bt_6_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Password.Password += "6";
        }

        private void Bt_7_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Password.Password += "7";
        }

        private void Bt_8_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Password.Password += "8";
        }

        private void Bt_9_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Password.Password += "9";
        }

        private void Bt_0_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Password.Password += "0";
        }

        private void Bt_OK_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (tb_Password.Password == "2551630")
            {
                Rooms d = new Rooms();
                NavigationService.Navigate(d);
            }
        }
    }
}
