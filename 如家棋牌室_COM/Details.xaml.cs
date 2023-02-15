using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace 如家棋牌室_COM
{
    /// <summary>
    /// Details.xaml 的交互逻辑
    /// </summary>
    public partial class Details : Page
    {
        public Details()
        {
            InitializeComponent();
        }

        ObservableCollection<Item> Items = new ObservableCollection<Item>();

        ObservableCollection<Selected> Selecteds = new ObservableCollection<Selected>();

        public string Order_ID = string.Empty;

        public class Item
        {
            public int Item_ID { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Type { get; set; }
            public BitmapImage Image { get; set; }
        }

        public class Selected
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public decimal Num { get; set; }
            public decimal Amount { get; set; }
        }

        private void Load_Items()
        {
            System.Data.DataSet Ds = DB_Work.DataSetCmd("SELECT id, Name, Price, Type, Image FROM Items WHERE Visual = 1 ORDER BY Type, Price");

            Items.Clear();

            byte[] br = null;

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                if (Ds.Tables[0].Rows[i][4] == DBNull.Value)
                {
                    Items.Add(new Item
                    {
                        Item_ID = Convert.ToInt32(Ds.Tables[0].Rows[i][0]),
                        Name = Ds.Tables[0].Rows[i][1].ToString(),
                        Price = Convert.ToDecimal(Ds.Tables[0].Rows[i][2]),
                        Type = Ds.Tables[0].Rows[i][3].ToString(),
                        Image = null
                    });
                }
                else
                {
                    br = (byte[])(Ds.Tables[0].Rows[i][4]);

                    BitmapImage img = ImageConverter.ByteArrayToBitmapImage(br);

                    Items.Add(new Item
                    {
                        Item_ID = Convert.ToInt32(Ds.Tables[0].Rows[i][0]),
                        Name = Ds.Tables[0].Rows[i][1].ToString(),
                        Price = Convert.ToDecimal(Ds.Tables[0].Rows[i][2]),
                        Type = Ds.Tables[0].Rows[i][3].ToString(),
                        Image = img
                    });
                }
            }

            lvItems.ItemsSource = Items;
        }

        private void Load_Detail()
        {
            System.Data.DataSet Ds = DB_Work.DataSetCmd($"SELECT Bills_Detail.id, Items.Name, Items.Price, Bills_Detail.Num, Items.Price * Bills_Detail.Num, Items.Type FROM Bills_Detail LEFT JOIN Items ON Items.id = Bills_Detail.Item_ID WHERE Order_ID = '{Order_ID}'");

            Selecteds.Clear();

            tbAmount_YL.Text = "0.00";
            tbAmount_XY.Text = "0.00";
            tbAmount_QT.Text = "0.00";
            tbAmount.Text = "0.00";

            string strStatic = string.Empty;

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                Selecteds.Add(new Selected
                {
                    ID = Convert.ToInt32(Ds.Tables[0].Rows[i][0]),
                    Name = Ds.Tables[0].Rows[i][1].ToString(),
                    Price = Convert.ToDecimal(Ds.Tables[0].Rows[i][2]),
                    Num = Convert.ToDecimal(Ds.Tables[0].Rows[i][3]),
                    Amount = Convert.ToDecimal(Ds.Tables[0].Rows[i][4])
                });

                if (Ds.Tables[0].Rows[i][5].ToString() == "饮料类")
                {
                    tbAmount_YL.Text = (Convert.ToDecimal(tbAmount_YL.Text) + Convert.ToDecimal(Ds.Tables[0].Rows[i][4])).ToString();
                }
                else if (Ds.Tables[0].Rows[i][5].ToString() == "香烟类")
                {
                    tbAmount_XY.Text = (Convert.ToDecimal(tbAmount_XY.Text) + Convert.ToDecimal(Ds.Tables[0].Rows[i][4])).ToString();
                }
                else
                {
                    tbAmount_QT.Text = (Convert.ToDecimal(tbAmount_QT.Text) + Convert.ToDecimal(Ds.Tables[0].Rows[i][4])).ToString();
                }

                tbAmount.Text = (Convert.ToDecimal(tbAmount.Text) + Convert.ToDecimal(Ds.Tables[0].Rows[i][4])).ToString();
            }

            lvDetail.ItemsSource = Selecteds;
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Title == "占用")
            {
                System.Data.DataSet Ds = DB_Work.DataSetCmd($"SELECT Rooms_Name, Start_Time, Members, ISNULL(Members.Integral, 0) FROM Bills LEFT JOIN Members ON Members.Tel = Bills.Members WHERE Order_ID='{Order_ID}'");

                if (Ds.Tables[0].Rows[0][0].ToString() == tbRooms.Text)
                {
                    tbStart.Text = Convert.ToDateTime(Ds.Tables[0].Rows[0][1]).ToString("HH:mm");
                    tbExpend.Text = Public.ExecDateDiff(Convert.ToDateTime(Ds.Tables[0].Rows[0][1]), DateTime.Now);
                    tb_Members.Text = Ds.Tables[0].Rows[0][2].ToString();
                    tb_Integral.Text = Convert.ToDecimal(Ds.Tables[0].Rows[0][3]).ToString();

                    Load_Detail();
                }
                else
                {
                    Public.Sys_MsgBox("读取错误！");

                    Rooms r = new Rooms();
                    NavigationService.Navigate(r);
                }

                Load_Items();
            }
        }

        private void BtStart_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DB_Work.ExecuteCmd($"UPDATE Rooms SET Order_ID = '{Order_ID}',Static = '占用' WHERE Rooms_Name = {tbRooms.Text}");

            btStart.IsEnabled = false;
            btEnd.IsEnabled = true;

            Light.Light_Trun_On(tbRooms.Text);

            BtClose_Click(null, null);
        }

        private void BtEnd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Details_Checkout dc = new Details_Checkout()
            {
                Order_ID = Order_ID
            };

            dc.tbRooms.Text = tbRooms.Text;

            NavigationService.Navigate(dc);
        }

        private void BtChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Details_Change c = new Details_Change();
            c.ShowDialog();

            if (c.DialogResult.Value)
            {
                DB_Work.ExecuteCmd($"Insert_Changes '{Order_ID}', '{tbRooms.Text}', '{c.Rooms_Name}'");

                Light.Light_Turn_Off(tbRooms.Text);

                tbRooms.Text = c.Rooms_Name;

                Thread.Sleep(200);

                Light.Light_Trun_On(tbRooms.Text);
            }
        }

        private void BtClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Title == "空闲" && btStart.IsEnabled)
            {
                DB_Work.ExecuteCmd($"DELETE FROM Bills WHERE Order_ID='{Order_ID}'");
            }

            Rooms r = new Rooms();
            NavigationService.Navigate(r);
        }

        private void LvItems_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvItems.SelectedItems.Count > 0)
            {
                Item d = (Item)lvItems.SelectedItem;

                System.Data.DataSet Ds = DB_Work.DataSetCmd($"SELECT id FROM Bills_Detail WHERE Order_ID = '{Order_ID}' AND Item_ID = {d.Item_ID}");

                if (Ds.Tables[0].Rows.Count > 0)
                {
                    DB_Work.ExecuteCmd($"UPDATE Bills_Detail SET Num = Num + 1 WHERE id = {Ds.Tables[0].Rows[0][0].ToString()}");
                }
                else
                {
                    DB_Work.ExecuteCmd($"INSERT INTO Bills_Detail(Order_ID, Item_ID, Num, Gift) VALUES('{Order_ID}', {d.Item_ID}, 1, 0)");
                }

                Load_Detail();
            }
        }

        private void LvDetail_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvDetail.SelectedItems.Count > 0)
            {
                Selected i = (Selected)lvDetail.SelectedItem;

                if (i.Num == 1)
                {
                    DB_Work.ExecuteCmd($"DELETE FROM Bills_Detail WHERE id = {i.ID}");
                }
                else
                {
                    DB_Work.ExecuteCmd($"UPDATE Bills_Detail SET Num = Num -1 WHERE id = {i.ID}");
                }

                Load_Detail();
            }
        }

        private void BtTurnOn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Light.Light_Trun_On(tbRooms.Text);

            BtClose_Click(null, null);
        }

        private void BtTurnOff_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Light.Light_Turn_Off(tbRooms.Text);

            BtClose_Click(null, null);
        }

        private void Bt_Members_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Members_Search ms = new Members_Search()
            {
                Order_ID = Order_ID,
                Rooms = tbRooms.Text
            };

            NavigationService.Navigate(ms);
        }

        private void Bt_Clean_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DB_Work.ExecuteCmd($"UPDATE Rooms SET Static = '空闲' WHERE Rooms_Name = '{tbRooms.Text}'");

            BtClose_Click(null, null);
        }

        private void Bt_Clean_Good_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DB_Work.ExecuteCmd($"Insert_Arranges '{tbRooms.Text}'");

            BtClose_Click(null, null);
        }
    }
}
