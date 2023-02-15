using System;
using System.Windows;

namespace 如家棋牌室_TCP
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //AppSetter.tcpClient.Connect(AppSetter.strIP, AppSetter.intPort);

                //AppSetter.ntwStream = AppSetter.tcpClient.GetStream();

                //ShowTimer = new DispatcherTimer();
                //ShowTimer.Tick += new EventHandler(Times_Switchs);//起个Timer一直获取当前时间
                //ShowTimer.Interval = new TimeSpan(0, 0, 0, 30, 0);
                //ShowTimer.Start();

                //fMain.Source = new Uri("Rooms.xaml", UriKind.Relative);

                fMain.Source = new Uri("Lock.xaml", UriKind.Relative);
            }
            catch (Exception Ex)
            {
                Public.Sys_MsgBox(Ex.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                //AppSetter.ntwStream.Close();

                //AppSetter.tcpClient.Close();
            }
            catch
            {

            }
        }
    }
}
