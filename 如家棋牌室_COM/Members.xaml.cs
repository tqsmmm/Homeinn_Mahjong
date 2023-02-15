using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace 如家棋牌室_COM
{
    /// <summary>
    /// Members.xaml 的交互逻辑
    /// </summary>
    public partial class Members : Page
    {
        public Members()
        {
            InitializeComponent();
        }

        ObservableCollection<Member> members = new ObservableCollection<Member>();

        public class Member
        {
            public string Card { get; set; }
            public string Name { get; set; }
            public string Tel { get; set; }
            public decimal Integrals { get; set; }
            public string Create_Time { get; set; }
            public decimal Total_Expend { get; set; }
            public decimal In_Times { get; set; }
            public string Last_Date { get; set; }
            public decimal Last_Expend { get; set; }
        }

        private void Bt_Close_Click(object sender, RoutedEventArgs e)
        {
            Rooms r = new Rooms();
            NavigationService.Navigate(r);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            System.Data.DataSet Ds = DB_Work.DataSetCmd("SELECT * FROM Views_Members ORDER BY 累计消费 DESC");

            members.Clear();

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                members.Add(new Member
                {
                    Card = Ds.Tables[0].Rows[i][0].ToString(),
                    Name = Ds.Tables[0].Rows[i][1].ToString(),
                    Tel = Ds.Tables[0].Rows[i][2].ToString(),
                    Integrals = Convert.ToDecimal(Ds.Tables[0].Rows[i][3]),
                    Create_Time = Ds.Tables[0].Rows[i][4].ToString(),
                    Total_Expend = Convert.ToDecimal(Ds.Tables[0].Rows[i][5]),
                    In_Times = Convert.ToDecimal(Ds.Tables[0].Rows[i][6]),
                    Last_Date = Ds.Tables[0].Rows[i][7].ToString(),
                    Last_Expend = Convert.ToDecimal(Ds.Tables[0].Rows[i][8])
                });
            }

            lvMembers.ItemsSource = members;
        }

        private void BtExchange_Click(object sender, RoutedEventArgs e)
        {
            Integrals r = new Integrals();
            NavigationService.Navigate(r);
        }
    }
}
