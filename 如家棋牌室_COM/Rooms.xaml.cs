using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Threading;

namespace 如家棋牌室_COM
{
    /// <summary>
    /// Rooms.xaml 的交互逻辑
    /// </summary>
    public partial class Rooms : Page
    {
        public Rooms()
        {
            InitializeComponent();
        }

        private DispatcherTimer ShowTimer;

        ObservableCollection<Room> rooms = new ObservableCollection<Room>();

        public class Room
        {
            public string Order_ID { get; set; }
            public string Rooms_Name { get; set; }
            public string Type { get; set; }
            public string Start_Time { get; set; }
            public string End_Time { get; set; }
            public string Expend_Time { get; set; }
            public string Static { get; set; }
            public string Background { get; set; }
            public string Light_Static { get; set; }
            public string Members_Static { get; set; }
            public string Clean_Static { get; set; }
        }

        private void LvRooms_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvRooms.SelectedItems.Count > 0)
            {
                Room room = (Room)lvRooms.SelectedItem;

                switch (room.Static)
                {
                    case "空闲":
                        Details d1 = new Details()
                        {
                            Title = "空闲"
                        };

                        d1.btEnd.IsEnabled = false;
                        d1.bt_Clean.IsEnabled = false;
                        d1.btChange.IsEnabled = false;
                        d1.bt_Members.IsEnabled = false;
                        d1.Order_ID = DB_Work.DataSetCmd($"Insert_Bills '{room.Rooms_Name}'").Tables[0].Rows[0][0].ToString();
                        d1.tbRooms.Text = room.Rooms_Name;
                        NavigationService.Navigate(d1);
                        break;
                    case "脏房":
                        Details d2 = new Details()
                        {
                            Title = "空闲"
                        };

                        d2.btStart.IsEnabled = false;
                        d2.btEnd.IsEnabled = false;
                        d2.btChange.IsEnabled = false;
                        d2.bt_Members.IsEnabled = false;
                        d2.Order_ID = room.Order_ID;
                        d2.tbRooms.Text = room.Rooms_Name;
                        NavigationService.Navigate(d2);
                        break;
                    case "包夜":
                        Details d3 = new Details()
                        {
                            Title = "空闲"
                        };

                        d3.btStart.IsEnabled = false;
                        d3.btEnd.IsEnabled = false;
                        d3.bt_Members.IsEnabled = false;
                        d3.Order_ID = room.Order_ID;
                        d3.tbRooms.Text = room.Rooms_Name;
                        NavigationService.Navigate(d3);
                        break;
                    case "占用":
                        Details d4 = new Details()
                        {
                            Title = "占用"
                        };

                        d4.btStart.IsEnabled = false;
                        d4.bt_Clean.IsEnabled = false;
                        d4.Order_ID = room.Order_ID;
                        d4.tbRooms.Text = room.Rooms_Name;
                        NavigationService.Navigate(d4);
                        break;
                }
            }
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ShowTimer = new DispatcherTimer();
            ShowTimer.Tick += new EventHandler(ShowCurTimer);//起个Timer一直获取当前时间
            ShowTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            ShowTimer.Start();

