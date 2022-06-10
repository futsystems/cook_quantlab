using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradingLib.API
{
    /// <summary>
    /// 服务端没有应答hellserver,则触发该IP无服务异常
    /// </summary>
    public class QSNoServerException:Exception
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class QSAsyncServerError : Exception
    { 
    
    }

    //AsyncClient端错误
    public class QSAsyncClientError : Exception
    { }

    


    #region 日志系统 记录交易信息异常

    public class QSTranLogOrderError : Exception
    {
        public Order OFail;
        public QSTranLogOrderError(Order o)
        {
            OFail = o;
        }
    }

    public class QSTranLogOrderUpdateError : Exception
    {
        public Order OFail;
        public QSTranLogOrderUpdateError(Order o)
        {
            OFail = o;
        }
    }

    public class QSTranLogFillError:Exception
    {
        public Trade FFail;
        public QSTranLogFillError(Trade f)
        {
            FFail = f;
        }
        
    }

    public class QSTranLogCancelError : Exception
    { 
        public long CFail;
        public QSTranLogCancelError(long val)
        {
            CFail = val;
            
        }
    }
    public class QSTranLogPosTransError : Exception
    {
        public object v;
        public QSTranLogPosTransError(object o)
        {
            v = o;
            
        }
    }
    #endregion

    public class QSError : Exception
    {
        public QSError(Exception raw,string label)
        {
            _label = label;
            _raw = raw;
        }

        public Exception RawException { get { return _raw; } }
        Exception _raw;

        public string Label { get { return _label; } }
        string _label;
    }

    public class QSUnknownOffsetException : QSError
    {
        public QSUnknownOffsetException()
            :base(new Exception(),"未知开平标识")
        { 
            
        }
    }
   

    public class QSErrorAccountNotExist : QSError
    {
        public QSErrorAccountNotExist()
            :base(new Exception(),"指定帐号不存在")
        { 

            
        }
    }

    public class QSDebug : QSError
    {
        public QSDebug(Exception ex, string label = "QSDebug Error")
            :base(ex,label)
        {
            
        }
    }


    public class QSClearCentreError : QSError
    {
        public QSClearCentreError(Exception ex, string label)
            : base(ex, label)
        { 
        
        }
    }

    public class QSClearCentreInitError : QSClearCentreError
    {
        public QSClearCentreInitError(Exception ex, string label)
            : base(ex, label)
        { }
      
    }
    /// <summary>
    /// 
    /// </summary>
    public class QSClearCentreLoadSymbolTableError : QSClearCentreError
    {
        public QSClearCentreLoadSymbolTableError(Exception ex, string label)
            : base(ex, label)
        {

        }
       
    }
    /// <summary>
    /// 加载账户信息异常
    /// </summary>
    public class QSClearCentreLoadAccountError : QSClearCentreError
    {
        public QSClearCentreLoadAccountError(Exception ex, string label)
            : base(ex, label)
        {

        }

    }

    /// <summary>
    /// 清算中心从数据库恢复交易信息异常
    /// </summary>
    public class QSClearCentreResotreError : QSClearCentreError
    {
        public QSClearCentreResotreError(Exception ex, string label)
            : base(ex, label)
        {

        }
    }



    /// <summary>
    /// 在TLServer初始化过程中发生的数据库异常
    /// </summary>
    public class QSClearCentreSQLError : QSClearCentreError
    {

        public QSClearCentreSQLError(Exception ex,string label="SQL")
            : base(ex, label)
        {

        }
    }
    

    public class QSTLServerMQError : QSError
    {

        public QSTLServerMQError(Exception ex, string label)
            : base(ex, label)
        { 
        
        }
    }
    /// <summary>
    /// TLServer初始化异常
    /// </summary>
    public class QSTLServerInitError : QSError
    {
        public QSTLServerInitError(Exception ex, string label = "TLServer初始化异常")
            : base(ex, label)
        {

        }
    }
    /// <summary>
    /// 在TLServer初始化过程中发生的数据库异常
    /// </summary>
    public class QSTLServerMQSQLError : QSTLServerMQError
    {
        
        public QSTLServerMQSQLError(Exception ex,string label = "SQL")
            : base(ex, label)
        {

        }
    }


    public class QSTradingServerError : QSError
    {
        public QSTradingServerError(Exception ex, string label)
            : base(ex, label)
        { 
        
        }
    
    }

    public class QSTradingServerInitError:QSTradingServerError
    {
        public QSTradingServerInitError(Exception ex,string label = "TradingServer初始化异常")
            : base(ex, label)
        {

        }
    }
    /// <summary>
    /// 更新账户状态异常
    /// </summary>
    public class QSTradingServerUpdateAccountError : QSTradingServerError
    {
        public QSTradingServerUpdateAccountError(Exception ex, string label = "TradingServer更新账户状态异常")
            : base(ex, label)
        {

        }
    }
    /// <summary>
    /// 验证账户异常
    /// </summary>
    public class QSTradingServerValidAccountError : QSTradingServerError
    {
        public QSTradingServerValidAccountError(Exception ex, string label = "TradingServer验证账户异常")
            : base(ex, label)
        {

        }
    }
    /// <summary>
    /// 请求市场数据异常
    /// </summary>
    public class QSTradingServerRegistSymbolError : QSTradingServerError
    {
        public QSTradingServerRegistSymbolError(Exception ex, string label = "TradingServer请求市场数据异常")
            : base(ex, label)
        {

        }
    }
    /// <summary>
    /// 取消委托异常
    /// </summary>
    public class QSTradingServerCancleError : QSTradingServerError
    {
        public QSTradingServerCancleError(Exception ex, string label = "TradingServer取消委托异常")
            : base(ex, label)
        {

        }
    }
    /// <summary>
    /// 发送委托异常
    /// </summary>
    public class QSTradingServerSendOrderError : QSTradingServerError
    {
        public QSTradingServerSendOrderError(Exception ex, string label = "TradingServer发送委托异常")
            : base(ex, label)
        {

        }
    }
    /// <summary>
    /// 请求Bar数据异常
    /// </summary>
    public class QSTradingServerBARREQUESTError : QSTradingServerError
    {
        public QSTradingServerBARREQUESTError(Exception ex, string label = "TradingServer请求Bar数据异常")
            : base(ex, label)
        {

        }
    }
    /// <summary>
    /// 请求恢复交易数据异常
    /// </summary>
    public class QSTradingServerRESUMEError : QSTradingServerError
    {
        public QSTradingServerRESUMEError(Exception ex, string label = "TradingServer请求恢复交易数据异常")
            : base(ex, label)
        {

        }
    }
    /// <summary>
    /// 请求查询账户信息异常
    /// </summary>
    public class QSTradingServerQRYACCOUNTINFOError : QSTradingServerError
    {
        public QSTradingServerQRYACCOUNTINFOError(Exception ex, string label = "TradingServer请求查询账户信息异常")
            : base(ex, label)
        {

        }
    }
    /// <summary>
    /// 请求修改密码异常
    /// </summary>
    public class QSTradingServerREQCHANGEPASSError : QSTradingServerError
    {
        public QSTradingServerREQCHANGEPASSError(Exception ex, string label = "TradingServer请求修改密码异常")
            : base(ex, label)
        {

        }
    }
    /// <summary>
    /// 查询可开仓数量异常
    /// </summary>
    public class QSTradingServerQRYCANOPENPOSITIONError : QSTradingServerError
    {
        public QSTradingServerQRYCANOPENPOSITIONError(Exception ex, string label = "TradingServer查询可开仓数量异常")
            : base(ex, label)
        {

        }
    }
}
