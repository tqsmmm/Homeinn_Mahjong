using System;
using System.Threading;

namespace 如家棋牌室_COM
{
    class Light
    {
        public static bool Light_Trun_On(string Room)
        {
            try
            {
                byte[] array = Switchs.Return_Byte(1, Convert.ToInt32(Room), true);

                AppSetter.SPCom1.Write(array, 0, array.Length);

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
                byte[] array = Switchs.Return_Byte(1, Convert.ToInt32(Room), false);

                AppSetter.SPCom1.Write(array, 0, array.Length);

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

        public static bool Switch_Turn_On(int Num)
        {
            try
            {
                byte[] array = Switchs.Return_Byte(2, Num, true);

                AppSetter.SPCom1.Write(array, 0, array.Length);

                Thread.Sleep(200);

                AppSetter.Switchs[Num - 1] = true;

                DB_Work.ExecuteCmd($"UPDATE Power_Switchs SET Static = 1 WHERE Num = {Num}");

                return true;
            }
            catch (Exception Ex)
            {
                Public.Sys_MsgBox(Ex.Message);

                return false;
            }
        }

        public static bool Switch_Turn_Off(int Num)
        {
            try
            {
                byte[] array = Switchs.Return_Byte(2, Num, false);

                AppSetter.SPCom1.Write(array, 0, array.Length);

                Thread.Sleep(200);

                AppSetter.Switchs[Num - 1] = false;

                DB_Work.ExecuteCmd($"UPDATE Power_Switchs SET Static = 0 WHERE Num = {Num}");

                return true;
            }
            catch (Exception Ex)
            {
                Public.Sys_MsgBox(Ex.Message);

                return false;
            }
        }

        public static void Load_Switchs()
        {
            System.Data.DataSet Ds = DB_Work.DataSetCmd("SELECT Static FROM Power_Switchs ORDER BY Num");

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                AppSetter.Switchs[i] = Convert.ToBoolean(Ds.Tables[0].Rows[i][0]);
            }
        }

        private static int Load_Lights()
        {
            int intLight = 0;

            System.Data.DataSet Ds = DB_Work.DataSetCmd("SELECT COUNT(*) FROM Rooms WHERE Light = 1");

            intLight = Convert.ToInt32(Ds.Tables[0].Rows[0][0]);

            return intLight;
        }

        public static void Reset_Swictchs(string strSwitch)
        {

            switch (strSwitch)
            {
                case "开关全开":
                    if (!AppSetter.Switchs[0])
                    {
                        Switch_Turn_On(1);

                        Thread.Sleep(200);
                    }

                    if (!AppSetter.Switchs[1])
                    {
                        Switch_Turn_On(2);

                        Thread.Sleep(200);
                    }

                    if (!AppSetter.Switchs[3])
                    {
                        Switch_Turn_On(4);

                        Thread.Sleep(200);
                    }
                    break;
                case "白天日常":
                    if (!AppSetter.Switchs[0])
                    {
                        Switch_Turn_On(1);

                        Thread.Sleep(200);
                    }

                    if (!AppSetter.Switchs[1])
                    {
                        Switch_Turn_On(2);

                        Thread.Sleep(200);
                    }

                    if (AppSetter.Switchs[3])
                    {
                        Switch_Turn_Off(4);

                        Thread.Sleep(200);
                    }
                    break;
                case "夜间日常":
                    if (!AppSetter.Switchs[3])
                    {
                        Switch_Turn_On(4);

                        Thread.Sleep(200);
                    }

                    if (AppSetter.Switchs[0])
                    {
                        Switch_Turn_Off(1);

                        Thread.Sleep(200);
                    }

                    if (AppSetter.Switchs[1])
                    {
                        Switch_Turn_Off(2);

                        Thread.Sleep(200);
                    }
                    break;
            }
        }
    }
}
