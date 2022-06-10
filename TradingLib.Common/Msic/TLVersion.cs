using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using System.ComponentModel;

namespace TradingLib.Common
{
    /// <summary>
    /// 产品类别
    /// 不同的产品类别 管理终端加载的界面会有区别，这里通过产品类别进行标识
    /// </summary>
    public enum QSEnumProductType
    { 
        /// <summary>
        /// 分帐户柜台交易系统
        /// </summary>
        [Description("分账户柜台系统")]
        CounterSystem,

        /// <summary>
        /// 主帐户监控系统
        /// </summary>
        [Description("主帐户监控系统")]
        VendorMoniter,
    }

    /// <summary>
    /// 系统版本信息
    /// </summary>
    public class TLVersion
    {
        /// <summary>
        /// 主版本号
        /// </summary>
        public int  Major { get; set; }
        /// <summary>
        /// 次版本号
        /// </summary>
        public int Minor { get; set; }
        /// <summary>
        /// 修正版本号
        /// </summary>
        public int Fix { get; set; }

        /// <summary>
        /// 版本日期
        /// </summary>
        public int Date { get; set; }

        /// <summary>
        /// 产品类别
        /// </summary>
        public QSEnumProductType ProductType { get; set; }


        public PlatformID Platfrom { get; set; }

        public override string ToString()
        {
            return string.Format("{0}-{1}", this.DeployID, this.Version);
        }
        /// <summary>
        /// 部署编号 用于区分不同柜台的部署编号
        /// </summary>
        public string DeployID { get; set; }

        string _version = null;
        public string Version
        {
            get
            { 
                if(_version == null)
                {
                    _version = string.Format("{0}.{1}.{2}.{3}",this.Major,this.Minor,this.Fix,this.Date);
                }
                return _version;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int BuildNum
        {
            get
            {
                return this.Major * 10000 + this.Minor * 100 + Fix;
            }
        }

        public static string Serialize(TLVersion version)
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6}", version.Major, version.Minor, version.Fix, version.Date, version.ProductType,version.Platfrom,version.DeployID);
        }

        public static TLVersion Deserialize(string content)
        {
            string[] rec = content.Split(',');
            TLVersion v = new TLVersion();
            v.Major = int.Parse(rec[0]);
            v.Minor = int.Parse(rec[1]);
            v.Fix = int.Parse(rec[2]);
            v.Date = int.Parse(rec[3]);
            v.ProductType = (QSEnumProductType)Enum.Parse(typeof(QSEnumProductType), rec[4]);
            v.Platfrom = (PlatformID)Enum.Parse(typeof(PlatformID), rec[5]);
            v.DeployID = rec[6];
            return v;
        }

       
    }
}
