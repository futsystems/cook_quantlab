//Copyright 2013 by FutSystems,Inc.
//20161223 删除前置类别 确认服务端保持前置中立 将差异化的特性放到不同的前置服务中心去实现

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using TradingLib.API;

namespace TradingLib.Common
{

    public delegate void ClientLoginInfoDelegate<T>(T client,bool login) where T:ClientInfoBase;


    /// <summary>
    /// 记录交易客户端通讯的信息
    /// </summary>
    public class ClientInfoBase
    {
        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public ILocation Location { get; set; }

        /// <summary>
        /// 连接端硬件码
        /// </summary>
        public string HardWareCode { get; set; }

        /// <summary>
        /// 客户端的IP地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 终端设备名称 记录客户端使用的软件名称
        /// </summary>
        public string ProductInfo { get; set; }

        /// <summary>
        /// 客户端最近心跳时间
        /// </summary>
        public DateTime HeartBeat { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; private set; }

        /// <summary>
        /// 客户端登入ID
        /// 登入ID与回话ISession中AuthorizedID区别
        /// 登入按认证方式可以通过邮件或手机登入，但是回话中的Authoerized必须与对应的交易帐户所对应
        /// </summary>
        public string LoginID { get; set; }

        /// <summary>
        /// 监察该客户端是否登入
        /// </summary>
        public bool Authorized { get; protected set; }

        /// <summary>
        /// 前置整数编号
        /// </summary>
        public int FrontIDi { get; set; }

        /// <summary>
        /// 前置整数编号
        /// </summary>
        public int SessionIDi { get; set; }


        public ClientInfoBase(ClientInfoBase copythis)
        {
            Location = new Location(copythis.Location.FrontID, copythis.Location.ClientID);

            CreatedTime = copythis.CreatedTime;
            HardWareCode = copythis.HardWareCode;
            IPAddress = copythis.IPAddress;
            ProductInfo = copythis.ProductInfo;

            HeartBeat = copythis.HeartBeat;
            LoginID = copythis.LoginID;
            Authorized = copythis.Authorized;

            FrontIDi = copythis.FrontIDi;
            SessionIDi = copythis.SessionIDi;
        }

        public ClientInfoBase()
        {
            Location = new Location();
            CreatedTime = DateTime.Now;
            HardWareCode = string.Empty;
            IPAddress = string.Empty;
            ProductInfo = string.Empty;
            HeartBeat = DateTime.Now;
            LoginID = string.Empty;
            Authorized = false;

            FrontIDi = 0;
            SessionIDi = 0;
        }

        /// <summary>
        /// 初始化客户端信息 用于记录客户端地址
        /// </summary>
        /// <param name="frontid"></param>
        /// <param name="clientid"></param>
        public void Init(string frontid, string clientid)
        {
            Location.FrontID = frontid;
            Location.ClientID = clientid;
        }

        /// <summary>
        /// 将业务对象绑定到Client
        /// 交易侧对象为 Account
        /// 管理侧对象为 Manager
        /// </summary>
        /// <param name="obj"></param>
        public virtual void BindState(object obj)
        { 
            
        }
    }
}
