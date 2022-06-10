//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using TradingLib.API;

//namespace TradingLib.Common
//{
//    /// <summary>
//    /// 消息级别^消息类别^消息内容 
//    /// 拼接后按SysMessage或OrderMessage消息码进行发送
//    /// </summary>
//    public class QSMessageHelper
//    {
//        /// <summary>
//        /// 分割符号,由于消息中可能会出现,!等特殊字符因此我们用上尖^进行区分和解析
//        /// </summary>
//        const char Delimiter = '^';
//        /// <summary>
//        /// 按照默认的消息错误级别将系统消息与具体原因组成消息体
//        /// </summary>
//        /// <param name="sysmsg"></param>
//        /// <param name="reason"></param>
//        /// <returns></returns>
//        public static string Message(QSEnumSysMessageType sysmsg, string reason)
//        {
//            switch (sysmsg)
//            {
//                case QSEnumSysMessageType.LOGGINFAILED:
//                case QSEnumSysMessageType.PASSCHANGEFAILED:
//                    return ((int)QSEnumMessageLevel.Error).ToString() + Delimiter + ((int)sysmsg).ToString() + Delimiter + reason;
//                default:
//                    return ((int)QSEnumMessageLevel.Info).ToString() + Delimiter + ((int)sysmsg).ToString() + Delimiter + reason;
//            }
//        }
//        /// <summary>
//        /// 由消息错误级别+系统消息主题+具体原因组成消息体
//        /// </summary>
//        /// <param name="type"></param>
//        /// <param name="sysmsg"></param>
//        /// <param name="reason"></param>
//        /// <returns></returns>
//        public static string Message(QSEnumMessageLevel type, QSEnumSysMessageType sysmsg, string reason)
//        {
//            return ((int)type).ToString() + Delimiter + ((int)sysmsg).ToString() + Delimiter + reason;
//        }


//        public static string Message(QSEnumMessageLevel type, QSEnumOrderMessageType omsg, string reason)
//        {
//            return ((int)type).ToString() + Delimiter + ((int)omsg).ToString() + Delimiter + reason;
//        }

//        public static string Message(QSEnumOrderMessageType omsg, string reason)
//        {
//            switch (omsg)
//            {
//                case QSEnumOrderMessageType.REJECT:
//                    return ((int)QSEnumMessageLevel.Error).ToString() + Delimiter + ((int)omsg).ToString() + Delimiter + reason;
//                default :
//                    return ((int)QSEnumMessageLevel.Info).ToString() + Delimiter + ((int)omsg).ToString() + Delimiter + reason;
//            }
//        }
//        /// <summary>
//        /// 有文本消息体解析成OrderMsg
//        /// </summary>
//        /// <param name="body"></param>
//        /// <returns></returns>
//        public static OrderMsg Msg2OrderMsg(string body)
//        {
//            string[] p = body.Split(Delimiter);
//            QSEnumMessageLevel t = (QSEnumMessageLevel)Enum.Parse(typeof(QSEnumMessageLevel), p[0]);
//            QSEnumOrderMessageType m = (QSEnumOrderMessageType)Enum.Parse(typeof(QSEnumOrderMessageType), p[1]);
//            string r = p[2];
//            return new OrderMsg(t, m, r,long.Parse(p[3]));
//        }

//        /// <summary>
//        /// 由文本消息体解析成SysMsg
//        /// </summary>
//        /// <param name="body"></param>
//        /// <returns></returns>
//        public static SysMsg Msg2SysMsg(string body)
//        {
//            string[] p = body.Split(Delimiter);
//            QSEnumMessageLevel t = (QSEnumMessageLevel)Enum.Parse(typeof(QSEnumMessageLevel), p[0]);
//            QSEnumSysMessageType m = (QSEnumSysMessageType)Enum.Parse(typeof(QSEnumSysMessageType), p[1]);
//            string r = p[2];
//            return new SysMsg(t, m, r);
//        }
//    }

//    /// <summary>
//    /// 委托消息解析结构
//    /// </summary>
//    public struct OrderMsg
//    {
//        public QSEnumMessageLevel Type;
//        public QSEnumOrderMessageType Message;
//        public string Reason;
//        public long OrderID;
//        public OrderMsg(QSEnumMessageLevel t, QSEnumOrderMessageType m, string r, long id)
//        {
//            Type = t;
//            Message = m;
//            Reason = r;
//            OrderID = id;
//        }
//    }

//    /// <summary>
//    /// 系统消息解析结构
//    /// </summary>
//    public struct SysMsg
//    {
//        public QSEnumMessageLevel Type;
//        public QSEnumSysMessageType Message;
//        public string Reason;
//        public SysMsg(QSEnumMessageLevel t, QSEnumSysMessageType m, string r)
//        {
//            Type = t;
//            Message = m;
//            Reason = r;
//        }
//    }
//}
