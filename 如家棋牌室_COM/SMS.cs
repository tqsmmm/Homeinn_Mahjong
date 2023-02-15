using System;
using System.Data;

namespace 如家棋牌室_COM
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

                    string Text = "尊敬的会员，感谢您对如家棋牌室的支持，消费积分共累计：" + Ds.Tables[0].Rows[0][1].ToString() + "。积分可兑换话费等奖品，可兑换时系统会短信方式提醒您。";

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
                string Text = $"尊敬的会员，感谢您对如家棋牌室的支持！积分已经为您兑换话费，请注意查收！当前剩余积分：{(Integral - 200).ToString()}。";

                cn.woxp.gateway.SendResult status = wsms.FastSend(strIdentity, Tel, Text, "", "");

                if (status.RetCode > 0)
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
                    string Text = $"时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}；\n" +
                        $"房费：{Ds.Tables[0].Rows[0][0].ToString()}；\n" +
                        $"数量：{Ds.Tables[0].Rows[0][1].ToString()}；\n" +
                        $"饮料：{Ds.Tables[0].Rows[0][2].ToString()}；\n" +
                        $"香烟：{Ds.Tables[0].Rows[0][3].ToString()}；\n" +
                        $"其他：{Ds.Tables[0].Rows[0][4].ToString()}；\n" +
                        $"总计：{Ds.Tables[0].Rows[0][5].ToString()}；\n" +
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
