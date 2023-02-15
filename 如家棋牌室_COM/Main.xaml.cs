using System;
using System.Threading;
using System.Windows;

namespace 如家棋牌室_COM
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();

            AppSetter.SPCom1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        private void DataReceivedHandler(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (AppSetter.SPCom1.IsOpen)
            {
                
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AppSetter.SPCom1.IsOpen)
                {
                    AppSetter.SPCom1.Close();
                    AppSetter.SPCom1.Open();
                }
                else
                {
                    AppSetter.SPCom1.Open();
                }
            }
            catch (Exception Ex)
            {
                Public.Sys_MsgBox(Ex.Message);
            }

            Light.Load_Switchs();

            //ShowTimer = new DispatcherTimer();
            //ShowTimer.Tick += new EventHandler(Times_Switchs);//起个Timer一直获取当前时间
            //ShowTimer.Interval = new TimeSpan(0, 0, 0, 30, 0);
            //ShowTimer.Start();

            fMain.Source = new Uri("Rooms.xaml", UriKind.Relative);
        }

        private void Times_Switchs(object sender, EventArgs e)
        {
            if (DateTime.Now.Hour == 2 && DateTime.Now.Minute == 0)
            {
                if (AppSetter.Switchs[2])
                {
                    Light.Switch_Turn_Off(3);

                    Thread.Sleep(200);
                }
            }

            if (DateTime.Now.Hour == 7 && DateTime.Now.Minute == 30)
            {
                if (!AppSetter.Switchs[2])
                {
                    Light.Switch_Turn_On(3);

                    Thread.Sleep(200);
                }
            }

            if (DateTime.Now.Hour == 8 && DateTime.Now.Minute == 0)
            {
                Light.Reset_Swictchs("白天日常");
            }

            if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 0)
            {
                Light.Reset_Swictchs("夜间日常");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            AppSetter.SPCom1.Close();
        }
    }
}
