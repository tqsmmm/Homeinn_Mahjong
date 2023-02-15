using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace 如家棋牌室_COM
{
    /// <summary>
    /// Statistic_Detail.xaml 的交互逻辑
    /// </summary>
    public partial class Statistic_Detail : Window
    {
        public Statistic_Detail()
        {
            InitializeComponent();
        }

        ObservableCollection<Employee> Employees = new ObservableCollection<Employee>();

        public class Employee
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public BitmapImage Image { get; set; }
        }

        private void BtOk_Click(object sender, RoutedEventArgs e)
        {
            Employee t = (Employee)lvEmployees.SelectedItem;

            Prints.Print_Settlement(t.Name);

            //SMS.Send_Settlement(t.Name);

            if (DB_Work.ExecuteCmd($"UPDATE Bills SET Checked = 1, Settlement_Employees = {t.Code}, Settlement_Date = GETDATE() WHERE Checked IS NULL AND Type IS NOT NULL"))
            {
                DialogResult = true;

                Bt_Close_Click(null, null);
            }
        }

        private void Bt_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void lvEmployees_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lvEmployees.SelectedItems.Count > 0)
            {
                bt_Ok.IsEnabled = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Data.DataSet Ds = DB_Work.DataSetCmd("SELECT id, Code, Name, Image FROM Employees WHERE Visual = 1");

            Employees.Clear();

            byte[] br = null;

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                if (Ds.Tables[0].Rows[i][3] == DBNull.Value)
                {
                    Employees.Add(new Employee
                    {
                        Id = Convert.ToInt32(Ds.Tables[0].Rows[i][0]),
                        Code = Ds.Tables[0].Rows[i][1].ToString(),
                        Name = Ds.Tables[0].Rows[i][2].ToString(),
                        Image = null
                    });
                }
                else
                {
                    br = (byte[])(Ds.Tables[0].Rows[i][3]);

                    BitmapImage img = ImageConverter.ByteArrayToBitmapImage(br);

                    Employees.Add(new Employee
                    {
                        Id = Convert.ToInt32(Ds.Tables[0].Rows[i][0]),
                        Code = Ds.Tables[0].Rows[i][1].ToString(),
                        Name = Ds.Tables[0].Rows[i][2].ToString(),
                        Image = img
                    });
                }
            }

            lvEmployees.ItemsSource = Employees;
        }
    }
}
