using System;
using System.Text;
using System.IO;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// A tick is both the smallest unit of time and the most simple unit of data in TradeLink (and the markets)
    /// It is an abstract container for last trade, last trade size, best bid, best offer, bid and offer sizes.
    /// </summary>
    public class TickImpl : Tick
    {

        public EnumTickType Type  { get; set; }

        /// <summary>
        /// 更新类别
        /// </summary>
        public string UpdateType { get; set; }
        
        /// <summary>
        /// 交易品种字头
        /// </summary>
        public string Symbol { get; set; }
        
        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange { get; set; }


        private ulong _tickTime;
        /// <summary>
        /// 行情时间
        /// </summary>
        public ulong TickTime
        {
            get { return _tickTime;}
            set
            {
                _tickTime = value;
                this.DateTime = _tickTime.ToDateTime();
                this.Date = this.DateTime.ToTLDate();
                this.Time = this.DateTime.ToTLTime();
            }
        }
        
        /// <summary>
        /// 本地服务器时间
        /// </summary>
        public ulong HostTime { get; set; }
        
        
        public int Date { get; set; }
        public int Time { get; set; }
        
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 成交数量
        /// </summary>
        public double Size { get; set; }
        
        /// <summary>
        /// 成交价格
        /// </summary>
        public double Price { get; set; }
        
        /// <summary>
        /// 交易Id
        /// </summary>
        public string TradeId { get; set; }
        
        /// <summary>
        /// 交易标识
        /// 0 Unknown 1 Buy 2 Sell 3 Buy estimated 4 Sell estimated
        /// </summary>
        public int TradeFlag { get; set; }
        
        
        /// <summary>
        /// 深度
        /// </summary>
        public int Depth { get; set; }
        
        public double BidSize { get; set; }
        
        public double AskSize { get; set; }

        public string AskExchange { get; set; }

        public string BidExchange { get; set; }
        
        public double BidPrice { get; set; }
        public double AskPrice { get; set; }
        public double AskPrice2 { get; set; }
        public double BidPrice2 { get; set; }
        public double AskSize2 { get; set; }
        public double BidSize2 { get; set; }

        public double AskPrice3 { get; set; }
        public double BidPrice3 { get; set; }
        public double AskSize3 { get; set; }
        public double BidSize3 { get; set; }

        public double AskPrice4 { get; set; }
        public double BidPrice4 { get; set; }
        public double AskSize4 { get; set; }
        public double BidSize4 { get; set; }

        public double AskPrice5 { get; set; }
        public double BidPrice5 { get; set; }
        public double AskSize5 { get; set; }
        public double BidSize5 { get; set; }

        public double AskPrice6 { get; set; }
        public double BidPrice6 { get; set; }
        public double AskSize6 { get; set; }
        public double BidSize6 { get; set; }

        public double AskPrice7 { get; set; }
        public double BidPrice7 { get; set; }
        public double AskSize7 { get; set; }
        public double BidSize7 { get; set; }

        public double AskPrice8 { get; set; }
        public double BidPrice8 { get; set; }
        public double AskSize8 { get; set; }
        public double BidSize8 { get; set; }

        public double AskPrice9 { get; set; }
        public double BidPrice9 { get; set; }
        public double AskSize9 { get; set; }
        public double BidSize9 { get; set; }

        public double AskPrice10 { get; set; }
        public double BidPrice10 { get; set; }
        public double AskSize10 { get; set; }
        public double BidSize10 { get; set; }
        

  


        public TickImpl(string exchange,string symbol)
        {
            this.Exchange = exchange;
            this.Symbol = symbol;
        }

        public TickImpl() : this("", "")
        {
            
        }

        public string TickContent1 { get; set; }

        public string TickContent2 { get; set; }
        
        public string TickContent3 { get; set; }

        public string TickContent4 { get; set; }

        public static TickImpl Copy(Tick c)
        {
            TickImpl k = new TickImpl(c.Exchange,c.Symbol);

            k.Type = c.Type;
            k.UpdateType = c.UpdateType;
            
            k.Size = c.Size;
            k.Price = c.Price;
            k.TradeId = c.TradeId;
            k.TradeFlag = c.TradeFlag;

            k.Depth = c.Depth;
            
            k.AskPrice = c.AskPrice;
            k.BidPrice = c.BidPrice;
            k.AskSize = c.AskSize;
            k.BidSize = c.BidSize;
            
            k.AskPrice2 = c.AskPrice2;
            k.BidPrice2 = c.BidPrice2;
            k.AskSize2 = c.AskSize2;
            k.BidSize2 = c.BidSize2;

            k.AskPrice3 = c.AskPrice3;
            k.BidPrice3 = c.BidPrice3;
            k.AskSize3 = c.AskSize3;
            k.BidSize3 = c.BidSize3;

            k.AskPrice4 = c.AskPrice4;
            k.BidPrice4 = c.BidPrice4;
            k.AskSize4 = c.AskSize4;
            k.BidSize4 = c.BidSize4;

            k.AskPrice5 = c.AskPrice5;
            k.BidPrice5 = c.BidPrice5;
            k.AskSize5 = c.AskSize5;
            k.BidSize5 = c.BidSize5;

            k.AskPrice6 = c.AskPrice6;
            k.BidPrice6 = c.BidPrice6;
            k.AskSize6 = c.AskSize6;
            k.BidSize6 = c.BidSize6;

            k.AskPrice7 = c.AskPrice7;
            k.BidPrice7 = c.BidPrice7;
            k.AskSize7 = c.AskSize7;
            k.BidSize7 = c.BidSize7;

            k.AskPrice8 = c.AskPrice8;
            k.BidPrice8 = c.BidPrice8;
            k.AskSize8 = c.AskSize8;
            k.BidSize8 = c.BidSize8;

            k.AskPrice9 = c.AskPrice9;
            k.BidPrice9 = c.BidPrice9;
            k.AskSize9 = c.AskSize9;
            k.BidSize9 = c.BidSize9;

            k.AskPrice10 = c.AskPrice10;
            k.BidPrice10 = c.BidPrice10;
            k.AskSize10 = c.AskSize10;
            k.BidSize10 = c.BidSize10;

            k.TickContent1 = c.TickContent1;
            k.TickContent2 = c.TickContent2;
            k.TickContent3 = c.TickContent3;
            k.TickContent4 = c.TickContent4;
            
            return k;
        }

        /// <summary>
        /// 1:主动买
        /// 2:主动卖
        /// 3:预计主动买
        /// 4:预计主动卖
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="ask"></param>
        /// <param name="bid"></param>
        /// <returns></returns>
        public static int CalcTradeFlag(double trade, double ask, double bid)
        {
            //离Ask更近 为主动买 否则为主动卖
            if (Math.Abs(ask - trade) <= Math.Abs(bid - trade)) return 3;
            return 4;

        }

        public override string ToString()
        {
            switch (this.UpdateType)
            {
                case "H":
                    return "HeartBeat";
                case "X":
                    return Symbol + " " + this.Size + "@" + this.Price + " " + this.Exchange;
                case "Q":
                    return Symbol + " " + this.BidPrice + "x" + this.AskPrice + " (" + this.BidSize + "x" + this.AskSize + ")";
                case "A":
                    return Symbol + " Ask:" + this.AskPrice + "/" + this.AskSize;
                case "B":
                    return Symbol + " Bid:" + this.BidPrice + "/" + this.BidSize;
                case "F":
                    return Symbol + " O:" + this.Open + " H:" + this.High + " L:" + this.Low + " PreClose:" + this.PreClose + " Settle:" + this.PreSettlement + "/" + this.Settlement + " OI:" + this.PreOpenInterest + "/" + this.OpenInterest;
                case "T":
                    return "Time:" + Util.ToTLDateTime(this.Date, this.Time);
                //快照模式 该模式用于维护某个Tick的当前最新市场状态
                case "S":
                    return Symbol + " Snapshot";
                default:
                    return "UNKNOWN TICK";
            }
        }

        #region 快照字段
        public double Vol { get; set; }

       
        public double Open { get; set; }

        public double High { get; set; }

        public double Low { get; set; }
        
        public double PreOpenInterest { get; set; }

        public double OpenInterest { get; set; }


        public double PreSettlement { get; set; }

        public double Settlement { get; set; }

        public double PreClose { get; set; }
        #endregion
        


        static char[] spliter = new char[] { ',', ',' };
        static char d = ',';
        /// <summary>
        /// 快速替换序列化后的行情数据的合约 避免多次序列化与创建Tick对象
        /// </summary>
        /// <param name="tick"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static string ReplaceTickSymbol(string tick, string symbol)
        {
            string[] rec = tick.Split(spliter, 3);
            StringBuilder sb = new StringBuilder();
            sb.Append(rec[0]);
            sb.Append(d);
            sb.Append(symbol);
            sb.Append(d);
            sb.Append(rec[2]);
            return sb.ToString();
        }
        
        /// <summary>
        /// 序列化方式2
        /// 通过UpdateType来实现差异序列化 使得行情报价更加完善
        /// UpdateType
        /// 成交
        /// T,Symbol,Date,Time,DataFeed,Price,Size,Vol,Ask,Bid,Exchange
        /// 盘口
        /// Q,Symbol,Date,Time,DataFeed,AskPrice,BidPrice,AskSize,BidSize,AskExchange,BidExchange
        /// A,Symbol,Date,Time,DataFeed,AskPrice,AskSize,AskExchange
        /// B,Symbol,Date,Time,DataFeed,BidPrice,BidSize,BidExchange
        /// 快照
        /// S,Symbol,Date,Time,Price,Size,Exchange,AskPrice,AskSize,AskExchange,BidPrice,BidSize,BidExchange,Vol,Open,High,Low,PreClose,PreSettle,Settle,PreOI,OI,UpLimit,LowLimit
        /// 
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static string Serialize(Tick k)
        {
            const char d = ',';
            StringBuilder sb = new StringBuilder();
            if (k.UpdateType == "H")
                return "H,";
            sb.Append(k.UpdateType);//0
            sb.Append(d);
            sb.Append(k.Symbol);//1
            sb.Append(d);
            sb.Append(k.Exchange);//2
            sb.Append(d);
            sb.Append(k.HostTime);//3
            sb.Append(d);
            sb.Append(k.TickTime);//4
            sb.Append(d);
            sb.Append("");//5
            sb.Append(d);
            sb.Append("");//6
            sb.Append(d);
            switch (k.UpdateType)
            {
                case "X"://成交数据
                    {
                        sb.Append(k.Price);//7
                        sb.Append(d);
                        sb.Append(k.Size);//8
                        sb.Append(d);
                        sb.Append(k.TradeFlag);//9
                        sb.Append(d);
                        sb.Append(k.TradeId);//10
                        break;
                    }
                case "A"://卖盘报价
                    {
                        sb.Append(k.AskPrice);
                        sb.Append(d);
                        sb.Append(k.AskSize);
                        sb.Append(d);
                        sb.Append(k.AskExchange);
                        sb.Append(d);
                        sb.Append(k.Exchange);
                        break;
                    }
                case "B"://买盘报价
                    {
                        sb.Append(k.BidPrice);
                        sb.Append(d);
                        sb.Append(k.BidSize);
                        sb.Append(d);
                        sb.Append(k.BidExchange);
                        sb.Append(d);
                        sb.Append(k.Exchange);
                        break;
                    }
                case "Q"://双边盘口快照
                    {
                        sb.Append(k.AskPrice);//7
                        sb.Append(d);
                        sb.Append(k.AskSize);
                        sb.Append(d);
                        sb.Append(k.AskExchange);
                        sb.Append(d);
                        sb.Append(k.BidPrice);
                        sb.Append(d);
                        sb.Append(k.BidSize);
                        sb.Append(d);
                        sb.Append(k.BidExchange);
                        break;
                    }
                case "O":
                    {
                        sb.Append(k.AskPrice);//7
                        sb.Append(d);
                        sb.Append(k.AskPrice2);//8
                        sb.Append(d);
                        sb.Append(k.AskPrice3);//9
                        sb.Append(d);
                        sb.Append(k.AskPrice4);//10
                        sb.Append(d);
                        sb.Append(k.AskPrice5);//11
                        sb.Append(d);
                        sb.Append(k.AskPrice6);//12
                        sb.Append(d);
                        sb.Append(k.AskPrice7);//13
                        sb.Append(d);
                        sb.Append(k.AskPrice8);//14
                        sb.Append(d);
                        sb.Append(k.AskPrice9);//15
                        sb.Append(d);
                        sb.Append(k.AskPrice10);//16
                        sb.Append(d);
                        sb.Append(k.AskSize);//17
                        sb.Append(d);
                        sb.Append(k.AskSize2);//18
                        sb.Append(d);
                        sb.Append(k.AskSize3);//19
                        sb.Append(d);
                        sb.Append(k.AskSize4);//20
                        sb.Append(d);
                        sb.Append(k.AskSize5);//21
                        sb.Append(d);
                        sb.Append(k.AskSize6);//22
                        sb.Append(d);
                        sb.Append(k.AskSize7);//23
                        sb.Append(d);
                        sb.Append(k.AskSize8);//24
                        sb.Append(d);
                        sb.Append(k.AskSize9);//25
                        sb.Append(d);
                        sb.Append(k.AskSize10);//26

                        sb.Append(d);
                        sb.Append(k.BidPrice);//27
                        sb.Append(d);
                        sb.Append(k.BidPrice2);//28
                        sb.Append(d);
                        sb.Append(k.BidPrice3);//29
                        sb.Append(d);
                        sb.Append(k.BidPrice4);//30
                        sb.Append(d);
                        sb.Append(k.BidPrice5);//31
                        sb.Append(d);
                        sb.Append(k.BidPrice6);//32
                        sb.Append(d);
                        sb.Append(k.BidPrice7);//33
                        sb.Append(d);
                        sb.Append(k.BidPrice8);//34
                        sb.Append(d);
                        sb.Append(k.BidPrice9);//35
                        sb.Append(d);
                        sb.Append(k.BidPrice10);//36
                        sb.Append(d);
                        sb.Append(k.BidSize);//37
                        sb.Append(d);
                        sb.Append(k.BidSize2);//38
                        sb.Append(d);
                        sb.Append(k.BidSize3);//39
                        sb.Append(d);
                        sb.Append(k.BidSize4);//40
                        sb.Append(d);
                        sb.Append(k.BidSize5);//41
                        sb.Append(d);
                        sb.Append(k.BidSize6);//42
                        sb.Append(d);
                        sb.Append(k.BidSize7);//43
                        sb.Append(d);
                        sb.Append(k.BidSize8);//44
                        sb.Append(d);
                        sb.Append(k.BidSize9);//45
                        sb.Append(d);
                        sb.Append(k.BidSize10);//46
                        break;
                    }
               
                case "T"://行情源时间Tick
                    {
                        break;
                    }
                case "DO":
                {
                    sb.Append(k.TickContent1);
                    sb.Append(d);
                    sb.Append(k.TickContent2);
                    sb.Append(d);
                    sb.Append(k.TickContent3);
                    sb.Append(d);
                    sb.Append(k.TickContent4);
                    break;
                }
                case "2U"://Level2 Update
                    {
                        sb.Append(k.Depth);
                        sb.Append(d);
                        switch (k.Depth)
                        {
                            case 1:
                                {
                                    sb.Append(k.AskPrice);
                                    sb.Append(d);
                                    sb.Append(k.AskSize);
                                    sb.Append(d);
                                    sb.Append(k.BidPrice);
                                    sb.Append(d);
                                    sb.Append(k.BidSize);
                                    break;
                                }
                            case 2:
                                {
                                    sb.Append(k.AskPrice2);
                                    sb.Append(d);
                                    sb.Append(k.AskSize2);
                                    sb.Append(d);
                                    sb.Append(k.BidPrice2);
                                    sb.Append(d);
                                    sb.Append(k.BidSize2);
                                    break;
                                }
                            case 3:
                                {
                                    sb.Append(k.AskPrice3);
                                    sb.Append(d);
                                    sb.Append(k.AskSize3);
                                    sb.Append(d);
                                    sb.Append(k.BidPrice3);
                                    sb.Append(d);
                                    sb.Append(k.BidSize3);
                                    break;
                                }
                            case 4:
                                {
                                    sb.Append(k.AskPrice4);
                                    sb.Append(d);
                                    sb.Append(k.AskSize4);
                                    sb.Append(d);
                                    sb.Append(k.BidPrice4);
                                    sb.Append(d);
                                    sb.Append(k.BidSize4);
                                    break;
                                }
                            case 5:
                                {
                                    sb.Append(k.AskPrice5);
                                    sb.Append(d);
                                    sb.Append(k.AskSize5);
                                    sb.Append(d);
                                    sb.Append(k.BidPrice5);
                                    sb.Append(d);
                                    sb.Append(k.BidSize5);
                                    break;
                                }
                            case 6:
                                {
                                    sb.Append(k.AskPrice6);
                                    sb.Append(d);
                                    sb.Append(k.AskSize6);
                                    sb.Append(d);
                                    sb.Append(k.BidPrice6);
                                    sb.Append(d);
                                    sb.Append(k.BidSize6);
                                    break;
                                }
                            case 7:
                                {
                                    sb.Append(k.AskPrice7);
                                    sb.Append(d);
                                    sb.Append(k.AskSize7);
                                    sb.Append(d);
                                    sb.Append(k.BidPrice7);
                                    sb.Append(d);
                                    sb.Append(k.BidSize7);
                                    break;
                                }
                            case 8:
                                {
                                    sb.Append(k.AskPrice8);
                                    sb.Append(d);
                                    sb.Append(k.AskSize8);
                                    sb.Append(d);
                                    sb.Append(k.BidPrice8);
                                    sb.Append(d);
                                    sb.Append(k.BidSize8);
                                    break;
                                }
                            case 9:
                                {
                                    sb.Append(k.AskPrice9);
                                    sb.Append(d);
                                    sb.Append(k.AskSize9);
                                    sb.Append(d);
                                    sb.Append(k.BidPrice9);
                                    sb.Append(d);
                                    sb.Append(k.BidSize9);
                                    break;
                                }
                            case 10:
                                {
                                    sb.Append(k.AskPrice10);
                                    sb.Append(d);
                                    sb.Append(k.AskSize10);
                                    sb.Append(d);
                                    sb.Append(k.BidPrice10);
                                    sb.Append(d);
                                    sb.Append(k.BidSize10);
                                    break;
                                }
                        }
                        sb.Append(d);
                        sb.Append(k.Exchange);
                        break;
                        

                    }

            }
            return sb.ToString();
        }

        public static Tick Deserialize(string msg)
        {
            if (msg == "H,")
            {
                Tick heartbeat = new TickImpl();
                heartbeat.UpdateType = "H";
            }
            string[] r = msg.Split(',');
            if (r.Length <= 5) return null;
            Tick k = new TickImpl();
            k.UpdateType = r[0];
            k.Symbol = r[1];
            k.Exchange = r[2];
            k.HostTime = ulong.Parse(r[3]);
            k.TickTime = ulong.Parse(r[4]);
            
            double val = 0;
            switch (k.UpdateType)
            {
                case "X":
                    {
                        k.Price = double.Parse(r[7]);
                        k.Size = double.Parse(r[8]);
                        k.TradeFlag = int.Parse(r[9]);
                        k.TradeId = r[10];
                        // k.AskPrice = double.Parse(r[10]);
                        // k.BidPrice = double.Parse(r[11]);
                        // k.Exchange = r[12];
                        // if (r.Length >= 14 && !string.IsNullOrEmpty(r[13]))
                        // {
                        //     k.TradeFlag = int.Parse(r[13]);
                        // }
                        // else
                        // {
                        //     k.TradeFlag = TickImpl.CalcTradeFlag(k.Trade, k.AskPrice, k.BidPrice);
                        // }
                        break;
                    }
                case "A":
                    {
                        k.AskPrice = double.Parse(r[7]);
                        k.AskSize = double.Parse(r[8]);
                        k.AskExchange = r[9];
                        k.Exchange = r[10];
                        break;
                    }
                case "B":
                    {
                        k.BidPrice = double.Parse(r[7]);
                        k.BidSize = double.Parse(r[8]);
                        k.BidExchange = r[9];
                        k.Exchange = r[10];
                        break;
                    }
                case "Q":
                {
                    k.AskPrice = double.Parse(r[7]);
                    k.AskSize = double.Parse(r[8]);
                    k.AskExchange = r[9];
                    k.BidPrice = double.Parse(r[10]);
                    k.BidSize = double.Parse(r[11]);
                    k.BidExchange = r[12];
                    break;
                }
                case "O":
                {
                    k.AskPrice = double.Parse(r[7]);
                    k.AskPrice2 = double.Parse(r[8]);
                    k.AskPrice3 = double.Parse(r[9]);
                    k.AskPrice4 = double.Parse(r[10]);
                    k.AskPrice5 = double.Parse(r[11]);
                    k.AskPrice6 = double.Parse(r[12]);
                    k.AskPrice7 = double.Parse(r[13]);
                    k.AskPrice8 = double.Parse(r[14]);
                    k.AskPrice9 = double.Parse(r[15]);
                    k.AskPrice10 = double.Parse(r[16]);
                    k.AskSize = double.Parse(r[17]);
                    k.AskSize2 = double.Parse(r[18]);
                    k.AskSize3 = double.Parse(r[19]);
                    k.AskSize4 = double.Parse(r[20]);
                    k.AskSize5 = double.Parse(r[21]);
                    k.AskSize6 = double.Parse(r[22]);
                    k.AskSize7 = double.Parse(r[23]);
                    k.AskSize8 = double.Parse(r[24]);
                    k.AskSize9 = double.Parse(r[25]);
                    k.AskSize10 = double.Parse(r[26]);

                    k.BidPrice = double.Parse(r[27]);
                    k.BidPrice2 = double.Parse(r[28]);
                    k.BidPrice3 = double.Parse(r[29]);
                    k.BidPrice4 = double.Parse(r[30]);
                    k.BidPrice5 = double.Parse(r[31]);
                    k.BidPrice6 = double.Parse(r[32]);
                    k.BidPrice7 = double.Parse(r[33]);
                    k.BidPrice8 = double.Parse(r[34]);
                    k.BidPrice9 = double.Parse(r[35]);
                    k.BidPrice10 = double.Parse(r[36]);

                    k.BidSize = double.Parse(r[37]);
                    k.BidSize2 = double.Parse(r[38]);
                    k.BidSize3 = double.Parse(r[39]);
                    k.BidSize4 = double.Parse(r[40]);
                    k.BidSize5 = double.Parse(r[41]);
                    k.BidSize6 = double.Parse(r[42]);
                    k.BidSize7 = double.Parse(r[43]);
                    k.BidSize8 = double.Parse(r[44]);
                    k.BidSize9 = double.Parse(r[45]);
                    k.BidSize10 = double.Parse(r[46]);
                    break;
                }
                case "T":
                    {
                        break;
                    }
                case "DO":
                {
                    k.TickContent1 = r[7];
                    k.TickContent2 = r[8];
                    k.TickContent3 = r[9];
                    k.TickContent4 = r[10];
                    break;
                }
                case "2U":
                    {
                        k.Depth = int.Parse(r[7]);
                        switch (k.Depth)
                        {
                            case 1:
                                {
                                    k.AskPrice = double.Parse(r[8]);
                                    k.AskSize = double.Parse(r[9]);
                                    k.BidPrice = double.Parse(r[10]);
                                    k.BidSize = double.Parse(r[11]);
                                    break;
                                }
                            case 2:
                                {
                                    k.AskPrice2 = double.Parse(r[8]);
                                    k.AskSize2 = double.Parse(r[9]);
                                    k.BidPrice2 = double.Parse(r[10]);
                                    k.BidSize2 = double.Parse(r[11]);
                                    break;
                                }
                            case 3:
                                {
                                    k.AskPrice3 = double.Parse(r[8]);
                                    k.AskSize3 = double.Parse(r[9]);
                                    k.BidPrice3 = double.Parse(r[10]);
                                    k.BidSize3 = double.Parse(r[11]);
                                    break;
                                }
                            case 4:
                                {
                                    k.AskPrice4 = double.Parse(r[8]);
                                    k.AskSize4 = double.Parse(r[9]);
                                    k.BidPrice4 = double.Parse(r[10]);
                                    k.BidSize4 = double.Parse(r[11]);
                                    break;
                                }
                            case 5:
                                {
                                    k.AskPrice5 = double.Parse(r[8]);
                                    k.AskSize5 = double.Parse(r[9]);
                                    k.BidPrice5 = double.Parse(r[10]);
                                    k.BidSize5 = double.Parse(r[11]);
                                    break;
                                }
                            case 6:
                                {
                                    k.AskPrice6 = double.Parse(r[8]);
                                    k.AskSize6 = double.Parse(r[9]);
                                    k.BidPrice6 = double.Parse(r[10]);
                                    k.BidSize6 = double.Parse(r[11]);
                                    break;
                                }
                            case 7:
                                {
                                    k.AskPrice7 = double.Parse(r[8]);
                                    k.AskSize7 = double.Parse(r[9]);
                                    k.BidPrice7 = double.Parse(r[10]);
                                    k.BidSize7 = double.Parse(r[11]);
                                    break;
                                }
                            case 8:
                                {
                                    k.AskPrice8 = double.Parse(r[8]);
                                    k.AskSize8 = double.Parse(r[9]);
                                    k.BidPrice8 = double.Parse(r[10]);
                                    k.BidSize8 = double.Parse(r[11]);
                                    break;
                                }
                            case 9:
                                {
                                    k.AskPrice9 = double.Parse(r[8]);
                                    k.AskSize9 = double.Parse(r[9]);
                                    k.BidPrice9 = double.Parse(r[10]);
                                    k.BidSize9 = double.Parse(r[11]);
                                    break;
                                }
                            case 10:
                                {
                                    k.AskPrice10 = double.Parse(r[8]);
                                    k.AskSize10 = double.Parse(r[9]);
                                    k.BidPrice10 = double.Parse(r[10]);
                                    k.BidSize10 = double.Parse(r[11]);
                                    break;
                                }

                        }
                        k.Exchange = r[12];
                        break;
                    }
                default:
                    return null;
            }

            return k;


        }


        public static void WriteTradeSplit(BinaryWriter writer,Tick k)
        {
            writer.Write(k.UpdateType);
            writer.Write(k.Date);
            writer.Write(k.Time);
            writer.Write((double)k.Price);
            writer.Write(k.Size);
            writer.Write(k.TradeFlag);
            
        }

        public static Tick ReadTradeSplit(BinaryReader reader)
        {
            Tick k = new TickImpl();
            k.UpdateType = reader.ReadString();
            k.Date = reader.ReadInt32();
            k.Time = reader.ReadInt32();
            k.Price = (double)reader.ReadDouble();
            k.Size = reader.ReadInt32();
            k.TradeFlag = reader.ReadInt32();
            return k;

        }

       
        public void SetQuote(ulong ticktime,double bid, double ask, double bidsize, double asksize, int depth=0)
        {
            this.TickTime = ticktime;
            
            this.BidPrice = bid;
            this.AskPrice = ask;
            this.BidSize = bidsize;
            this.AskSize = asksize;
            this.Depth = depth;
            
            this.Price = 0;
            this.Size = 0;
            
        }
        
        public void SetTrade(ulong ticktime, double price, double size,string tradeId)
        {
            this.TickTime = ticktime;
            this.Price = price;
            this.Size = size;
            this.TradeId = tradeId;
            
            this.BidPrice = 0;
            this.AskPrice = 0;
            this.AskSize = 0;
            this.BidSize = 0;
        }

        
        public static string SaveTrade(Tick k)
        {
            if (k.IsTrade())
            {
                StringBuilder sb = new StringBuilder();
                char d = ',';
                sb.Append(k.Date);
                sb.Append(d);
                sb.Append(k.Time);
                sb.Append(d);
                sb.Append(k.Price);
                sb.Append(d);
                sb.Append(k.Size);
                sb.Append("\n");
                return sb.ToString();
            }
            return null;
        }

        public static Tick ReadTrade(string tmp)
        {
            string[] rec = tmp.Split(',');
            if (rec.Length < 4) return null;
            long tldatetime = long.Parse(rec[0]);
            int date = (int)(tldatetime / 1000000);
            int time = (int)(tldatetime - date * 1000000);
            double price = double.Parse(rec[1]);
            double size = double.Parse(rec[2]);
            double vol = double.Parse(rec[3]);
            TickImpl k = new TickImpl();
            k.Date = date;
            k.Time = time;
            k.Price = price;
            k.Size = size;
            k.Vol = vol;
            return k;
        }



    }

    enum TickField
    { // tick message fields from TL server
        symbol = 0,//0
        date,//1
        time,//2
        KUNUSED,//3
        trade,//4
        tsize,//5
        tex,//6
        bid,//7
        ask,//8
        bidsize,//9
        asksize,//10
        bidex,//11
        askex,//12
        tdepth,//13
        vol,//14
        open,//15
        high,//16
        low,//17
        preoi,//18
        oi,//19
        presettlement,//20
        settlement,//21
        upper,//22
        lower,//23
        preclose,//24
        datafeed,//25
    }
}