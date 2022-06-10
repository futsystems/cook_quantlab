using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using NetMQ;
using NetMQ.Sockets;


namespace TradingLib.DataFeed
{
    public class TickPortMgr
    {
        NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        string _address = "127.0.0.1";
        int _port = 900;

        private string _exchange = "DEFAULT";

        TickPot _tickPot = null;
        DataFeedBase _datafeed = null;

        public TickPortMgr(string address, int port, string exchange)
        {
            _address = address;
            _port = port;
            _exchange = exchange;
        }
        public void RegisterTickPort(TickPot tickpot)
        {
            _tickPot = tickpot;
        }

        public void RegisterDataFeed(DataFeedBase datafeed)
        {
            _datafeed = datafeed;
        }
        
        private NetMQ.NetMQPoller _poller = null;
        
        public void Join()
        {
            if (_poller != null)
            {
                logger.Info("TickPotMgr already started");
                return;
            }
            
            using (var sub = new SubscriberSocket())
            {

                string add = string.Format("tcp://{0}:{1}", _address, _port);
                logger.Info(string.Format("TickPotMgr Init Mgr Service:{0}", add));
                sub.SubscribeToAnyTopic();
                sub.Connect(add);
                sub.ReceiveReady += new EventHandler<NetMQSocketEventArgs>(sub_ReceiveReady);
                _poller = new NetMQ.NetMQPoller {sub};
                // start the poller
                _poller.Run();
            }
        }

        public void Release()
        {
            if (_poller==null)
            {
                logger.Info("TickPotMgr not started");
                return;
            }
            _poller.Stop();
        }


        void sub_ReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            try
            {
                byte[] buffer = new byte[0];
                int size = 0;
                buffer = e.Socket.ReceiveFrameBytes();
                Message msg = Message.gotmessage(buffer);
                logger.Info(string.Format("Got Message Type:{0} Content:{1}", msg.Type, msg.Content));
                IPacket packet = PacketHelper.SrvRecvRequest(msg, "", "");

                switch (packet.Type)
                {
                    case MessageTypes.MGR_MD_STARTDATAFEED:
                        {
                            MDReqStartDataFeedRequest request = packet as MDReqStartDataFeedRequest;
                            //if (this._exchange == request.DataFeed && this._datafeed != null)
                            { 
                                this._datafeed.Start();
                            }
                            break;
                        }
                    case MessageTypes.MGR_MD_STOPDATAFEED:
                        {
                            MDReqStopDataFeedRequest request = packet as MDReqStopDataFeedRequest;
                            //if (this._datafeedType == request.DataFeed && this._datafeed != null)
                            {
                                this._datafeed.Stop();
                            }
                            break;
                        }

                    case MessageTypes.MGR_MD_REGISTERSYMBOLS:
                        {
                            MDRegisterSymbolsRequest request = packet as MDRegisterSymbolsRequest;
                            if (this._exchange == request.Exchange && this._datafeed != null)
                            {
                                this._datafeed.OnRegisterSymbols(request.Exchange, request.SymbolList);
                            }
                            break;
                        }
                    default:
                        logger.Warn("Request type:" + msg.Type + " msg:" + msg.Content + " is not supported");
                        break;

                }
            }
            catch (Exception ex)
            {
                logger.Error("Request Handler Error:" + ex.ToString());
            }
        }
    }
}
