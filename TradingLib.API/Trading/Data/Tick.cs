using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface Tick
    {
        /// <summary>
        /// Tick类型
        /// </summary>
        EnumTickType Type { get; set; }

        /// <summary>
        /// 更新类型
        /// </summary>
        string UpdateType { get; set; }
        
        /// <summary>
        /// symbol for tick
        /// </summary>
        string Symbol { get; set; }

        /// <summary>
        /// trade exchange
        /// </summary>
        string Exchange { get; set; }
        
        /// <summary>
        /// tick date
        /// </summary>
        int Date { get; set; }

        /// <summary>
        /// tick time in 24 format (4:59pm => 1659)
        /// </summary>
        int Time { get; set; }

        /// <summary>
        /// HostTime
        /// </summary>
        ulong HostTime { get; set; }
        
        /// <summary>
        /// TickTime
        /// </summary>
        ulong TickTime { get; set; }

        #region Trade
        /// <summary>
        /// size of last trade
        /// </summary>
        double Size { get; set; } 
        /// <summary>
        /// trade price
        /// </summary>
        double Price { get; set; }


        /// <summary>
        /// Trade id
        /// </summary>
        string TradeId { get; set; }

        /// <summary>
        /// trade side flag
        /// 1 seller market maker 主动买
        /// 2 buyer market maker 主动卖
        /// 3 预计主动买
        /// 4 预计主动卖
        /// </summary>
        int TradeFlag { get; set;}
        #endregion

        #region Quote/Level2
        /// <summary>
        /// bid price
        /// </summary>
        double BidPrice { get; set; } 
        
        /// <summary>
        /// bid size
        /// </summary>
        double BidSize { get; set; } 
        

        /// <summary>
        /// offer price
        /// </summary>
        double AskPrice { get; set; }
        
        /// <summary>
        /// ask size
        /// </summary>
        double AskSize { get; set; }

        string AskExchange { get; set; }

        string BidExchange { get; set; }
        int Depth { get; set; }
        
        double AskPrice2 { get; set; }
        double BidPrice2 { get; set; }
        double AskSize2 { get; set; }
        double BidSize2 { get; set; }

        double AskPrice3 { get; set; }
        double BidPrice3 { get; set; }
        double AskSize3 { get; set; }
        double BidSize3 { get; set; }

        double AskPrice4 { get; set; }
        double BidPrice4 { get; set; }
        double AskSize4 { get; set; }
        double BidSize4 { get; set; }

        double AskPrice5 { get; set; }
        double BidPrice5 { get; set; }
        double AskSize5 { get; set; }
        double BidSize5 { get; set; }

        double AskPrice6 { get; set; }
        double BidPrice6 { get; set; }
        double AskSize6 { get; set; }
        double BidSize6 { get; set; }

        double AskPrice7 { get; set; }
        double BidPrice7 { get; set; }
        double AskSize7 { get; set; }
        double BidSize7 { get; set; }

        double AskPrice8 { get; set; }
        double BidPrice8 { get; set; }
        double AskSize8 { get; set; }
        double BidSize8 { get; set; }

        double AskPrice9 { get; set; }
        double BidPrice9 { get; set; }
        double AskSize9 { get; set; }
        double BidSize9 { get; set; }

        double AskPrice10 { get; set; }
        double BidPrice10 { get; set; }
        double AskSize10 { get; set; }
        double BidSize10 { get; set; }
        
        #endregion
        

       
        


      
    }

    /// <summary>
    /// ���������鴫������е��������
    /// ���ݲ�ͬ�����ݿ��Խ�������ͬ����������
    /// ������ÿ�ζ�������ͬ������ ����߿����յȲ������仯�ı���
    /// Tick����Ϊ�˼��ݺ����ֲ�ͬ�����������鱨�� ��Ҫ�����ۺϴ���
    /// 
    /// </summary>
    public enum EnumTickType
    { 
        /// <summary>
        /// �ɽ���Ϣ
        /// </summary>
        TRADE=0,
        /// <summary>
        /// ������Ϣ
        /// </summary>
        QUOTE=1,
        /// <summary>
        /// ���������Ϣ
        /// </summary>
        LEVEL2=2,
        /// <summary>
        /// ͳ����Ϣ Open 
        /// </summary>
        SUMMARY=3,
        /// <summary>
        /// ��������
        /// </summary>
        SNAPSHOT=4,

        /// <summary>
        /// ʱ�� ���ڸ��µ�ǰ����ʱ��
        /// </summary>
        TIME = 5,

        /// <summary>
        /// ��Ʊ��������
        /// </summary>
        STKSNAPSHOT = 6,

    }

    public class InvalidTick : Exception { }

}
