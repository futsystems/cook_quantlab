using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    //public class CashTransaction
    //{
    //    /// <summary>
    //    /// 交易帐号
    //    /// </summary>
    //    public string Account { get; set; }

    //    /// <summary>
    //    /// 结算日
    //    /// </summary>
    //    public int Settleday { get; set; }

    //    /// <summary>
    //    /// 出入金操作时间
    //    /// </summary>
    //    public DateTime DateTime { get; set; }

    //    /// <summary>
    //    /// 金额
    //    /// </summary>
    //    public decimal Amount { get; set; }

    //    /// <summary>
    //    /// 出入金备注
    //    /// </summary>
    //    public string Comment { get; set; }

    //    /// <summary>
    //    /// 出入金流水号
    //    /// </summary>
    //    public string TransRef { get; set; }

    //    public CashTransaction()
    //    {
    //        this.Account = string.Empty;
    //        this.Settleday = 0;
    //        this.DateTime = DateTime.Now;
    //        this.Amount = 0;
    //        this.Comment = string.Empty;
    //        this.TransRef = string.Empty;
    //    }

    //    public static string Serialize(CashTransaction c)
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        char d =',';
    //        sb.Append(c.Account);
    //        sb.Append(d);
    //        sb.Append(c.Settleday.ToString());
    //        sb.Append(d);
    //        sb.Append(Util.ToTLDateTime(c.DateTime).ToString());
    //        sb.Append(d);
    //        sb.Append(c.Amount.ToString());
    //        sb.Append(d);
    //        sb.Append(c.Comment);
    //        sb.Append(d);
    //        sb.Append(c.TransRef);

    //        return sb.ToString();
    //    }

    //    public static CashTransaction Deserialize(string msg)
    //    {
    //        string[] rec = msg.Split(',');
    //        CashTransaction s = new CashTransaction();
    //        s.Account = rec[0];
    //        s.Settleday = int.Parse(rec[1]);
    //        s.DateTime = Util.ToDateTime(long.Parse(rec[2]));
    //        s.Amount = decimal.Parse(rec[3]);
    //        s.Comment = rec[4];
    //        s.TransRef = rec[5];
    //        return s;
    //    }
    //}
}
