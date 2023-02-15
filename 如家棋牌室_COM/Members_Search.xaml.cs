using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace 如家棋牌室_COM
{
    /// <summary>
    /// Members_Search.xaml 的交互逻辑
    /// </summary>
    public partial class Members_Search : Page
    {
        public Members_Search()
        {
            InitializeComponent();
        }

        public string Order_ID = string.Empty;
        public string Rooms = string.Empty;

        ObservableCollection<Member> members = new ObservableCollection<Member>();

        public class Member
        {
            public string Card { get; set; }
            public string Name { get; set; }
            public string Tel { get; set; }
            public decimal Integral { get; set; }
        }

        private void LvMembers_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Member mem = (Member)lvMembers.SelectedItem;
        }

        private void Bt_1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Search.Text = tb_Search.Text + "1";
        }

        private void Bt_2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Search.Text = tb_Search.Text + "2";
        }

        private void Bt_3_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Search.Text = tb_Search.Text + "3";
        }

        private void Bt_4_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Search.Text = tb_Search.Text + "4";
        }

        private void Bt_5_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Search.Text = tb_Search.Text + "5";
        }

        private void Bt_6_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Search.Text = tb_Search.Text + "6";
        }

        private void Bt_7_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Search.Text = tb_Search.Text + "7";
        }

        private void Bt_8_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Search.Text = tb_Search.Text + "8";
        }

        private void Bt_9_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Search.Text = tb_Search.Text + "9";
        }

        private void Bt_0_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Search.Text = tb_Search.Text + "0";
        }

        private void Bt_Dot_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void Bt_Clear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tb_Search.Text = string.Empty;
        }

        private void Bt_Close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Details d = new Details()
            {
                Title = "占用"
            };

            d.btStart.IsEnabled = false;
            d.Order_ID = Order_ID;
            d.tbRooms.Text = Rooms.ToString();
            NavigationService.Navigate(d);
        }

        private void Bt_Search_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (tb_Search.Text.Length != 11)
            {
                Public.Sys_MsgBox("输入的号码必须为手机号码，请确定是否为电话号码！");
            }
            else
            {
                System.Data.DataSet Ds = DB_Work.DataSetCmd($"SELECT Card, Name, Tel, ISNULL(Integral, 0) FROM Members WHERE Tel = '{tb_Search.Text}'");

                members.Clear();

                if (Ds.Tables[0].Rows.Count == 0)
                {
                    if (Public.Sys_MsgYN("没有查找到这个电话号码，是否确定创建这个会员卡？"))
                    {
                        DB_Work.ExecuteCmd($"INSERT INTO Members(Card, Name, Tel, Integral, Create_Time) VALUES('', '', '{tb_Search.Text}', 0, GETDATE())");

                        DB_Work.ExecuteCmd($"UPDATE Bills SET Members = '{tb_Search.Text}' WHERE Order_ID = '{Order_ID}'");

                        Bt_Close_Click(null, null);
                    }
                }
                else if (Ds.Tables[0].Rows.Count == 1)
                {
                    DB_Work.ExecuteCmd($"UPDATE Bills SET Members = '{tb_Search.Text}' WHERE Order_ID = '{Order_ID}'");

                    Bt_Close_Click(null, null);
                }
                else
                {
                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {
                        members.Add(new Member
                        {
                            Card = Ds.Tables[0].Rows[i][0].ToString(),
                            Name = Ds.Tables[0].Rows[i][1].ToString(),
                            Tel = Ds.Tables[0].Rows[i][2].ToString(),
                            Integral = Convert.ToDecimal(Ds.Tables[0].Rows[i][3])
                        });
                    }

                    lvMembers.ItemsSource = members;
                }
            }
        }
    }
}
