using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public static class Util_String_FastConverter
    {

        public static int FastToInt(this string val)
        {
            int y = 0;
            for (int i = 0; i < val.Length; i++)
            {
                y = y * 10 + (val[i] - '0');
            }
            return y;
        }

        public static int[] powof10 = new int[10]
        {
            1,
            10,
            100,
            1000,
            10000,
            100000,
            1000000,
            10000000,
            100000000,
            1000000000
        };

        public static decimal FastToDecimal(this string val)
        {
            int len = val.Length;
            if (len != 0)
            {
                bool negative = false;
                long n = 0;
                int start = 0;
                if (val[0] == '-')
                {
                    negative = true;
                    start = 1;
                }
                if (len <= 19)
                {
                    int decpos = len;
                    for (int k = start; k < len; k++)
                    {
                        char c = val[k];
                        if (c == '.')
                        {
                            decpos = k + 1;
                        }
                        else
                        {
                            n = (n * 10) + (int)(c - '0');
                        }
                    }
                    return new decimal((int)n, (int)(n >> 32), 0, negative, (byte)(len - decpos));
                }
                else
                {
                    if (len > 28)
                    {
                        len = 28;
                    }
                    int decpos = len;
                    for (int k = start; k < 19; k++)
                    {
                        char c = val[k];
                        if (c == '.')
                        {
                            decpos = k + 1;
                        }
                        else
                        {
                            n = (n * 10) + (int)(c - '0');
                        }
                    }
                    int n2 = 0;
                    bool secondhalfdec = false;
                    for (int k = 19; k < len; k++)
                    {
                        char c = val[k];
                        if (c == '.')
                        {
                            decpos = k + 1;
                            secondhalfdec = true;
                        }
                        else
                        {
                            n2 = (n2 * 10) + (int)(c - '0');
                        }
                    }
                    byte decimalPosition = (byte)(len - decpos);
                    return new decimal((int)n, (int)(n >> 32), 0, negative, decimalPosition) * powof10[len - (!secondhalfdec ? 19 : 20)] + new decimal(n2, 0, 0, negative, decimalPosition);
                }
            }
            return 0;
        }

        /// <summary>
        /// 解析时间
        /// 11:59:48.581288
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int FastToTime(this string val)
        {
            if (val.Length < 8) return 0;
            int y = 0;
            for (int i = 0; i < 8; i++)
            {
                if (val[i] == ':') continue;
                y = y * 10 + (val[i] - '0');
            }
            return y;
        }

        /// <summary>
        /// 解析日期
        /// 10/14/2016
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int FastToDateUS(this string val)
        {
            if (val.Length < 10) return 0;
            int y = 0;
            int x = 0;
            for (int i = 0; i < val.Length; i++)
            {
                if (val[i] == '/') continue;
                if (i <= 5)
                {
                    y = y * 10 + (val[i] - '0');
                }
                else
                {
                    x = x * 10 + (val[i] - '0');
                }
            }
            return x * 10000 + y;
        }


        /// <summary>
        /// 将字符串按某个编码转换成某个长度的Byte数组
        /// </summary>
        /// <param name="val">字符串</param>
        /// <param name="size">目标Byte数组长度</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string val, int size, Encoding encoding)
        {
            byte[] dest = new byte[size];
            byte[] source = encoding.GetBytes(val);
            int len = source.Length <= size ? source.Length : size;
            Array.Copy(source, 0, dest, 0, len);
            return dest;
        }
    }
}
