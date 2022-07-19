using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TradingLib.API;
using TradingLib.Common;

using NetMQ;
using NetMQ.Sockets;


namespace TradingLib.DataFeed
{
    public class DataFeedBase
    {

        protected NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        TickPot _tickpot = null;
        private string _exchange = null;
        string _address = "127.0.0.1";
        int _qryport = 7777;
        public  DataFeedBase(TickPot tickpot,  string exchange, string address, int qryport)
        {
            _tickpot = tickpot;
            _exchange = exchange;
            _address = address;
            _qryport = qryport;
        }



        ThreadSafeList<FeedRegisterInfo> symbolRegisterList = new ThreadSafeList<FeedRegisterInfo>();

        ConcurrentDictionary<string, string> feedTickSymbolMap = new ConcurrentDictionary<string, string>();
        /// <summary>
        /// 响应合约注册请求
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbollist"></param>
        public void OnRegisterSymbols(string exchange, List<string> symbollist)
        {
            logger.Info("Register Market Data Exchange:" + exchange + " Symbols:" + string.Join(" ", symbollist.ToArray()));
            foreach (var symbol in symbollist)
            {
                //1.解析合约
                SymbolInfo info = SymbolInfo.ParseSymbol(symbol);
                info.Exchange = exchange;
                
                if (!ValidSymbolInfo(info))
                {
                    logger.Warn(string.Format("{0} is not valid", symbol));
                    continue;
                }


                //2.验证行情注册请求
                if (!ValidRegister(info))
                {
                    logger.Warn(string.Format("exchange:{0} symbol:{1} is not allowed", exchange, symbol));
                    continue;
                }

                //3.生成行情源侧的交易所和合约
                string feedexchange = this.ConvertExchange2FeedFormat(info);
                string feedsymbol = this.ConvertSymbol2FeedFormat(info);
                
                if (string.IsNullOrEmpty(feedexchange) || string.IsNullOrEmpty(feedsymbol))
                {
                    logger.Warn("Exchange or Symbol can not be null or empty");
                    continue;
                }

                //建立FeedSymbol到本地辨准Symbol的映射
                if (!feedTickSymbolMap.Keys.Contains(feedsymbol))
                {
                    feedTickSymbolMap.TryAdd(feedsymbol, symbol);
                    logger.Info($"Register FeedSymbol: {feedsymbol} LocalSymbol: {symbol} ");
                    
                    this.SubMarketData(info, feedexchange, feedsymbol);
                    
                    FeedRegisterInfo reg = new FeedRegisterInfo();
                    reg.Info = info;
                    reg.Exchange = feedexchange;
                    reg.Symbol = feedsymbol;

                    symbolRegisterList.Add(reg);
                }



                // //不存在symbol则添加
                // if (!feedTickSymbolMap[feedsymbol].Keys.Contains(symbol))
                // {
                //     Tick k = new TickImpl();
                //     k.Symbol = symbol;
                //     feedTickSymbolMap[feedsymbol].TryAdd(symbol, k);
                //     logger.Info("Register TickCache FeedSymbol:" + feedsymbol + " <-- LocalSymbol:" + symbol);
                //
                // }
            }
        }


        /// <summary>
        /// 接口侧注册合约行情
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="symbol"></param>
        public virtual void SubMarketData(SymbolInfo info, string exchange, string symbol)
        {
            logger.Info("SubMarketData exchange:" + exchange + " symbol:" + symbol);
        }

        /// <summary>
        /// 启动DataFeed
        /// </summary>
        public virtual void Start()
        {
            OnConnected();
        }

        /// <summary>
        /// 停止Datafeed
        /// </summary>
        public virtual void Stop()
        { 
        
        }


        /// <summary>
        /// 行情通道建立
        /// </summary>
        protected virtual void OnConnected()
        {
            logger.Info("Restore Symbol Registers from cache");
            foreach (var reg in symbolRegisterList)
            {
                this.SubMarketData(reg.Info, reg.Exchange, reg.Symbol);
            }

            logger.Info("Restore Symbol Registers from TickPubSrv");
            this.QrySymbolsRegisted();
        }

        /// <summary>
        /// 行情通道断开
        /// </summary>
        protected virtual void OnDisconnected()
        { 
            
        }

        /// <summary>
        /// 是否处于运行状态
        /// </summary>
        public virtual bool IsRunning
        {
            get { return true; }
        }

        TimeSpan PollerTimeOut = new TimeSpan(0, 0, 2);
        
