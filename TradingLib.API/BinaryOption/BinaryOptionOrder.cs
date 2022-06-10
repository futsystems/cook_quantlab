using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// 二元期权委托
    /// </summary>
    public interface BinaryOptionOrder
    {

        /// <summary>
        /// 结算日
        /// </summary>
        int SettleDay { get; set; }

        /// <summary>
        /// 结算标识
        /// </summary>
        bool Settled { get; set; }


        /// <summary>
        /// 委托编号
        /// </summary>
        long ID { get; set; }

        /// <summary>
        /// 交易帐户
        /// </summary>
        string Account { get; set; }

        /// <summary>
        /// 委托提交日期
        /// </summary>
        int Date { get; set; }

        /// <summary>
        /// 委托提交时间 用于记录系统收到委托请求的时间
        /// </summary>
        int Time { get; set; }

        /// <summary>
        /// 合约对象
        /// </summary>
        Symbol oSymbol { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        decimal Amount { get; set; }


        /// <summary>
        /// 下单方向
        /// </summary>
        EnumBinaryOptionSideType Side { get; set; }

        /// <summary>
        /// 委托下单二元期权
        /// </summary>
        BinaryOption BinaryOption { get; set; }
        

        /// <summary>
        /// 委托状态
        /// </summary>
        EnumBOOrderStatus Status { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        string Comment { get; set; }

        /// <summary>
        /// 平权胜负
        /// </summary>
        EnumBinaryOptionResultType Result { get; set; }


        /// <summary>
        /// 开权时间
        /// </summary>
        long EntryTime { get; set; }

        /// <summary>
        /// 开权价格
        /// </summary>
        decimal EntryPrice { get; set; }

        /// <summary>
        /// 平权时间
        /// </summary>
        long ExitTime { get; set; }

        /// <summary>
        /// 平权价格
        /// </summary>
        decimal ExitPrice { get; set; }

        /// <summary>
        /// 浮动盈亏 根据不同的权类状态判定当前浮动盈亏,需要实时更新当前最新价格
        /// </summary>
        decimal UnRealizedPL { get;}

        /// <summary>
        /// 平权盈亏 平权后 按最终状态计算盈亏权益
        /// </summary>
        decimal RealizedPL { get; }



        #region 行情信息
        /// <summary>
        /// 最新价格
        /// </summary>
        decimal LastPrice { get; set; }


        /// <summary>
        /// 持权期内最高价
        /// </summary>
        decimal Highest { get; set; }


        /// <summary>
        /// 持权期内最低价
        /// </summary>
        decimal Lowest { get; set; }
        #endregion


        /// <summary>
        /// 响应行情数据
        /// </summary>
        /// <param name="k"></param>
        void GotTick(Tick k);

        /// <summary>
        /// 响应时间
        /// </summary>
        /// <param name="time"></param>
        void GotTime(long time);

    }
}
