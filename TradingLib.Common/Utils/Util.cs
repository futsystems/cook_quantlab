using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Net;
using TradingLib.API;
using System.Xml.Serialization;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.Net.Sockets;


namespace TradingLib.Common
{
    /// <summary>
    /// Utility class holding commonly used properties
    /// </summary>
    public partial class Util
    {
        const string PROGRAME = "GlobalUtil";

        public static string GlobalPrefix = ">>> ";


        public static void LoadStatus(string body, bool samecolor = false)
        {
            StatusSection(body, "LOAD",QSEnumInfoColor.INFOGREEN, samecolor);
        }
        public static void InitStatus(string body, bool samecolor = false)
        {
            StatusSection(body, "INIT", QSEnumInfoColor.INFOGREEN, samecolor);
        }

        public static void DestoryStatus(string body, bool samecolor = false)
        {
            StatusSection(body, "DESTORY", QSEnumInfoColor.INFODARKRED, samecolor);
        }

        /// <summary>
        /// 开始信息
        /// </summary>
        /// <param name="body"></param>
        public static void StartStatus(string body,bool samecolor=false)
        {
            StatusSection(body, "START",QSEnumInfoColor.INFOGREEN, samecolor);
        }

        /// <summary>
        /// 停止信息
        /// </summary>
        /// <param name="body"></param>
        public static void StopStatus(string body,bool samecolor = false)
        {
            StatusSection(body, "STOP", QSEnumInfoColor.INFODARKRED, samecolor);
        }
        public static int GetAvabileConsoleWidth()
        {
            int width = Console.LargestWindowWidth;
            if (width > 0 & width < 1000)
                return width;
            else
                return 100;
        }
        public static void StatusSection(string body, string status, QSEnumInfoColor color,bool samecolor = false)
        {
            //WriteSectionLine();
            //_logger.Info(body.PadLeft(20, ' '));
            WriteSectionLine(body, status);
            //Console.WriteLine();
            //Console.WriteLine("".PadLeft(GetAvabileConsoleWidth()/ 2 - 1, '.'));
            //ConsoleColorStatus(body, string.Format("[{0}]", status), samecolor ? color : QSEnumInfoColor.INFOWHITE, color);
            //Console.WriteLine();
        }

        static int MaxLengh = 100;
        public static void WriteSectionLine()
        {
            _logger.Info("".PadLeft(MaxLengh, '.'));
        }

        public static void WriteSectionLine(string body, string status)
        {
            string s = Util.padRightEx(string.Format(" {0} [{1}]", body, status), MaxLengh, '.');
            _logger.Info(s);
        }




        //#region 全局日志输出函数 避免多个类中去获得单独的日志对象
        ///* 大量创建的对象，或者临时性日志输出，则可以通过调用全局日志输出函数进行
        // * 信息输出，避免创建过多logger对象 
        // * 
        // * 
        // * 
        // * **/
        static NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();




