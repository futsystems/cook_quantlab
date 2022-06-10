using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.Common
{
    /// <summary>
    /// 分时数据
    /// </summary>
    public class MinuteData
    {
        public MinuteData(int date,int time,double close,int vol,double avgprice)
        {
            this.Date = date;
            this.Time = time;
            this.Close = close;
            this.Vol = vol;
            this.AvgPrice = avgprice;
            this.DateTime = Util.ToDateTime(this.Date, this.Time);
        }

        //public MinuteData()
        //    :this(0,0,0,0,0)
        //{
        //    this.DateTime = DateTime.MinValue;
        //}

        public DateTime DateTime { get; private set; }
        /// <summary>
        /// 日期
        /// </summary>
        public int Date { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// 某分钟收盘价
        /// </summary>
        public double Close { get; set; }


        public int Vol { get; set; }

        /// <summary>
        /// 某分钟成交均价
        /// </summary>
        public double AvgPrice { get; set; }

        public static void Write(BinaryWriter writer, MinuteData data)
        {
            writer.Write(data.Date);
            writer.Write(data.Time);
            writer.Write(data.Close);
            writer.Write(data.AvgPrice);
            writer.Write(data.Vol);
        }

        public static MinuteData Read(BinaryReader reader)
        {
            int date = reader.ReadInt32();
            int time = reader.ReadInt32();
            double close = reader.ReadDouble();
            double avg = reader.ReadDouble();
            int vol = reader.ReadInt32();
            return new MinuteData(date, time, close, vol, avg);
            
        }

    }
}
