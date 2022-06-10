using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TradingLib.API
{
    /*
     * 定义lib中所使用的委托类型
     */
    public delegate void HandleTLMessageClient(MessageTypes type, string msg);//客户端处理消息委托

    public delegate void IPacketDelegate(IPacket packet);
  
    public delegate void AccoundIDDel(string account);

 
    //以Connecter为参数的函数调用 connecter在基础api中 用于客户自定义connecter
    public delegate void IConnecterParamDel(string tocken);

    //获得某个symbol的tick委托
    public delegate Tick GetSymbolTickDel(string symbol);

    public delegate Order FindOrderDel(long oid);
    //public delegate Security FindSecurity(string symbol);//通过symbol找到对应的security
    public delegate Symbol Str2SymbolDel(string symbol);//通过symbol找到对应的symbol对象

    //与服务端连接建立与断开委托
    public delegate void ConnectDel();//连接建立委托
    public delegate void DisconnectDel();//连接断开委托
    public delegate void DataPubConnectDel();//Tick数据连接成功
    public delegate void DataPubDisconnectDel();//Tick数据连接成功
    

}
