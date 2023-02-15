using System.Data.SqlClient;
using System.IO.Ports;

namespace 如家棋牌室_COM
{
    class AppSetter
    {
        public static string strApplicationName = "如家棋牌室";

        public static SqlConnection SqlConn = new SqlConnection($"Server={"192.168.49.100"},{"1433"};Database={"Home-Major"};Uid={"sa"};Pwd={"23long"};Persist Security Info=True");

        public static SerialPort SPCom1 = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);

        public static string Printing_Name = "XP-80";

        public static bool[] Switchs = new bool[16];
    }
}