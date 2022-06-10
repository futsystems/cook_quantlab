using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// LSPosition Tracker
    /// 持仓维护器
    /// 按持仓的多空方向进行维护,每个方向的持仓维护其PositionTracker可以同时维护多个合约
    /// </summary>
    public class LSPositionTracker : IEnumerable<Position>
    {
        public PositionTracker LongPositionTracker { get { return _ltk; } }
        /// <summary>
        /// long position tracker
        /// </summary>
        public PositionTracker _ltk;

        public PositionTracker ShortPositionTracker { get { return _stk; } }

        //昨日持仓对象
        public List<PositionDetail> _ydpositions = new List<PositionDetail>();

        /// <summary>
        /// 所有隔夜持仓明细
        /// </summary>
        public IEnumerable<PositionDetail> YDPositionDetails { get { return _ydpositions; } }

        /// <summary>
        /// 所有持仓
        /// </summary>
        public IEnumerable<Position> Positions { get { return poslist; } }
        /// <summary>
        /// short position tracker
        /// </summary>
        PositionTracker _stk;
        string _defaultacct = string.Empty;


        public LSPositionTracker(string account)
        {
            _defaultacct = account;
            _ltk = new PositionTracker(account,QSEnumPositionDirectionType.Long);
            _ltk.NewPositionEvent +=new PositionDelegate(_ltk_NewPositionEvent);

            _stk = new PositionTracker(account,QSEnumPositionDirectionType.Short);
            _stk.NewPositionEvent +=new PositionDelegate(_stk_NewPositionEvent);
        }

        //由于多 空 分别在不同的poslist中生成并维护 为了方便对所有postion进行访问，在新的postion生成时我们在poslist内保存一个引用
        ThreadSafeList<Position> poslist = new ThreadSafeList<Position>();
        void _ltk_NewPositionEvent(Position pos)
        {
            poslist.Add(pos);
            NewPosition(pos);
            //对外暴露持仓明细和平仓明细事件
            pos.NewPositionCloseDetailEvent += new Action<Trade,PositionCloseDetail>(NewPositionCloseDetail);
            pos.NewPositionDetailEvent += new Action<Trade, PositionDetail>(NewPositionDetail);
        }

        void _stk_NewPositionEvent(Position pos)
        {
            poslist.Add(pos);
            NewPosition(pos);
            pos.NewPositionCloseDetailEvent += new Action<Trade,PositionCloseDetail>(NewPositionCloseDetail);
            pos.NewPositionDetailEvent += new Action<Trade, PositionDetail>(NewPositionDetail);
        }

        #region 响应交易对象数据
        /// <summary>
        /// 更新持仓管理器中的最新行情数据
        /// </summary>
        /// <param name="?"></param>
        public void GotTick(Tick k)
        {
            _ltk.GotTick(k);
            _stk.GotTick(k);
        }

        /// <summary>
        /// 获得一个成交记录
        /// 用于更新持仓状态
        /// </summary>
        /// <param name="f"></param>
        public void GotFill(Trade f,out bool accept)
        {
            bool entryposition = f.IsEntryPosition;
            //开仓 多头 /平仓 空头
            if ((f.IsEntryPosition && f.Side) || ((!f.IsEntryPosition) && (!f.Side)))
            {
                _ltk.GotFill(f,out accept);
            }
            else
            {
                _stk.GotFill(f,out accept);
            }
        }


        /// <summary>
        /// 获得一个持仓明细记录
        /// 用于恢复历史持仓
        /// </summary>
        /// <param name="p"></param>
        public void GotPosition(PositionDetail p)
        {
            if (p.Volume == 0) return;//无实际持仓
            if (p.Side)
            {
                _ltk.GotPosition(p);
            }
            else
            {
                _stk.GotPosition(p);
            }
            _ydpositions.Add(p);
        }
        #endregion

        bool _inReCalculate = false;
        /// <summary>
        /// 是否处于重新计算状态
        /// 重新计算状态用于重新加载持仓明细和成交数据生成当前持仓状态 不对外触发平仓明细与持仓事件
        /// </summary>
        public bool InReCalculate { get { return _inReCalculate; } set { _inReCalculate = value; } }
        /// <summary>
        /// 清空记录的数据
        /// </summary>
        public void Clear()
        {
            _ydpositions.Clear();
            _ltk.Clear();
            _stk.Clear();
            poslist.Clear();
        }

        /// <summary>
        /// 将结算过的持仓丢弃
        /// </summary>
        public void DropSettled()
        {
            _ltk.DropSettled();
            _stk.DropSettled();
        }


        #region 获得对应的持仓数据
        public Position this[string symbol, bool side]
        {
            get
            {
                return this[symbol, _defaultacct, side];
            }
        }
        /// <summary>
        /// 获得某个合约 某个帐户 某个方向的持仓
        /// 获得持仓时需要明确提供对应的方向
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="account"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        public Position this[string symbol, string account, bool side]
        {
            get
            {
                if (side)
                {
                    return _ltk[symbol, account];
                }
                else
                {
                    return _stk[symbol, account];
                }
            }
        }
        #endregion


        #region Enumerator 用于遍历position
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<Position> IEnumerable<Position>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Position> GetEnumerator()
        {
            return poslist.Where(pos=>!pos.Settled&&pos.oSymbol!=null).GetEnumerator();
        }
        #endregion


        #region 对外触发持仓类事件
        /// <summary>
        /// 新的持仓明细生成事件
        /// </summary>
        void NewPositionDetail(Trade open, PositionDetail detail)
        {
            if (NewPositionDetailEvent != null && !_inReCalculate)
            {
                NewPositionDetailEvent(open, detail);
            }
        }
        public event Action<Trade, PositionDetail> NewPositionDetailEvent;

        /// <summary>
        /// 产生新的持仓对象
        /// </summary>
        /// <param name="detail"></param>
        void NewPositionCloseDetail(Trade close,PositionCloseDetail detail)
        {
            if (NewPositionCloseDetailEvent != null && !_inReCalculate)
                NewPositionCloseDetailEvent(close,detail);
        }
        public event Action<Trade,PositionCloseDetail> NewPositionCloseDetailEvent;


        /// <summary>
        /// 产生新的平仓明细
        /// </summary>
        /// <param name="pos"></param>
        void NewPosition(Position pos)
        {
            if (NewPositionEvent != null && !_inReCalculate)
                NewPositionEvent(pos);
        }
        public event Action<Position> NewPositionEvent;
        #endregion


    }
}
