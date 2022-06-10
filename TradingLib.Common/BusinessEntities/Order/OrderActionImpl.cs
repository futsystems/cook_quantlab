using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    public class OrderActionImpl : OrderAction
    {
        /// <summary>
        /// 交易帐号
        /// </summary>
        public string Account { get; set; }


        /// <summary>
        /// 委托操作标识
        /// </summary>
        public QSEnumOrderActionFlag ActionFlag { get; set; }



        /// <summary>
        /// 服务端委托唯一编号
        /// </summary>
        public long OrderID { get; set; }

        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID { get; set; }

        /// <summary>
        /// 会话编号,每次建立的会话都有一个唯一的SessionID分配给客户端,同时客户端还绑有UUID用于通讯寻址
        /// 通过组合SessionID和OrderRef就可以定位某个委托
        /// 或者通过ExchOrderID来进行唯一定位
        /// </summary>
        public int SessionID { get; set; }

        /// <summary>
        /// 客户端委托引用
        /// </summary>
        public string OrderRef { get; set; }

        /// <summary>
        /// 交易所编号
        /// </summary>
        public string Exchagne { get; set; }

        /// <summary>
        /// 交易所委托编号类似于CTP的OrderSysID
        /// </summary>
        public string OrderExchID { get; set; }

        /// <summary>
        /// 合约
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 请求 ID
        /// </summary>
        public int RequestID { get; set; }

        public static string Serialize(OrderAction action)
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(action.Account);
            sb.Append(d);
            sb.Append(action.ActionFlag.ToString());
            sb.Append(d);
            sb.Append(action.OrderID.ToString());
            sb.Append(d);
            sb.Append(action.FrontID.ToString());
            sb.Append(d);
            sb.Append(action.SessionID.ToString());
            sb.Append(d);
            sb.Append(action.OrderRef);
            sb.Append(d);
            sb.Append(action.Exchagne);
            sb.Append(d);
            sb.Append(action.OrderExchID);
            sb.Append(d);
            sb.Append(action.Symbol);
            sb.Append(d);
            sb.Append(action.RequestID);
            return sb.ToString();
        }

        public static OrderAction Deserialize(string message)
        {
            string[] rec = message.Split(',');
            OrderAction action = new OrderActionImpl();
            action.Account = rec[0];
            action.ActionFlag = (QSEnumOrderActionFlag)Enum.Parse(typeof(QSEnumOrderActionFlag), rec[1]);
            action.OrderID = long.Parse(rec[2]);
            action.FrontID = int.Parse(rec[3]);
            action.SessionID = int.Parse(rec[4]);
            action.OrderRef = rec[5];
            action.Exchagne = rec[6];
            action.OrderExchID = rec[7];
            action.Symbol = rec[8];
            if (rec.Length > 9)
            {
                action.RequestID = int.Parse(rec[9]);
            }
            return action;
        }
    }
}
