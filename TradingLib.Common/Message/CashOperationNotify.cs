﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 出入金通知
    /// 用于向客户端通知 出入金操作情况
    /// </summary>
    public class CashOperationNotify : NotifyResponsePacket
    {
        public CashOperationNotify()
        {
            _type = MessageTypes.CASHOPERATIONNOTIFY;
        }

        /// <summary>
        /// 出入金金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 出入金类型
        /// </summary>
        public QSEnumCashOperation OperationType { get; set; }



        public override string ContentSerialize()
        {
            return string.Format("{0},{1}", this.Amount, this.OperationType);
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            this.Amount = decimal.Parse(rec[0]);
            this.OperationType = (QSEnumCashOperation)Enum.Parse(typeof(QSEnumCashOperation), rec[1]);
        }
    }
}