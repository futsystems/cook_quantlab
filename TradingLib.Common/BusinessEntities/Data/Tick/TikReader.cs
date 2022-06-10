using System;
using System.Collections.Generic;
using System.Text;
using TradingLib.API;
using System.IO;

namespace TradingLib.Common
{
    
    /// <summary>
    /// read tradelink tick files
    /// </summary>
    public class TikReader : BinaryReader
    {
        string _realsymbol = string.Empty;
        string _sym = string.Empty;
        
        string _path = string.Empty;
        /// <summary>
        /// estimate of ticks contained in file
        /// </summary>
        //public int ApproxTicks = 0;
        /// <summary>
        /// real symbol for data represented in file
        /// </summary>
        public string RealSymbol { get { return _realsymbol; } }

        public string Exchange { get; set; }

        /// <summary>
        /// security-parsed symbol
        /// </summary>
        public string Symbol { get { return _sym; } }

        /// <summary>
        /// file is readable, has version and real symbol
        /// </summary>
        //public bool isValid { get { return (_filever != 0) && (_realsymbol != string.Empty) && BaseStream.CanRead; } }
        /// <summary>
        /// count of ticks presently read
        /// </summary>
        public int Count = 0;
        public TikReader(string filepath) : base(new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read)) 
        {
            _path = filepath;
            FileInfo fi = new FileInfo(filepath);
            
        }


        public event TickDelegate gotTick;

        /// <summary>
        /// returns true if more data to process, false otherwise
        /// </summary>
        /// <returns></returns>
        public bool NextTick()
        {
            try
            {
                // get tick type
                byte type = ReadByte();
                // prepare a tick
                TickImpl k = new TickImpl(this.Exchange,_realsymbol);
                // get the tick
                string tmp = this.ReadString();

                // send any tick we have
                if (gotTick != null)
                    gotTick(k);
                // count it
                Count++;
                // assume there is more
                return true;
            }
            catch (EndOfStreamException)
            {
                Close();
                return false;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }
    }

    public class BadTikFile : Exception
    {
        public BadTikFile() : base() { }
        public BadTikFile(string message) : base(message) { }
    }

}
