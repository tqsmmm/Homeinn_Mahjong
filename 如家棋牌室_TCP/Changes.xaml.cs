using System.Windows;
using System.Windows.Controls;

namespace 如家棋牌室_TCP
{
    /// <summary>
    /// Changes.xaml 的交互逻辑
    /// </summary>
    public partial class Changes : Page
    {
        public Changes()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            System.Data.DataSet Ds = DB_Work.DataSetCmd("SELECT * FROM Views_Changes");

            if (Ds.Tables[0].Rows.Count > 0)
            {
                dgChanges.ItemsSource = Ds.Tables[0].DefaultView;
            }
        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            Rooms r = new Rooms();
            NavigationService.Navigate(r);
        }
    }
}
