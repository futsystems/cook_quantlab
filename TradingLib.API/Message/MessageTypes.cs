

namespace TradingLib.API
{
    /// <summary>
    /// 交易协议,消息信息类别
    /// </summary>
    public enum MessageTypes
    {
        //状态类消息
        // START  STATUS MESSAGES - DO NOT REMOVE OR RENAME MESSAGES (only add/insert)
        // IF CHANGED, MUST COPY THIS ENUM'S CONTENTS TO BROKERSERVERS\TRADELIBFAST\TRADELINK.H
        ORDER_NOT_FOUND = -112,//没有该委托
        TLCLIENT_NOT_FOUND = -111,//没有该客户端
        ACCOUNT_NOT_LOGGEDIN = -110,//无效账户
        UNKNOWN_ERROR = -109,//未知错误
        FEATURE_NOT_IMPLEMENTED = -108,//功能没有实现
        CLIENTNOTREGISTERED = -107,//客户端没有注册
        EMPTY_ORDER = -106,//空委托
        UNKNOWN_MESSAGE = -105,//未知消息
        UNKNOWN_SYMBOL = -104,//未知symbol
        BROKERSERVER_NOT_FOUND = -103,//Broker未找到
        INVALID_ORDERSIZE = -102,//无效委托数量
        DUPLICATE_ORDERID = -101,//重复委托ID
        SYMBOL_NOT_LOADED = -100,//合约没有加载
        INVALID_ORDER=-99,//无效委托
        OK = 0,//ok
        // END STATUS MESSAGES

        
        // START CUSTOM MESSAGES  - DO NOT REMOVE OR RENAME MESSAGES
        QRYENDPOINTCONNECTED = 1,//用于接入服务器查询 通过该接入服务器所连接的客户数,用于接入服务器 恢复unknow这样可以避免服务过载
        LOGICLIVEREQUEST=2,//前置与逻辑服务器之间的心跳包
        LOGICLIVERESPONSE = 3,//当服务端收到逻辑心跳包后,服务端会返回一个Response告知服务端Router端处可工作状态
        UPDATECLIENTFRONTID=4,//重启前置后，由于前置编号发生变化，需要更新原来交易客户端回话的前置地址，否则后期的通讯将会被丢弃
        FRONTSTATUSREQUEST=5,//前置机工作状态请求
        FRONTSTATUSRESPONSE=6,//前置机工作状态回报
        SERVICEREQUEST=7,//服务查询请求
        SERVICERESPONSE=8,//服务查询回报
        NOTIFYCLEARCLIENT=9,//断开某个Client
        NOTIFYREBOOTMQSRV=10,//通知前置重启MQServer重新建立Router Dealar连接
       
        // END CUSTOM MESSAGES
        


        //////////////////////////////////////////////////////交易消息码///////////////////////////////////////////////////////////////////////////////////////////
        // START STANDARD MESSAGES
        // basic request
        REQUEST = 5000,
        VERSIONREQUEST,//版本
        BROKERNAMEREQUEST,//Broker名称
        FEATUREREQUEST,//请求功能特征
        HEARTBEATREQUEST,//请求服务端给客户端发送一个消息 已确认客户端与服务端连接有效
        HEARTBEAT,//客户端向服务端定时发送HEARTBEAT,以证明客户端存活,超过一定时间后服务端没有收到客户端心跳就会认为客户端已经死掉，注意这里是一个双向心跳机制
        REGISTERCLIENT,//注册客户端
        CLEARCLIENT,//注销客户端

        REGISTERSYMTICK,//注册市场数据
        UNREGISTERSYMTICK,//注销市场数据

        
        //
        SENDORDER=5100,//发送委托
        SENDORDERACTION,//请求委托取消


        //extra request
        LOGINREQUEST=5200,//登入请求
        QRYINVESTOR,//交易者信息查询
        QRYSYMBOL,//查询合约
        QRYSETTLEINFOCONFIRM,//查询结算
        XQRYSETTLEINFO,
        QRYORDER,//查询委托
        QRYTRADE,//查询成交
        QRYPOSITION,//查询持仓

