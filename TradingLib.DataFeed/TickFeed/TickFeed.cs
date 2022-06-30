using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TradingLib.API;
using TradingLib.Common;
using NetMQ;
using NetMQ.Sockets;

namespace TradingLib.DataFeed
{
    /// <summary>
    /// 行情源
    /// 用于连接到TickSrv接受实时行情
    /// </summary>
    public class FastTickDataFeed : ITickFeed
    {
        private NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        
        string _master = "127.0.0.1";
        string _slave = "127.0.0.1";
        int _port = 6000;
        int _reqport = 6001;
        
        bool _usemaster = true;

        const string NAME = "FastTickFeed";
        public string Name { get { return NAME; } }
        public string CurrentServer
        {
            get
            {
                if (string.IsNullOrEmpty(_slave)) return _master;
                return _usemaster ? _master : _slave;
            }
        }

        public bool IsLive
        {
            get
            {
                return _tickreceiveruning;
            }
        }
        public FastTickDataFeed(string master, string slave, int tickPort, int reqPort)
        {
            _master = master;
            _slave = slave;
            _port = tickPort;
            _reqport = reqPort;
        }



        /// <summary>
        /// 订阅前缀
        /// </summary>
        /// <param name="prefix"></param>
        public void Register(string prefix)
        {
            if (_tickgo)
            {
                _subscriber.Subscribe(Encoding.UTF8.GetBytes(prefix));
            }
        }

        

        /// <summary>
        /// 取消订阅前缀
        /// </summary>
        /// <param name="prefix"></param>
        public void Unregister(string prefix)
        {
            if (_tickgo)
            {
                _subscriber.Unsubscribe(Encoding.UTF8.GetBytes(prefix));
            }
        }


        public event Action<ITickFeed,Tick> TickEvent;
        void OnTick(Tick k)
        {
            if (TickEvent != null)
                TickEvent(this, k);
        }

        public event Action<ITickFeed> ConnectEvent;
        void OnConnected()
        {
            if (ConnectEvent != null)
                ConnectEvent(this);
        }

        public event Action<ITickFeed> DisconnectEvent;
        void OnDisconnected()
        {
            if (DisconnectEvent != null)
                DisconnectEvent(this);
        }


        public void Start()
        {
            logger.Info(string.Format("MasterServer:{0} SlaveServer:{1} Port:{2} ReqPort:{3}", _master, _slave, _port, _reqport));

            StartTickHandler();

            StartHB();
        }

        public void Stop()
        {

            StopHB();

            StopTickHandler();
            //重新将行情服务器标识设置为主，这样停止后会再次重连主服务器
            _usemaster = true;
        }


        #region 行情服务监控线程 用于当行情服务停止时 切换到备用服务器
        void StartHB()
        {
            if (_hb) return;
            _hb = true;
            _hbthread = new Thread(HeartBeatWatch);
            _hbthread.IsBackground = true;
            _hbthread.Name = "FasktTickDF HBWatch";
            _hbthread.Start();
            _lastheartbeat = DateTime.Now;
        }

        void StopHB()
        {
            if (!_hb) return;
            _hb = false;
            int _wait = 0;
            while (_hbthread.IsAlive && (_wait++ < 5))
            {
                logger.Info("#:" + _wait.ToString() + "  FastTickHB is stoping...." + "MessageThread Status:" + _hbthread.IsAlive.ToString());
                Thread.Sleep(500);
            }
            if (!_hbthread.IsAlive)
            {
                _hbthread = null;
                logger.Info("FastTickHB Stopped successfull...");
            }
            else
            {
                logger.Error("Some Error Happend In Stoping FastTickHB");
            }
        }


        DateTime _lastheartbeat = DateTime.Now;
        bool _hb = false;
        Thread _hbthread = null;

        bool _switched = false;
        /// <summary>
        /// 切换行情源服务器
        /// </summary>
        public void SwitchTickSrv()
        {
            _switched = true;
        }

        private void HeartBeatWatch()
        {
            while (_hb)
            {
                if (_switched)
                {
                    _usemaster = !_usemaster;
                    //停止行情服务线程
                    StopTickHandler();
                    //启动行情服务
                    StartTickHandler();
                    //更新行情心跳时间
                    _lastheartbeat = DateTime.Now;
                    logger.Info("TickServer switched to :" + CurrentServer);
                    _switched = false;
                }

                if (DateTime.Now.Subtract(_lastheartbeat).TotalSeconds > 5)
                {
                    if (_tickgo)
                    {
                        logger.Error("TickHeartBeat lost, try to ReConnect to tick server");
                        _usemaster = !_usemaster;
                        //停止行情服务线程
                        StopTickHandler();
                        //启动行情服务
                        StartTickHandler();
                        //更新行情心跳时间
                        _lastheartbeat = DateTime.Now;
                        logger.Info("Connect to TickServer success");
                    }
                }
                
                Thread.Sleep(200);
            }
        }

        #endregion


