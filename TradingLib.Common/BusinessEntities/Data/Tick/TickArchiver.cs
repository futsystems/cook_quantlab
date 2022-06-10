//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.IO;
//using TradingLib.API;

//namespace TradingLib.Common
//{
//    /// <summary>
//    /// archive ticks as they arrive.   Once archived, ticks can be replayed, viewed or analyzed in any of the tradelink programs.
//    /// </summary>
//    public class TickArchiver
//    {
//        string _path;

//        Dictionary<string, TikWriter> filedict = new Dictionary<string, TikWriter>();
//        Dictionary<string, int> datedict = new Dictionary<string, int>();
//        public TickArchiver() : this(Util.TLTickDir) { }
//        public TickArchiver(string folderpath)
//        {
//            _path = folderpath;
//        }
//        bool _stopped = false;
//        public void Stop()
//        {
//            try
//            {
//                foreach (string file in filedict.Keys)
//                    filedict[file].Close();
//                _stopped = true;
//            }
//            catch { }
//        }

//        public bool newTick(Tick t)
//        {
//            if (_stopped) return false;
//            if ((t.Symbol == null) || (t.Symbol == "")) return false;
//            TikWriter tw;
//            // prepare last date of tick
//            int lastdate = 0;
//            // get last date
//            bool havedate = datedict.TryGetValue(t.Symbol, out lastdate);
//            // if we don't have date, use present date
//            if (!havedate)
//            {
//                lastdate = t.Date;
//                datedict.Add(t.Symbol, t.Date);
//            }
//            // see if we need a new day
//            bool samedate = lastdate == t.Date;
//            // see if we have stream already
//            bool havestream = filedict.TryGetValue(t.Symbol, out tw);
//            // if no changes, just save tick
//            if (samedate && havestream)
//            {
//                try
//                {
//                    tw.newTick((TickImpl)t);
//                    return true;
//                }
//                catch (IOException) { return false; }
//            }
//            else
//            {
//                try
//                {
//                    // if new date, close stream
//                    if (!samedate)
//                    {
//                        try
//                        {
//                            tw.Close();
//                        }
//                        catch (IOException) { }
//                    }
//                    // ensure file is writable
//                    string fn = TikWriter.SafeFilename(t.Symbol, _path, t.Date);
//                    if (TikUtil.IsFileWritetable(fn))
//                    {
//                        // open new stream
//                        tw = new TikWriter(_path, t.Symbol, t.Date);
//                        // save tick
//                        tw.newTick((TickImpl)t);
//                        // save stream
//                        if (!havestream)
//                            filedict.Add(t.Symbol, tw);
//                        else
//                            filedict[t.Symbol] = tw;
//                        // save date if changed
//                        if (!samedate)
//                        {
//                            datedict[t.Symbol] = t.Date;
//                        }
//                    }
//                }
//                catch (IOException) { return false; }
//                catch (Exception) { return false; }
//            }

//            return false;
//        }

//    }


//}