        /// <summary>
        /// 检查ip地址是否正常
        /// </summary>
        /// <param name="ipaddr"></param>
        /// <returns></returns>
        public static bool IsValidAddress(string ipaddr)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(ipaddr);
                return true;
            }
            catch { }
            return false;

        }

        /// <summary>
        /// 判断某个文件是否可写
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileWritetable(string path)
        {
            FileStream stream = null;

            try
            {
                if (!System.IO.File.Exists(path))
                    return true;
                System.IO.FileInfo file = new FileInfo(path);
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return false;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return true;
        }

        /// <summary>
        /// 安全的调用输出某个对象的ToString
        /// 需要先判断obj是否是null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SafeToString(object obj)
        {
            if (obj == null)
            {
                return "null";
            }
            else
            {
                return obj.ToString();
            }
        }

        /// <summary>
        /// 以Json格式输出对象信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string PrintObj(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        ///// <summary>
        ///// 格式化输出数字
        ///// </summary>
        ///// <param name="d"></param>
        ///// <param name="format"></param>
        ///// <returns></returns>
        //public static string FormatDecimal(decimal d, string format = "{0:F2}")
        //{
        //    return string.Format(format, d);
        //}
        ///// <summary>
        ///// 格式化输出数字
        ///// </summary>
        ///// <param name="d"></param>
        ///// <param name="format"></param>
        ///// <returns></returns>
        public static string FormatDouble(double d, string format = "{0:F2}")
        {
            return string.Format(format, d);
        }

        /// <summary>
        /// 获得小数点位
        /// </summary>
        /// <param name="pricetick"></param>
        /// <returns></returns>
        public static int GetDecimalPlace(decimal pricetick)
        {
            //1 0.2
            string[] p = pricetick.ToString().Split('.');
            if (p.Length <= 1)
                return 0;
            else
                return p[1].ToCharArray().Length;
        }

        /// <summary>
        /// 是否在时间段内
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static bool IsInPeriod(DateTime start, DateTime end)
        {
            return DateTime.Now >= start && DateTime.Now <= end;
        }

        /// <summary>
        /// 等待某个线程结束
        /// </summary>
        /// <param name="thread"></param>
        /// <param name="waitnum"></param>
        public static void WaitThreadStop(Thread thread, int waitnum = 10)
        {
            int mainwait = 0;
            while (thread.IsAlive && mainwait < waitnum)
            {
                Thread.Sleep(1000);
                mainwait++;
            }
            thread.Abort();
            thread = null;
        }

        /// <summary>
        /// sleep current thread a few milliseconds
        /// </summary>
        /// <param name="ms"></param>
        public static void sleep(int ms)
        {
            if (ms != 0)
                System.Threading.Thread.Sleep(ms);
        }


        /// <summary>
        /// 获得当前地址信息
        /// </summary>
        /// <returns></returns>
        public static LocationInfo GetLocationInfo()
        {
            try
            {
                String direction = "";
                WebRequest request = WebRequest.Create("http://ip.360.cn/IPShare/info");
                using (WebResponse response = request.GetResponse())
                using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    direction = stream.ReadToEnd();
                    var tmp = direction.DeserializeObject();
                    string ip = tmp["ip"].ToString().Trim();
                    string location = tmp["location"].ToString().Trim();
                    return new LocationInfo() { IP = ip, Location = location, MAC = "" };

                }
            }
            catch (Exception ex)
            {
                return new LocationInfo() { MAC = "" };
            }
        }


        public static bool IsToday(DateTime dt)
        {

            DateTime today = DateTime.Today;
            if ((dt.Year == today.Year) && (dt.Month == today.Month) && (dt.Day == today.Day))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        

        /// <summary>
        /// 获得某个Enum的描述
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetEnumDescription(object e)
        {
            //获取字段信息 
            System.Reflection.FieldInfo[] ms = e.GetType().GetFields();
            Type t = e.GetType();
            foreach (System.Reflection.FieldInfo f in ms)
            {
                //判断名称是否相等 
                if (f.Name != e.ToString()) continue;
                //反射出自定义属性 
                foreach (Attribute attr in f.GetCustomAttributes(true))
                {
                    //类型转换找到一个Description，用Description作为成员名称 
                    System.ComponentModel.DescriptionAttribute dscript = attr as System.ComponentModel.DescriptionAttribute;
                    if (dscript != null)
                        return dscript.Description;
                }
            }
            //如果没有检测到合适的注释，则用默认名称 
            return e.ToString();
        }


        /// <summary>
        /// 通过枚举字符串返回枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumstr"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(string enumstr)
        {
            return (T)Enum.Parse(typeof(T), enumstr);
        }


        /// <summary>
        /// 用于记录当前服务器版本，客户端版本不能小于服务端版本，如果客户端版本小于服务端版本，则表明服务端有更新，需要更新客户端API
        /// </summary>
        public const int Version = 2400;

        public const int TICK_BUFFER_SIZE = 10000;

       

        /// <summary>
        /// 获得配置文件
        /// </summary>
        /// <param name="cfgname"></param>
        /// <returns></returns>
        public static string GetConfigFile(string cfgname)
        {
            string filepath = Path.Combine(new string[] { BaseDir, "config", cfgname });
            return filepath;
        }

        public static string GetHolidayPath()
        {
            return Path.Combine(new string[] { BaseDir, "config", "holiday" });
        }

        /// <summary>
        /// 获得某个资源目录
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static string GetResourceDirectory(string resource)
        {
            //Resource\RechargeGateway
            string dict = Path.Combine(new string[] { BaseDir, "Resource", resource });
            return dict;
        }

		public static string GetPluginPath(string path)
		{
			return Path.Combine (new string[]{ BaseDir, path });
		}

        static Util()
        {
            if (string.IsNullOrEmpty(BaseDir))
            {
                BaseDir = AppDomain.CurrentDomain.BaseDirectory;
            }
            DirectoryInfo info = new DirectoryInfo(BaseDir);
            ParentDir = info.Parent.FullName;
        }

        static string BaseDir = string.Empty;
        static string ParentDir = string.Empty;
        public static string TLBaseDir
        {
            get
            {

                return Path.Combine(new string[] { ParentDir, "TLData" });
            }
        }


        public static string ProgramData(string PROGRAM)
        {
            string path = Path.Combine(new string[] { TLBaseDir, PROGRAM });
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }


        public static string TLLogDir
        {
            get
            {
                return ProgramData("Log");
            }
        }
        /// <summary>
        /// path to  tick folder
        /// </summary>
        public static string TLTickDir
        {
            get
            {
                return ProgramData("TickData");
            }
        }

        /// <summary>
        /// 数据储存日志目录
        /// </summary>
        public static string DataRepositoryDir
        {
            get
            {
                return ProgramData("DataRepository");
            }
        }


        public static TickFileInfo ParseFile(string filepath)
        {
            TickFileInfo tfi;
            tfi.type = TickFileType.Invalid;
            tfi.date = DateTime.MinValue;
            tfi.symbol = "";

            try
            {
                string fn = System.IO.Path.GetFileNameWithoutExtension(filepath);
                string ext = System.IO.Path.GetExtension(filepath).Replace(".", "");
                string date = Regex.Match(fn, "[0-9]{8}$").Value;
                tfi.type = (TickFileType)Enum.Parse(typeof(TickFileType), ext.ToUpper());
                tfi.date = TLD2DT(Convert.ToInt32(date));
                tfi.symbol = Regex.Match(fn, "^[A-Z]+").Value;
            }
            catch (Exception) { tfi.type = TickFileType.Invalid; }
            return tfi;
        }

        #region TLDate and TLTime
        /// <summary>
        /// Converts date to DateTime (eg 20070926 to "DateTime.Mon = 9, DateTime.Day = 26, DateTime.ShortDate = Sept 29, 2007"
        /// </summary>
        /// <param name="TradeLinkDate"></param>
        /// <returns></returns>
        public static DateTime TLD2DT(int TradeLinkDate)
        {
            if (TradeLinkDate < 10000) throw new Exception("Not a date, or invalid date provided");
            return ToDateTime(TradeLinkDate, 0);
        }
        /// <summary>
        /// Converts  Time to DateTime.  If not using seconds, put a zero.
        /// </summary>
        /// <param name="TradeLinkTime"></param>
        /// <param name="TradeLinkSec"></param>
        /// <returns></returns>
        public static DateTime TLT2DT(int TradeLinkTime)
        {
            return ToDateTime(0, TradeLinkTime);
        }
        /// <summary>
        /// gets datetime of a tick
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static DateTime TLT2DT(Tick k)
        {
            return ToDateTime(0, k.Time);
        }


        public static DateTime ToDateTime(long tldatetime)
        {
            int date = (int)(tldatetime / 1000000);
            int time = (int)(tldatetime - date * 1000000);
            return ToDateTime(date, time);
        }
        /// <summary>
        /// Converts TradeLink Date and Time format to a DateTime. 
        /// eg DateTime ticktime = ToDateTime(tick.date,tick.time);
        /// </summary>
        /// <param name="TradeLinkDate"></param>
        /// <param name="TradeLinkTime"></param>
        /// <param name="TradeLinkSec"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(int TradeLinkDate, int TradeLinkTime)
        {
            int sec = TradeLinkTime % 100;
            int hm = TradeLinkTime % 10000;
            int hour = (int)((TradeLinkTime - hm) / 10000);
            int min = (TradeLinkTime - (hour * 10000)) / 100;
            if (sec > 59) { sec -= 60; min++; }
            if (min > 59) { hour++; min -= 60; }
            int year = 1, day = 1, month = 1;
            if (TradeLinkDate != 0)
            {
                int ym = (TradeLinkDate % 10000);
                year = (int)((TradeLinkDate - ym) / 10000);
                int mm = ym % 100;
                month = (int)((ym - mm) / 100);
                day = mm;
            }
            return new DateTime(year, month, day, hour, min, sec);
        }
        /// <summary>
        /// gets fasttime/tradelink time for now
        /// </summary>
        /// <returns></returns>
        public static int DT2FT() { return DT2FT(DateTime.Now); }
        /// <summary>
        /// converts datetime to fasttime format
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int DT2FT(DateTime d) { return TL2FT(d.Hour, d.Minute, d.Second); }
        /// <summary>
        /// converts tradelink time to fasttime
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="min"></param>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static int TL2FT(int hour, int min, int sec) { return hour * 10000 + min * 100 + sec; }
        /// <summary>
        /// gets fasttime from a tradelink tick
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int TL2FT(Tick t) { return t.Time; }
        /// <summary>
        /// gets elapsed seconds between two fasttimes
        /// </summary>
        /// <param name="firsttime"></param>
        /// <param name="latertime"></param>
        /// <returns></returns>
        public static int FTDIFF(int firsttime, int latertime)
        {
            int span1 = FT2FTS(firsttime);
            int span2 = FT2FTS(latertime);
            return span2 - span1;
        }
        /// <summary>
        /// converts fasttime to fasttime span, or elapsed seconds
        /// 获得fasttime对应的秒数
        /// </summary>
        /// <param name="fasttime"></param>
        /// <returns></returns>
        public static int FT2FTS(int fasttime)
        {
            int s1 = fasttime % 100;
            int m1 = ((fasttime - s1) / 100) % 100;
            int h1 = (int)((fasttime - (m1 * 100) - s1) / 10000);
            return h1 * 3600 + m1 * 60 + s1;
        }
        /// <summary>
        /// adds fasttime and fasttimespan (in seconds).  does not rollover 24hr periods.
        /// </summary>
        /// <param name="firsttime"></param>
        /// <param name="secondtime"></param>
        /// <returns></returns>
        public static int FTADD(int firsttime, int fasttimespaninseconds)
        {
            int s1 = firsttime % 100;
            int m1 = ((firsttime - s1) / 100) % 100;
            int h1 = (int)((firsttime - m1 * 100 - s1) / 10000);
            s1 += fasttimespaninseconds;
            if (s1 >= 60)
            {
                m1 += (int)(s1 / 60);
                s1 = s1 % 60;
            }
            if (m1 >= 60)
            {
                h1 += (int)(m1 / 60);
                m1 = m1 % 60;
            }
            int sum = h1 * 10000 + m1 * 100 + s1;
            return sum;


        }
        /// <summary>
        /// converts fasttime to a datetime
        /// </summary>
        /// <param name="ftime"></param>
        /// <returns></returns>
        public static DateTime FT2DT(int ftime)
        {
            int s = ftime % 100;
            int m = ((ftime - s) / 100) % 100;
            int h = (int)((ftime - m * 100 - s) / 10000);
            return new DateTime(1, 1, 1, h, m, s);
        }

        /// <summary>
        /// 获得某个时间当天最后一刻
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ToTLDateTimeEnd(DateTime datetime)
        {
            return ToTLDateTime(Util.ToTLDate(datetime),235959);
        }

        public static long ToTLDateTimeEnd(long datetime)
        {
            return ToTLDateTimeEnd(ToDateTime(datetime));
        }

        public static long ToTLDateTime(int tldate, int tltime) { return (long)tldate * 1000000 + (long)tltime; }
        /// <summary>
        /// get long for current date + time
        /// </summary>
        /// <returns></returns>
        public static long ToTLDateTime() { return ((long)ToTLDate() * 1000000) + (long)ToTLTime(); }
        /// <summary>
        /// get long for date + time
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long ToTLDateTime(DateTime dt)
        {
            if (dt == DateTime.MinValue) return long.MinValue;
            if (dt == DateTime.MaxValue) return long.MaxValue;

            return ((long)ToTLDate(dt) * 1000000) + (long)ToTLTime(dt);
        }
        /// <summary>
        /// gets TradeLink date for today
        /// </summary>
        /// <returns></returns>
        public static int ToTLDate() { return ToTLDate(DateTime.Now); }
        /// <summary>
        /// Converts a DateTime to TradeLink Date (eg July 11, 2006 = 20060711)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int ToTLDate(DateTime dt)
        {

            return (dt.Year * 10000) + (dt.Month * 100) + dt.Day;
        }
        /// <summary>
        /// Converts a DateTime.Ticks values to TLDate (eg 8million milliseconds since 1970 ~= 19960101 (new years 1996)
        /// </summary>
        /// <param name="DateTimeTicks"></param>
        /// <returns></returns>
        public static int ToTLDate(long DateTimeTicks)
        {
            return ToTLDate(new DateTime(DateTimeTicks));
        }
        /// <summary>
        /// gets tradelink time for now
        /// </summary>
        /// <returns></returns>
        public static int ToTLTime() { return DT2FT(DateTime.Now); }
        /// <summary>
        /// gets tradelink time from date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int ToTLTime(DateTime date)
        {
            return DT2FT(date);
        }


        /// <summary>
        /// Converts a TLDate integer format into an array of ints
        /// </summary>
        /// <param name="fulltime">The fulltime.</param>
        /// <returns>int[3] { year, month, day}</returns>
        static int[] TLDateSplit(int fulltime)
        {
            int[] splittime = new int[3]; // year, month, day
            splittime[2] = (int)((double)fulltime / 10000);
            splittime[1] = (int)((double)(fulltime - (splittime[2] * 10000)) / 100);
            double tmp = (int)((double)fulltime / 100);
            double tmp2 = (double)fulltime / 100;
            splittime[0] = (int)(Math.Round(tmp2 - tmp, 2, MidpointRounding.AwayFromZero) * 100);
            return splittime;
        }

        public static string GetRandomString(int length)
        {
            return GetRandomString(length, true, true, true, false, string.Empty);
        }
        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }

        /// <summary>
        /// Tests if two dates are the same, given a mask as DateMatchType.
        /// 
        /// ie, 20070605 will match 20070705 if DateMatchType is Day or Year.
        /// </summary>
        /// <param name="fulldate">The fulldate in TLDate format (int).</param>
        /// <param name="matchdate">The matchdate to test against (int).</param>
        /// <param name="dmt">The "mask" that says what to pay attention to when matching.</param>
        /// <returns></returns>
        public static bool TLDateMatch(int fulldate, int matchdate, DateMatchType dmt)
        {
            const int d = 0, m = 1, y = 2;
            if (dmt == DateMatchType.None)
                return false;
            bool matched = true;
            // if we're requesting a day match,
            if ((dmt & DateMatchType.Day) == DateMatchType.Day)
                matched &= TLDateSplit(fulldate)[d] == TLDateSplit(matchdate)[d];
            if ((dmt & DateMatchType.Month) == DateMatchType.Month)
                matched &= TLDateSplit(fulldate)[m] == TLDateSplit(matchdate)[m];
            if ((dmt & DateMatchType.Year) == DateMatchType.Year)
                matched &= TLDateSplit(fulldate)[y] == TLDateSplit(matchdate)[y];
            return matched;
        }


        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string CleanProtocolString(string field)
        {
            if (string.IsNullOrEmpty(field)) return string.Empty;
            return field.Replace(',', ' ').Replace('|',' ').Replace('^',' ');
        }

        #region Security and symbol

        /// <summary>
        /// 获得某个PriceTick对应的数字格式化输出样式
        /// </summary>
        /// <param name="pricetick"></param>
        /// <returns></returns>
        //public static string GetPriceTickFormat(decimal pricetick)
        //{
        //    string[] p = pricetick.ToString().Split('.');
        //    if (p.Length <= 1)
        //        return "{0:F0}";
        //    else
        //        return "{0:F" + p[1].ToCharArray().Length.ToString() + "}";

        //}

        #endregion
        public const string ZEROBUILD = "0";
        /// <summary>
        /// Gets a number representing the build of an installation.
        /// Build is usually stored in VERSION.TXT and full path is presented via filepath.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        [Obsolete]
        public static int BuildFromFile(string filepath)
        {
            string builds = "";
            int build = 0;
            if (File.Exists(filepath))
            {
                try
                {
                    StreamReader sr = new StreamReader(filepath);
                    builds = sr.ReadToEnd();
                    sr.Close();
                    build = Convert.ToInt32(builds);
                }
                catch (Exception) { }
            }
            return build;
        }


        /// <summary>
        /// 清空某个事件的所有绑定的委托
        /// </summary>
        /// <param name="objectHasEvents"></param>
        /// <param name="eventName"></param>
        public static void ClearAllEvents(object objectHasEvents, string eventName)
        {
            if (objectHasEvents == null)
            {
                return;
            }
            try
            {
                EventInfo[] events = objectHasEvents.GetType().GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (events == null || events.Length < 1)
                {
                    return;
                }

                for (int i = 0; i < events.Length; i++)
                {
                    EventInfo ei = events[i];
                    if (ei.Name == eventName)
                    {
                        FieldInfo fi = ei.DeclaringType.GetField(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (fi != null)
                        {
                            fi.SetValue(objectHasEvents, null);
                        }
                        break;
                    }
                }
            }
            catch
            {
            }
        }




        /// <summary>
        /// Gets string representing the version of this suite.
        /// </summary>
        /// <returns></returns>
        //public static string TLVersion()
        //{
        //    int build = 2;// TLBuild();
        //    int maj = (int)((double)build / 1000);
        //    int min = (int)(((double)build - maj * 1000) / 100);
        //    int fix = build - (maj * 1000 + min * 100);
        //    string nicebuild = maj + "." + min + "." + fix;
        //    return nicebuild;
        //}

        /// <summary>
        /// Gets a string representing the identity of this suite.
        /// </summary>
        /// <returns></returns>
        //public static string TLSIdentity()
        //{
        //    return "TradeLinkSuite-" + TLVersion();
        //}


        /// <summary>
        /// 通过过期日 获得合约的月份后缀
        /// </summary>
        /// <param name="expiredate"></param>
        /// <returns></returns>
        //public static string GetSymbolMonth(DateTime expiredate)
        //{ 
            
        //}

        /// <summary>
        /// Obtains a version out of a string that contains version + other information.
        /// </summary>
        /// <param name="ver">string containing version</param>
        /// <returns>version number</returns>
        public static int CleanVer(string ver)
        {
            Regex re = new Regex("[0-9]+");
            Match m = re.Match(ver);
            if (m.Success) return Convert.ToInt32(m.Value);
            return 0;
        }

        /// <summary>
        /// Provide date in TLDate format, returns whether market (NYSE) closes early on this day.
        /// </summary>
        /// <param name="today"></param>
        /// <returns></returns>
        public static bool isEarlyClose(int today)
        {
            try
            {
                return GetCloseTime().Contains(today);
            }
            catch (Exception) { return false; }
        }
        /// <summary>
        /// Gets early close time for a given date.   Returns zero if not an early close.
        /// </summary>
        /// <param name="today"></param>
        /// <returns></returns>
        public static int GetEarlyClose(int today)
        {
            try
            {
                return (int)GetCloseTime()[today];
            }
            catch (Exception) { return 0; }
        }
        static Hashtable GetCloseTime()
        {
            StreamReader f = new StreamReader("EarlyClose.csv");
            string[] r = new string[2];
            string line = "";
            Hashtable h = new Hashtable();
            while ((line = f.ReadLine()) != null)
            {
                r = line.Split(',');
                h.Add(Convert.ToInt32(r[0]), Convert.ToInt32(r[1]));
            }
            f.Close();
            return h;
        }


        /// <summary>
        /// Converts list of trades to a delimited file readable by excel, R, matlab, google spreadsheets, etc.
        /// </summary>
        /// <param name="stocktrades"></param>
        /// <param name="delimiter"></param>
        /// <param name="filepath"></param>
        public static void FillsToText(List<TradeImpl> stocktrades, char delimiter, string filepath)
        { // works on a queue of Trade objects
            StreamWriter sw = new StreamWriter(filepath, false);
            sw.WriteLine("Date,Time,Symbol,Side,xSize,xPrice,Comment");
            foreach (TradeImpl t in stocktrades)
                sw.WriteLine(t.GetTradStr());
            sw.Close();
        }

        /// <summary>
        /// converts a trade to an array of comma-delimited string data also containing closedPL, suitable for output to file for reading by excel, R, matlab, etc.
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        //public static string[] TradesToClosedPL(Trade trade)
        //{
        //    List<Trade> trades = new List<Trade>();
        //    trades.Add(trade);
        //    return TradesToClosedPL(trades);
        //}

        /// <summary>
        /// Converts a list of trades to an array of comma-delimited string data also containing closedPL, suitable for output to file for reading by excel, R, matlab, etc.
        /// </summary>
        /// <param name="tradelist"></param>
        /// <returns></returns>
        public static string[] TradesToClosedPL(List<Trade> tradelist) { return TradesToClosedPL(tradelist, ','); }
        /// <summary>
        /// Converts a list of trades to an array of delimited string data also containing closedPL, suitable for output to file for reading by excel, R, matlab, etc.
        /// </summary>
        /// <param name="tradelist"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string[] TradesToClosedPL(List<Trade> tradelist, char delimiter)
        {
            List<string> rowoutput = new List<string>();
            PositionTracker pt = new PositionTracker();
            bool accept= false;
            foreach (TradeImpl t in tradelist)
            {
                string r = t.GetTradStr() + delimiter;
                string s = t.Symbol;
                decimal cpl = 0;
                decimal opl = 0;
                int csize = 0;
                cpl = pt.Adjust(t,out accept);
                opl = Calc.OpenPL(t.xPrice, pt[s]); // get any leftover open pl
                if (cpl != 0) csize = t.xSize; // if we closed any pl, get the size
                string[] pl = new string[] { opl.ToString("f2", System.Globalization.CultureInfo.InvariantCulture), cpl.ToString("f2", System.Globalization.CultureInfo.InvariantCulture), pt[s].Size.ToString(System.Globalization.CultureInfo.InvariantCulture), csize.ToString(System.Globalization.CultureInfo.InvariantCulture), pt[s].AvgPrice.ToString("f2", System.Globalization.CultureInfo.InvariantCulture) };
                r += string.Join(delimiter.ToString(), pl);
                rowoutput.Add(r);
            }
            return rowoutput.ToArray();

        }
        /// <summary>
        /// Converts a list of trades to delimited text file containing closedPL, suitable for reading by excel, R, matlab, etc.
        /// </summary>
        /// <param name="tradelist"></param>
        /// <param name="delimiter"></param>
        /// <param name="filepath"></param>
        public static void ClosedPLToText(List<Trade> tradelist, char delimiter, string filepath) { ClosedPLToText(tradelist, delimiter, filepath, false); }
        public static void ClosedPLToText(List<Trade> tradelist, char delimiter, string filepath, bool generateheaderOnEmpty) { ClosedPLToText(tradelist, delimiter, filepath, false, false); }
        public static void ClosedPLToText(Trade trade, string filepath)
        {
            List<Trade> trades = new List<Trade>();
            trades.Add(trade);

            ClosedPLToText(trades, ',', filepath, false, true);
        }
        public static void ClosedPLToText(List<Trade> tradelist, char delimiter, string filepath, bool generateheaderOnEmpty, bool append)
        {
            if ((tradelist.Count == 0) && !generateheaderOnEmpty) return;
            bool exists = File.Exists(filepath);
            StreamWriter sw = new StreamWriter(filepath, append);
            string header = string.Join(delimiter.ToString(), Enum.GetNames(typeof(TradePLField)));
            if (!append || !exists)
                sw.WriteLine(header);
            string[] lines = TradesToClosedPL(tradelist, delimiter);
            foreach (string line in lines)
                sw.WriteLine(line);
            sw.Close();
        }



        /// <summary>
        /// gets list of readable tickfiles in top level of a folder.
        /// 2nd dimension of list is size of file in bytes (as string)
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static string[,] TickFileIndex(string folder) { return TickFileIndex(folder, TikConst.WILDCARD_EXT); }
        /// <summary>
        /// builds list of readable tick files with given extension found in top level of folder
        /// </summary>
        /// <param name="Folder"></param>
        /// <param name="tickext"></param>
        /// <returns></returns>
        public static string[,] TickFileIndex(string Folder, string tickext) { return TickFileIndex(Folder, tickext, false, null); }
        /// <summary>
        /// builds list of readable tickfiles found in given folder
        /// </summary>
        /// <param name="Folder">path containing tickfiles</param>
        /// <param name="tickext">file extension</param>
        /// <returns></returns>
        public static string[,] TickFileIndex(string Folder, string tickext, bool searchSubFolders) { return TickFileIndex(Folder, tickext, searchSubFolders, null); }
        /// <summary>
        /// builds list of readable tickfiles (and their byte-size) found in folder
        /// </summary>
        /// <param name="Folder"></param>
        /// <param name="tickext"></param>
        /// <param name="searchSubFolders"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static string[,] TickFileIndex(string Folder, string tickext, bool searchSubFolders, DebugDelegate debug)
        {
            string[] _tickfiles = Directory.GetFiles(Folder, tickext);
            DirectoryInfo di = new DirectoryInfo(Folder);
            FileInfo[] fi = di.GetFiles(tickext, searchSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            string[,] index = new string[_tickfiles.Length, 2];
            int i = 0;
            int qtr = fi.Length / 4;
            foreach (FileInfo thisfi in fi)
            {
                if ((debug != null) && (i % qtr == 0))
                    debug((fi.Length - i).ToString("N0") + " files remaining to index...");
                index[i, 0] = thisfi.Name;
                index[i, 1] = thisfi.Length.ToString();
                i++;
            }
            return index;
        }



        public static string decode(string data)
        {
            string s = string.Empty;
            for (int i = 0; i <= data.Length - 2; i += 2)
                s += Convert.ToChar(Convert.ToUInt32(data.Substring(i, 2), 16)).ToString();
            return s;
        }

        /// <summary>
        /// dumps public properties and fields of an object as an xml string
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string DumpObjectProperties(Object o) { return DumpObjectProperties(o, null); }
        /// <summary>
        /// dumps public properties and fields as xml string, with optional debugging for errors
        /// </summary>
        /// <param name="o"></param>
        /// <param name="deb"></param>
        /// <returns></returns>
        public static string DumpObjectProperties(Object o, DebugDelegate deb) { return DumpObjectProperties(o, true, deb); }
        public static string DumpObjectProperties(Object o, bool stripheader, DebugDelegate deb)
        {
            try
            {
                System.Type t = o.GetType();
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(t);
                System.IO.StringWriter sw = new System.IO.StringWriter();
                xs.Serialize(sw, o);
                sw.Close();
                string xml = sw.ToString();
                return stripheader ? stripxmlhead(xml) : xml;
            }
            catch (Exception ex)
            {
                string inner = string.Empty;
                try
                {
                    inner = (ex.InnerException == null) ? string.Empty : " inner err: " + ex.InnerException.Message + ex.InnerException.StackTrace;
                }
                catch { }
                if (deb != null)
                    deb("Error dumping: " + o.ToString() + " " + ex.Message + ex.StackTrace + inner);
            }
            return string.Empty;
        }

        static string stripxmlhead(string xml)
        {
            xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
            xml = xml.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
            return xml;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PROGRAM"></param>
        /// <returns></returns>
        public static int ProgramCount(string PROGRAM)
        {
            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcesses();
            int count = 0;
            foreach (System.Diagnostics.Process p in ps)
                if (p.ProcessName.ToLower().Contains(PROGRAM.ToLower()))
                    count++;
            return count;
        }

        /// <summary>
        /// 2.0.1 = 20001
        /// 2.1.1 = 20101
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static int GetBuildNum(string version)
        {
            string[] rec = version.Split('.');
            int major;
            int minor;
            int fix;
            int.TryParse(rec[0], out major);
            int.TryParse(rec[1], out minor);
            int.TryParse(rec[2], out fix);

            return GetBuildNum(major, minor, fix);
        }

        public static int GetBuildNum(int major, int minor, int fix)
        {
            return major * 10000 + minor * 100 + fix;
        }

        #region Serialize Deserialize
        /// <summary>
        /// convert any structure/type to a string (can be converted back using Deserialize)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string Serialize<T>(T o) { return Serialize(o, false, null); }
        /// <summary>
        /// convert any structure/type to a string (can be converted back using Deserialize)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static string Serialize<T>(T o, DebugDelegate debug) { return Serialize(o, false, debug); }
        /// <summary>
        /// convert any structure/type to a string (can be converted back using Deserialize)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="compress"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static string Serialize<T>(T o, bool compress, DebugDelegate debug)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                System.IO.StringWriter sw = new System.IO.StringWriter(sb);
                XmlSerializer xs = new XmlSerializer(o.GetType());
                xs.Serialize(sw, o);
                sw.Close();
                if (!compress)
                    return sb.ToString();
                string r = GZip.Compress(sb.ToString());
                return r;
            }
            catch (Exception ex)
            {
                if (debug != null)
                {
                    string inner = string.Empty;
                    try
                    {
                        inner = (ex.InnerException == null) ? string.Empty : " inner err: " + ex.InnerException.Message + ex.InnerException.StackTrace;
                    }
                    catch { }
                    debug("unable to save " + Util.DumpObjectProperties(o));
                    debug(ex.Message + ex.StackTrace + inner);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// deserialize a structure/type from a string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string msg) { return Deserialize<T>(msg, false, null); }
        /// <summary>
        /// deserialize a structure/type from a string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string msg, DebugDelegate debug) { return Deserialize<T>(msg, false, debug); }
        /// <summary>
        /// deserialize a structure/type from a string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="uncompress"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string msg, bool uncompress, DebugDelegate debug)
        {

            try
            {
                if (uncompress)
                    msg = GZip.Uncompress(msg);
                System.IO.StringReader sr = new System.IO.StringReader(msg);
                XmlSerializer xs = new XmlSerializer(typeof(T));
                T O = (T)xs.Deserialize(sr);
                sr.Close();
                return O;
            }
            catch (Exception ex)
            {
                if (debug != null)
                {
                    string inner = string.Empty;
                    try
                    {
                        inner = (ex.InnerException == null) ? string.Empty : " inner err: " + ex.InnerException.Message + ex.InnerException.StackTrace;
                    }
                    catch { }
                    debug("UNABLE TO read: " + msg);
                    debug(ex.Message + ex.StackTrace + inner);
                }
            }
            return default(T);
        }
        #endregion

        #region Serialize Deserialize File
        /// <summary>
        /// create an xml file from any data structure (can be restored with FromFile)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool ToFile<T>(T o, string file) { return ToFile<T>(o, file, null); }
        /// <summary>
        /// create an xml file from any data structure (can be restored with FromFile)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="file"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static bool ToFile<T>(T o, string file, DebugDelegate debug)
        {
            try
            {
                string msg = Serialize(o, debug);
                if (msg == string.Empty) return false;
                System.IO.StreamWriter sw = new System.IO.StreamWriter(file, false);
                sw.WriteLine(msg);
                sw.Close();

            }
            catch (Exception ex)
            {
                if (debug != null)
                {
                    debug("error writing filename: " + file);
                    debug(ex.Message + ex.StackTrace);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// restore data structure(s) from a file (created with ToFile)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool FromFile<T>(string file, ref T o) { return FromFile(file, ref o, null); }
        /// <summary>
        /// restore data structure(s) from a file (created with ToFile)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="o"></param>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static bool FromFile<T>(string file, ref T o, DebugDelegate debug)
        {
            try
            {
                if (!File.Exists(file))
                {
                    if (debug != null)
                    {
                        debug("file does not exist: " + file);
                        return false;
                    }
                }
                System.IO.StreamReader sr = new System.IO.StreamReader(file);
                o = Deserialize<T>(sr.ReadToEnd(), debug);
                sr.Close();
            }
            catch (Exception ex)
            {
                if (debug != null)
                {
                    debug("error writing filename: " + file);
                    debug(ex.Message + ex.StackTrace);
                    return false;
                }
            }
            return (o != null) && (o.GetType() == typeof(T));
        }
        #endregion



    }


        public enum TickFileType
        {
            Invalid = 0,
            EPF,
            IDX,
            TIK,
        }

        public struct TickFileInfo
        {
            public string symbol;
            public DateTime date;
            public TickFileType type;
        }

        public enum DateMatchType
        {
            None = 0,
            Day = 1,
            Month = 2,
            Year = 4,
        }

        public enum TradePLField
        {
            Date = 0,
            Time,
            Symbol,
            Side,
            xSize,
            xPrice,
            Comment,
            OpenPL,
            ClosedPL,
            OpenSize,
            ClosedSize,
            AvgPrice,
        }
    



    
}
