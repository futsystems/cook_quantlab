using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface IDataFeed:IConnecter
    {
        /// <summary>
        /// 订阅一组合约的行情,指定该组合约通过哪个数据通道进行发送
        /// 用于兼容FastTickSrv 在FastTick模式下
        /// 行情服务器对接所有行情通道接口,在接受客户端的行情订阅时需要明确指定某组合约通过哪个通道进行订阅
        /// </summary>
        /// <param name="symbols"></param>
        /// <param name="type"></param>
        void RegisterSymbols(List<Symbol> symbols);

        /// <summary>
        /// 获得行情回报
        /// </summary>
        event TickDelegate GotTickEvent;
    }
}