        QRYACCOUNTINFO,//查询交易账户信息
        QRYMAXORDERVOL,//查询最大开仓量
        BARREQUEST,//请求Bar数据
        CONTRIBREQUEST,//扩展请求
        REQCHANGEPASS,//请求修改密码
        QRYNOTICE,//查询交易服务器通知
        CONFIRMSETTLEMENT,//确认结算数据
        QRYCONTRACTBANK,//查询签约银行
        QRYREGISTERBANKACCOUNT,//查询银期转账帐户
        QRYTRANSFERSERIAL,//查询转账流水
        XQRYPOSITIONDETAIL,//查询持仓明细
        QRYINSTRUMENTCOMMISSIONRATE,//查询合约手续费率
        QRYINSTRUMENTMARGINRATE,//查询合约保证金率
        QRYMARKETDATA,//查询市场行情
        QRYTRADINGPARAMS,//查询交易参数

        XQRYMARKETTIME,//查询交易时间段
        XQRYEXCHANGE,//查询交易所
        XQRYSECURITY,//查询品种
        XQRYSYMBOL,//查询合约
        XQRYYDPOSITION,//查询隔夜持仓 (通过隔夜持仓数据与当日成交数据可以完全恢复一个交易帐户的交易状态)
        XQRYORDER,//查询委托
        XQRYTRADE,//查询成交
        UPDATELOCATION,//更新地址信息
        XQRYTICKSNAPSHOT,//查询行情快照
        XQRYACCOUNT,//查询交易账户
        XQRYMAXORDERVOL,//查询可下单数
        XQRYACCOUNTFINANCE,//查询账户财务信息
        XQRYTRADSPLIT,//查询成交数据
        XQRYPRICEVOL,//查询价格成交量
        XQRYMINUTEDATA,//查询分时数据
        XQRYEXCHANGERATE,//查询汇率数据


        DOMREQUEST,//请求DOM市场Level2数据
        IMBALANCEREQUEST,//imbalance..查询这个是什么意思

        BOSENDORDER=5300,//发送二元期权委托


        // responses or acks
        RESPONSE = 6000,
        VERSIONRESPONSE,//版本回报
        BROKERNAMERESPONSE,//服务名查询回报
        FEATURERESPONSE,//功能特征回报
        HEARTBEATRESPONSE,//服务端应答客户端,如果客户端在一定时间内没有收到数据 就会触发发送heartbeatrequest,然后服务端就会发送一个response以证明客户端与服务端之间连接有效
        REGISTERCLIENTRESPONSE,//客户端注册连接回报


        TICKNOTIFY=6100,//Tick数据
        TICKHEARTBEAT,//行情心跳
        INDICATORNOTIFY,//指标通知
        OLDPOSITIONNOTIFY,//隔夜持仓回报 用于恢复日内数据时,先发送结算后的持仓,然后再发送日内交易数据 用于形成当前持仓状态[昨天+当日变动] = 当前状态
        ORDERNOTIFY,//委托回报
        ERRORORDERNOTIFY,//委托错误回报
        EXECUTENOTIFY,//成交回报
        POSITIONUPDATENOTIFY,//服务端向客户端发仓位状态信息,PC交易客户端自己计算持仓数据,网页交易客户端则需要服务端进行响应
        ORDERACTIONNOTIFY,//委托操作回报
        ERRORORDERACTIONNOTIFY,//委托操作回报
        CASHOPERATIONNOTIFY,//出入金操作回报
        TRADINGNOTICENOTIFY,//交易通知回报

        //request replay
        LOGINRESPONSE=6200,//登入回报
        INVESTORRESPONSE,//交易者信息回报
        SYMBOLRESPONSE,//合约查询回报
        SETTLEINFOCONFIRMRESPONSE,//结算确认回报
        XSETTLEINFORESPONSE,
        ORDERRESPONSE,//查询委托回报
        TRADERESPONSE,//查询成交回报
        POSITIONRESPONSE,//查询持仓回报
        ACCOUNTINFORESPONSE,//交易账户信息回报
        
