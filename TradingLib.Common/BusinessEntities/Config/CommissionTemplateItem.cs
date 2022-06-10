using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class CommissionTemplateItemSetting
    {
        /// <summary>
        /// 设置项ID
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
        /// 开仓手续费 按金额
        /// </summary>
        public decimal OpenByMoney { get; set; }

        /// <summary>
        /// 开仓手续费 按手数
        /// </summary>
        public decimal OpenByVolume { get; set; }


        /// <summary>
        /// 平今手续费 按金额
        /// </summary>
        public decimal CloseTodayByMoney { get; set; }

        /// <summary>
        /// 平今手续费 按手数
        /// </summary>
        public decimal CloseTodayByVolume { get; set; }

        /// <summary>
        /// 平仓手续费 按金额
        /// </summary>
        public decimal CloseByMoney { get; set; }

        /// <summary>
        /// 平仓手续费 按手数
        /// </summary>
        public decimal CloseByVolume { get; set; }

        /// <summary>
        /// 上浮一定百分比
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

        /// <summary>
        /// 品种类别
        /// </summary>
        public SecurityType SecurityType { get; set; }

        //[NoJsonExportAttr()]
        [Newtonsoft.Json.JsonIgnore]
        public string CommissionItemKey
        {
            get
            {
                return string.Format("{0}-{1}-{2}", this.SecurityType, this.Code, this.Month);
            }
        }
    }


    /// <summary>
    /// 手续费模板的设置项
    /// </summary>
    public class CommissionTemplateItem : CommissionTemplateItemSetting
    {


        /// <summary>
        /// 计算手续费
        /// </summary>
        /// <param name="f"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public decimal CalCommission(Trade f, QSEnumOffsetFlag offset)
        {
            decimal commission = 0;
            switch(f.oSymbol.SecurityFamily.Type)
            {
                case SecurityType.FUT:
                    {
                        if (f.IsEntryPosition)
                        {
                            commission= (this.OpenByMoney != 0 ? CalcCommissionByMoney(f, this.OpenByMoney) : CalcCommissionByVolume(f, this.OpenByVolume));
                        }
                        else
                        {
                            foreach (var close in f.CloseDetails)
                            {

                                if (!close.IsCloseYdPosition)
                                {
                                    commission += (this.CloseTodayByMoney != 0 ? CalcCommissionByMoney(close, this.CloseTodayByMoney) : CalcCommissionByVolume(close, this.CloseTodayByVolume));
                                }

                                else
                                {
                                    commission += (this.CloseByMoney != 0 ? CalcCommissionByMoney(close, this.CloseByMoney) : CalcCommissionByVolume(close, this.CloseByVolume));
                                }
                            }
                        }
                    }
                    break;
                case SecurityType.STK:
                    {
                        switch (f.OffsetFlag)
                        {
                            case QSEnumOffsetFlag.OPEN://开仓手续费
                                commission = (this.OpenByMoney != 0 ? CalcCommissionByMoney(f, this.OpenByMoney) : CalcCommissionByVolume(f, this.OpenByVolume));
                                break;
                            case QSEnumOffsetFlag.CLOSE://平仓手续费
                            case QSEnumOffsetFlag.CLOSEYESTERDAY:
                            case QSEnumOffsetFlag.CLOSETODAY:
                                commission = (this.CloseByMoney != 0 ? CalcCommissionByMoney(f, this.CloseByMoney) : CalcCommissionByVolume(f, this.CloseByVolume));
                                break;
                            default:
                                commission = (this.OpenByMoney != 0 ? CalcCommissionByMoney(f, this.OpenByMoney) : CalcCommissionByVolume(f, this.OpenByVolume));
                                break;
                        }
                    }
                    break;
                default :
                    commission = 0;
                    break;
            }
            return commission;

        }


        decimal CalcCommissionByMoney(Trade f,decimal commissionrate)
        {
            return commissionrate * f.xPrice * f.UnsignedSize * f.oSymbol.Multiple;
        }

        decimal CalcCommissionByVolume(Trade f, decimal commissionrate)
        {
            return commissionrate * f.UnsignedSize;
        }

        decimal CalcCommissionByMoney(PositionCloseDetail close, decimal commissionrate)
        {
            return commissionrate * close.ClosePrice * close.CloseVolume * close.oSymbol.Multiple;
        }

        decimal CalcCommissionByVolume(PositionCloseDetail close, decimal commisionrate)
        {
            return commisionrate * close.CloseVolume;
        }

    }
}
