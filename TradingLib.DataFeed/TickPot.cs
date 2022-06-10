using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using NetMQ;
using NetMQ.Sockets;


namespace TradingLib.DataFeed
{
    public class TickPot
    {
        const int TICK_HEART_BEAT = 1;
        const int TICK_BUFFER_SIZE = 10000;
        ManualResetEvent _bufferwaiting = new ManualResetEvent(false);

        NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        string _address = "127.0.0.1";
        int _port = 6666;


        public TickPot(string address,int port)
        {
            _address = address;
            _port = port;
        }
        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            if (_bufferGo)
            {
                logger.Info("Buffer Send Thread already started");
            }
            else
            {
                _bufferGo = true;
                _bufferSendThread = new Thread(BufferProcess);
                _bufferSendThread.IsBackground = true;
                _bufferSendThread.Start();
            }

            StartHeartBeat();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (!_bufferGo)
            {
                logger.Info("Buffer Send Thread not started");
            }
            else
            {
                Util.WaitThreadStop(_bufferSendThread);
            }
        }


        /// <summary>
        /// 是否处于运行状态
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return _bufferGo;
            }
        }


        RingBuffer<string> tickbuffer = new RingBuffer<string>(TICK_BUFFER_SIZE);
        public void NewTick(Tick k)
        {
            //logger.Info("send tick to buffer");
            string str_tosend = k.Symbol +"^" + TickImpl.Serialize(k);
            tickbuffer.Write(str_tosend);
            newdata();
        }

        public void NewTickStr(string tickStr)
        {
            tickbuffer.Write(tickStr);
            newdata();
        }


        void NewHeartBeat()
        {
            string str_tosend = "TICKHEARTBEAT";
            tickbuffer.Write(str_tosend);
            newdata();
        }


        Thread _bufferSendThread = null;
        bool _bufferGo = false;

        void BufferProcess()
        {

            using (var pub = new PublisherSocket())
            {
                string address = string.Format("tcp://{0}:{1}", _address, _port);
                pub.Connect(address);
                logger.Info(string.Format("TickPubSrv Start Pub Socket:{0}", address));
                while (_bufferGo)
                {
                    try
                    {

                        while (tickbuffer.hasItems)
                        {
                            string tosend = tickbuffer.Read();
                            if (!string.IsNullOrEmpty(tosend))
                            {
                                pub.SendFrame(System.Text.Encoding.UTF8.GetBytes(tosend), false);

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    // clear current flag signal
                    _bufferwaiting.Reset();

                    // wait for a new signal to continue reading
                    _bufferwaiting.WaitOne(10);
                }
            }

        }

        void StartHeartBeat()
        {
            if (_hbgo)
            {
                logger.Info("HeartBeat Thread already started");
                return;
            }
            _hbgo = true;
            _hbthread = new Thread(HeartBeatProcess);
            _hbthread.IsBackground = true;
            _hbthread.Start();
        }

        void StopHeartBeat()
        {
            if (!_hbgo)
            {
                logger.Info("HeartBeat Thread not started");
                return;
            }
            Util.WaitThreadStop(_hbthread);
        }

        Thread _hbthread = null;
        bool _hbgo = false;
        DateTime _lastHeartBeat = DateTime.Now;
        void HeartBeatProcess()
        {
            while (_hbgo)
            {
                DateTime now = DateTime.Now;
                if (now.Subtract(_lastHeartBeat).TotalSeconds > TICK_HEART_BEAT)
                {
                    NewHeartBeat();
                    _lastHeartBeat = now;
                }
                Util.sleep(200);
            }
        }


        private void newdata()
        {
            if ((_bufferSendThread != null) && (_bufferSendThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin))
            {
                _bufferwaiting.Set();
            }

        }
    }
}
