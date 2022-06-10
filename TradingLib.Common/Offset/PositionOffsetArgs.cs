// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using TradingLib.API;
//
// namespace TradingLib.Common
// {
//     /// <summary>
//     /// 持仓止盈与止损,用于向服务端提交止损与止盈参数,从而实现服务端的止损与止盈
//     /// </summary>
//     public class PositionOffsetArg
//     {
//         public PositionOffsetArg(string account, string symbol,bool side,QSEnumPositionOffsetDirection direction)
//         {
//             _account = account;
//             _symbol = symbol;
//             _direction = direction;
//             _side = side;
//             Enable = false;
//             OffsetType = QSEnumPositionOffsetType.POINTS;
//             Value = 0;
//             Start = 0;
//             Size = 0;
//             Fired = false;
//             FlatOrderRefList = new List<string>();
//         }
//
//         
//
//         string _account;
//         /// <summary>
//         /// 该监控所对应的账户
//         /// </summary>
//         public string Account { get { return _account; } }
//
//         string _symbol;
//         /// <summary>
//         /// 该监控的合约
//         /// </summary>
//         public string Symbol { get { return _symbol; } }
//
//         bool _side = false;
//         public bool Side { get { return _side; } }
//
//         QSEnumPositionOffsetDirection _direction;
//         /// <summary>
//         /// 止盈还是止损标识
//         /// </summary>
//         public QSEnumPositionOffsetDirection Direction { get { return _direction; } }
//
//         /// <summary>
//         /// 止盈是否有效
//         /// </summary>
//         public bool Enable { get; set; }
//
//         /// <summary>
//         /// 止盈方式
//         /// </summary>
//         public QSEnumPositionOffsetType OffsetType { get; set; }
//
//         /// <summary>
//         /// 止盈值
//         /// </summary>
//         public decimal Value { get; set; }
//
//         /// <summary>
//         /// 跟踪止盈的启动值
//         /// </summary>
//         public decimal Start { get; set; }
//
//         /// <summary>
//         /// 止盈手数
//         /// </summary>
//         public int Size { get; set; }
//
//
//         /// <summary>
//         /// 是否已经触发
//         /// </summary>
//         public bool Fired { get; set; }
//
//         /// <summary>
//         /// 强平触发时间
//         /// 用于强平异常后 重新执行强平操作
//         /// </summary>
//         public DateTime SentTime { get; set; }
//
//         /// <summary>
//         /// 止盈止损触发委托ID
//         /// 用于监控对应ID委托状态
//         /// </summary>
//         public List<string> FlatOrderRefList { get; set; }
//
//
//
//         /// <summary>
//         /// 用一个止盈止损参数来更新当前止盈止损参数
//         /// </summary>
//         /// <param name="args"></param>
//         public void UpdateArgs(PositionOffsetArg args)
//         {
//             //account symbol direction相同的情况下才可以传递参数
//             if((Account == args.Account) && (Symbol == args.Symbol) &&(Direction == args.Direction))
//             {
//                 Enable = args.Enable;
//                 OffsetType = args.OffsetType;
//                 Value = args.Value;
//                 Start = args.Start;
//                 Size = args.Size;
//             }
//         }
//
//
//         /// <summary>
//         /// 计算止损价格
//         /// </summary>
//         /// <param name="LossArgs"></param>
//         /// <param name="pos"></param>
//         /// <returns></returns>
//         public decimal CaculateLossTakePrice(Position pos)
//         {
//             if (pos == null) return -1;
//             decimal hitprice = -1;
//             switch (this.OffsetType)
//             {
//                 case QSEnumPositionOffsetType.POINTS:
//                     hitprice = pos.isLong ? (pos.AvgPrice - this.Value) : (pos.AvgPrice + this.Value);
//                     break;
//                 case QSEnumPositionOffsetType.PRICE:
//                     hitprice = this.Value;
//                     break;
//                 case QSEnumPositionOffsetType.PERCENT:
//                     hitprice = pos.isLong ? (pos.AvgPrice * (1 - this.Value / 100)) : (pos.AvgPrice * (1 + this.Value / 100));
//                     break;
//                 case QSEnumPositionOffsetType.TRAILING:
//                     {
//                         if (pos.isLong && pos.AvgPrice > 0)
//                         {
//                             if (pos.Highest > pos.AvgPrice + this.Start)
//                                 hitprice = pos.Highest - this.Value;
//                         }
//                         if (pos.isShort && pos.AvgPrice > 0)
//                         {
//                             if (pos.Lowest < pos.AvgPrice - this.Start)
//                                 hitprice = pos.Lowest + this.Value;
//                         }
//                     }
//                     break;
//                 default:
//                     break;
//             }
//             return hitprice;
//         }
//
//         /// <summary>
//         /// 计算止盈价格
//         /// </summary>
//         /// <param name="pos"></param>
//         /// <returns></returns>
//         decimal CaculateProfitTakePrice(Position pos)
//         {
//             if (pos == null) return -1;
//             decimal hitprice = -1;
//             switch (this.OffsetType)
//             {
//                 case QSEnumPositionOffsetType.POINTS:
//                     hitprice = pos.isLong ? (pos.AvgPrice + this.Value) : (pos.AvgPrice - this.Value);
//                     break;
//                 case QSEnumPositionOffsetType.PRICE:
//                     hitprice = this.Value;
//                     break;
//                 case QSEnumPositionOffsetType.PERCENT:
//                     hitprice = pos.isLong ? (pos.AvgPrice * (1 + this.Value / 100)) : (pos.AvgPrice * (1 - this.Value / 100));
//                     break;
//                 case QSEnumPositionOffsetType.TRAILING:
//                     {
//                         if (pos.isLong && pos.AvgPrice > 0)
//                         {
//                             if (pos.Highest > pos.AvgPrice + this.Start)
//                                 hitprice = pos.Highest - this.Value;
//                         }
//                         if (pos.isShort && pos.AvgPrice > 0)
//                         {
//                             if (pos.Lowest < pos.AvgPrice - this.Start)
//                                 hitprice = pos.Lowest + this.Value;
//                         }
//                     }
//                     break;
//                 default:
//                     break;
//             }
//             return hitprice;
//         }
//
//         /// <summary>
//         /// 计算针对某个position其触发的止盈止损价格
//         /// </summary>
//         /// <param name="pos"></param>
//         /// <returns></returns>
//         public decimal TargetPrice(Position pos) {
//                 if (!this.Enable) return -1;
//                 decimal price = 0;
//                 if (Direction == QSEnumPositionOffsetDirection.LOSS)
//                 {
//                     price = CaculateLossTakePrice(pos);
//                     return price;
//                 }
//                 else
//                 {
//                     price = CaculateProfitTakePrice(pos);
//                     return price;
//                 }
//         }
//
//         /// <summary>
//         /// 根据止盈止损参数检查 是否需要发送委托
//         /// </summary>
//         /// <param name="pos"></param>
//         /// <param name="k"></param>
//         /// <returns></returns>
//         public bool NeedSendOrder(Position pos,Tick k)
//         {
//             if (!this.Enable) return false;
//
//             decimal hitprice = TargetPrice(pos);
//             bool side = pos.DirectionType == QSEnumPositionDirectionType.Long;
//             if (this.Direction == QSEnumPositionOffsetDirection.LOSS)
//             {
//                 if (side)
//                 {
//                     if (k.Trade <= hitprice)
//                     {
//                         return true;
//                     }
//                     return false;
//                 }
//                 else
//                 {
//                     if (k.Trade >= hitprice)
//                     {
//                         return true;
//                     }
//                     return false;
//                 }
//             }
//             else
//             {
//                 if (this.OffsetType == QSEnumPositionOffsetType.TRAILING)
//                 {
//                     //decimal hitprice = ProfitTakePrice;
//                     if (hitprice > 0)
//                     {
//                         if (k.Trade <= hitprice)
//                         {
//                             return true;//执行止盈
//                         }
//                     }
//                 }
//                 else
//                 {
//                     //Util.Debug("profittakeprice:" + ProfitTakePrice.ToString() + " profit arg enable:" + this.ProfitArg.Enable.ToString());
//                     //decimal hitprice = ProfitTakePrice;
//                     if (side)
//                     {
//                         if (k.Trade >= hitprice)
//                         {
//                             return true;//执行止盈
//                         }
//                         return false;
//                     }
//                     else//空
//                     {
//                         if (k.Trade <= hitprice)
//                         {
//                             return true;
//                         }
//                         return false;
//                     }
//                 }
//                 return false;//不止盈
//             }
//         }
//
//         public override string ToString()
//         {
//             return Util.GetEnumDescription(Direction) + " " + (Enable ? "有效" : "无效") + " 类型:" + OffsetType.ToString() +" V:"+Value.ToString() +" S:"+Size.ToString() +" St:"+Start.ToString() + " Fire:"+Fired.ToString();
//         }
//
//         public static string Serialize(PositionOffsetArg po)
//         {
//             const char d = ',';
//             StringBuilder sb = new StringBuilder();
//             sb.Append(po.Account);
//             sb.Append(d);
//             sb.Append(po.Symbol);
//             sb.Append(d);
//             sb.Append(po.Enable.ToString());
//             sb.Append(d);
//             sb.Append(po.Direction);
//             sb.Append(d);
//             sb.Append(po.OffsetType.ToString());
//             sb.Append(d);
//             sb.Append(po.Value.ToString());
//             sb.Append(d);
//             sb.Append(po.Size.ToString());
//             sb.Append(d);
//             sb.Append(po.Start.ToString());
//             sb.Append(d);
//             sb.Append(po.Side.ToString());
//
//             return sb.ToString();
//         }
//
//         public static PositionOffsetArg Deserialize(string message)
//         {
//             string[] rec = message.Split(',');
//             if (rec.Length < 8) return null;
//
//             string account = rec[0];
//             string symbol = rec[1];
//             
//             bool enable = Convert.ToBoolean(rec[2]);
//             QSEnumPositionOffsetDirection direction = (QSEnumPositionOffsetDirection)Enum.Parse(typeof(QSEnumPositionOffsetDirection), rec[3]);
//             QSEnumPositionOffsetType type = (QSEnumPositionOffsetType)Enum.Parse(typeof(QSEnumPositionOffsetType), rec[4]);
//             decimal value = Convert.ToDecimal(rec[5]);
//             int size = Convert.ToInt32(rec[6]);
//             decimal start = Convert.ToDecimal(rec[7]);
//             bool side = Convert.ToBoolean(rec[8]);
//
//             PositionOffsetArg po = new PositionOffsetArg(account, symbol,side, direction);
//             po.Enable = enable;
//             po.OffsetType = type;
//
//             po.Value = value;
//             po.Size = size;
//             po.Start = start;
//
//             return po;
//         }
//     }
//
//
// }
