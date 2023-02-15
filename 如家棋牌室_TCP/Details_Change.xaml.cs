using System.Collections.ObjectModel;
using System.Windows;

namespace 如家棋牌室_TCP
{
    /// <summary>
    /// Change.xaml 的交互逻辑
    /// </summary>
    public partial class Details_Change : Window
    {
        public Details_Change()
        {
            InitializeComponent();
        }

        ObservableCollection<Room> Rooms = new ObservableCollection<Room>();

        public string Rooms_Name = string.Empty;

        public class Room
        {
            public string Name { get; set; }
            public string Background { get; set; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Data.DataSet Ds = DB_Work.DataSetCmd("SELECT Rooms_Name FROM Rooms WHERE Static = '空闲' ORDER BY Rooms_Name");

            Rooms.Clear();

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                Rooms.Add(new Room
                {
                    Name = Ds.Tables[0].Rows[i][0].ToString(),
                });
            }

            lvRooms.ItemsSource = Rooms;
        }

        private void LvItems_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Room t = (Room)lvRooms.SelectedItem;
            Rooms_Name = t.Name;

            DialogResult = true;
            Close();
        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
