using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingLib.API;

namespace TradingLib.Common
{
    public class BinaryOptionOrderImpl:BinaryOptionImpl,BinaryOptionOrder
    {

        public BinaryOptionOrderImpl()
        {

            this.ID = 0;
            this.Account = string.Empty;
            this.Date = Util.ToTLDate();
            this.Time = Util.ToTLTime();
            this.oSymbol = null;
            
            this.Amount = 0;
            this.Side = EnumBinaryOptionSideType.Call;
            this.BinaryOption = null;


            this.Status = EnumBOOrderStatus.Unknown;
            this.Comment = string.Empty;
            this.Result = EnumBinaryOptionResultType.HOLD;

            this.EntryTime = 0;
            this.EntryPrice = 0;
            this.ExitTime = 0;
            this.ExitPrice = 0;

            this.LastPrice = 0;
            this.Highest = decimal.MinValue;
            this.Lowest = decimal.MaxValue;

            this.SettleDay = 0;
            this.Settled = false;


        }

        public BinaryOptionOrderImpl(BinaryOptionOrder o)
        {
            this.ID = o.ID;
            this.Account = o.Account;
            this.Date = o.Date;
            this.Time = o.Time;
            this.oSymbol = o.oSymbol;

            this.Amount = o.Amount;
            this.Side = o.Side;
            this.BinaryOption = o.BinaryOption;
            
            this.ExitTime = o.ExitTime;
            this.EntryTime = o.EntryTime;
            this.ExitPrice = o.ExitPrice;
            this.EntryPrice = o.EntryPrice;

            this.LastPrice = o.LastPrice;
            this.Highest = o.Highest;
            this.Lowest = o.Lowest;

            this.Status = o.Status;
            this.Comment = o.Comment;
            this.Result = o.Result;

            this.SettleDay = o.SettleDay;
            this.Settled = o.Settled;

        }

        /// <summary>
        /// 结算日
        /// </summary>
        public int SettleDay { get; set; }

        /// <summary>
        /// 结算标识
        /// </summary>
        public bool Settled { get; set; }

