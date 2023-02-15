using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace 如家棋牌室_COM
{
    /// <summary>
    /// Power_Switchs.xaml 的交互逻辑
    /// </summary>
    public partial class Power_Switchs : Page
    {
        ObservableCollection<Room> rooms = new ObservableCollection<Room>();
        ObservableCollection<Switch> switchs = new ObservableCollection<Switch>();

        public class Room
        {
            public string Name { get; set; }
            public bool Static { get; set; }
            public string Background { get; set; }
        }

        public class Switch
        {
            public int Num { get; set; }
            public string Name { get; set; }
            public bool Static { get; set; }
            public string Background { get; set; }
        }

        public Power_Switchs()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            BtReload_Click(null, null);
        }

        private void Bt_Close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Rooms r = new Rooms();
            NavigationService.Navigate(r);
        }

        private void LvRooms_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvRooms.SelectedItems.Count > 0)
            {
                Room r = (Room)lvRooms.SelectedItem;

                if (r.Static)
                {
                    Light.Light_Turn_Off(r.Name);
                }
                else
                {
                    Light.Light_Trun_On(r.Name);
                }

                List_Rooms();
            }
        }

        private void LvSwitchs_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvSwitchs.SelectedItems.Count > 0)
            {
                Switch s = (Switch)lvSwitchs.SelectedItem;

                if (s.Static)
                {
                    Light.Switch_Turn_Off(s.Num);
                }
                else
                {
                    Light.Switch_Turn_On(s.Num);
                }

                List_Switchs();
            }
        }

        private void BtReload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            List_Rooms();

            List_Switchs();
        }

        private void List_Rooms()
        {
            System.Data.DataSet Ds = DB_Work.DataSetCmd("SELECT TOP 16 Rooms_Name, Light FROM Rooms ORDER BY Rooms_Name");

            rooms.Clear();

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                if (Convert.ToBoolean(Ds.Tables[0].Rows[i][1]))
                {
                    rooms.Add(new Room
                    {
                        Name = Ds.Tables[0].Rows[i][0].ToString(),
                        Static = true,
                        Background = "Blue"
                    });
                }
                else
                {
                    rooms.Add(new Room
                    {
                        Name = Ds.Tables[0].Rows[i][0].ToString(),
                        Static = false,
                        Background = "Green"
                    });
                }
            }

            lvRooms.ItemsSource = rooms;
        }

        private void List_Switchs()
        {
            System.Data.DataSet Ds = DB_Work.DataSetCmd("SELECT TOP 12 Num, Name, Static FROM Power_Switchs ORDER BY Num");

            switchs.Clear();

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                if (Convert.ToBoolean(Ds.Tables[0].Rows[i][2]))
                {
                    switchs.Add(new Switch
                    {
                        Num = Convert.ToInt32(Ds.Tables[0].Rows[i][0]),
                        Name = Ds.Tables[0].Rows[i][1].ToString(),
                        Static = true,
                        Background = "Blue"
                    });
                }
                else
                {
                    switchs.Add(new Switch
                    {
                        Num = Convert.ToInt32(Ds.Tables[0].Rows[i][0]),
                        Name = Ds.Tables[0].Rows[i][1].ToString(),
                        Static = false,
                        Background = "Green"
                    });
                }
            }

            lvSwitchs.ItemsSource = switchs;
        }
    }
}
