///////////////////////////////////////////////////////////////////////////////////////
// 用于客户端登入
// 客户端将登入信息和必要的复杂信息发送到服务端，服务都鉴权后将鉴权结果返回给客户端
//
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TradingLib.API;

namespace TradingLib.Common
{
    public delegate void LoginRequestDel<T>(T clientinfo,LoginRequest request, ref LoginResponse response) where T:ClientInfoBase;
    public delegate void LoginResponseDel(LoginResponse response);

    /// <summary>
    /// loginType =0 UCenter登入
    /// loginType =1 交易帐号登入
    /// 
    /// ServiceType = 0 模拟帐号
    /// ServiceType = 1 实盘
    /// </summary>
    public class LoginRequest:RequestPacket
    {
        public string LoginID{get;set;}
        public string Passwd { get; set; }
        public int LoginType { get; set; }
        public int ServiceType { get; set; }
        public string MAC { get; set; }
        public string IPAddress { get; set; }
        public string ProductInfo { get; set; }
        
        public LoginRequest()
        {
            _type = MessageTypes.LOGINREQUEST;
            LoginID = "";
            Passwd = "";
            MAC = "";
            LoginType = 0;
            ServiceType = 0;
            IPAddress = string.Empty;
            ProductInfo = string.Empty;
        }


        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="lr"></param>
        /// <returns></returns>
        public override string ContentSerialize()
        { 
            StringBuilder sb = new StringBuilder();
            char d =',';
            sb.Append(this.LoginID);
            sb.Append(d);
            sb.Append(this.Passwd);
            sb.Append(d);
            sb.Append(this.LoginType.ToString());
            sb.Append(d);
            sb.Append(this.ServiceType.ToString());
            sb.Append(d);
            sb.Append(this.MAC);
            sb.Append(d);
            sb.Append(this.IPAddress);
            sb.Append(d);
            sb.Append(this.ProductInfo);
            return sb.ToString();
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="lrstr"></param>
        /// <returns></returns>
        public override void ContentDeserialize(string lrstr)
        {
            string[] r = lrstr.Split(',');
            this.LoginID = r[0];
            this.Passwd = r[1];
            this.LoginType = int.Parse(r[2]);
            this.ServiceType = int.Parse(r[3]);
            this.MAC = r[4];
            this.IPAddress = r[5];
            this.ProductInfo = r[6];

        }
    }

    public class LoginResponse :RspResponsePacket
    {
        
        public bool Authorized { get; set; }
        /// <summary>
        /// 登入ID
        /// </summary>
        public string LoginID { get; set; }
        /// <summary>
        /// 交易帐号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 前置编号
        /// </summary>
        public string FrontUUID { get; set; }
        /// <summary>
        /// 客户ID UUID用于定位客户端的标识
        /// </summary>
        public string ClientUUID { get; set; }
        /// <summary>
        /// 当前日期
        /// </summary>
        public int TradingDay { get; set; }

        /// <summary>
        /// 前置整数编号
        /// </summary>
        public int FrontIDi { get; set; }

        /// <summary>
        /// 会话整数编号
        /// </summary>
        public int SessionIDi { get; set; }

        /// <summary>
        /// 交易帐户类别
        /// </summary>
        public QSEnumAccountCategory AccountType { get; set; }

        /// <summary>
        /// 用户全局ID
        /// </summary>
        public int UserID { get; set; }

        string _nickname = string.Empty;
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get { return _nickname; } set { _nickname = value.Replace(',', ' ').Replace('|', ' ').Replace('^', ' '); } }

        /// <summary>
        /// 邮件地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 账户货币类别
        /// </summary>
        public CurrencyType Currency { get; set; }

        public LoginResponse()
            : this("")
        {
            _type = MessageTypes.LOGINRESPONSE;

        }
        public LoginResponse(string loginid)
        {
            Authorized = false;
            LoginID = loginid;
            Account = "";
            FrontUUID = "";
            ClientUUID = "";
            TradingDay = 0;
            FrontIDi = 0;
            SessionIDi = 0;
            AccountType = QSEnumAccountCategory.SUBACCOUNT;
            UserID = 0;
            NickName = string.Empty;
            Email = string.Empty;
            Mobile = string.Empty;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="lr"></param>
        /// <returns></returns>
        public override string ResponseSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.Authorized.ToString());
            sb.Append(d);
            sb.Append(this.LoginID);
            sb.Append(d);
            sb.Append(this.Account);
            sb.Append(d);
            sb.Append(this.FrontUUID);
            sb.Append(d);
            sb.Append(this.ClientUUID);
            sb.Append(d);
            sb.Append(this.TradingDay);
            sb.Append(d);
            sb.Append(this.FrontIDi);
            sb.Append(d);
            sb.Append(this.SessionIDi);
            sb.Append(d);
            sb.Append(this.AccountType);
            sb.Append(d);
            sb.Append(this.NickName);
            sb.Append(d);
            sb.Append(this.Email);
            sb.Append(d);
            sb.Append(this.Mobile);
            sb.Append(d);
            sb.Append(this.Currency);
            return sb.ToString();
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="lrstr"></param>
        /// <returns></returns>
        public override void ResponseDeserialize(string lrstr)
        {
            string[] r = lrstr.Split(',');
            this.Authorized = bool.Parse(r[0]);
            this.LoginID = r[1];
            this.Account = r[2];
            this.FrontUUID = r[3];
            this.ClientUUID = r[4];
            this.TradingDay = int.Parse(r[5]);
            this.FrontIDi = int.Parse(r[6]);
            this.SessionIDi = int.Parse(r[7]);
            this.AccountType = (QSEnumAccountCategory)Enum.Parse(typeof(QSEnumAccountCategory), r[8]);
            this.NickName = r[9];
            this.Email = r[10];
            this.Mobile = r[11];
            this.Currency = r[12].ParseEnum<CurrencyType>();
        }
    }
}
