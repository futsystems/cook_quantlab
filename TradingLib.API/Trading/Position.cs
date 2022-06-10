using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface Position
    {
        /// <summary>
        /// 交易帐户
        /// </summary>
        string Account { get; }

        /// <summary>
        /// 合约
        /// </summary>
        string Symbol { get; }

        /// <summary>
        /// 持仓均价
        /// </summary>
        decimal AvgPrice { get; }

        /// <summary>
        /// 持仓数量
        /// </summary>
        int Size { get; }

        /// <summary>
        /// 持仓数量 绝对值
        /// </summary>
        int UnsignedSize { get; }

        /// <summary>
        /// 是否是多头
        /// </summary>
        bool isLong { get; }

        /// <summary>
        /// 是否是空头
        /// </summary>
        bool isShort { get; }

        /// <summary>
        /// 是否为无头寸
        /// </summary>
        bool isFlat { get; }

        /// <summary>
        /// 平仓数量
        /// </summary>
        int FlatSize { get; }
        
        /// <summary>
        /// 是否有效
        /// </summary>
        bool isValid { get; }

        /// <summary>
        /// 对应的合约对象 可以通过合约对象获得更多相关数据
        /// </summary>
        Symbol oSymbol { get; set; }

        /// <summary>
        /// 该持仓数据所描述的持仓类别
        /// Positon提现了某个类型的持仓的当前状态，可以单独维护多头持仓，空头持仓，或者是净持仓
        /// </summary>
        QSEnumPositionDirectionType DirectionType { get; set; }



        #region 持仓的累加 用于累加持仓反应持仓变化
        /// <summary>
        /// 用成交数据更新持仓状态
        /// </summary>
        /// <param name="newFill"></param>
        /// <returns></returns>
        decimal Adjust(Trade newFill,out bool accept);

        /// <summary>
        /// 用持仓明细更新持仓状态 用于从隔夜持仓初始化持仓状态
        /// </summary>
        /// <param name="newPositiondetail"></param>
        /// <returns></returns>
        decimal Adjust(PositionDetail newPositiondetail);

        #endregion

        /// <summary>
        /// 平仓盈亏点数
        /// </summary>
        decimal ClosedPL { get; }

        /// <summary>
        /// 浮动盈亏点数
        /// </summary>
        decimal UnRealizedPL { get; }

        /// <summary>
        /// 结算 盯市盈亏
        /// </summary>
        //decimal UnrealizedPLByDate { get; }


        #region 行情与价格信息
        /// <summary>
        /// 持仓响应行情更新
        /// </summary>
        /// <param name="k"></param>
        void GotTick(Tick k);

        /// <summary>
        /// 最新价格
        /// </summary>
        decimal LastPrice { get; }
        
        /// <summary>
        /// 开仓以来最高价
        /// </summary>
        decimal Highest { get; set; }

        /// <summary>
        /// 开仓以来最低价
        /// </summary>
        decimal Lowest { get; set; }

        /// <summary>
        /// 当日结算价格 用于收盘后获得结算价格 进行持仓结算
        /// </summary>
        decimal? SettlementPrice { get; set; }

        /// <summary>
        /// 昨日结算价格
        /// </summary>
        decimal? LastSettlementPrice { get; set; }

        #endregion


        #region 成交和相关明细数据
        /// <summary>
        /// 返回日内所有成交数据
        /// </summary>
        IEnumerable<Trade> Trades { get; }

        /// <summary>
        /// 历史持仓明细 不做具体业务操作
        /// </summary>
        IEnumerable<PositionDetail> PositionDetailYdRef { get; }

        /// <summary>
        /// 当前持仓明细 包括昨日与今日
        /// </summary>
        IEnumerable<PositionDetail> PositionDetailTotal { get; }

        /// <summary>
        /// 昨日持仓明细更新 如果当日有平仓 且昨日持仓明细发生变化 通过这里的明细提现
        /// </summary>
        IEnumerable<PositionDetail> PositionDetailYdNew { get; }

        /// <summary>
        /// 当日新开仓持仓明细
        /// </summary>
        IEnumerable<PositionDetail> PositionDetailTodayNew { get; }

        /// <summary>
        /// 当日平仓明细
        /// </summary>
        IEnumerable<PositionCloseDetail> PositionCloseDetail { get; }

        /// <summary>
        /// 新的平仓明细生成事件
        /// </summary>
        event Action<Trade, PositionCloseDetail> NewPositionCloseDetailEvent;

        /// <summary>
        /// 新的持仓明细生成事件
        /// </summary>
        event Action<Trade, PositionDetail> NewPositionDetailEvent;

        #endregion

        #region 日内开平统计
        /// <summary>
        /// 开仓金额
        /// </summary>
        decimal OpenAmount { get; }

        /// <summary>
        /// 开仓数量
        /// </summary>
        int OpenVolume { get; }

        /// <summary>
        /// 平仓金额
        /// </summary>
        decimal CloseAmount { get;}

        /// <summary>
        /// 平仓数量
        /// </summary>
        int CloseVolume { get; }
        #endregion

        //#region 委托分解属性
        ///// <summary>
        ///// 父委托编号
        ///// </summary>
        //string Broker { get; set; }

        ///// <summary>
        ///// 委托分解源
        ///// </summary>
        //QSEnumOrderBreedType Breed { get; set; }

        /// <summary>
        /// 标注该持仓是否已经被结算
        /// </summary>
        bool Settled { get; set; }


    }

    public class InvalidPosition : Exception {}
}
