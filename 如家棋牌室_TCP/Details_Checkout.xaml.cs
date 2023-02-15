using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace 如家棋牌室_TCP
{
    /// <summary>
    /// Detail_Checkout.xaml 的交互逻辑
    /// </summary>
    public partial class Details_Checkout : Page
    {
        public Details_Checkout()
        {
            InitializeComponent();
        }

        public string Order_ID = string.Empty;

        string Type = string.Empty;

        ObservableCollection<Item> Items = new ObservableCollection<Item>();

        public class Item
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public decimal Num { get; set; }
            public decimal Amount { get; set; }
            public string Type { get; set; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Data.DataSet Ds = DB_Work.DataSetCmd($"SELECT Rooms_Name, Start_Time, Members, ISNULL(Members.Integral, 0) FROM Bills LEFT JOIN Members ON Members.Tel = Bills.Members WHERE Order_ID='{Order_ID}'");

            if (Ds.Tables[0].Rows[0][0].ToString() == tbRooms.Text)
            {
                tbStart.Text = Convert.ToDateTime(Ds.Tables[0].Rows[0][1]).ToString("HH:mm");
                tbExpend.Text = Public.ExecDateDiff(Convert.ToDateTime(Ds.Tables[0].Rows[0][1]), DateTime.Now);
                //tb_Members.Text = "会员：" + Ds.Tables[0].Rows[0][2].ToString();
                //tb_Integral.Text = "积分：" + Convert.ToDecimal(Ds.Tables[0].Rows[0][3]).ToString();

                int intYS = 0;

                if (Convert.ToInt32(Convert.ToDateTime(tbExpend.Text).Minute) > 20)
                {
                    intYS = Convert.ToInt32(Convert.ToDateTime(tbExpend.Text).Hour) + 1;
                }
                else
                {
                    intYS = Convert.ToInt32(Convert.ToDateTime(tbExpend.Text).Hour);
                }

                tbAmount_YS.Text = (intYS * 10).ToString("c").Substring(1);

                Load_Detail();
            }
            else
            {
                Public.Sys_MsgBox("读取错误！");

                Rooms r = new Rooms();
                NavigationService.Navigate(r);
            }
        }

        private void Load_Detail()
        {
            System.Data.DataSet Ds = DB_Work.DataSetCmd($"SELECT Items.Name AS [商品名称], Items.Price AS [单价], Bills_Detail.Num AS [数量], Items.Price * Bills_Detail.Num AS [总价], Items.Type AS [类型] FROM Bills_Detail LEFT JOIN Items ON Items.id = Bills_Detail.Item_ID WHERE Order_ID = '{Order_ID}'");

            Items.Clear();

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                Items.Add(new Item
                {
                    Name = Ds.Tables[0].Rows[i][0].ToString(),
                    Price = Convert.ToDecimal(Ds.Tables[0].Rows[i][1]),
                    Num = Convert.ToDecimal(Ds.Tables[0].Rows[i][2]),
                    Amount=Convert.ToDecimal(Ds.Tables[0].Rows[i][3]),
                    Type = Ds.Tables[0].Rows[i][4].ToString()
                });

                if (Ds.Tables[0].Rows[i][4].ToString() == "饮料类")
                {
                    tbAmount_YL.Text = (Convert.ToDecimal(tbAmount_YL.Text) + Convert.ToDecimal(Ds.Tables[0].Rows[i][3])).ToString();
                }
                else if (Ds.Tables[0].Rows[i][4].ToString() == "香烟类")
                {
                    tbAmount_XY.Text = (Convert.ToDecimal(tbAmount_XY.Text) + Convert.ToDecimal(Ds.Tables[0].Rows[i][3])).ToString();
                }
                else
                {
                    tbAmount_QT.Text = (Convert.ToDecimal(tbAmount_QT.Text) + Convert.ToDecimal(Ds.Tables[0].Rows[i][3])).ToString();
                }

                tbAmount.Text = (Convert.ToDecimal(tbAmount.Text) + Convert.ToDecimal(Ds.Tables[0].Rows[i][3])).ToString();
            }

            lvDetail.ItemsSource = Items;
        }

        private void Bt_OverNight_Click(object sender, RoutedEventArgs e)
        {
            Type = "包夜";

            bt_OverNight.IsEnabled = false;
            bt_MeiTuan.IsEnabled = true;
            bt_Timing.IsEnabled = true;

            bt_CheckOut.IsEnabled = true;
        }

        private void Bt_MeiTuan_Click(object sender, RoutedEventArgs e)
        {
            Type = "美团";

            bt_OverNight.IsEnabled = true;
            bt_MeiTuan.IsEnabled = false;
            bt_Timing.IsEnabled = true;

            bt_CheckOut.IsEnabled = true;
        }

        private void Bt_Timing_Click(object sender, RoutedEventArgs e)
        {
            Type = "计时";

            bt_OverNight.IsEnabled = true;
            bt_MeiTuan.IsEnabled = true;
            bt_Timing.IsEnabled = false;

            bt_CheckOut.IsEnabled = true;
        }

        private void Bt_Exempt_Click(object sender, RoutedEventArgs e)
        {
            tbAmount_SS.Text = "0.00";
        }

        private void Bt_30_Click(object sender, RoutedEventArgs e)
        {
            tbAmount_SS.Text = "30.00";
        }

        private void Bt_40_Click(object sender, RoutedEventArgs e)
        {
            tbAmount_SS.Text = "40.00";
        }

        private void Bt_50_Click(object sender, RoutedEventArgs e)
        {
            tbAmount_SS.Text = "50.00";
        }

        private void Bt_1_Click(object sender, RoutedEventArgs e)
        {
            tbAmount_SS.Text += "1";
        }

        private void Bt_2_Click(object sender, RoutedEventArgs e)
        {
            tbAmount_SS.Text += "2";
        }

        private void Bt_3_Click(object sender, RoutedEventArgs e)
        {
            tbAmount_SS.Text += "3";
        }

        private void Bt_4_Click(object sender, RoutedEventArgs e)
        {
            tbAmount_SS.Text += "4";
        }

        private void Bt_5_Click(object sender, RoutedEventArgs e)
        {
            tbAmount_SS.Text += "5";
        }

        private void Bt_6_Click(object sender, RoutedEventArgs e)
        {
            tbAmount_SS.Text += "6";
        }

        private void Bt_7_Click(object sender, RoutedEventArgs e)
        {
            tbAmount_SS.Text += "7";
        }

        private void Bt_8_Click(object sender, RoutedEventArgs e)
        {
            tbAmount_SS.Text += "8";
        }

        private void Bt_9_Click(object sender, RoutedEventArgs e)
        {
            tbAmount_SS.Text += "9";
        }

        private void Bt_0_Click(object sender, RoutedEventArgs e)
        {
            tbAmount_SS.Text += "0";
        }

        private void Bt_Dot_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Bt_Clear_Click(object sender, RoutedEventArgs e)
        {
            tbAmount_SS.Text = "0.00";
        }

        private void Bt_Close_Click(object sender, RoutedEventArgs e)
        {
            Details d = new Details()
            {
                Title = "占用"
            };

            d.btStart.IsEnabled = false;
            d.Order_ID = Order_ID;
            d.tbRooms.Text = tbRooms.Text.ToString();
            NavigationService.Navigate(d);
        }

        private void Bt_CheckOut_Click(object sender, RoutedEventArgs e)
        {
            if (Type != string.Empty && tbAmount_SS.Text.ToString().Length > 0)
            {
                DB_Work.ExecuteCmd($"Update_Bills '{Order_ID}',{tbAmount_SS.Text},'{Type}'");

                if (SMS.CheckGate())
                {
                    SMS.Send_SMS(Order_ID);
                }

                Rooms r = new Rooms();
                NavigationService.Navigate(r);
            }
        }
    }
}
