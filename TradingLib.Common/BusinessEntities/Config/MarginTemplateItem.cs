using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class MarginTemplateItemSetting
    {
        /// <summary>
        /// 全局编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 品种代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 合约月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 保证金额度
        /// 按比例 百分之多少
        /// </summary>
        public decimal MarginByMoney { get; set; }

        /// <summary>
        /// 保证金额度
        /// 按手数 1手多少保证金
        /// </summary>
        public decimal MarginByVolume { get; set; }

        /// <summary>
        /// 上浮百分比
        /// </summary>
        public decimal Percent { get; set; }

        /// <summary>
        /// 加收方式
        /// </summary>
        public QSEnumChargeType ChargeType { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        public int Template_ID { get; set; }

    }

    public class MarginTemplateItem : MarginTemplateItemSetting
    {
        /// <summary>
        /// 获得该手续费项目的键值
        /// </summary>
        /// <returns></returns>
        public string GetItemKey()
        {
            return string.Format("{0}-{1}", this.Code, this.Month);
        }

        /// <summary>
        /// 计算某个持仓的保证金
        /// price 按该价格计算保证金
        /// 开仓价，最新价 等多种计算方式
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public decimal CalMargin(Position p,decimal price)
        {
            //异化合约按照固定金额来计算
            //if (p.oSymbol.SecurityType == SecurityType.INNOV)
            //{
            //    return p.UnsignedSize * (p.oSymbol.Margin + (p.oSymbol.ExtraMargin > 0 ? p.oSymbol.ExtraMargin : 0));//通过固定保证金来计算持仓保证金占用
            //}

            //其余品种保证金按照最新价格计算
            if (this.MarginByMoney > 0)
            {
                return p.UnsignedSize * price * p.oSymbol.Multiple * this.MarginByMoney;
            }
            if(this.MarginByVolume>0)
            {
                return p.UnsignedSize * this.MarginByVolume;
            }
            return 0;
        }

        /// <summary>
        /// 计算某个委托冻结保证金
        /// </summary>
        /// <param name="o"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public decimal CalMarginFrozen(Order o, decimal currentPrice)
        {
            if (!o.IsEntryPosition) return 0;//平仓委托不冻结保证金
            decimal targetPrice = o.isMarket ? currentPrice : (o.isLimit ? o.LimitPrice : o.StopPrice);
            if (this.MarginByMoney>0)
            {
                return o.UnsignedSize *targetPrice* o.oSymbol.Multiple * this.MarginByMoney;
            }
            if (this.MarginByVolume > 0)
            {
                return o.UnsignedSize * this.MarginByVolume;
            }
            return 0;
        }

        public decimal CalMarginFrozen(Symbol symbol, int size,decimal price, QSEnumOffsetFlag flag = QSEnumOffsetFlag.OPEN)
        {
            if (flag != QSEnumOffsetFlag.OPEN) return 0;
            if (this.MarginByMoney > 0)
            {
                return size * price * symbol.Multiple * this.MarginByMoney;
            }
            if (this.MarginByVolume > 0)
            {
                return size * this.MarginByVolume;
            }
            return 0;
        }
    }

    
}
