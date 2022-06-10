﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class ExStrategyTemplateSetting
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 域ID
        /// </summary>
        public int Domain_ID { get; set; }


        /// <summary>
        /// 管理员主域ID
        /// </summary>
        public int Manager_ID { get; set; }


        public override string ToString()
        {
            return this.Name;
        }
    }


    public class ExStrategyTemplate : ExStrategyTemplateSetting
    {
        /// <summary>
        /// 该策略模板对应的策略实例
        /// </summary>
        public ExStrategy ExStrategy { get; set; }
    }
    /// <summary>
    /// 交易系统核心算法参数
    /// 用于设定保证金计算方式，可用资金计算方式以及其他与交易相关的数据或参数的算法方式
    /// </summary>
    public class ExStrategy
    {
        public ExStrategy()
        {
            this.ID = 0;
            this.Template_ID = 0;
            this.MarginPrice = QSEnumMarginPrice.OpenPrice;
            this.IncludeCloseProfit = true;
            this.IncludePositionProfit = true;
            this.Algorithm = QSEnumAlgorithm.AG_All;
            this.SideMargin = true;
            this.CreditSeparate = true;
            this.PositionLock = true;
            this.EntrySlip = 0;
            this.ExitSlip = 0;
            this.LimitCheck = false;
            this.Probability = 100;

            this.SimExecuteCFFEXStrategy = false;
            this.SimExecuteFillAll = false;
            this.SimExecuteMinSize = 0;
            this.SimExecuteStickLimitPrice = false;
            this.SimExecuteTimeCheck = false;
            this.SimExecuteUseAskBid = true;


        }
        /// <summary>
        /// 数据库ID编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 模板编号
        /// </summary>
        public int Template_ID { get; set; }
        /// <summary>
        /// 保证金计算方法
        /// 按不同的保证金计算方法来计算交易账户的持仓保证金
        /// </summary>
        public QSEnumMarginPrice MarginPrice { get; set; }

        /// <summary>
        /// 可用资金是否包含平仓盈亏
        /// </summary>
        public bool IncludeCloseProfit { get; set; }


        /// <summary>
        /// 可用资金是否包含浮动盈亏
        /// </summary>
        public bool IncludePositionProfit { get; set; }


        /// <summary>
        /// 浮动盈亏算法
        /// </summary>
        public QSEnumAlgorithm Algorithm { get; set; }


        /// <summary>
        /// 是否支持单向大边保证金制度
        /// </summary>
        public bool SideMargin { get; set; }

        /// <summary>
        /// 交易账户信用额度是否单独显示(博弈客户端是否单独显示交易账户的信用额度)
        /// </summary>
        public bool CreditSeparate { get; set; }


        /// <summary>
        /// 是否支持锁仓
        /// </summary>
        public bool PositionLock { get; set; }


        /// <summary>
        /// 开仓滑点
        /// </summary>
        public int EntrySlip { get; set; }

        /// <summary>
        /// 平仓滑点
        /// </summary>
        public int ExitSlip { get; set; }

        /// <summary>
        /// 限价单检查
        /// </summary>
        public bool LimitCheck { get; set; }

        /// <summary>
        /// 执行概率
        /// </summary>
        public int Probability { get; set; }


        /// <summary>
        /// 始终按挂单价成交
        /// </summary>
        public bool SimExecuteStickLimitPrice { get; set; }

        /// <summary>
        /// 成交所有
        /// </summary>
        public bool SimExecuteFillAll { get; set; }

        /// <summary>
        /// 最小成交数量
        /// </summary>
        public int SimExecuteMinSize { get; set; }

        /// <summary>
        /// 成交Tick时间检查
        /// 委托时间11:23:25 则Tick时间需在11:23:25秒之后 
        /// </summary>
        public bool SimExecuteTimeCheck { get; set; }

        /// <summary>
        /// 使用盘口成交
        /// 如果设置为false则按最新价成交
        /// </summary>
        public bool SimExecuteUseAskBid { get; set; }

        /// <summary>
        /// 中金所挂单未成以最新价成交策略
        /// </summary>
        public bool SimExecuteCFFEXStrategy { get; set; }


        /// <summary>
        /// 入金手续费比例
        /// </summary>
        public decimal DepositCommission { get; set; }

        /// <summary>
        /// 出金手续费比例
        /// </summary>
        public decimal WithdrawCommission { get; set; }

        /// <summary>
        /// 杠杆比例
        /// </summary>
        public decimal LeverageRatio { get; set; }
    }
}