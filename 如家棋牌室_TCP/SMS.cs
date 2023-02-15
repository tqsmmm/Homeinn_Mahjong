using System;
using System.Data;

namespace 如家棋牌室_TCP
{
    class SMS
    {
        private static cn.woxp.gateway.WebSMS wsms = new cn.woxp.gateway.WebSMS();

        private static string strIdentity = "";

        public static int eid = 12002;
        public static int gateid = 300;
        public static string uid = "tqsmmm";
        public static string upwd = "123456";

        public static void Send_SMS(string strOrder_ID)
        {
            try
            {
                var Ds = DB_Work.DataSetCmd($"SELECT Members, ISNULL(Members.Integral, 0) FROM Bills LEFT JOIN Members ON Members.Tel = Bills.Members WHERE Order_ID = '{strOrder_ID}'");

                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string Tel = Ds.Tables[0].Rows[0][0].ToString();

                    string Text = "尊敬的会员，感谢您对如家棋牌室的支持！消费积分共累计：" + Ds.Tables[0].Rows[0][1].ToString() + "。消费积分返微信红包，请加微信：17896229002";

                    cn.woxp.gateway.SendResult status = wsms.FastSend(strIdentity, Tel, Text, "", "");

                    if (status.RetCode > 0)
                    {
                        DB_Work.ExecuteCmd($"UPDATE Bills SET SMS = 1 WHERE Order_ID = '{strOrder_ID}'");
                    }
                }
            }
            catch
            {

            }
        }

        public static void Send_Exchange(string Tel, int Integral)
        {
            try
            {
                string Text = $"尊敬的会员，感谢您对如家棋牌室的支持！积分已经以微信红包形式发送给您，请尽快添加我的微信。当前剩余积分：{Integral - 200}。";

                cn.woxp.gateway.SendResult status = wsms.FastSend(strIdentity, Tel, Text, "", "");

                if (status.RetCode > 0)
                {
                    DB_Work.ExecuteCmd($"UPDATE Members SET Integral = Integral - 200 WHERE Tel = '{Tel}'");

                    DB_Work.ExecuteCmd($"INSERT INTO Integrals(Order_ID, DateTime, Tel, [Before], [After], Type) VALUES ('', GETDATE(), '{Tel}', {Integral}, {Integral - 200}, '兑换')");
                }
                else
                {
                    DB_Work.ExecuteCmd($"UPDATE Members SET Integral = Integral - 200 WHERE Tel = '{Tel}'");

                    DB_Work.ExecuteCmd($"INSERT INTO Integrals(Order_ID, DateTime, Tel, [Before], [After], Type) VALUES ('', GETDATE(), '{Tel}', {Integral}, {Integral - 200}, '兑换')");
                }
            }
            catch
            {

            }
        }

        public static void Send_Settlement(string Employees)
        {
            try
            {
                DataSet Ds = DB_Work.DataSetCmd("Prints_Settlement");

                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string Text = $"时间：{DateTime.Now:yyyy-MM-dd HH:mm}；\n" +
                        $"房费：{Ds.Tables[0].Rows[0][0]}；\n" +
                        $"数量：{Ds.Tables[0].Rows[0][1]}；\n" +
                        $"饮料：{Ds.Tables[0].Rows[0][2]}；\n" +
                        $"香烟：{Ds.Tables[0].Rows[0][3]}；\n" +
                        $"其他：{Ds.Tables[0].Rows[0][4]}；\n" +
                        $"总计：{Ds.Tables[0].Rows[0][5]}；\n" +
                        $"员工：{Employees}；\n";

                    cn.woxp.gateway.SendResult status = wsms.FastSendLongSMS(strIdentity, "13591240016", Text, "", "");

                    if (status.RetCode > 0)
                    {

                    }
                }
            }
            catch (Exception Ex)
            {
                Public.Sys_MsgBox(Ex.Message);
            }
        }

        public static bool CheckGate()
        {
            try
            {
                strIdentity = wsms.GetIdentityMark(eid, uid, upwd, gateid);

                if (strIdentity == null || strIdentity == "")
                {
                    Public.Sys_MsgBox("获取身份标识串失败!");
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
