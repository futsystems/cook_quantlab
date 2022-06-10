using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public partial class Util
    {
        public static string padLeftEx(string str, int totalByteCount,char p=' ')
        {
            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())//判断字符是2位还是1位 如果是2位就将移位+1
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }
            string w = str.PadRight(totalByteCount - dcount, p);
            return w;
        }


        public static string padRightEx(string str, int totalByteCount, char p = ' ')
        {
            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())//判断字符是2位还是1位 如果是2位就将移位+1
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }

            string w = str.PadLeft(totalByteCount - dcount,p);
            return w;
        }

        public static string padCenterEx(string str, int totalByteCount, char p = ' ')
        {
            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())//判断字符是2位还是1位 如果是2位就将移位+1
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }
            int strcnt = dcount + str.Length;
            int remaincnt = totalByteCount - strcnt;
            int leftcnt = remaincnt / 2;
            return str.PadLeft(leftcnt + strcnt - dcount,p).PadRight(totalByteCount - dcount,p);
        }

    }
}
