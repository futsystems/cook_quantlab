using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// easily trade positions for a collection of securities.
    /// automatically builds positions from existing positions and new trades.
    /// 管理交易仓位
    /// </summary>
    [Serializable]
    public class PositionTracker : GenericTracker<Position>,IEnumerable<Position>
    {
        string _defaultacct = string.Empty;
        public string DefaultAccount { get { return _defaultacct; } set { _defaultacct=value; } }
        /// <summary>
        /// 持仓维护器 持仓方向
        /// </summary>
        public QSEnumPositionDirectionType DirectionType { get { return _directiontype; } }
        protected QSEnumPositionDirectionType _directiontype = QSEnumPositionDirectionType.Net;

        ///// <summary>
        ///// 设定一个默认的PositionTracker其为Net类型的持仓管理
        ///// </summary>
        public PositionTracker() : this("",QSEnumPositionDirectionType.Net) { }

        /// <summary>
        /// create tracker with approximate # of positions
        /// </summary>
        /// <param name="estimatedPositions"></param>
        public PositionTracker(string account,QSEnumPositionDirectionType type) 
            : base(type.ToString()+"-POSITION")
        {
            _directiontype = type;
            NewTxt += new TextIdxDelegate(PositionTracker_NewTxt);
        }

        /// <summary>
        /// 当有新的标签创建时 对外触发通知持仓对象的生成
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="idx"></param>
        void PositionTracker_NewTxt(string txt, int idx)
        {
            if (NewSymbol!= null)
                NewSymbol(txt);
            if (NewPositionEvent != null)
                NewPositionEvent(this[idx]);
        }

        #region 通过Position PositionDetail Trade更新对应持仓对象
        /// <summary>
        /// 获得一个持仓明细数据 用于加载隔夜持仓数据 生成初始化持仓状态
        /// </summary>
        /// <param name="p"></param>
        public void GotPosition(PositionDetail p)
        {
            Adjust(p);
        }

        public void GotFill(Trade f)
        { 
            bool accept= false;
            GotFill(f, out accept);
        }
        /// <summary>
        /// 获得一个成交对象
        /// </summary>
        /// <param name="f"></param>
        public void GotFill(Trade f,out bool accept) 
        { 
            Adjust(f,out accept); 
        }
        #endregion



        /// <summary>
        /// 用Tick行情驱动持仓对象 实时更新持仓的行情数据用于获得动态浮动盈亏
        /// </summary>
        /// <param name="k"></param>
        public void GotTick(Tick k)
        {
            //clearCentre中计算账户保证金,平仓盈利,浮动盈利等均需要遍历所有的position来进行计算,遍历的时候使用foreach(position p in positiontracker)
            //委托处理与tick是同步进行的，tick更新会调用gottick来遍历所有的position进行价格更新,这里出现2个线程同时操作一个对象。
            //因此gottick不能用Enumerator,使用toArray来进行遍历 避免冲突
            //Position[] parray = this.ToArray();
            foreach (Position p in this)
            {
                p.GotTick(k);
            }
        }

        

        /// <summary>
        /// clear all positions.  use with caution.
        /// also resets default account.
        /// </summary>
        public override void Clear()
        {
            base.Clear();
        }

        


        #region 检索获得持仓对象
        /// <summary>
        /// get position given positions symbol (assumes default account)
        /// 查询某个symbol的position
        /// 如果没有对应的持仓会返回一个空的默认持仓
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public new Position this[string symbol] 
        { 
            get
            {
                return this[symbol, _defaultacct];
            }
        }

        /// <summary>
        /// get a position in tracker given symbol and account
        /// 通过symbol,account来查询某个position
        /// 如果没有对应的持仓会返回一个空的默认持仓,此处并没有在PositionTracker加入该持仓
        /// 此处持仓是通过合约symbol创建的因此没有对应oSymbol数据需要在获得PositionDetail或成交时候获得对应的Symbol数据
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public Position this[string symbol, string account] 
        { 
            get 
            {
                int idx = getindex(symbol + account);
                if (idx<0)
                    return new PositionImpl(symbol,0,0,0,account,_directiontype);
                    //addindex(symbol + account, new PositionImpl(symbol, 0, 0, 0, account));//当没有任何成交记录时,首次访问获得该合约-帐户所对应的持仓 并且初始化该持仓否则 对该pos的引用将发生错误 应为持仓本身发生了变化
                return this[idx];
            } 
        }

        /// <summary>
        /// get position given positions symbol (assumes default account)
        /// 通过idx来查询position
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public new Position this[int idx]
        {
            get
            {
                if (idx < 0)
                    return new PositionImpl();
                Position p = base[idx];
                if (p == null)
                    return new PositionImpl();
                    //return new PositionImpl(getlabel(idx));
                return p;
            }
        }
        #endregion

        public override Type TrackedType
        {
            get
            {
                return typeof(Position);
            }
        }

        /// <summary>
        /// 覆盖当前仓位或者新建一个仓位
        /// 当初始加载时 如果有持仓需要管理 则用储存的持仓数据填充当前持仓状态
        /// 覆盖该持仓时是以复制持仓的形式进行
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        //public decimal Adjust(Position newpos)
        //{
        //    //设定默认帐户
        //    if (_defaultacct == string.Empty)
        //        _defaultacct = newpos.Account;
        //    int idx = getindex(newpos.Symbol + newpos.Account);

        //    //如果没有symbol+account对应的position则新增加一个仓位,或者用新仓位覆盖掉原来的仓位
        //    Position p = new PositionImpl(newpos);
        //    p.DirectionType = _directiontype;

        //    if (idx < 0)//如果没有对应持仓对象 则添加Position
        //    {
        //        addindex(newpos.Symbol + newpos.Account, p);
        //    }
        //    else//如果存在对应持仓对象 更新持仓对象 并将对应的ClosedPL反映到当前PositionTracker上去
        //    {
        //        base[idx] = p;
        //    }
        //    return p.ClosedPL;
        //}


        /// <summary>
        /// 处理一笔成交,用于更新position状态,通过symbol+account形成唯一position标识符
        /// 对于新的持仓 是通过从成交转化成Position获得持仓
        /// </summary>
        /// <param name="fill"></param>
        /// <returns>any closed PL for adjustment</returns>
        public decimal Adjust(Trade fill,out bool accept)
        {
            //设定默认帐户
            if (_defaultacct == string.Empty)
                _defaultacct = fill.Account;
            int idx = getindex(fill.Symbol + fill.Account);
            decimal cpl = 0;

            if (idx < 0)//如果没有持仓对象 则创建一个空的持仓对象 并添加到Tracker
            {
                PositionImpl newpos = new PositionImpl(fill.Account, fill.Symbol, this.DirectionType);
                addindex(fill.Symbol + fill.Account, newpos);
                idx = getindex(fill.Symbol + fill.Account);
            }

            cpl += this[idx].Adjust(fill,out accept);//调用position来处理fill.形成closedpl
            return cpl;
        }

        /// <summary>
        /// 获得持仓明细数据
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public decimal Adjust(PositionDetail pos)
        {
            //设定默认帐户
            if (_defaultacct == string.Empty)
                _defaultacct = pos.Account;
            int idx = getindex(pos.Symbol + pos.Account);
            decimal cpl = 0;

            if (idx < 0)
            {
                //生成空的持仓数据并添加到维护其 然后再用持仓对象去更形pos
                PositionImpl newpos = new PositionImpl(pos.Account, pos.Symbol, this.DirectionType);
                addindex(pos.Symbol + pos.Account, newpos);//如果没有持仓添加对应的持仓 该持仓数据0数据
                idx = getindex(pos.Symbol + pos.Account);
            }
            cpl += this[idx].Adjust(pos);
            return cpl;
        }

        /// <summary>
        /// 注销某个持仓
        /// 通过getindex将无法获得该持仓对应的idx
        /// </summary>
        /// <param name="pos"></param>
        public void DropPosition(Position pos)
        {
            removeindex(pos.Symbol + pos.Account, pos);
        }

        //某个交易所结算完毕后 从持仓维护器中将结算完毕的持仓去掉并重置 如果只去index 则下个交易所 任然会遍历到该结算掉的持仓
        //TODO:完善实时结算过程中持仓对象的屏蔽
        public void DropSettled()
        {
            
            Position[] poslist = this.Where(pos => pos.Settled).ToArray();
            foreach (var pos in poslist)
            {
                removeindex(pos.Symbol + pos.Account, pos);
            }
        }

        /// <summary>
        /// called when a new position is added to tracker.
        /// 当有新的symbol产生position时,触发该事件
        /// </summary>
        public event SymDelegate NewSymbol;


        /// <summary>
        /// 当有新的持仓数据被建立时触发
        /// 比如某个帐户原来没有持仓，新建开仓单开仓后,PositionTracker就会为他创建一个Position
        /// </summary>
        public event PositionDelegate NewPositionEvent;

    }
       
}
