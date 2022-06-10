using System;
[assembly: CLSCompliant(true)]

namespace TradingLib.API
{
    //请求验证账户
    public delegate string AccountRequestDel(string ac, string pass);

    public delegate Symbol FindSymbolDel(string symbol);
    public delegate void SymbolDelegate(Symbol symbol);

    public delegate void ResponseStringDel(int idx, string data);
    public delegate void BoolDelegate(bool v);
    public delegate void TextIdxDelegate(string txt, int idx);
    public delegate void SymDelegate(string sym);
    public delegate void Int32Delegate(int val);
    public delegate void IntDelegate(int val);
    public delegate void LongDelegate(long val);
    public delegate void StringParamDelegate(string param);
    public delegate void TickDelegate(Tick t);
    public delegate void FillDelegate(Trade t);

    public delegate void OrderActionDelegate(OrderAction o);
    public delegate void OrderDelegate(Order order);
    public delegate void BOOrderDelegate(BinaryOptionOrder order);
    public delegate void OrderErrorDelegate(Order order,RspInfo error);
    public delegate void BOOrderErrorDelegate(BinaryOptionOrder order,RspInfo error);

    public delegate void OrderActionErrorDelegate(OrderAction action,RspInfo error);

    public delegate void OrderSourceDelegate(Order o, int source);
    public delegate void LongSourceDelegate(long val, int source);
    public delegate long OrderDelegateStatus(Order o);
    

    public delegate void Int64Delegate(Int64 number);
    //public delegate void SecurityDelegate(Security sec);
    public delegate void StringDecimalDelegate(string txt, decimal val);
    public delegate void DecimalDelgate(decimal val);
    public delegate void MessageTypesMsgDelegate(MessageTypes[] messages);
    public delegate void DebugDelegate(string msg);
    
    public delegate void SymbolRegisterDel(string client, string symbols);
    public delegate void ObjectArrayDelegate(object[] parameters);
    public delegate void PositionDelegate(Position pos);
    public delegate void PositionFlatFailDel(Position pos,string reason);//强平失效事件
    
    public delegate decimal DecimalStringDelegate(string s);
    public delegate int IntStringDelegate(string s);
    public delegate string StringDelegate();
    public delegate Position[] PositionArrayDelegate(string account);
    public delegate void OrderCancelDelegate(string sym, bool side, long id);

    public delegate MessageTypes[] MessageArrayDelegate();
    public delegate void MessageArrayDel(MessageTypes[] messages);



    public delegate void SymBarIntervalDelegate(string symbol, int interval);
    public delegate void ImbalanceDelegate(Imbalance imb);
    public delegate void VoidDelegate();
    public delegate void MessageDelegate(MessageTypes type, long source, long dest, long msgid, string request,ref string response);
    public delegate void MessageFullDelegate(GenericMessage m);
    //public delegate void BasketDelegate(SymbolBasket b, int id);
    public delegate void BarListDelegate(BarList b);

    
    public delegate void Str1Del(string arg1);
    public delegate void Str2Del(string arg1,string arg2);
    public delegate void Str3Del(string arg1,string arg2,string arg3);
    public delegate void Str4Del(string arg1,string arg2,string arg3,string arg4);

}
