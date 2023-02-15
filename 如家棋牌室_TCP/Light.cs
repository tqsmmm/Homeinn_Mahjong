using System;
using System.Threading;

namespace 如家棋牌室_TCP
{
    class Light
    {
        public static bool Light_Trun_On(string Room)
        {
            try
            {
                byte[] array = Switchs.Return_Byte(Convert.ToInt32(Room), true);

                AppSetter.ntwStream.Write(array, 0, array.Length);

                Thread.Sleep(200);

                DB_Work.ExecuteCmd($"UPDATE Rooms SET Light = 1 WHERE Rooms_Name = '{Room}'");

                DB_Work.ExecuteCmd($"INSERT INTO Lights(Rooms_Name, [Type], DateTime) VALUES('{Room}', '开灯', GETDATE())");

                return true;
            }
            catch (Exception Ex)
            {
                Public.Sys_MsgBox(Ex.Message);

                return false;
            }
        }

        public static bool Light_Turn_Off(string Room)
        {
            try
            {
                byte[] array = Switchs.Return_Byte(Convert.ToInt32(Room), false);

                AppSetter.ntwStream.Write(array, 0, array.Length);

                Thread.Sleep(200);

                DB_Work.ExecuteCmd($"UPDATE Rooms SET Light = 0 WHERE Rooms_Name = '{Room}'");

                DB_Work.ExecuteCmd($"INSERT INTO Lights(Rooms_Name, [Type], DateTime) VALUES('{Room}', '关灯', GETDATE())");

                return true;
            }
            catch (Exception Ex)
            {
                Public.Sys_MsgBox(Ex.Message);

                return false;
            }
        }
    }
}