        /// <summary>
        /// 委托编号
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 交易账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public int Date { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// 合约对象
        /// </summary>
        public Symbol oSymbol { get; set; }


        /// <summary>
        /// 下单金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 下单方向
        /// </summary>
        public EnumBinaryOptionSideType Side { get; set; }

        /// <summary>
        /// 二元期权
        /// </summary>
        public BinaryOption BinaryOption { get; set; }



        /// <summary>
        /// 委托状态
        /// </summary>
        public EnumBOOrderStatus Status { get; set; }

        string _comment = string.Empty;
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Comment { get { return _comment; } set { _comment = value; } }


        /// <summary>
        /// 平权胜负
        /// </summary>
        public EnumBinaryOptionResultType Result { get; set; }



        #region 委托过程信息
        /// <summary>
        /// 开权时间
        /// </summary>
        public long EntryTime { get; set; }

        /// <summary>
        /// 平权时间
        /// </summary>
        public long ExitTime { get; set; }

        /// <summary>
        /// 开权价格
        /// </summary>
        public decimal EntryPrice { get; set; }

        /// <summary>
        /// 平权价格
        /// </summary>
        public decimal ExitPrice { get; set; }

        /// <summary>
        /// 最新价格
        /// </summary>
        public decimal LastPrice { get; set; }

        /// <summary>
        /// 持权期内最高价
        /// </summary>
        public decimal Highest { get; set; }

        /// <summary>
        /// 持权期内最低价
        /// </summary>
        public decimal Lowest { get; set; }

        #endregion






        #region 响应行情与时间 事件
        /// <summary>
        /// 响应行情数据
        /// 注意 行情事件统一调整成系统本地时间
        /// 返回true表面委托关闭
        /// </summary>
        /// <param name="k"></param>
        public void GotTick(Tick k)
        {
            //持权状态才处理行情更新
            if (this.Status == EnumBOOrderStatus.Entry)
            {
                if (k.Symbol != (this.oSymbol != null ? this.oSymbol.Symbol : this.Symbol))//非本委托合约的行情直接返回
                    return;

                //当前大于我们设定的下次平权时间 则执行平权操作
                if (Util.ToTLDateTime() > this.ExpireTime)
                { 
                    //执行平权
                    BinaryOptionOrderImpl.ExitOrder(this as BinaryOptionOrder);
                    return;
                }

                double _last = 0;
                bool _needupdate = false;
                //只处理成交价格
                if (k.IsTrade())
                {
                    _last = k.Price;
                    _needupdate = true;
                }

                if (_needupdate)
                {
                    //如果处于持权状态,则需要更新行情数据到先关参数
                    // this.LastPrice = _last;
                    // this.Highest = this.Highest >= _last ? this.Highest : _last;
                    // this.Lowest = this.Lowest <= _last ? this.Lowest : _last;

                }
            }

        }


        /// <summary>
        /// 响应定时时间
        /// </summary>
        /// <param name="time"></param>
        public void GotTime(long time)
        {
            if (this.Status == EnumBOOrderStatus.Entry)
            {
                if (time >= this.ExpireTime)
                {
                    //执行平权
                    BinaryOptionOrderImpl.ExitOrder(this as BinaryOptionOrder);
                }
            }
        }

        #endregion



        /// <summary>
        /// 浮动盈亏 根据不同的权类状态判定当前浮动盈亏,需要实时更新当前最新价格
        /// </summary>
        public decimal UnRealizedPL
        {
            get
            {
                switch (this.Status)
                {
                    case EnumBOOrderStatus.Entry:
                        {
                            switch (this.BinaryOption.OptionType)
                            {
                                //涨跌 价格高于开仓价格为赢 否则为数
                                case EnumBinaryOptionType.CallPut:
                                    if (this.Side == EnumBinaryOptionSideType.Call)
                                    {
                                        if (this.LastPrice > this.EntryPrice) return this.Amount * this.BinaryOption.Rate;
                                        return -1 * this.Amount;
                                    }
                                    else if(this.Side == EnumBinaryOptionSideType.Put)
                                    {
                                        if (this.LastPrice < this.EntryPrice) return this.Amount * this.BinaryOption.Rate;
                                        return -1 * this.Amount ;
                                    }
                                    return 0;
                                case EnumBinaryOptionType.AboveDown:
                                case EnumBinaryOptionType.Range:
                                    return 0;
                                default:
                                    return 0;
                            }
                        }
                    default:
                        return 0;
                }
            }
        }

        decimal ?_realizedpl = null;
        /// <summary>
        /// 平权盈亏 平权后 按最终状态计算盈亏权益
        /// </summary>
        public decimal RealizedPL
        {
            get
            {
                if (_realizedpl != null) return (decimal)_realizedpl;
                switch(this.Result)
                {
                    case EnumBinaryOptionResultType.InTheMoney:
                        return this.Amount * this.Rate;
                    case EnumBinaryOptionResultType.OutOfTheMoney:
                        return -1*this.Amount;
                    default:
                        return 0;
                }                
            }
            set
            {
                //if (this.Status == EnumBOOrderStatus.Exit)
                {
                    _realizedpl = value;
                }
            }
        }

        string GetProcessString()
        {
            switch (this.OptionType)
            {
                case EnumBinaryOptionType.CallPut:
                    {
                        //买涨 3021@201603180000 
                        switch (this.Status)
                        {
                            case EnumBOOrderStatus.Entry:
                                return string.Format("{0} {1}@{2}",this.Side, this.EntryPrice, GetTime(this.EntryTime),GetTime(this.BinaryOption.ExpireTime));
                            case EnumBOOrderStatus.Exit:
                                return string.Format("{0} {1}@{2} {3}@{4} Profit:{8}",this.Side, this.EntryPrice, GetTime(this.EntryTime), this.ExitPrice, GetTime(this.ExitTime),this.RealizedPL);
                            default:
                                return "";
                        }
                    }
                default:
                    return "Not Support";
            }
        }

        int GetTime(long time)
        {
            return Util.ToDateTime(time).ToTLTime();
        }

        public override string ToString()
        {
            //ID:635876700233953163[8500002] 1000/BO:CallPut CN1603@MIN2 Exp:20160319120300
            return string.Format("ID:{0}[{1}] {2}/{3}  Status:[{4}] Process:{5} Result:{6}", this.ID, this.Account, this.Amount, this.BinaryOption, this.Status, GetProcessString(),this.Result);
        }

        

        /// <summary>
        /// 开权操作
        /// </summary>
        /// <param name="order"></param>
        /// <param name="k"></param>
        public static void EntryOrder(BinaryOptionOrder order, Tick k)
        {
            //记录开权时间
            order.EntryTime = DateTime.Now.ToTLDateTime();
            //计算平权时间
            //order.ExpireTime = BinaryOptionOrderImpl.CalcExpireTime(order.EntryTime, order.TimeSpanType);
            //记录开权价格
            order.EntryPrice = (decimal)k.Price;
            order.LastPrice = (decimal)k.Price;
            order.Status = EnumBOOrderStatus.Entry;
        }


        /// <summary>
        /// 平权操作
        /// </summary>
        /// <param name="order"></param>
        public static void ExitOrder(BinaryOptionOrder order)
        {
            order.ExitTime = DateTime.Now.ToTLDateTime();
            order.ExitPrice = order.LastPrice;
            order.Status = EnumBOOrderStatus.Exit; 

            //判定最终输赢状态
            switch (order.BinaryOption.OptionType)
            {
                case EnumBinaryOptionType.CallPut:
                    {
                        if (order.Side == EnumBinaryOptionSideType.Call)
                        {
                            if (order.ExitPrice > order.EntryPrice)
                            {
                                order.Result = EnumBinaryOptionResultType.InTheMoney;
                            }
                            else
                            {
                                order.Result = EnumBinaryOptionResultType.OutOfTheMoney;
                            }
                        }
                        else if(order.Side == EnumBinaryOptionSideType.Put)
                        {
                            if (order.ExitPrice < order.EntryPrice)
                            {
                                order.Result = EnumBinaryOptionResultType.InTheMoney;
                            }
                            else
                            {
                                order.Result = EnumBinaryOptionResultType.OutOfTheMoney;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 复制委托状态
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void Copy(BinaryOptionOrder from, BinaryOptionOrder to)
        {

            to.EntryTime = from.EntryTime;
            to.EntryPrice = from.EntryPrice;
            to.ExitTime = from.ExitTime;
            to.ExitPrice = from.ExitPrice;

            to.LastPrice = from.LastPrice;
            to.Highest = from.Highest;
            to.Lowest = from.Lowest;

            to.Comment = from.Comment;
            to.Result = from.Result;
            to.Status = from.Status;
        }

        public static string Serialize(BinaryOptionOrder o)
        {
            char d = ',';
            StringBuilder sb = new StringBuilder();
            sb.Append(o.ID);
            sb.Append(d);
            sb.Append(o.Account);
            sb.Append(d);
            sb.Append(o.Date);
            sb.Append(d);
            sb.Append(o.Time);
            sb.Append(d);
            sb.Append("");
            sb.Append(d);
            sb.Append(o.Amount);
            sb.Append(d);
            sb.Append(o.Side);
            sb.Append(d);
            sb.Append(o.Status);
            sb.Append(d);
            sb.Append(o.EntryTime);
            sb.Append(d);
            sb.Append(o.ExitTime);//14
            sb.Append(d);
            sb.Append(o.EntryPrice);
            sb.Append(d);
            sb.Append(o.ExitPrice);
            sb.Append(d);
            sb.Append(o.LastPrice);
            sb.Append(d);
            sb.Append(o.Highest);
            sb.Append(d);
            sb.Append(o.Lowest);
            sb.Append(d);
            sb.Append(o.Status);
            sb.Append(d);
            sb.Append(o.Result);
            sb.Append(d);
            sb.Append(o.Comment);//22
            return sb.ToString();
        }

        public  static BinaryOptionOrder Deserialize(string message)
        {

            BinaryOptionOrder o = new BinaryOptionOrderImpl();
            string[] rec = message.Split(',');
            o.ID = long.Parse(rec[0]);
            o.Account = rec[1];
            o.Date = int.Parse(rec[2]);
            o.Time = int.Parse(rec[3]);
            o.Amount = decimal.Parse(rec[6]);
           
            o.Status = (EnumBOOrderStatus)Enum.Parse(typeof(EnumBOOrderStatus), rec[11]);
            o.EntryTime = long.Parse(rec[12]);
            o.ExitTime = long.Parse(rec[14]);

            o.EntryPrice = decimal.Parse(rec[15]);
            o.ExitPrice = decimal.Parse(rec[16]);
            o.LastPrice = decimal.Parse(rec[17]);
            o.Highest = decimal.Parse(rec[18]);
            o.Lowest = decimal.Parse(rec[19]);

            o.Side = rec[20].ParseEnum<EnumBinaryOptionSideType>();
            o.Result = (EnumBinaryOptionResultType)Enum.Parse(typeof(EnumBinaryOptionResultType), rec[21]);
            o.Comment = rec[22];
            return o;
        }
            
    }
}
