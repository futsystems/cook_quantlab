using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TradingLib.API;


namespace TradingLib.Common
{
    public class PriceVol
    {
        public PriceVol(decimal price)
            : this(price, 0)
        { 
        
        }


        public PriceVol(decimal price, int vol)
        {
            this.Price = price;
            this.Vol = vol;
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Price { get; set; }

        public int Vol { get; set; }

        public static void Write(BinaryWriter writer, PriceVol pv)
        {
            writer.Write((double)pv.Price);
            writer.Write(pv.Vol);
        }

        public static PriceVol Read(BinaryReader reader)
        {
            double price = reader.ReadDouble();
            int vol = reader.ReadInt32();
            return new PriceVol((decimal)price, vol);
        }
    }
}
