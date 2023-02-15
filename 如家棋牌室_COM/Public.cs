using System;
using System.Windows;

namespace 如家棋牌室_COM
{
    class Public
    {
        public static void Sys_MsgBox(string strMsg)
        {
            MessageBox.Show(strMsg, AppSetter.strApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static bool Sys_MsgYN(string strMsg)
        {
            if (MessageBox.Show(strMsg, AppSetter.strApplicationName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string ExecDateDiff(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            //你想转的格式
            return ts3.ToString("c").Substring(0, 5);
        }
    }
}