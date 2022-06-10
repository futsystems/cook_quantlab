using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using NodaTime;
using NodaTime.Extensions;

namespace TradingLib.Common
{


    public class ExchangeImpl : Exchange
    {

        private int _id=0;//数据库编号
        private string _ex;//交易所代码
        private string _name;//交易所名称
        private Country _country;//交易所所处国家
        private string _title;//简称
        private string _calendar;//假日
        private string _timezoneid;//时区信息
        private int _closetime;//收盘时间
        private QSEnumSettleType _settletype;//结算方式
        private QSEnumDataFeedTypes _datafeed;//行情源
        /// <summary>
        /// 交易所数据库编号
        /// </summary>
        public int ID { get { return _id; } set { _id = value; } }

        /// <summary>
        /// 交易所编码
        /// </summary>
        public string EXCode { get { return _ex; } set { _ex = value; } }

        /// <summary>
        /// 交易所名称
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }

        /// <summary>
        /// 国家
        /// </summary>
        public Country Country { get { return _country; } set { _country = value; } }

        /// <summary>
        /// 交易所简称
        /// </summary>
        public string Title { get { return _title; } set { _title = value; } }

        /// <summary>
        /// 假日
        /// </summary>
        public string Calendar { get { return _calendar; } set { _calendar=value; } }

        /// <summary>
        /// 时区
        /// </summary>
        public string TimeZoneID { get { return _timezoneid; } set { _genTimeZone = false; _timezoneid = value; } }

        /// <summary>
        /// 收盘时间
        /// </summary>
        public int CloseTime { get { return _closetime; } set { _closetime = value; } }

        /// <summary>
        /// 结算方式
        /// </summary>
        public QSEnumSettleType SettleType { get { return _settletype; } set { _settletype = value; } }

        public QSEnumDataFeedTypes DataFeed { get { return _datafeed; } set { _datafeed = value; } }
        public ExchangeImpl()
        {
            this.ID = 0;
            this.EXCode = "";
            this.Name = "";
            this.Country = Country.CN;
            this.TimeZoneID = "";
            this.Title = "";
            this.DataFeed = QSEnumDataFeedTypes.DEFAULT;
        }


        //TimeZoneInfo _targetTimeZone = null;
        bool _genTimeZone = false;

        //public TimeZoneInfo TimeZoneInfo
        //{
        //    get
        //    {
        //        if (!_genTimeZone)//延迟生成时区对象
        //        {
        //            _genTimeZone = true;
        //            if (string.IsNullOrEmpty(this.TimeZone))
        //            {
        //                _targetTimeZone = null;//没有提供具体市区信息则与本地系统时间一致
        //            }
        //            else
        //            {
        //                _targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(this.TimeZone);
        //                //DateTime t = new DateTime(2015,1,1,1,1,1,DateTimeKind.
        //            }
        //        }
        //        return _targetTimeZone;
        //    }
        //}


        DateTimeZone _exTz = null;
        NodaTime.DateTimeZone DateTimeZone
        {
            get
            {
                if (!_genTimeZone)
                {
                    _genTimeZone = true;
                    //如果没有具体提供时区ID则我们使用系统默认时区ID
                    if (string.IsNullOrEmpty(this.TimeZoneID))
                    {
                        _exTz = NodaTime.DateTimeZoneProviders.Tzdb.GetZoneOrNull("Asia/Shanghai");
                    }
                    else //如果提供了时区ID则通过ID查找对应的时区
                    {
                        _exTz = NodaTime.DateTimeZoneProviders.Tzdb.GetZoneOrNull(this.TimeZoneID);
                    }
                }
                if (_exTz == null)
                {
                    throw new ArgumentNullException("Exchange TimeZoneID not exist");
                }
                return _exTz;
            }
        }




        /// <summary>
        /// 获得交易所当前时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetExchangeTime()
        {
            return DateTime.UtcNow; //SystemClock.Instance.InZone(this.DateTimeZone).ToDateTimeUnspecified();
        }

        /// <summary>
        /// 将系统时间转换成交易所时间
        /// </summary>
        /// <param name="systime"></param>
        /// <returns></returns>
        public DateTime ConvertToExchangeTime(DateTime systime)
        {
            if (systime.Kind != DateTimeKind.Local)
            {
                throw new ArgumentException("Systime should be local DateTime");
            }
            return Instant.FromDateTimeUtc(systime.ToUniversalTime()).InZone(this.DateTimeZone).ToDateTimeUnspecified();
        }

        /// <summary>
        /// 将交易所时间转换成系统时间
        /// </summary>
        /// <param name="extime"></param>
        /// <returns></returns>
        public DateTime ConvertToSystemTime(DateTime extime)
        {
            if(extime.Kind != DateTimeKind.Unspecified)
            {
                throw new ArgumentException("Exchange DateTime can not timezone aware");
            }
            //获得交易所本地时间
            LocalDateTime ldt = LocalDateTime.FromDateTime(extime);

            //DateTime 转换成UTC时间 然后通过UTC标注时间生成Instance,结合时区对象获得ZonedDateTime 从而可将时间转换到任意时区
            //new ZonedDateTime(ldt,_exTz,)
            ZonedDateTime dt = this.DateTimeZone.AtStrictly(ldt);

            NodaTime.DateTimeZone systz = NodaTime.DateTimeZoneProviders.Tzdb.GetZoneOrNull("Asia/Shanghai");
            //dt.ToInstant().InZone(systz).ToDateTimeUnspecified();
            return dt.WithZone(systz).ToDateTimeUnspecified();
        }


        public override string ToString()
        {
            return "ID:" + ID.ToString() + " Code:" + EXCode.ToString() + " Name:" + Name.ToString() + " Country:" + Country.ToString(); 
        }


        public static string Serialize(ExchangeImpl ex)
        {
            if (ex == null) return string.Empty;
            StringBuilder sb = new StringBuilder();
            char d = ',';

            sb.Append(ex.ID.ToString());
            sb.Append(d);
            sb.Append(ex.EXCode.ToString());
            sb.Append(d);
            sb.Append(ex.Name);
            sb.Append(d);
            sb.Append(ex.Country.ToString());
            sb.Append(d);
            sb.Append(ex.Title);
            sb.Append(d);
            sb.Append(ex.Calendar);
            sb.Append(d);
            sb.Append(ex.TimeZoneID);
            sb.Append(d);
            sb.Append(ex.CloseTime);
            sb.Append(d);
            sb.Append(ex.SettleType.ToString());
            sb.Append(d);
            sb.Append(ex.DataFeed);
            return sb.ToString();
        }

        public static ExchangeImpl Deserialize(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            ExchangeImpl ex = new ExchangeImpl();

            string[] rec = content.Split(',');
            ex.ID = int.Parse(rec[0]);
            ex.EXCode = rec[1];
            ex.Name = rec[2];
            ex.Country = (Country)Enum.Parse(typeof(Country), rec[3]);
            ex.Title = rec[4];
            ex.Calendar = rec[5];
            ex.TimeZoneID = rec[6];
            ex.CloseTime = int.Parse(rec[7]);
            ex.SettleType = (QSEnumSettleType)Enum.Parse(typeof(QSEnumSettleType), rec[8]);
            ex.DataFeed = rec[9].ParseEnum<QSEnumDataFeedTypes>();
            return ex;

        }
    }
}
