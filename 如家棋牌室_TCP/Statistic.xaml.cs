using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace 如家棋牌室_TCP
{
    /// <summary>
    /// Statistic.xaml 的交互逻辑
    /// </summary>
    public partial class Statistic : Page
    {
        public Statistic()
        {
            InitializeComponent();
        }

        readonly ObservableCollection<Item> Items = new ObservableCollection<Item>();
        readonly ObservableCollection<Order> Orders = new ObservableCollection<Order>();

        public class Item
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public decimal Num { get; set; }
            public decimal Amount { get; set; }
            public string Type { get; set; }
        }

        public class Order
        {
            public string Order_ID { get; set; }
            public string Rooms_Name { get; set; }
            public string Type { get; set; }
            public string Static { get; set; }
            public decimal Amount { get; set; }
            public string Start_Time { get; set; }
            public string End_Time { get; set; }
            public string Expend_Time { get; set; }
            public int Mintues { get; set; }
        }

        private void Bt_Close_Click(object sender, RoutedEventArgs e)
        {
            Rooms r = new Rooms();
            NavigationService.Navigate(r);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            System.Data.DataSet Ds_Items = DB_Work.DataSetCmd("SELECT * FROM Views_Statistic_Bills_Detail ORDER BY 类型 DESC, 名称");

            Items.Clear();

            for (int i = 0; i < Ds_Items.Tables[0].Rows.Count; i++)
            {
                Items.Add(new Item
                {
                    Name = Ds_Items.Tables[0].Rows[i][0].ToString(),
                    Price = Convert.ToDecimal(Ds_Items.Tables[0].Rows[i][1]),
                    Num = Convert.ToDecimal(Ds_Items.Tables[0].Rows[i][2]),
                    Amount = Convert.ToDecimal(Ds_Items.Tables[0].Rows[i][3]),
                    Type = Ds_Items.Tables[0].Rows[i][4].ToString()
                });

                if (Ds_Items.Tables[0].Rows[i][4].ToString() == "饮料类")
                {
                    tbAmount_YL.Text = (Convert.ToDecimal(tbAmount_YL.Text) + Convert.ToDecimal(Ds_Items.Tables[0].Rows[i][3])).ToString();
                }
                else if (Ds_Items.Tables[0].Rows[i][4].ToString() == "香烟类")
                {
                    tbAmount_XY.Text = (Convert.ToDecimal(tbAmount_XY.Text) + Convert.ToDecimal(Ds_Items.Tables[0].Rows[i][3])).ToString();
                }
                else
                {
                    tbAmount_QT.Text = (Convert.ToDecimal(tbAmount_QT.Text) + Convert.ToDecimal(Ds_Items.Tables[0].Rows[i][3])).ToString();
                }

                tbAmount.Text = (Convert.ToDecimal(tbAmount.Text) + Convert.ToDecimal(Ds_Items.Tables[0].Rows[i][3])).ToString();
            }

            lvItems.ItemsSource = Items;

            System.Data.DataSet Ds_Orders = DB_Work.DataSetCmd("SELECT * FROM Views_Statistic_Bills ORDER BY 订单");

            Orders.Clear();            

            int intYS = 0;

            for (int i = 0; i < Ds_Orders.Tables[0].Rows.Count; i++)
            {
                Orders.Add(new Order
                {
                    Order_ID = Ds_Orders.Tables[0].Rows[i][0].ToString(),
                    Rooms_Name = Ds_Orders.Tables[0].Rows[i][1].ToString(),
                    Type = Ds_Orders.Tables[0].Rows[i][2].ToString(),
                    Static = Ds_Orders.Tables[0].Rows[i][3].ToString(),
                    Amount = Convert.ToDecimal(Ds_Orders.Tables[0].Rows[i][4]),
                    Start_Time = Ds_Orders.Tables[0].Rows[i][5].ToString(),
                    End_Time = Ds_Orders.Tables[0].Rows[i][6].ToString(),
                    Expend_Time = Ds_Orders.Tables[0].Rows[i][7].ToString(),
                    Mintues = Convert.ToInt16(Ds_Orders.Tables[0].Rows[i][8])
                });

                tbRooms_SS.Text = (Convert.ToDecimal(tbRooms_SS.Text) + Convert.ToDecimal(Ds_Orders.Tables[0].Rows[i][4])).ToString();

                if (Ds_Orders.Tables[0].Rows[i][2].ToString()=="计时")
                {
                    if (Convert.ToInt32(Ds_Orders.Tables[0].Rows[i][8]) > 0)
                    {
                        if ((Convert.ToInt32(Ds_Orders.Tables[0].Rows[i][8]) % 60) > 20)
                        {
                            intYS = Convert.ToInt32(Ds_Orders.Tables[0].Rows[i][8]) / 60 + 1;
                        }
                        else
                        {
                            intYS = Convert.ToInt32(Ds_Orders.Tables[0].Rows[i][8]) / 60;
                        }

                        tbRooms_YS.Text = (Convert.ToDecimal(tbRooms_YS.Text) + (intYS * 10)).ToString("c").Substring(1);
                    }
                }
                else if (Ds_Orders.Tables[0].Rows[i][2].ToString() == "美团")
                {
                    if ((Convert.ToInt32(Ds_Orders.Tables[0].Rows[i][8]) / 60) > 5)
                    {
                        intYS = Convert.ToInt32(Ds_Orders.Tables[0].Rows[i][8]) / 60 - 5;
                    }

                    tbRooms_YS.Text = (Convert.ToDecimal(tbRooms_YS.Text) + (intYS * 10)).ToString("c").Substring(1);
                }
                else if (Ds_Orders.Tables[0].Rows[i][2].ToString() == "包夜")
                {
                    tbRooms_YS.Text = (Convert.ToDecimal(tbRooms_YS.Text) + 40).ToString("c").Substring(1);
                }
                
            }

            lvOrders.ItemsSource = Orders;

            tbRooms_Num.Text = Ds_Orders.Tables[0].Rows.Count.ToString();
        }

        private void Bt_Settlement_Click(object sender, RoutedEventArgs e)
        {
            Statistic_Detail sd = new Statistic_Detail();

            sd.tbAmount_YL.Text = tbAmount_YL.Text;
            sd.tbAmount_XY.Text = tbAmount_XY.Text;
            sd.tbAmount_QT.Text = tbAmount_QT.Text;
            sd.tbAmount.Text = tbAmount.Text;

            sd.tbRooms_Num.Text = tbRooms_Num.Text;
            sd.tbRooms_YS.Text = tbRooms_YS.Text;
            sd.tbRooms_SS.Text = tbRooms_SS.Text;

            sd.ShowDialog();
            
            if (sd.DialogResult.Value)
            {
                Rooms r = new Rooms();
                NavigationService.Navigate(r);
            }
        }

        private void Bt_Print_List_Click(object sender, RoutedEventArgs e)
        {
            Prints.Print_Settlement_Inventory();
        }
    }
}
