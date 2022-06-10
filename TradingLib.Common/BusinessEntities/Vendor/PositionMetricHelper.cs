using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public  class PositionMetricHelper
    {
        public static int GenPositionIncrement(PositionMetric mertic, PositionAdjustmentResult operation, bool justoneside = false)
        {
            if (justoneside)
            {
                int size = Math.Max(mertic.LongHoldSize, mertic.ShortHoldSize);//取多空持仓中大的一个
                int aftersize = Math.Max(mertic.LongHoldSize + operation.LongEntry - operation.LongExit, mertic.ShortHoldSize + operation.ShortEntry - operation.ShortExit);
                return aftersize - size;
            }
            else
            {
                //开仓数量- 平仓数量 = 累计的开仓数量
                return operation.LongEntry + operation.ShortEntry - operation.LongExit - operation.ShortExit;
            }
        }

        /// <summary>
        /// 返回净持仓开仓操作结果
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static PositionAdjustmentResult GenPositionAdjustmentResult(PositionMetric mertic, Order o)
        {
            bool posside = o.PositionSide;//持仓方向
            int opensize = o.UnsignedSize;//委托数量

            PositionAdjustmentResult result = new PositionAdjustmentResult();
            int canexitsize = posside ? mertic.ShortCanExitSaize : mertic.LongCanExitSize;//获得反向持仓的可平数量
            //方式一 开仓数量 可以全部转换成平仓数量 该操作不会增加任何保证金
            //如果开仓的数量 小于等于 反向持仓的可平数量,则该委托可以以净持仓的方式处理
            if (opensize <= canexitsize)
            {
                if (posside)//多头开仓
                {
                    result.ShortExit = opensize;
                }
                else//空头开仓
                {
                    result.LongExit = opensize;
                }
            }
            else
            {
                if (posside)
                {
                    result.ShortExit = canexitsize;
                    result.LongEntry = opensize - canexitsize;
                }
                else
                {
                    result.LongExit = canexitsize;
                    result.ShortEntry = opensize - canexitsize;
                }
            }
            return result;
        }
    }
}
