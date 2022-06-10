using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public static class Util_String
    {

        

        /// <summary>
        /// 解析生成对应的Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumStr"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(this string enumStr)
        {
            
            return (T)Enum.Parse(typeof(T), enumStr);
        }

        /// <summary>
        /// 格式化输出
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Put(this string str, params object[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (args.Length != 0)
            {
                return string.Format(str, args);
            }
            return str;
        }

        /// <summary>
        /// 超过长度的字符串自动截断
        /// </summary>
        /// <param name="oldStr"></param>
        /// <param name="maxLength"></param>
        /// <param name="endWith"></param>
        /// <returns></returns>
        public static string Truncat(this string oldStr, int maxLength, string endWith)
        {
            //判断原字符串是否为空  
            if (string.IsNullOrEmpty(oldStr))
                return oldStr + endWith;


            //返回字符串的长度必须大于1  
            if (maxLength < 1)
                throw new Exception("返回的字符串长度必须大于[0] ");


            //判断原字符串是否大于最大长度  
            if (oldStr.Length > maxLength)
            {
                //截取原字符串  
                string strTmp = oldStr.Substring(0, maxLength);

                //判断后缀是否为空  
                if (string.IsNullOrEmpty(endWith))
                    return strTmp;
                else
                    return strTmp + endWith;
            }
            return oldStr;
        }

        public static string PadLeftEx(this string str, int totalByteCount)
        {
            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())//判断字符是2位还是1位 如果是2位就将移位+1
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }
            string w = str.PadRight(totalByteCount - dcount);
            return w;
        }


        public static string PadRightEx(this string str, int totalByteCount)
        {
            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())//判断字符是2位还是1位 如果是2位就将移位+1
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }

            string w = str.PadLeft(totalByteCount - dcount);
            return w;
        }

        public static string PadCenterEx(this string str, int totalByteCount)
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
            return str.PadLeft(leftcnt + strcnt - dcount).PadRight(totalByteCount - dcount);
        }

    }
}
