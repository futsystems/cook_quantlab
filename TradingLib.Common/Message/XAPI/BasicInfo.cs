using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 查询交易所信息
    /// </summary>
    public class XQryExchangeRequuest : RequestPacket
    {
        public XQryExchangeRequuest()
        {
            _type = MessageTypes.XQRYEXCHANGE;
        }

        public override string ContentSerialize()
        {
            return string.Empty;
        }

        public override void ContentDeserialize(string contentstr)
        {

        }
    }

    /// <summary>
    /// 查询交易所回报
    /// </summary>
    public class RspXQryExchangeResponse : RspResponsePacket
    {
        public RspXQryExchangeResponse()
        {
            _type = MessageTypes.XEXCHANGERESPNSE;
            Exchange = null;
        }

        public ExchangeImpl Exchange { get; set; }
        public override string ResponseSerialize()
        {
            if (this.Exchange == null) return string.Empty;
            return TradingLib.Common.ExchangeImpl.Serialize(this.Exchange);
        }

        public override void ResponseDeserialize(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                this.Exchange = null;
                return;
            }
            this.Exchange = TradingLib.Common.ExchangeImpl.Deserialize(content);
        }
    }


    public class XQryMarketTimeRequest : RequestPacket
    {
        public XQryMarketTimeRequest()
        {
            _type = MessageTypes.XQRYMARKETTIME;
        }
        public override string ContentSerialize()
        {
            return string.Empty;
        }

        public override void ContentDeserialize(string contentstr)
        {

        }

    }

    public class RspXQryMarketTimeResponse : RspResponsePacket
    {
        public RspXQryMarketTimeResponse()
        {
            _type = MessageTypes.XMARKETTIMERESPONSE;
            this.MarketTime = null;
        }

        public MarketTimeImpl MarketTime { get; set; }

        public override string ResponseSerialize()
        {
            if (this.MarketTime == null)
                return string.Empty;
            return TradingLib.Common.MarketTimeImpl.Serialize(this.MarketTime);
        }

        public override void ResponseDeserialize(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                this.MarketTime = null;
                return;
            }
            this.MarketTime = TradingLib.Common.MarketTimeImpl.Deserialize(content);
        }
    }

    /// <summary>
    /// 查询品种
    /// </summary>
    public class XQrySecurityRequest : RequestPacket
    {
        public XQrySecurityRequest()
        {
            _type = MessageTypes.XQRYSECURITY;
        }
        public override string ContentSerialize()
        {
            return string.Empty;
        }

        public override void ContentDeserialize(string contentstr)
        {

        }
    }

    /// <summary>
    /// 查询品种回报
    /// </summary>
    public class RspXQrySecurityResponse : RspResponsePacket
    {
        public RspXQrySecurityResponse()
        {
            _type = MessageTypes.XSECURITYRESPONSE;
            SecurityFaimly = null;
        }

        public SecurityFamilyImpl SecurityFaimly { get; set; }

        public override string ResponseSerialize()
        {
            return SecurityFamilyImpl.Serialize(this.SecurityFaimly);
        }

        public override void ResponseDeserialize(string content)
        {
            this.SecurityFaimly = SecurityFamilyImpl.Deserialize(content);
        }
    }

    /// <summary>
    /// 查询合约信息
    /// </summary>
    public class XQrySymbolRequest : RequestPacket
    {
        public XQrySymbolRequest()
        {
            _type = MessageTypes.XQRYSYMBOL;
            this.Symbol = string.Empty;
            this.Exchange = string.Empty;
        }

        public string Exchange { get; set; }
        public string Symbol { get; set; }
        public override string ContentSerialize()
        {
            return this.Exchange+","+this.Symbol;
        }

        public override void ContentDeserialize(string contentstr)
        {
            if (string.IsNullOrEmpty(contentstr))
            {
                this.Exchange = string.Empty;
                this.Symbol = string.Empty;
            }
            else
            {
                string[] rec = contentstr.Split(',');
                this.Exchange = rec[0];
                this.Symbol = rec[1];
            }
        }

    }

    /// <summary>
    /// 合约信息回报
    /// </summary>
    public class RspXQrySymbolResponse : RspResponsePacket
    {
        public RspXQrySymbolResponse()
        {
            _type = MessageTypes.XSYMBOLRESPONSE;
            this.Symbol = null;
        }

        public SymbolImpl Symbol { get; set; }

        public override string ResponseSerialize()
        {
            return SymbolImpl.Serialize(this.Symbol);
        }

        public override void ResponseDeserialize(string content)
        {
            this.Symbol = SymbolImpl.Deserialize(content);
        }
    }

    /// <summary>
    /// 查询行情快照请求
    /// </summary>
    public class XQryTickSnapShotRequest : RequestPacket
    {
        public XQryTickSnapShotRequest()
        {
            _type = MessageTypes.XQRYTICKSNAPSHOT;

            this.Exchange = string.Empty;
            this.Symbol = string.Empty;
        }

        public string Exchange { get; set; }
        public string Symbol { get; set; }

        public override string ContentSerialize()
        {
            return string.Format("{0},{1}", this.Exchange, this.Symbol);
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            if (rec.Length == 2)
            {
                this.Exchange = rec[0];
                this.Symbol = rec[1];
            }
        }

    }

    /// <summary>
    /// 行情快照回报
    /// </summary>
    public class RspXQryTickSnapShotResponse : RspResponsePacket
    {
        public RspXQryTickSnapShotResponse()
        {
            _type = MessageTypes.XTICKSNAPSHOTRESPONSE;
            this.Tick = null;
        }

        public Tick Tick { get; set; }

        public override string ResponseSerialize()
        {
            if (this.Tick == null)
                return string.Empty;
            return TickImpl.Serialize(this.Tick);
        }

        public override void ResponseDeserialize(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                this.Tick = null;
                return;
            }
            this.Tick = TickImpl.Deserialize(content);
        }
    }


    /// <summary>
    /// 查交易账户请求
    /// </summary>
    public class XQryAccountRequest : RequestPacket
    {
        public XQryAccountRequest()
        {
            _type = MessageTypes.XQRYACCOUNT;
        }

      
        public override string ContentSerialize()
        {
            return string.Empty;
        }

        public override void ContentDeserialize(string contentstr)
        {
          
        }

    }

    /// <summary>
    /// 查询交易账户回报
    /// </summary>
    public class RspXQryAccountResponse : RspResponsePacket
    {
        public RspXQryAccountResponse()
        {
            _type = MessageTypes.XACCOUNTRESPONSE;
            this.Account = null;
        }

        public AccountItem Account { get; set; }

        public override string ResponseSerialize()
        {
            if (this.Account == null)
                return string.Empty;
            return AccountItem.Serialize(this.Account);
        }

        public override void ResponseDeserialize(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                this.Account = null;
                return;
            }
            this.Account = AccountItem.Deserialize(content);
        }
    }



    /// <summary>
    /// 查交易账户财务信息请求
    /// </summary>
    public class XQryAccountFinanceRequest : RequestPacket
    {
        public XQryAccountFinanceRequest()
        {
            _type = MessageTypes.XQRYACCOUNTFINANCE;
        }


        public override string ContentSerialize()
        {
            return string.Empty;
        }

        public override void ContentDeserialize(string contentstr)
        {

        }

    }

    /// <summary>
    /// 查询交易账户财务信息回报
    /// </summary>
    public class RspXQryAccountFinanceResponse : RspResponsePacket
    {
        public RspXQryAccountFinanceResponse()
        {
            _type = MessageTypes.XQRYACCOUNTFINANCERESPONSE;
            this.Report = null;
        }

        public AccountInfo Report { get; set; }

        public override string ResponseSerialize()
        {
            if (this.Report == null)
                return string.Empty;
            return AccountInfo.Serialize(this.Report);
        }

        public override void ResponseDeserialize(string content)
        {
            if (string.IsNullOrEmpty(content))
                this.Report = null;
            else
                this.Report = AccountInfo.Deserialize(content);
        }
    }


    /// <summary>
    /// 查询汇率信息
    /// </summary>
    public class XQryExchangeRateRequest : RequestPacket
    {
        public XQryExchangeRateRequest()
        {
            _type = MessageTypes.XQRYEXCHANGERATE;
        }

        public override string ContentSerialize()
        {
            return string.Empty;
        }

        public override void ContentDeserialize(string contentstr)
        {

        }
    }

    /// <summary>
    /// 查询汇率信息回报
    /// </summary>
    public class RspXQryExchangeRateResponse : RspResponsePacket
    {
        public RspXQryExchangeRateResponse()
        {
            _type = MessageTypes.XQRYEXCHANGERATERESPONSE;
            ExchangeRate = null;
        }

        public ExchangeRate ExchangeRate { get; set; }
        public override string ResponseSerialize()
        {
            if (this.ExchangeRate == null) return string.Empty;
            return TradingLib.Common.ExchangeRate.Serialize(this.ExchangeRate);
        }

        public override void ResponseDeserialize(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                this.ExchangeRate = null;
                return;
            }
            this.ExchangeRate = TradingLib.Common.ExchangeRate.Deserialize(content);
        }
    }

}
