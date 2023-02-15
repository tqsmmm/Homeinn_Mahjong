using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace 如家棋牌室_COM
{
    /// <summary>
    /// Integrals.xaml 的交互逻辑
    /// </summary>
    public partial class Integrals : Page
    {
        public Integrals()
        {
            InitializeComponent();
        }

        ObservableCollection<Integral> integrals = new ObservableCollection<Integral>();

        ObservableCollection<Member> members = new ObservableCollection<Member>();

        public DataSet Ds_Exchange = null;

        public class Integral
        {
            public string Tel { get; set; }
            public string DateTime { get; set; }
            public decimal Integral_Before { get; set; }
            public decimal Integral_After { get; set; }
            public string Type { get; set; }
        }

        public class Member
        {
            public string Tel { get; set; }
            public decimal Integral { get; set; }
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
            DataSet Ds_Integrals = DB_Work.DataSetCmd("SELECT TOP 20 * FROM Views_Integrals ORDER BY 时间 DESC");

            integrals.Clear();

            for (int i = 0; i < Ds_Integrals.Tables[0].Rows.Count; i++)
            {
                integrals.Add(new Integral
                {
                    Tel = Ds_Integrals.Tables[0].Rows[i][0].ToString(),
                    DateTime = Ds_Integrals.Tables[0].Rows[i][1].ToString(),
                    Integral_Before = Convert.ToDecimal(Ds_Integrals.Tables[0].Rows[i][2]),
                    Integral_After = Convert.ToDecimal(Ds_Integrals.Tables[0].Rows[i][3]),
                    Type = Ds_Integrals.Tables[0].Rows[i][4].ToString()
                });
            }

            lvIntegrals.ItemsSource = integrals;

            Ds_Exchange = DB_Work.DataSetCmd("SELECT * FROM Views_Exchange ORDER BY 积分");

            members.Clear();

            for (int i = 0; i < Ds_Exchange.Tables[0].Rows.Count; i++)
            {
                members.Add(new Member
                {
                    Tel = Ds_Exchange.Tables[0].Rows[i][0].ToString(),
                    Integral = Convert.ToDecimal(Ds_Exchange.Tables[0].Rows[i][1]),
                    Last_Date = Convert.ToDateTime(Ds_Exchange.Tables[0].Rows[i][2]).ToString("yyyy-MM-dd HH:mm:ss"),
                    Last_Expend = Convert.ToDecimal(Ds_Exchange.Tables[0].Rows[i][3])
                });
            }

            lvExchange.ItemsSource = members;

            if (members.Count > 0)
            {
                Bt_Exchange.IsEnabled = true;
            }
        }

        private void Bt_Exchange_Click(object sender, RoutedEventArgs e)
        {
            if (Public.Sys_MsgYN("是否确定兑换积分？"))
            {
                for (int i = 0; i < Ds_Exchange.Tables[0].Rows.Count; i++)
                {
                    if (SMS.CheckGate())
                    {
                        SMS.Send_Exchange(Ds_Exchange.Tables[0].Rows[i][0].ToString(), Convert.ToInt16(Ds_Exchange.Tables[0].Rows[i][1]));

                        Thread.Sleep(200);
                    }
                }

                Bt_Close_Click(null, null);
            }
        }
    }
}