        ACCOUNTRESPONSE,//账户通知
        MAXORDERVOLRESPONSE,//最大可开仓回报
        BARRESPONSE,//Bar数据回报
        CONTRIBRESPONSE,//扩展回报
        CHANGEPASSRESPONSE,//修改密码回报
        NOTICERESPONSE,//交易通知回报
        CONFIRMSETTLEMENTRESPONSE,//确认结算回报
        CONTRACTBANKRESPONSE,//查询签约银行回报
        REGISTERBANKACCOUNTRESPONSE,//查询银期签约帐户回报
        TRANSFERSERIALRESPONSE,//查询转账流水回报
        XPOSITIONDETAILRESPONSE,//查询持仓明细回报
        INSTRUMENTCOMMISSIONRATERESPONSE,//查询合约手续费率回报
        INSTRUMENTMARGINRATERESPONSE,//查询合约保证金率回报
        MARKETDATARESPONSE,//查询市场行情回报
        TRADINGPARAMSRESPONSE,//查询交易参数回报

        XMARKETTIMERESPONSE,//查询交易时间段
        XEXCHANGERESPNSE,//查询交易所
        XSECURITYRESPONSE,//查询品种
        XSYMBOLRESPONSE,//查询合约
        XYDPOSITIONRESPONSE,//隔夜持仓回报
        XORDERRESPONSE,//委托回报
        XTRADERESPONSE,//成交回报
        //UPDATELOCATIONRESPONSE,//更新地址回报
        XTICKSNAPSHOTRESPONSE,//行情快照回报
        XACCOUNTRESPONSE,//交易账户回报
        XQRYMAXORDERVOLRESPONSE,//可下单回报
        XQRYACCOUNTFINANCERESPONSE,//查询交易账户财务信息
        
        BIN_BARRESPONSE,//二进制Bar回报
        XQRYTRADSPLITRESPONSE,//成交明细回报
        XQRYPRICEVOLRESPONSE,//价格成交量回报
        XQRYMINUTEDATARESPONSE,//分时数据回报
        XQRYEXCHANGERATERESPONSE,//汇率数据回报
     


        MD_DEMOTICK,//行情服务器测试Tick
        // END STANDARD MESSAGES


        BOORDERNOTIFY = 6300,//二元委托回报
        BOORDERERRORNOTIFY,//二元委托错误回报

        //////////////////////////////////////////////////////管理消息码///////////////////////////////////////////////////////////////////////////////////////////
        //MGR 服务端控制 行情服务,行情通道等
        MGR_MD_STARTTICKPUB=7000,//启动tickpub服务
        MGR_MD_STOPTICKPUB=7001,//停止tickpub服务
        MGR_MD_STARTDATAFEED=7002,//启动数据通道
        MGR_MD_STOPDATAFEED=7003,//停止数据通道
        MGR_MD_REGISTERSYMBOLS=7004,//订阅行情
        MGR_MD_UNREGISTERSYMBOLS,//注销行情订阅
        MGR_MD_QRYSYMBOLSREGISTED,//查询已注册合约
        MGR_MD_QRYSYMBOLSREGISTEDRESPONSE,//查询已注册合约回报
        MGR_MD_SETSYMBOLFILTER,//设置合约过滤条件
        MGR_MD_UPLOADBARDATA,//上传Bar历史数据

        // START MANAGER MESSAGES
        MGR_REQ_LOGIN=8000,//管理员登入请求
        MGR_REQ_CONTRIB,//管理扩展请求

        MGR_RSP_LOGIN = 9000,//管理登入回报
        MGR_RSP_CONTRIB,//管理扩展回报 携带返回数据
        MGR_RTN_CONTRIB,//管理扩展通知 携带通知消息
        MGR_RSP,//管理端应答 携带操作执行正确或异常信息

    }

}