            BtReload_Click(null, null);
        }

        private void ShowCurTimer(object sender, EventArgs e)
        {
            //"星期"+DateTime.Now.DayOfWeek.ToString(("d"))

            //获得星期几
            tb_Time.Text = DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("zh-cn"));
            tb_Time.Text += " ";
            //获得年月日
            tb_Time.Text += DateTime.Now.ToString("yyyy年MM月dd日");   //yyyy年MM月dd日
            tb_Time.Text += " ";
            //获得时分秒
            tb_Time.Text += DateTime.Now.ToString("HH:mm:ss");
            //System.Diagnostics.Debug.Print("this.ShowCurrentTime {0}", this.ShowCurrentTime);
        }

        private void BtReload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                System.Data.DataSet Ds = DB_Work.DataSetCmd("SELECT * FROM Views_Rooms ORDER BY Rooms_Name");

                rooms.Clear();

                string strLight_Static = string.Empty;
                string strClean_Static = string.Empty;
                string strMembers_Static = string.Empty;

                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToBoolean(Ds.Tables[0].Rows[i][6]))
                    {
                        strLight_Static = "/PNG/light_on_32px.png";
                    }
                    else
                    {
                        strLight_Static = "/PNG/light_off_32px.png";
                    }

                    if (Convert.ToBoolean(Ds.Tables[0].Rows[i][7]))
                    {
                        strClean_Static = "/PNG/Clean_Smlie_32px.png";
                    }
                    else
                    {
                        strClean_Static = "/PNG/Clean_Sad_32px.png";
                    }

                    if (Convert.ToBoolean(Ds.Tables[0].Rows[i][8]))
                    {
                        strMembers_Static = "/PNG/user_32px.png";
                    }
                    else
                    {
                        strMembers_Static = "/PNG/offline_user_32px.png";
                    }

                    switch (Ds.Tables[0].Rows[i][5].ToString())
                    {
                        case "空闲":
                            rooms.Add(new Room
                            {
                                Order_ID = Ds.Tables[0].Rows[i][0].ToString(),
                                Rooms_Name = Ds.Tables[0].Rows[i][1].ToString(),
                                Type = Ds.Tables[0].Rows[i][2].ToString(),
                                Start_Time = Convert.ToDateTime(Ds.Tables[0].Rows[i][3]).ToString("HH:mm"),
                                End_Time = Convert.ToDateTime(Ds.Tables[0].Rows[i][4]).ToString("HH:mm"),
                                Static = Ds.Tables[0].Rows[i][5].ToString(),
                                Expend_Time = Public.ExecDateDiff(Convert.ToDateTime(Ds.Tables[0].Rows[i][3]), Convert.ToDateTime(Ds.Tables[0].Rows[i][4])),
                                Background = "Green",
                                Light_Static = strLight_Static,
                                Clean_Static = strClean_Static,
                                Members_Static = strMembers_Static
                            });
                            break;
                        case "脏房":
                            rooms.Add(new Room
                            {
                                Order_ID = Ds.Tables[0].Rows[i][0].ToString(),
                                Rooms_Name = Ds.Tables[0].Rows[i][1].ToString(),
                                Type = Ds.Tables[0].Rows[i][2].ToString(),
                                Start_Time = Convert.ToDateTime(Ds.Tables[0].Rows[i][3]).ToString("HH:mm"),
                                End_Time = Convert.ToDateTime(Ds.Tables[0].Rows[i][4]).ToString("HH:mm"),
                                Static = Ds.Tables[0].Rows[i][5].ToString(),
                                Expend_Time = Public.ExecDateDiff(Convert.ToDateTime(Ds.Tables[0].Rows[i][3]), Convert.ToDateTime(Ds.Tables[0].Rows[i][4])),
                                Background = "Orange",
                                Light_Static = strLight_Static,
                                Clean_Static = strClean_Static,
                                Members_Static = strMembers_Static
                            });
                            break;
                        case "包夜":
                            rooms.Add(new Room
                            {
                                Order_ID = Ds.Tables[0].Rows[i][0].ToString(),
                                Rooms_Name = Ds.Tables[0].Rows[i][1].ToString(),
                                Type = Ds.Tables[0].Rows[i][2].ToString(),
                                Start_Time = Convert.ToDateTime(Ds.Tables[0].Rows[i][3]).ToString("HH:mm"),
                                End_Time = Convert.ToDateTime(Ds.Tables[0].Rows[i][4]).ToString("HH:mm"),
                                Static = Ds.Tables[0].Rows[i][5].ToString(),
                                Expend_Time = Public.ExecDateDiff(Convert.ToDateTime(Ds.Tables[0].Rows[i][3]), DateTime.Now),
                                Background = "Purple",
                                Light_Static = strLight_Static,
                                Clean_Static = strClean_Static,
                                Members_Static = strMembers_Static
                            });
                            break;
                        case "占用":
                            rooms.Add(new Room
                            {
                                Order_ID = Ds.Tables[0].Rows[i][0].ToString(),
                                Rooms_Name = Ds.Tables[0].Rows[i][1].ToString(),
                                Type = Ds.Tables[0].Rows[i][2].ToString(),
                                Start_Time = Convert.ToDateTime(Ds.Tables[0].Rows[i][3]).ToString("HH:mm"),
                                End_Time = Convert.ToDateTime(Ds.Tables[0].Rows[i][4]).ToString("HH:mm"),
                                Static = Ds.Tables[0].Rows[i][5].ToString(),
                                Expend_Time = Public.ExecDateDiff(Convert.ToDateTime(Ds.Tables[0].Rows[i][3]), DateTime.Now),
                                Background = "Blue",
                                Light_Static = strLight_Static,
                                Clean_Static = strClean_Static,
                                Members_Static = strMembers_Static
                            });
                            break;
                    }
                }

                lvRooms.ItemsSource = rooms;
            }
            catch (Exception Ex)
            {
                Public.Sys_MsgBox(Ex.Message);
            }
        }

        private void BtStatistic_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Statistic s = new Statistic();
            NavigationService.Navigate(s);
        }

        private void BtRetail_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Retail r = new Retail();
            NavigationService.Navigate(r);
        }

        private void BtMembers_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Members r = new Members();
            NavigationService.Navigate(r);
        }

        private void BtChanges_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Changes r = new Changes();
            NavigationService.Navigate(r);
        }

        private void BtPower_Swich_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Power_Switchs r = new Power_Switchs();
            NavigationService.Navigate(r);
        }
    }
}
