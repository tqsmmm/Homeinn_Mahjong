using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace 如家棋牌室_COM
{
    /// <summary>
    /// Retail.xaml 的交互逻辑
    /// </summary>
    public partial class Retail : Page
    {
        public Retail()
        {
            InitializeComponent();
        }

        ObservableCollection<Item> Items = new ObservableCollection<Item>();

        ObservableCollection<Selected> Selecteds = new ObservableCollection<Selected>();

        string Order_ID = string.Empty;

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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Order_ID = DB_Work.DataSetCmd("Insert_Bills '零售'").Tables[0].Rows[0][0].ToString();

            Load_Items();

            Load_Detail();
        }

        private void Bt_Close_Click(object sender, RoutedEventArgs e)
        {
            if (lvDetail.Items.Count == 0)
            {
                DB_Work.ExecuteCmd($"DELETE FROM Bills WHERE Order_ID = '{Order_ID}'");
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
    }
}
