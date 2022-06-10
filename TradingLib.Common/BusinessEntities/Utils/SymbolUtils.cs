using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public static class SymbolUtils
    {
        /// <summary>
        /// Calc base commission
        /// 按设定的手续费率计算手续费
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static decimal CalcBaseCommission(this Symbol symbol, Trade f)
        {
            if (f.IsEntryPosition)
            {
                if (symbol.EntryCommission < 1)
                {
                    return symbol.EntryCommission * f.xPrice * f.UnsignedSize * f.oSymbol.Multiple;
                }
                else
                {
                    return symbol.EntryCommission * f.UnsignedSize;
                }
            }
            else
            {
                decimal commission = 0;
                foreach (var close in f.CloseDetails)
                {
                    if (!close.IsCloseYdPosition)
                    {
                        if (symbol.ExitCommission < 1)
                        {
                            commission += symbol.ExitCommission * close.ClosePrice * close.CloseVolume * close.oSymbol.Multiple;
                        }
                        else
                        {
                            commission += symbol.ExitCommission * close.CloseVolume;
                        }
                    }
                    else
                    {
                        if (symbol.ExitCommissionToday < 1)
                        {
                            commission += symbol.ExitCommissionToday * close.ClosePrice * close.CloseVolume * close.oSymbol.Multiple;
                        }
                        else
                        {
                            commission += symbol.ExitCommissionToday * close.CloseVolume;
                        }
                    }
                }
                return commission;
            }
        }

        /// <summary>
        /// symbol name
        /// 获得文字名称
        /// 股指1701
        /// 恒指1701
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="price"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static string GetTitleName(this Symbol symbol,bool islong)
        {
            if (!string.IsNullOrEmpty(symbol.Name)) return symbol.Name;
            switch (symbol.SecurityFamily.Type)
            { 
                case SecurityType.FUT:
                    return string.Format("{0}{1}", symbol.SecurityFamily.Name, symbol.GetFutureNumSuffix(islong));
                default:
                    return symbol.SecurityFamily.Code;
            }
        }

        /// <summary>
        /// IF1701 IF01
        /// HSI1701 HSI01
        /// 获得合约的字母名
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="islong"></param>
        /// <returns></returns>
        public static string GetAlphabetName(this Symbol symbol, bool islong)
        {
            switch (symbol.SecurityFamily.Type)
            {
                case SecurityType.FUT:
                    return string.Format("{0}{1}", symbol.SecurityFamily.Code, symbol.GetFutureNumSuffix(islong));
                default:
                    return symbol.SecurityFamily.Code;
            }
        }


        /// <summary>
        /// 获得合约手续费项目键值
        /// 品种类型-代码代码-月份
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        public static string GetCommissionItemKey(this Symbol sym)
        {
            return string.Format("{0}-{1}-{2}", sym.SecurityFamily.Type, sym.SecurityFamily.Code, sym.GetMonth());
        }

        /// <summary>
        /// 获得合约月份
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        public static int GetMonth(this Symbol sym)
        {
            if (sym.SecurityFamily.Type == SecurityType.FUT)
            {
                string month = sym.ExpireDate.ToString().Substring(4, 2);//20150101
                return int.Parse(month);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 可以通过ExpireDate进行快速处理
        /// </summary>
        /// <param name="sym"></param>
        /// <param name="isLong"></param>
        /// <returns></returns>
        static string GetFutureNumSuffix(this Symbol sym,bool isLong)
        {
            int year, month;
            string sec;
            sym.ParseFututureContract(out sec, out year, out month);
            if(isLong)
            {
                return string.Format("{0}{1:D2}", year, month).Substring(2);
            }
            else
            {
                return string.Format("{0}{1:D2}", year, month).Substring(4);
            }
        }

        /// <summary>
        /// 获得某个合约的手续费设置
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        public static CommissionConfig GetCommissionConfig(this Symbol sym)
        {
            CommissionConfig cfg = new CommissionConfig();
            cfg.Symbol = sym.Symbol;
            cfg.OpenRatioByMoney = sym.EntryCommission < 1 ? sym.EntryCommission : 0;
            cfg.OpenRatioByVolume = sym.EntryCommission > 1 ? sym.EntryCommission : 0;
            cfg.CloseRatioByVolume = sym.ExitCommission > 1 ? sym.ExitCommission : 0;
            cfg.CloseRatioByMoney = sym.ExitCommission < 1 ? sym.ExitCommission : 0;
            cfg.CloseTodayRatioByMoney = sym.ExitCommission < 1 ? sym.ExitCommission : 0;
            cfg.CloseTodayRatioByVolume = sym.ExitCommission > 1 ? sym.ExitCommission : 0;
            return cfg;
        }

        /// <summary>
        /// 获得某个合约的保证金设置
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        public static MarginConfig GetMarginConfig(this Symbol sym)
        {
            MarginConfig cfg = new MarginConfig();
            cfg.Symbol = sym.Symbol;
            cfg.LongMarginRatioByMoney = sym.Margin < 1 ? sym.Margin : 0;
            cfg.ShortMarginRatioByMoney = sym.Margin < 1 ? sym.Margin : 0;
            cfg.LongMarginRatioByVolume = sym.Margin > 1 ? sym.Margin : 0;
            cfg.ShortMarginRatioByVoume = sym.Margin > 1 ? sym.Margin : 0;
            return cfg;
        }


        /// <summary>
        /// 获得合约对应的连续合约键值用于储存Bar数据
        /// HKEX-HSI-01
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        public static string GetContinuousKey(this Symbol sym)
        {
            return string.Format("{0}-{1}-{2}", sym.SecurityFamily.Exchange.EXCode, sym.SecurityFamily.Code, sym.Month);
        }


        /// <summary>
        /// 获得合约对应的连续合约编号
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        public static string GetContinuousSymbol(this Symbol sym)
        {
            return string.Format("{0}{1}", sym.SecurityFamily.Code, sym.Month);
        }

        /// <summary>
        /// 获得以交易所-合约 组合的唯一字符串键
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        public static string GetUniqueKey(this Symbol sym)
        {
            return string.Format("{0}-{1}",sym.Exchange,sym.Symbol);
        }

        /// <summary>
        /// 获得合约注册时 包含品种类别信息的合约字符串
        /// symbol@type
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        public static string GetFullSymbol(this Symbol sym)
        {
            switch (sym.SecurityType)
            { 
                case SecurityType.FUT:
                    return sym.Symbol;
                case SecurityType.STK:
                    return "{0}@STK".Put(sym.Symbol);
                default:
                    return "{0}@{1}".Put(sym.Symbol,sym.SecurityType.ToString());
            }
        }


        /// <summary>
        /// 将合约进行解析
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="secCode"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public static void ParseFututureContract(this Symbol sym, out string secCode, out int year, out int month)
        {
            
            string symbol = sym.Symbol;
            string expire = symbol.Substring(symbol.Length - 4, 4);
            //郑州CF610 格式
            if (sym.SecurityFamily.Exchange.EXCode == "CZCE")
            {
                expire = "1" + symbol.Substring(symbol.Length - 3, 3);
            }
            int num = 0;
            secCode = string.Empty;
            year = 0;
            month = 0;
            //后四位是数字 解析后判定月份和年份
            if (int.TryParse(expire, out num))
            {
                month = int.Parse(expire.ToString().Substring(expire.Length - 2, 2));
                year = 2000 + int.Parse(expire.ToString().Substring(0, 2));
                secCode = symbol.Substring(0, symbol.Length - 4);
            }
            else
            {
                expire = symbol.Substring(symbol.Length - 2, 2);//获取后面两位 年
                //后两位是数字 
                if (int.TryParse(expire, out num))
                {
                    year = 2000 + int.Parse(expire);
                    month = int.Parse(SymbolImpl.MonthLetter2Num(symbol.Substring(symbol.Length - 3, 1)));
                    secCode = symbol.Substring(0, symbol.Length - 3);
                }
                else
                {
                    expire = symbol.Substring(symbol.Length - 1, 1);//获取后面1位 年
                    if (int.TryParse(expire, out num))
                    {
                        year = 2000 + (10 + num);
                        month = int.Parse(SymbolImpl.MonthLetter2Num(symbol.Substring(symbol.Length - 2, 1)));
                        secCode = symbol.Substring(0, symbol.Length - 2);
                    }
                }
            }
        }

        
    }
}
