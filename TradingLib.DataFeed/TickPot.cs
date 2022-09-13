using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Text;
using System.Threading.Tasks;
using TradingLib.API;
using TradingLib.Common;
using NetMQ;
using NetMQ.Sockets;


namespace TradingLib.DataFeed
{
    public class TickPot
    {
        NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
        
        private const string HEART_BEAT = "TICKHEARTBEAT";
        const int TICK_HEART_BEAT = 1;
        const int TICK_BUFFER_SIZE = 10000;

        private Channel<string> channel = null;
        System.Timers.Timer timer;
        
        Thread _bufferSendThread = null;
        bool _bufferGo = false;
        
        string _address = "127.0.0.1";
        int _port = 6666;


        public TickPot(string address,int port)
        {
            _address = address;
            _port = port;
            channel = Channel.CreateUnbounded<string>();
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

            StartTimer();
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


        public void NewTickStr(string tickStr)
        {
            channel.Writer.WriteAsync(tickStr).GetAwaiter().GetResult();
        }
        
        void NewHeartBeat()
        {
            channel.Writer.WriteAsync(HEART_BEAT).GetAwaiter().GetResult();
        }

        async void BufferProcess()
        {
            using (var pub = new PublisherSocket())
            {
                pub.Options.SendHighWatermark = 10000;
                string address = string.Format("tcp://{0}:{1}", _address, _port);
                pub.Connect(address);
                logger.Info(string.Format("TickPubSrv Start Pub Socket:{0}", address));

                while (await channel.Reader.WaitToReadAsync() && _bufferGo)
                {
                    if (channel.Reader.TryRead(out var message))
                    {
                        //logger.Info($"msg to send:{message}");
                        //pub.SendFrame(System.Text.Encoding.UTF8.GetBytes(message), false);
                    }
                }
            }
        }
        
        void StartTimer()
        {
            int interval = 1000 * TICK_HEART_BEAT;//多少时间发送一次心跳
            timer = new System.Timers.Timer(interval);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(IntervalTask);
            timer.Start();
        }
        
        private void IntervalTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            logger.Debug("Send Hartbeat");
            NewHeartBeat();
        }
    }
}
