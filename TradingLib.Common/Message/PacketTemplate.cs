///////////////////////////////////////////////////////////////////////////////////////
// PacketTemplate逻辑包模板类
// 1.将逻辑包的生成过程封装到一个函数中，实现代码简化和统一
// 2.同时逻辑包的解析与生成归纳成四大类，服务端收到请求，服务端生成回报，客户端生成请求，服务端收到回报
// 3.针对集中解析分布处理的原则，在服务端收到请求，客户端收到回报时按照统一的格式进行了逻辑包的解析同时
// 在逻辑包统一解析部分做了路由非路由内的操作码均无法解析到正常的数据包，并且会抛出异常
// 基本处理思路如下
//  传输层-->消息层Packet集中解析生成IPacket-->逻辑层handle(IPacket)通过判断IPacket的类型进行相关取值与操作
// 
//
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{

    public class PacketError : QSError
    {
        public PacketError()
            :base(new Exception(),"packet error")
        { 
            
        }
    }

    public class PacketTypeNotAvabile : PacketError
    {
        public MessageTypes Type { get; set; }
        public string Content { get; set; }
        public string FrontID { get; set; }
        public string ClientID { get; set; }
        public PacketTypeNotAvabile(MessageTypes type,string content,string frontid,string client)
        {
            Type = type;
            Content = content;
            FrontID = frontid;
            ClientID = client;
        }
    }
    public class PacketParseError : PacketError
    {

        public Exception RawException { get; set; }
        public MessageTypes Type { get; set; }
        public string Content { get; set; }
        public string FrontID { get; set; }
        public string ClientID { get; set; }
        public PacketParseError(Exception raw,MessageTypes type,string content,string frontid,string client)
        {
            RawException = raw;
            Type = type;
            Content = content;
            FrontID = frontid;
            ClientID = client;

        }
    }
    public class PacketHelper
    {
        public static IPacket SrvRecvRequest(Message message,string frontid, string clientid)
        {
            try
            {
                switch (message.Type)
                {

                    case MessageTypes.SERVICEREQUEST:
                        return RequestTemplate<QryServiceRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //逻辑活动请求
                    case MessageTypes.LOGICLIVEREQUEST:
                        return RequestTemplate<LogicLiveRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //客户端注册
                    case MessageTypes.REGISTERCLIENT:
                        return RequestTemplate<RegisterClientRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //请求注销
                    case MessageTypes.CLEARCLIENT:
                        return RequestTemplate<UnregisterClientRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //发送心跳
                    case MessageTypes.HEARTBEAT:
                        return RequestTemplate<HeartBeat>.SrvRecvRequest(frontid, clientid, message.Content);

                    //功能码请求
                    case MessageTypes.FEATUREREQUEST:
                        return RequestTemplate<FeatureRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //版本查询与连接初始化
                    case MessageTypes.VERSIONREQUEST:
                        return RequestTemplate<VersionRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //发送心跳请求
                    case MessageTypes.HEARTBEATREQUEST:
                        return RequestTemplate<HeartBeatRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //请求登入
                    case MessageTypes.LOGINREQUEST:
                        return RequestTemplate<LoginRequest>.SrvRecvRequest(frontid, clientid, message.Content);


                    //服务查询
                    case MessageTypes.BROKERNAMEREQUEST:
                        return RequestTemplate<BrokerNameRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //注册合约
                    case MessageTypes.REGISTERSYMTICK:
                        return RequestTemplate<RegisterSymbolTickRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //注销合约
                    case MessageTypes.UNREGISTERSYMTICK:
                        return RequestTemplate<UnregisterSymbolTickRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //发送委托
                    case MessageTypes.SENDORDER:
                        return RequestTemplate<OrderInsertRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //发送委托操作
                    case MessageTypes.SENDORDERACTION:
                        return RequestTemplate<OrderActionRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询交易员
                    case MessageTypes.QRYINVESTOR:
                        return RequestTemplate<QryInvestorRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //账户信息查询
                    case MessageTypes.QRYACCOUNTINFO:
                        return RequestTemplate<QryAccountInfoRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询合约
                    case MessageTypes.QRYSYMBOL:
                        return RequestTemplate<QrySymbolRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询结算确认
                    case MessageTypes.QRYSETTLEINFOCONFIRM:
                        return RequestTemplate<QrySettleInfoConfirmRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询结算信息
                    case MessageTypes.XQRYSETTLEINFO:
                        return RequestTemplate<XQrySettleInfoRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询委托
                    case MessageTypes.QRYORDER:
                        return RequestTemplate<QryOrderRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询成交
                    case MessageTypes.QRYTRADE:
                        return RequestTemplate<QryTradeRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询持仓
                    case MessageTypes.QRYPOSITION:
                        return RequestTemplate<QryPositionRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询持仓明细
                    case MessageTypes.XQRYPOSITIONDETAIL:
                        return RequestTemplate<XQryPositionDetailRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询最大保单
                    case MessageTypes.QRYMAXORDERVOL:
                        return RequestTemplate<QryMaxOrderVolRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //确认结算单
                    case MessageTypes.CONFIRMSETTLEMENT:
                        return RequestTemplate<ConfirmSettlementRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询历史行情
                    case MessageTypes.BARREQUEST:
                        return RequestTemplate<QryBarRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //扩展命令请求
                    case MessageTypes.CONTRIBREQUEST:
                        return RequestTemplate<ContribRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //请求修改密码
                    case MessageTypes.REQCHANGEPASS:
                        return RequestTemplate<ReqChangePasswordRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //请求查询系统通知
                    case MessageTypes.QRYNOTICE:
                        return RequestTemplate<QryNoticeRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //请求查询签约银行列表
                    case MessageTypes.QRYCONTRACTBANK:
                        return RequestTemplate<QryContractBankRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //请求查询银行帐户
                    case MessageTypes.QRYREGISTERBANKACCOUNT:
                        return RequestTemplate<QryRegisterBankAccountRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询出入金流水记录
                    case MessageTypes.QRYTRANSFERSERIAL:
                        return RequestTemplate<QryTransferSerialRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询合约手续费率
                    case MessageTypes.QRYINSTRUMENTCOMMISSIONRATE:
                        return RequestTemplate<QryInstrumentCommissionRateRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询合约保证金率
                    case MessageTypes.QRYINSTRUMENTMARGINRATE:
                        return RequestTemplate<QryInstrumentMarginRateRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询市场行情
                    case MessageTypes.QRYMARKETDATA:
                        return RequestTemplate<QryMarketDataRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询交易参数
                    case MessageTypes.QRYTRADINGPARAMS:
                        return RequestTemplate<QryTradingParamsRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询交易时间段
                    case MessageTypes.XQRYMARKETTIME:
                        return RequestTemplate<XQryMarketTimeRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询交易所
                    case MessageTypes.XQRYEXCHANGE:
                        return RequestTemplate<XQryExchangeRequuest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询品种
                    case MessageTypes.XQRYSECURITY:
                        return RequestTemplate<XQrySecurityRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询合约
                    case MessageTypes.XQRYSYMBOL:
                        return RequestTemplate<XQrySymbolRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询隔夜持仓
                    case MessageTypes.XQRYYDPOSITION:
                        return RequestTemplate<XQryYDPositionRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询委托
                    case MessageTypes.XQRYORDER:
                        return RequestTemplate<XQryOrderRequest>.SrvRecvRequest(frontid,clientid,message.Content);
                    //查询成交
                    case MessageTypes.XQRYTRADE:
                        return RequestTemplate<XQryTradeRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //更新地址信息
                    case MessageTypes.UPDATELOCATION:
                        return RequestTemplate<UpdateLocationInfoRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询行情快照
                    case MessageTypes.XQRYTICKSNAPSHOT:
                        return RequestTemplate<XQryTickSnapShotRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询交易账户
                    case MessageTypes.XQRYACCOUNT:
                        return RequestTemplate<XQryAccountRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询最大下单量
                    case MessageTypes.XQRYMAXORDERVOL:
                        return RequestTemplate<XQryMaxOrderVolRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询账户财务信息
                    case MessageTypes.XQRYACCOUNTFINANCE:
                        return RequestTemplate<XQryAccountFinanceRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    //查询汇率信息
                    case MessageTypes.XQRYEXCHANGERATE:
                        return RequestTemplate<XQryExchangeRateRequest>.SrvRecvRequest(frontid, clientid, message.Content);

                    #region manager
                    case MessageTypes.MGR_REQ_LOGIN://请求登入
                        return RequestTemplate<MGRLoginRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    case MessageTypes.MGR_REQ_CONTRIB://扩展请求
                        return RequestTemplate<MGRContribRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    #endregion

                    #region 行情部分
                    case MessageTypes.MGR_MD_STARTDATAFEED://启动行情通道
                        return RequestTemplate<MDReqStartDataFeedRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    case MessageTypes.MGR_MD_STOPDATAFEED://停止行情通道
                        return RequestTemplate<MDReqStopDataFeedRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    case MessageTypes.MGR_MD_REGISTERSYMBOLS://注册行情
                        return RequestTemplate<MDRegisterSymbolsRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    #endregion

                    case MessageTypes.MD_DEMOTICK:
                        return RequestTemplate<MDDemoTickRequest>.SrvRecvRequest(frontid, clientid, message.Content);

                    case MessageTypes.BOSENDORDER:
                        return RequestTemplate<BOOrderInsertRequest>.SrvRecvRequest(frontid, clientid, message.Content);

                    case MessageTypes.MGR_MD_UPLOADBARDATA:
                        UploadBarDataRequest request = new UploadBarDataRequest();
                        request.DeserializeBin(message.Data);
                        return request;
                    case MessageTypes.XQRYTRADSPLIT://查询成交明细
                        return RequestTemplate<XQryTradeSplitRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    case MessageTypes.XQRYPRICEVOL://查询价格成交量分布
                        return RequestTemplate<XQryPriceVolRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    case MessageTypes.XQRYMINUTEDATA://查询分时数据
                        return RequestTemplate<XQryMinuteDataRequest>.SrvRecvRequest(frontid, clientid, message.Content);
                    default:
                        throw new PacketTypeNotAvabile(message.Type, message.Content, frontid, clientid);
                }
            }
            catch (Exception ex)
            {
                throw new PacketParseError(ex, message.Type, message.Content,frontid,clientid);
            }
        }


        public static IPacket CliRecvResponse(Message message)
        {
            switch (message.Type)
            {
                case MessageTypes.BIN_BARRESPONSE:
                    {
                        RspQryBarResponseBin response = new RspQryBarResponseBin();
                        response.DeserializeBin(message.Data);
                        return response;
                    }
                case MessageTypes.XQRYTRADSPLITRESPONSE:
                    {
                        RspXQryTradeSplitResponse response = new RspXQryTradeSplitResponse();
                        response.DeserializeBin(message.Data);
                        return response;
                    }
                case MessageTypes.XQRYPRICEVOLRESPONSE:
                    {
                        RspXQryPriceVolResponse response = new RspXQryPriceVolResponse();
                        response.DeserializeBin(message.Data);
                        return response;
                    }
                case MessageTypes.XQRYMINUTEDATARESPONSE:
                    {
                        RspXQryMinuteDataResponse response = new RspXQryMinuteDataResponse();
                        response.DeserializeBin(message.Data);
                        return response;
                    }

                case MessageTypes.SERVICERESPONSE:
                    return ResponseTemplate<RspQryServiceResponse>.CliRecvResponse(message);
                case MessageTypes.REGISTERCLIENTRESPONSE:
                    return ResponseTemplate<RspRegisterClientResponse>.CliRecvResponse(message);
                case MessageTypes.LOGICLIVERESPONSE:
                    return ResponseTemplate<LogicLiveResponse>.CliRecvResponse(message);
                case MessageTypes.NOTIFYCLEARCLIENT:
                    return ResponseTemplate<NotifyClearClient>.CliRecvResponse(message);
                case MessageTypes.NOTIFYREBOOTMQSRV:
                    return ResponseTemplate<NotifyRebooMQSrv>.CliRecvResponse(message);
                case MessageTypes.FEATURERESPONSE:
                    return ResponseTemplate<FeatureResponse>.CliRecvResponse(message);
                case MessageTypes.VERSIONRESPONSE:
                    return ResponseTemplate<VersionResponse>.CliRecvResponse(message);
                case MessageTypes.HEARTBEATRESPONSE:
                    return ResponseTemplate<HeartBeatResponse>.CliRecvResponse(message);
                case MessageTypes.LOGINRESPONSE:
                    return ResponseTemplate<LoginResponse>.CliRecvResponse(message);
                case MessageTypes.BROKERNAMERESPONSE:
                    return ResponseTemplate<BrokerNameResponse>.CliRecvResponse(message);

                case MessageTypes.ORDERNOTIFY://委托通知
                    return ResponseTemplate<OrderNotify>.CliRecvResponse(message);
                case MessageTypes.ERRORORDERNOTIFY://委托错误通知
                    return ResponseTemplate<ErrorOrderNotify>.CliRecvResponse(message);
                case MessageTypes.EXECUTENOTIFY://成交通知
                    return ResponseTemplate<TradeNotify>.CliRecvResponse(message);
                case MessageTypes.POSITIONUPDATENOTIFY://持仓更新通知
                    return ResponseTemplate<PositionNotify>.CliRecvResponse(message);
                case MessageTypes.OLDPOSITIONNOTIFY://隔夜持仓通知
                    return ResponseTemplate<HoldPositionNotify>.CliRecvResponse(message);
                case MessageTypes.ORDERACTIONNOTIFY://委托操作通知
                    return ResponseTemplate<OrderActionNotify>.CliRecvResponse(message);
                case MessageTypes.ERRORORDERACTIONNOTIFY://委托操作错误通知
                    return ResponseTemplate<ErrorOrderActionNotify>.CliRecvResponse(message);
                case MessageTypes.CASHOPERATIONNOTIFY://出入金操作通知
                    return ResponseTemplate<CashOperationNotify>.CliRecvResponse(message);
                case MessageTypes.TRADINGNOTICENOTIFY://交易通知
                    return ResponseTemplate<TradingNoticeNotify>.CliRecvResponse(message);

                case MessageTypes.ORDERRESPONSE://查询委托回报
                    return ResponseTemplate<RspQryOrderResponse>.CliRecvResponse(message);
                case MessageTypes.TRADERESPONSE://成交查询回报
                    return ResponseTemplate<RspQryTradeResponse>.CliRecvResponse(message);
                case MessageTypes.POSITIONRESPONSE://持仓查询回报
                    return ResponseTemplate<RspQryPositionResponse>.CliRecvResponse(message);
                case MessageTypes.XPOSITIONDETAILRESPONSE://查询持仓明细回报
                    return ResponseTemplate<RspXQryPositionDetailResponse>.CliRecvResponse(message);
                case MessageTypes.SYMBOLRESPONSE://合约查询回报
                    return ResponseTemplate<RspQrySymbolResponse>.CliRecvResponse(message);
                case MessageTypes.XSETTLEINFORESPONSE://结算信息回报
                    return ResponseTemplate<RspXQrySettleInfoResponse>.CliRecvResponse(message);
                case MessageTypes.BARRESPONSE://历史数据回报
                    return ResponseTemplate<RspQryBarResponse>.CliRecvResponse(message);
                case MessageTypes.SETTLEINFOCONFIRMRESPONSE://结算确认回报
                    return ResponseTemplate<RspQrySettleInfoConfirmResponse>.CliRecvResponse(message);
                case MessageTypes.CONFIRMSETTLEMENTRESPONSE://确认结算回报
                    return ResponseTemplate<RspConfirmSettlementResponse>.CliRecvResponse(message);
                case MessageTypes.MAXORDERVOLRESPONSE://可下单手数回报
                    return ResponseTemplate<RspQryMaxOrderVolResponse>.CliRecvResponse(message);
                case MessageTypes.ACCOUNTINFORESPONSE://帐户信息查询
                    return ResponseTemplate<RspQryAccountInfoResponse>.CliRecvResponse(message);
                case MessageTypes.INVESTORRESPONSE://交易者信息查询
                    return ResponseTemplate<RspQryInvestorResponse>.CliRecvResponse(message);
                case MessageTypes.CONTRIBRESPONSE:
                    return ResponseTemplate<RspContribResponse>.CliRecvResponse(message);

                case MessageTypes.CHANGEPASSRESPONSE://修改密码回报
                    return ResponseTemplate<RspReqChangePasswordResponse>.CliRecvResponse(message);
                case MessageTypes.NOTICERESPONSE://查询系统通知回报
                    return ResponseTemplate<RspQryNoticeResponse>.CliRecvResponse(message);
                case MessageTypes.CONTRACTBANKRESPONSE://查询签约银行通知回报
                    return ResponseTemplate<RspQryContractBankResponse>.CliRecvResponse(message);
                case MessageTypes.REGISTERBANKACCOUNTRESPONSE://查询银行帐户回报
                    return ResponseTemplate<RspQryRegisterBankAccountResponse>.CliRecvResponse(message);
                case MessageTypes.TRANSFERSERIALRESPONSE://查询出入金流水回报
                    return ResponseTemplate<RspQryTransferSerialResponse>.CliRecvResponse(message);
                case MessageTypes.INSTRUMENTCOMMISSIONRATERESPONSE://查询合约手续费率
                    return ResponseTemplate<RspQryInstrumentCommissionRateResponse>.CliRecvResponse(message);
                case MessageTypes.INSTRUMENTMARGINRATERESPONSE://查询保证金率
                    return ResponseTemplate<RspQryInstrumentMarginRateResponse>.CliRecvResponse(message);
                case MessageTypes.MARKETDATARESPONSE://查询市场行情回报
                    return ResponseTemplate<RspQryMarketDataResponse>.CliRecvResponse(message);
                case MessageTypes.TRADINGPARAMSRESPONSE://交易参数回报
                    return ResponseTemplate<RspQryTradingParamsResponse>.CliRecvResponse(message);

                case MessageTypes.XMARKETTIMERESPONSE://交易时间回报
                    return ResponseTemplate<RspXQryMarketTimeResponse>.CliRecvResponse(message);
                case MessageTypes.XEXCHANGERESPNSE://交易所回报
                    return ResponseTemplate<RspXQryExchangeResponse>.CliRecvResponse(message);
                case MessageTypes.XSECURITYRESPONSE://品种回报
                    return ResponseTemplate<RspXQrySecurityResponse>.CliRecvResponse(message);
                case MessageTypes.XSYMBOLRESPONSE://合约回报
                    return ResponseTemplate<RspXQrySymbolResponse>.CliRecvResponse(message);
                case MessageTypes.XYDPOSITIONRESPONSE://持仓回报
                    return ResponseTemplate<RspXQryYDPositionResponse>.CliRecvResponse(message);
                case MessageTypes.XORDERRESPONSE://委托回报
                    return ResponseTemplate<RspXQryOrderResponse>.CliRecvResponse(message);
                case MessageTypes.XTRADERESPONSE://成交回报
                    return ResponseTemplate<RspXQryTradeResponse>.CliRecvResponse(message);
                case MessageTypes.XTICKSNAPSHOTRESPONSE://行情快照回报
                    return ResponseTemplate<RspXQryTickSnapShotResponse>.CliRecvResponse(message);
                case MessageTypes.XACCOUNTRESPONSE://交易账户回报
                    return ResponseTemplate<RspXQryAccountResponse>.CliRecvResponse(message);
                case MessageTypes.XQRYMAXORDERVOLRESPONSE://最大下单数量回报
                    return ResponseTemplate<RspXQryMaxOrderVolResponse>.CliRecvResponse(message);
                case  MessageTypes.XQRYACCOUNTFINANCERESPONSE://账户财务数据回报
                    return ResponseTemplate<RspXQryAccountFinanceResponse>.CliRecvResponse(message);
                case MessageTypes.XQRYEXCHANGERATERESPONSE://汇率数据回报
                    return ResponseTemplate<RspXQryExchangeRateResponse>.CliRecvResponse(message);

                case MessageTypes.TICKNOTIFY:
                    return ResponseTemplate<TickNotify>.CliRecvResponse(message);

                case MessageTypes.TICKHEARTBEAT:
                    TickHeartBeatResponse tickhb = new TickHeartBeatResponse();
                    return tickhb;

                #region manager
                case MessageTypes.MGR_RSP_LOGIN://登入回报
                    return ResponseTemplate<RspMGRLoginResponse>.CliRecvResponse(message);
                case MessageTypes.MGR_RSP_CONTRIB://扩展回报
                    return ResponseTemplate<RspMGRContribResponse>.CliRecvResponse(message);
                case MessageTypes.MGR_RTN_CONTRIB://扩展回报
                    return ResponseTemplate<NotifyMGRContribNotify>.CliRecvResponse(message);
                case MessageTypes.MGR_RSP://操作应答
                    return ResponseTemplate<RspMGRResponse>.CliRecvResponse(message);
                #endregion


                #region 行情部分
                case MessageTypes.MGR_MD_QRYSYMBOLSREGISTEDRESPONSE://FeedHandler请求查询已注册合约回报
                    return ResponseTemplate<RspMDQrySymbolsRegistedResponse>.CliRecvResponse(message);
                #endregion
                default:
                    throw new PacketError();
            }
            
        }
    }
    public class RequestTemplate<T>
        where T : RequestPacket, new()
    {
        /// <summary>
        /// 生成请求Packet
        /// 将前置地址,客户端ID,以及数据内容生成对应的packet
        /// </summary>
        /// <param name="frontid"></param>
        /// <param name="clientid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static T SrvRecvRequest(string frontid, string clientid, string content)
        {
            T packet = new T();
            packet.SetSource(frontid, clientid);
            packet.Deserialize(content);
            return packet;
        }
        public static T CliSendRequest(int requestid)
        {
            T package = new T();
            package.SetRequestID(requestid);
            return package;
        }
    }

    /// <summary>
    /// 逻辑数据包模板
    /// PacketBase 是所有数据包的父类
    /// 这里需要确定子类和父类的构造函数的相关调用顺序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseTemplate<T>
        where T : ResponsePacket, new()
    {
        /// <summary>
        /// 从Requestpacket生成Responsepacket
        /// 然后再有处理逻辑填充对应的参数
        /// </summary>
        /// <param name="requestid"></param>
        /// <returns></returns>
        public static T SrvSendRspResponse(RequestPacket request)
        {
            T packet = new T();
            packet.BindRequest(request);
            return packet;
        }

        public static T SrvSendRspResponse(ISession session)
        {
            T packet = new T();
            packet.BindSession(session);
            return packet;
        }

        public static T SrvSendRspResponse(string front, string clientid, int reqId)
        {
            T packet = new T();
            packet.BindSession(front, clientid, reqId);
            return packet;
        }

        public static T SrvSendNotifyResponse()
        {
            T packet = new T();
            packet.BindAccount(string.Empty);
            return packet;
        }
        public static T SrvSendNotifyResponse(string account)
        {
            T packet = new T();
            packet.BindAccount(account);
            return packet;
        }

        public static T SrvSendNotifyResponse(ILocation location)
        {
            return SrvSendNotifyResponse(new ILocation[] { location });
        }

        public static T SrvSendNotifyResponse(IEnumerable<ILocation> locations)
        {
            T packet = new T();
            packet.BindLocation(locations);
            return packet;
        }

        public static T CliRecvResponse(Message message)
        {
            T packet = new T();
            packet.Deserialize(message.Content);
            return packet;
        }
    }


}
