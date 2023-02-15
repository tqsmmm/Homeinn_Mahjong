using System.Data.SqlClient;
using System.Net.Sockets;

namespace 如家棋牌室_TCP
{
    class AppSetter
    {
        public static string strApplicationName = "如家棋牌室";

        public static SqlConnection SqlConn = new SqlConnection($"Server={"59.47.78.198"},{"1433"};Database={"Home-Major"};Uid={"sa"};Pwd={"23long"};Persist Security Info=True");

        public static TcpClient tcpClient = new TcpClient();

        public static NetworkStream ntwStream;

        public static string strIP = "192.168.49.252";
        public static int intPort = 8000;

        public static string Printing_Name = "XP-80";
    }
}