        #region 行情数据处理
        void StartTickHandler()
        {
            if (_tickgo) return;
            logger.Info("Start TickHandler");
            _tickgo = true;
            _tickthread = new Thread(TickHandler);
            _tickthread.IsBackground = true;
            _tickthread.Name = "FasktTickDF TickHandler";
            _tickthread.Start();
           
            int i = 0;
            while (!_tickreceiveruning & i < 5)
            {
                Thread.Sleep(500);
                i++;
            }
        }


        void StopTickHandler()
        {
            if (_poller == null || !_poller.IsRunning) return;
            logger.Info("Stop TickHandler");
            _tickgo = false;
            _poller.Stop();
            //_ctx.Shutdown(); //安全关闭Socket不用调用Context的shutdown操作
            //int _wait = 0;
            _tickthread.Join();

            //while (_tickthread.IsAlive && (_wait++ < 5))
            //{
            //    logger.Info("#:" + _wait.ToString() + "  FastTick is stoping...." + "MessageThread Status:" + _tickthread.IsAlive.ToString());
            //    Thread.Sleep(500);
            //}
            //if (!_tickthread.IsAlive)
            //{
            //    _tickthread = null;
            //    logger.Info("FastTick Stopped successfull...");
            //}
            //else
            //{
            //    logger.Error("Some Error Happend In Stoping FastTick");
            //}
        }


        /// <summary>
        /// 注册市场数据
        /// </summary>
        /// <param name="symbols"></param>
        public void RegisterSymbols(string exchange, List<string> symbols)
        {
            logger.Info(string.Format("Register Symbol,{0},{1}", exchange, string.Join(" ", symbols.ToArray())));
            foreach (var sym in symbols)
            {
                MDRegisterSymbolsRequest request = RequestTemplate<MDRegisterSymbolsRequest>.CliSendRequest(0);
                request.Exchange = exchange;
                request.SymbolList.Add(sym);
                Send(request);
            }
        }
        
        void Send(IPacket request)
        {
            if (_symbolreq != null)
            {
                lock (_symbolreq)
                {
                    try
                    {
                        _symbolreq.SendFrame(request.Data);
                        if (_symbolreq.TryReceiveFrameBytes(TimeSpan.FromSeconds(5), out var buffer))
                        {
                            Message msg = Message.gotmessage(buffer);
                            logger.Info("Got Response Message Type:" + msg.Type.ToString() + " Content:" + msg.Content);
                        }
                        else
                        {
                            logger.Info("Send Request TimeOut");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("发送消息异常:" + ex.ToString());
                    }
                }
            }
        }

        private NetMQPoller _poller;

        SubscriberSocket _subscriber;//sub socket receive real time market data
        NetMQSocket _symbolreq;//req socket send request for subscribe
        bool _tickgo;
        Thread _tickthread;
        bool _tickreceiveruning = false;

        private void TickHandler()
        {

                using (SubscriberSocket subscriber = new SubscriberSocket())
                using(RequestSocket symbolreq = new RequestSocket())
                {
                    string reqAddress = "tcp://" + CurrentServer + ":" + _reqport;
                    subscriber.Options.Linger = TimeSpan.FromSeconds(0);
                    //subscriber.ReceiveTimeout = TimeSpan.FromSeconds(1);
                    //subscriber.SendTimeout = TimeSpan.FromSeconds(1);
                    symbolreq.Options.Linger = TimeSpan.FromSeconds(0);
                    //symbolreq.ReceiveTimeout = TimeSpan.FromSeconds(1);
                    //symbolreq.SendTimeout = TimeSpan.FromSeconds(1);

                    symbolreq.Connect(reqAddress);
                    string subAddress = "tcp://" + CurrentServer + ":" + _port;
                    subscriber.Connect(subAddress);
                    logger.Info(string.Format("Connect to FastTick Server:{0} ReqPort:{1} DataPort{2}", CurrentServer, _reqport, _port));
                   
                    //订阅行情心跳数据
                    //subscriber.SubscribeToAnyTopic();
                    subscriber.Subscribe(Encoding.UTF8.GetBytes("TICKHEARTBEAT"));
                    //subscriber.Subscribe(Encoding.UTF8.GetBytes("T,"));

                    _symbolreq = symbolreq;
                    _subscriber = subscriber;

                    subscriber.ReceiveReady += new EventHandler<NetMQSocketEventArgs>(sub_ReceiveReady);
                    _poller = new NetMQ.NetMQPoller {subscriber};
                    _tickreceiveruning = true;
                    OnConnected();
                    
                    
                    // start the poller
                    _poller.Run();
                    
                    _tickreceiveruning = false;
                    logger.Info("TickHandler Stopped");
                    OnDisconnected();
                }
            
        }

        void sub_ReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            try
            {
                var tickstr = e.Socket.ReceiveFrameString();
                logger.Debug(tickstr);
                Tick k = TickImpl.Deserialize(tickstr);
                if (k != null && k.UpdateType != "H")
                    OnTick(k);
                //记录数据到达时间
                _lastheartbeat = DateTime.Now;

            } catch (NetMQException ex)
            {
                logger.Error("Tick Sock错误:" + ex.ToString());

            }
            catch (System.Exception ex)
            {
                logger.Error("Tick数据处理错误" + ex.ToString());
            }
            
        }

        #endregion

    }
}
