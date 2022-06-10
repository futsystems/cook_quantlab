using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using TradingLib.API;


namespace TradingLib.Common
{
    /// <summary>
    /// �����첽ִ��tick����,���߳�ֱ�ӽ�Tick����ringbuffer��,Ȼ���ڵ������߳���ȥ����Tick
    /// </summary>
    public class AsyncResponse
    {
        private NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        const int MAXTICK = 10000;
        const int MAXIMB = 100000;
        RingBuffer<Tick> _tickcache;

        /// <summary>
        /// ���ӻ������첽��ȡһ��tickʱ����
        /// </summary>
        public event TickDelegate GotTick;
        /// <summary>
        ///  ��������û������ʱ����
        /// </summary>
        public event VoidDelegate GotTickQueueEmpty;
        /// <summary>
        /// �����µ�Tick����д�뻺��ʱ����
        /// </summary>
        public event VoidDelegate GotTickQueued;
        /// <summary>
        /// ������̫С,Tick�������ʱ����
        /// </summary>
        public int TickOverrun { get { return _tickcache.BufferOverrun; } }

        static ManualResetEvent _tickswaiting = new ManualResetEvent(false);
        Thread _readtickthread = null;

        volatile bool _readtick = false;
        int _nrt = 0;
        int _nwt = 0;

        /// <summary>
        /// �Ƿ�����Ч����״̬
        /// </summary>
        public bool isValid { get { return _readtick; } }
        /// <summary>
        /// �Ƿ��ڹ���״̬
        /// </summary>
        public bool IsRunning { get { return _readtick; } }
        void ReadTick()
        {
            while (_readtick)
            {
                try
                {

                    if (_tickcache.hasItems && (GotTickQueued != null))
                        GotTickQueued();

                    while (_tickcache.hasItems)
                    {
                        if (!_readtick)
                            break;
                        Tick k = _tickcache.Read();
                        if (k == null)
                        {
                            _nrt++;
                            if (GotBadTick != null)
                                GotBadTick();
                            continue;
                        }
                        if (GotTick != null)
                            GotTick(k);
                    }

                    // send event that queue is presently empty
                    if (_tickcache.isEmpty && (GotTickQueueEmpty != null))
                        GotTickQueueEmpty();
                    // clear current flag signal
                    _tickswaiting.Reset();
                    // wait for a new signal to continue reading
                    _tickswaiting.WaitOne(SLEEP);

                }
                catch (Exception ex)
                {

                    logger.Debug("process tick error:" + ex.ToString());
                }
            }
        }

        public const int SLEEPDEFAULTMS = 10;
        int _sleep = SLEEPDEFAULTMS;
        /// <summary>
        /// ÿ������ʱ����tick buffer���Ƿ����µ���������
        /// </summary>
        public int SLEEP { get { return _sleep; } set { _sleep = value; } }

        /// <summary>
        /// �������鵽��ʱ���øú���,������tick����Ringbuffer��,���ں�̨�߳��첽����
        /// </summary>
        /// <param name="k"></param>
        public void newTick(Tick k)
        {
            if (k == null)
            {
                _nwt++;
                if (GotBadTick != null)
                    GotBadTick();
                return;
            }
            _tickcache.Write(k);
            
            if ((_readtickthread != null) && (_readtickthread.ThreadState == ThreadState.Unstarted))
            {
                _readtick = true;
                _readtickthread.Start();

            }
            else
            if ((_readtickthread != null) && (_readtickthread.ThreadState == ThreadState.WaitSleepJoin))
            {
                _tickswaiting.Set(); // signal ReadIt thread to read now
            }
        }

        /// <summary>
        /// �����쳣tick��ȡʱ ����
        /// </summary>
        public event VoidDelegate GotBadTick;
        /// <summary>
        /// �������С̫Сʱ�򴥷�
        /// </summary>
        public event VoidDelegate GotTickOverrun;
        /// <summary>
        /// # of null ticks ignored at write
        /// </summary>
        public int BadTickWritten { get { return _nwt; } }
        /// <summary>
        /// # of null ticks ignored at read
        /// </summary>
        public int BadTickRead { get { return _nrt; } }

        
        /// <summary>
        /// create an asynchronous responder
        /// </summary>
        public AsyncResponse(string name) : this(name,MAXTICK) { }

        string _name = "";
        /// <summary>
        /// creates asynchronous responder with specified buffer sizes
        /// </summary>
        /// <param name="maxticks"></param>
        public AsyncResponse(string name,int maxticks)
        {
            _name = name;
            _tickcache = new RingBuffer<Tick>(maxticks);
            _tickcache.BufferOverrunEvent += new VoidDelegate(_tickcache_BufferOverrunEvent);
           
            //_readtickthread = new Thread(this.ReadTick);
            //_readtickthread.Name = "AsyncTickResponse-"+_name;
            //ThreadTracker.Register(_readtickthread);
        }

        void _tickcache_BufferOverrunEvent()
        {
            if (GotTickOverrun != null)
                GotTickOverrun();
        }
        /// <summary>
        /// stop the read threads and shutdown (call on exit)
        /// </summary>
        public void Stop()
        {
            /*
            _readtick = false;
            try
            {
                if ((_readtickthread != null) && ((_readtickthread.ThreadState != ThreadState.Stopped) && (_readtickthread.ThreadState != ThreadState.StopRequested)))
                    _readtickthread.Interrupt();
            }
            catch { }
            try
            {
                _tickcache = new RingBuffer<Tick>(MAXTICK);
                _tickswaiting.Reset();
            }
            catch { }
             * **/
            if (!_readtick) return;
            ThreadTracker.Unregister(_readtickthread);
            _readtick = false;
            int mainwait = 0;
            while (_readtickthread.IsAlive && mainwait < 10)
            {
                Thread.Sleep(1000);
                mainwait++;
            }
            try
            {
                _tickswaiting.Reset();
            }
            catch { }

            _readtickthread.Abort();
            _readtickthread = null;
            //_readtickthread
        }


        public void Start()
        {
            if (_readtick) return;
            _readtick = true;
            _readtickthread = new Thread(this.ReadTick);
            _readtickthread.Name = "AsyncTickResponse-" + _name;
            ThreadTracker.Register(_readtickthread);

            _readtickthread.Start();
            
        }

    }
}
