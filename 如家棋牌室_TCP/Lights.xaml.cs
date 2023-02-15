using System.Windows;
using System.Windows.Controls;

namespace 如家棋牌室_TCP
{
    /// <summary>
    /// Lights.xaml 的交互逻辑
    /// </summary>
    public partial class Lights : Page
    {
        public Lights()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            System.Data.DataSet Ds_Lights = DB_Work.DataSetCmd("SELECT id,Rooms_Name,Type, CONVERT(VARCHAR(16), DateTime, 120) AS [DateTime] FROM Lights WHERE CONVERT(VARCHAR(10), DateTime, 120) = CONVERT(VARCHAR(10), GETDATE(), 120) ORDER BY DateTime");

            if (Ds_Lights.Tables[0].Rows.Count > 0)
            {
                dgLights.ItemsSource = Ds_Lights.Tables[0].DefaultView;
            }
        }

        private void Bt_Close_Click(object sender, RoutedEventArgs e)
        {
            Rooms r = new Rooms();
            NavigationService.Navigate(r);
        }
    }
}