        /// <summary>
        /// 查询已注册的合约
        /// </summary>
        protected virtual void QrySymbolsRegisted()
        {
            try
            {
                using (var qrysocket = new RequestSocket())
                {
                    qrysocket.Options.Linger = TimeSpan.FromSeconds(5);
                    string add = string.Format("tcp://{0}:{1}", _address, _qryport);
                    logger.Info("Qry Symbol Registed from:" + add);
                    
                    qrysocket.Connect(add);
                    MDQrySymbolsRegistedRequest
                        request = RequestTemplate<MDQrySymbolsRegistedRequest>.CliSendRequest(0);
                    request.Exchange = _exchange;

                    qrysocket.SendFrame(request.Data);
                    
                    if (qrysocket.TryReceiveFrameBytes(TimeSpan.FromSeconds(5), out var buffer))
                    {
                        Message msg = Message.gotmessage(buffer);
                        logger.Info("Got Message Type:" + msg.Type.ToString() + " Content:" + msg.Content);
                        IPacket packet = PacketHelper.CliRecvResponse(msg);
                        if (packet.Type == MessageTypes.MGR_MD_QRYSYMBOLSREGISTEDRESPONSE)
                        {
                            RspMDQrySymbolsRegistedResponse response = packet as RspMDQrySymbolsRegistedResponse;
                            if (response.RspInfo.ErrorID == 0)
                            {
                                OnRegisterSymbols(response.Exchange, response.SymbolList);
                            }
                            else
                            {
                                logger.Error("Response ErrorID:" + response.RspInfo.ErrorID.ToString() + " Msg:" +
                                             response.RspInfo.ErrorMessage);
                            }
                        }
                    }
                    else
                    {
                        logger.Info("Qry Symbol Registed TimeOut");
                    }

                }

            }
            catch (Exception ex)
            {
                logger.Error("QrySymbolsRegisted Error:" + ex.ToString());
            }
        }

        //IPacket RecievePacket(ZmqSocket socket)
        //{
        //    try
        //    {
        //        byte[] buffer = new byte[0];
        //        int size = 0;

        //        buffer = socket.Receive(buffer, SocketFlags.DontWait, out size);
        //        Message msg = Message.gotmessage(buffer);

        //        IPacket packet = PacketHelper.SrvRecvRequest(msg.Type, msg.Content, "", "");

        //        return packet;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("ReceivePacket Error:" + ex.ToString());
        //        return null;
        //    }
        //}

        /// <summary>
        /// 输出行情
        /// </summary>
        /// <param name="k"></param>
        protected void NewTick(Tick k)
        {
            if (_tickpot == null) return;
            if(string.IsNullOrEmpty(k.Symbol)) return;
            if(string.IsNullOrEmpty(k.Exchange)) return;
            
            if (feedTickSymbolMap.TryGetValue(k.Symbol, out var localSymbol))
            {
                var tickStr = TickImpl.Serialize(k);
                var newTickStr = TickImpl.ReplaceTickSymbol(tickStr, localSymbol);
                _tickpot.NewTickStr(newTickStr);
            }
        }
        


        /// <summary>
        /// 检查某个合约行情数据请求是否有效(逻辑)
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual bool ValidRegister(SymbolInfo info)
        {
            return true;
        }

        /// <summary>
        /// 转换成行情源需要的合约格式
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual string ConvertSymbol2FeedFormat(SymbolInfo info)
        {
            return info.Symbol;
        }

        /// <summary>
        /// 转换成行情源需要的交易所
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual string ConvertExchange2FeedFormat(SymbolInfo info)
        {
            return info.Exchange;
        }


        /// <summary>
        /// 检查SymbolInfo是否有效
        /// 年月日 等信息检查
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool ValidSymbolInfo(SymbolInfo info)
        {
            if (info == null) return false;
            return true;
        }



        /// <summary>
        /// 按一定格式输出合约
        /// </summary>
        /// <param name="info"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        // public static string FormatSymbol(SymbolInfo info, QSEnumSymbolStyleTypes style)
        // {
        //     try
        //     {
        //         switch (style)
        //         {
        //             case QSEnumSymbolStyleTypes.NumStyle:
        //                 return string.Format("{0}{1}{2}", info.SecCode, info.Year, info.Month);
        //             case QSEnumSymbolStyleTypes.LetterLongStyle:
        //                 return string.Format("{0}{1}{2}", info.SecCode, MonthNum2Letter(info.Month), info.Year);
        //             case QSEnumSymbolStyleTypes.LetterShortStyle:
        //                 return string.Format("{0}{1}{2}", info.SecCode, MonthNum2Letter(info.Month), info.Year.Substring(1, 1));
        //             default:
        //                 return info.Symbol;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         return info.Symbol;
        //     }
        //
        // }

   
        /// <summary>
        /// 月份转换成字母
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string MonthNum2Letter(string month)
        {
            return SymbolImpl.MonthNum2Letter(month);
        }


        /// <summary>
        /// 字母转换成月份
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string MonthLetter2Num(string month)
        {
            return SymbolImpl.MonthLetter2Num(month);
        }


    }
}
