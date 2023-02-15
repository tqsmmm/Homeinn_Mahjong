using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;

namespace 如家棋牌室_TCP
{
    class Prints
    {
        public class Bill
        {
            public string Order_ID { get; set; }
            public string Rooms_Name { get; set; }
            public string Type { get; set; }
            public decimal Total { get; set; }
            public string Start_Time { get; set; }
            public string End_Time { get; set; }
            public string Time { get; set; }
        }

        public class Settlement
        {
            public decimal Total_Rooms { get; set; }
            public int Num_Rooms { get; set; }
            public decimal Total_Drink { get; set; }
            public decimal Total_Cigarette { get; set; }
            public decimal Total_Other { get; set; }
            public decimal Total { get; set; }
            public string Employees { get; set; }
        }

        private static readonly Bill Bills = new Bill();

        private static Settlement Settlements = new Settlement();

        private static DataSet Ds_Bills = new DataSet();
        private static DataSet Ds_Bills_Detail = new DataSet();

        public static void Print_Settlement_Inventory()
        {
            Ds_Bills_Detail = new DataSet();
            Ds_Bills_Detail = DB_Work.DataSetCmd("SELECT * FROM Views_Statistic_Bills_Detail ORDER BY 类型 DESC, 名称");

            PrintDocument Print_Settlement_Inventory = new PrintDocument();
            Print_Settlement_Inventory.PrinterSettings.PrinterName = AppSetter.Printing_Name;
            Print_Settlement_Inventory.PrintPage += new PrintPageEventHandler(Print_Settlement_Inventory_PrintPage);
            Print_Settlement_Inventory.Print();
        }

        public static void Print_Settlement(string strEmployees)
        {
            try
            {
                DataSet Ds = DB_Work.DataSetCmd("Prints_Settlement");

                if (Ds.Tables[0].Rows.Count > 0)
                {
                    Settlements = new Settlement()
                    {
                        Total_Rooms = Convert.ToDecimal(Ds.Tables[0].Rows[0][0]),
                        Num_Rooms = Convert.ToInt16(Ds.Tables[0].Rows[0][1]),
                        Total_Drink = Convert.ToDecimal(Ds.Tables[0].Rows[0][2]),
                        Total_Cigarette = Convert.ToDecimal(Ds.Tables[0].Rows[0][3]),
                        Total_Other = Convert.ToDecimal(Ds.Tables[0].Rows[0][4]),
                        Total = Convert.ToDecimal(Ds.Tables[0].Rows[0][5]),
                        Employees = strEmployees
                    };

                    Ds_Bills = new DataSet();
                    Ds_Bills = DB_Work.DataSetCmd("SELECT * FROM Views_Statistic_Bills ORDER BY 订单");

                    Print_Settlement_Inventory();

                    PrintDocument Printing_Summary = new PrintDocument();
                    Printing_Summary.PrinterSettings.PrinterName = AppSetter.Printing_Name;
                    Printing_Summary.PrintPage += new PrintPageEventHandler(Print_Settlement_Summary_PrintPage);
                    Printing_Summary.Print();

                    PrintDocument Printing_Detail = new PrintDocument();
                    Printing_Detail.PrinterSettings.PrinterName = AppSetter.Printing_Name;
                    Printing_Detail.PrintPage += new PrintPageEventHandler(Print_Settlement_Detail_PrintPage);
                    Printing_Detail.Print();
                }
            }
            catch (Exception Ex)
            {
                Public.Sys_MsgBox(Ex.Message);
            }
        }

        private static void Print_Settlement_Inventory_PrintPage(object sender, PrintPageEventArgs e)
        {
            var pen = new Pen(Brushes.Black, 1);

            e.Graphics.DrawString("每日库存单", new Font("微软雅黑", 18, FontStyle.Bold), Brushes.Black, 70, 0);

            e.Graphics.DrawLine(pen, new Point(0, 40), new Point(300, 40));

            var intX = 40;

            for (var i = 0; i < Ds_Bills_Detail.Tables[0].Rows.Count; i++)
            {
                e.Graphics.DrawString(Ds_Bills_Detail.Tables[0].Rows[i][0].ToString(), new Font("微软雅黑", 10), Brushes.Black, 0, intX);

                intX += 22;

                e.Graphics.DrawLine(pen, new Point(0, intX), new Point(300, intX));
            }

            e.Graphics.DrawLine(pen, new Point(0, intX), new Point(300, intX));

            e.Graphics.DrawLine(pen, new Point(0, intX + 500), new Point(300, intX + 500));
        }

