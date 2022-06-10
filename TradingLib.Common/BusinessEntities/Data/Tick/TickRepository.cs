using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TradingLib.API;


namespace TradingLib.Common
{
    /// <summary>
    /// 成交数据仓库 用于储存成交数据
    /// 成交数据格式
    /// 时间,价格,数量,总量
    /// </summary>
    public class TickRepository
    {
        Dictionary<string, TikWriter> tikWriterMap = new Dictionary<string, TikWriter>();
        Dictionary<string, int> dateMap = new Dictionary<string, int>();

        private NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        string _basedir = string.Empty;
        public TickRepository(string basedir)
        {
            _basedir = basedir;
        }

        public int WriterCount { get { return tikWriterMap.Count; } }


        /// <summary>
        /// 写入Tick
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="k"></param>
        public void NewTick(Tick k)
        {
            string uniquekey = string.Format("{0}-{1}", k.Exchange, k.Symbol);
            int tickDate = k.Date;

            int lastdate = 0;
            TikWriter tw = null;
            // get last date
            bool havedate = dateMap.TryGetValue(uniquekey, out lastdate);
            // if we don't have date, use present date
            if (!havedate)
            {
                lastdate = tickDate;
                dateMap.Add(uniquekey, lastdate);
            }

            // see if we need a new day
            bool samedate = lastdate == tickDate;
            // see if we have stream already
            bool havestream = tikWriterMap.TryGetValue(uniquekey, out tw);
            // if no changes, just save tick
            if (samedate && havestream)
            {
                try
                {
                    tw.NewTick(k);
                }
                catch (IOException ex)
                {
                    logger.Error("Write tick error:" + ex.ToString());
                }
            }
            else
            {
                try
                {
                    // if new date, close stream
                    if (!samedate)
                    {
                        try
                        {
                            //向前面一个tw写入结束标识 
                            tw.Close();
                        }
                        catch (IOException)
                        {
                            logger.Error("can not close writer");
                        }
                    }
                    // ensure file is writable
                    string path = TikWriter.GetTickPath(_basedir, k.Exchange,k.Symbol);
                    string fn = TikWriter.GetTickFileName(path,k.Symbol, tickDate);

                    if (TikUtil.IsFileWritetable(fn))
                    {
                        // open new stream
                        tw = new TikWriter(path, k.Symbol, tickDate);
                        // save tick
                        tw.NewTick(k);
                        // save stream
                        if (!havestream)
                            tikWriterMap.Add(uniquekey, tw);
                        else
                            tikWriterMap[uniquekey] = tw;
                        // save date if changed
                        if (!samedate)
                        {
                            dateMap[uniquekey] = tickDate;
                        }
                    }
                    else
                    {
                        logger.Error(string.Format("File {0} is not writeable", fn));
                    }
                }
                catch (IOException ex)
                {
                    logger.Error("Write tick error:" + ex.ToString());
                }
                catch (Exception ex)
                {
                    logger.Error("Write tick error:" + ex.ToString());
                }
            }
        }
    }
}
