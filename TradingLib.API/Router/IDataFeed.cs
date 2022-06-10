using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public interface IDataFeed:IConnecter
    {
        /// <summary>
        /// ����һ���Լ������,ָ�������Լͨ���ĸ�����ͨ�����з���
        /// ���ڼ���FastTickSrv ��FastTickģʽ��
        /// ����������Խ���������ͨ���ӿ�,�ڽ��ܿͻ��˵����鶩��ʱ��Ҫ��ȷָ��ĳ���Լͨ���ĸ�ͨ�����ж���
        /// </summary>
        /// <param name="symbols"></param>
        /// <param name="type"></param>
        void RegisterSymbols(List<Symbol> symbols);

        /// <summary>
        /// �������ر�
        /// </summary>
        event TickDelegate GotTickEvent;
    }
}