        private static void Print_Settlement_Summary_PrintPage(object sender, PrintPageEventArgs e)
        {
            var pen = new Pen(Brushes.Black, 1);

            e.Graphics.DrawString("每日结算单", new Font("微软雅黑", 18, FontStyle.Bold), Brushes.Black, 70, 0);

            e.Graphics.DrawLine(pen, new Point(0, 40), new Point(300, 40));

            var intX = 40;

            for (var i = 0; i < Ds_Bills_Detail.Tables[0].Rows.Count; i++)
            {
                e.Graphics.DrawString(Ds_Bills_Detail.Tables[0].Rows[i][0].ToString(), new Font("微软雅黑", 10), Brushes.Black, 0, intX);
                e.Graphics.DrawString(Ds_Bills_Detail.Tables[0].Rows[i][1].ToString(), new Font("微软雅黑", 10), Brushes.Black, 80, intX);
                e.Graphics.DrawString(Ds_Bills_Detail.Tables[0].Rows[i][2].ToString(), new Font("微软雅黑", 10, FontStyle.Underline), Brushes.Black, 130, intX);
                e.Graphics.DrawString(Ds_Bills_Detail.Tables[0].Rows[i][3].ToString(), new Font("微软雅黑", 10), Brushes.Black, 170, intX);

                intX += 22;

                e.Graphics.DrawLine(pen, new Point(0, intX), new Point(300, intX));
            }

            e.Graphics.DrawLine(pen, new Point(0, intX), new Point(300, intX));

            e.Graphics.DrawString("房费：" + Settlements.Total_Rooms, new Font("微软雅黑", 12), Brushes.Black, 0, intX);
            e.Graphics.DrawString("数量：" + Settlements.Num_Rooms, new Font("微软雅黑", 12), Brushes.Black, 140, intX);
            e.Graphics.DrawString("饮料：" + Settlements.Total_Drink, new Font("微软雅黑", 12), Brushes.Black, 0, intX + 25);
            e.Graphics.DrawString("香烟：" + Settlements.Total_Cigarette, new Font("微软雅黑", 12), Brushes.Black, 140, intX + 25);
            e.Graphics.DrawString("其他：" + Settlements.Total_Other, new Font("微软雅黑", 12), Brushes.Black, 0, intX + 50);
            e.Graphics.DrawString("总计：" + Settlements.Total, new Font("微软雅黑", 12), Brushes.Black, 140, intX + 50);

            e.Graphics.DrawLine(pen, new Point(0, intX + 75), new Point(300, intX + 75));

            e.Graphics.DrawString("微信：", new Font("微软雅黑", 12), Brushes.Black, 0, intX + 75);
            e.Graphics.DrawString("支付宝：", new Font("微软雅黑", 12), Brushes.Black, 140, intX + 75);
            e.Graphics.DrawString("支出：", new Font("微软雅黑", 12), Brushes.Black, 0, intX + 100);
            e.Graphics.DrawString("剩余：", new Font("微软雅黑", 12), Brushes.Black, 140, intX + 100);

            e.Graphics.DrawLine(pen, new Point(0, intX + 125), new Point(300, intX + 125));

            e.Graphics.DrawString("昨底钱：", new Font("微软雅黑", 12), Brushes.Black, 0, intX + 125);
            e.Graphics.DrawString("今底钱：", new Font("微软雅黑", 12), Brushes.Black, 140, intX + 125);

            e.Graphics.DrawLine(pen, new Point(0, intX + 150), new Point(300, intX + 150));

            e.Graphics.DrawString("结算：" + Settlements.Employees, new Font("微软雅黑", 12), Brushes.Black, 140, intX + 150);

            e.Graphics.DrawLine(pen, new Point(0, intX + 500), new Point(300, intX + 500));
        }

        private static void Print_Settlement_Detail_PrintPage(object sender, PrintPageEventArgs e)
        {
            var pen = new Pen(Brushes.Black, 1);

            e.Graphics.DrawString("房费明细单", new Font("微软雅黑", 18, FontStyle.Bold), Brushes.Black, 70, 0);

            e.Graphics.DrawLine(pen, new Point(0, 40), new Point(300, 40));

            var intX = 40;

            for (var i = 0; i < Ds_Bills.Tables[0].Rows.Count; i++)
            {
                e.Graphics.DrawString("客单：" + Ds_Bills.Tables[0].Rows[i][0].ToString(), new Font("微软雅黑", 10), Brushes.Black, 0, intX);
                intX += 20;

                e.Graphics.DrawString("房间：" + Ds_Bills.Tables[0].Rows[i][1].ToString(), new Font("微软雅黑", 10), Brushes.Black, 0, intX);
                e.Graphics.DrawString("类型：" + Ds_Bills.Tables[0].Rows[i][2].ToString(), new Font("微软雅黑", 10), Brushes.Black, 150, intX);
                intX += 20;

                e.Graphics.DrawString("开始：" + Convert.ToDateTime(Ds_Bills.Tables[0].Rows[i][5]).ToString("MM-dd HH:mm"), new Font("微软雅黑", 10), Brushes.Black, 0, intX);
                e.Graphics.DrawString("结束：" + Convert.ToDateTime(Ds_Bills.Tables[0].Rows[i][6]).ToString("MM-dd HH:mm"), new Font("微软雅黑", 10), Brushes.Black, 150, intX);
                intX += 20;

                e.Graphics.DrawString("时间：" + Ds_Bills.Tables[0].Rows[i][7].ToString(), new Font("微软雅黑", 10), Brushes.Black, 0, intX);
                e.Graphics.DrawString("消费：" + Ds_Bills.Tables[0].Rows[i][4].ToString(), new Font("微软雅黑", 10), Brushes.Black, 150, intX);
                intX += 20;

                e.Graphics.DrawLine(pen, new Point(0, intX), new Point(300, intX));
            }
        }
    }
}
