using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    /// <summary>
    /// ϵͳ������
    /// ��������ϵͳ����ʱ��ز���
    /// </summary>
    public static class Const
    {
        public const string APIVersion = "2.0.2";
        /// <summary>
        /// �������ɨ��Ƶ��
        /// ÿ�����ٺ���ɨ��һ�������б�
        /// </summary>
        public const int TASKFREQ = 100;
        /// <summary>
        /// integer precision
        /// </summary>
        public const int IPREC = 1000000;
        /// <summary>
        /// inverse integer precision
        /// </summary>
        public const decimal IPRECV = .000001m;


        //====���ز��ֵ�ϵͳ����=============================================================================
        /// <summary>
        /// ��Ϣ���𲽼��,��ĳ���ͻ��˷�����Ϣ����������ʱ,�������ؼ��
        /// </summary>
        public const int TPStartNum = 100;//����Ϣ�����ۼƵ�����ʱ��ʼ�������
        /// <summary>
        /// ���������ؼ���,����������Ϣ��ȷ�������ٶ�
        /// </summary>
        public const int TPCheckNum = 10000;//�������������Ϣ����Ŀ(�������Ŀ�ڼ���TP)
        /// <summary>
        /// ��TPֵ���ڶ���ʱ,�ܾ��ͻ��˷��͵���Ϣ
        /// </summary>
        public const double TPRejectValue = 1.5;//TP��ֵ�ﵽ���ٺ�ܾ��õ�ַ����Ϣ
        /// <summary>
        /// ��TPֵ���ڶ���ʱ,
        /// </summary>
        public const double TPStopValue = 1;//Tp��ֵ���͵����ٺ�ֹͣ���

        public const int TLDEFAULTBASEPORT = 5570;//����ͨѶ�˿� 5571(DNS) 5572(Tick)
        public const int TLDEFAULTMGRPORT = 6670;//����˿�

        /// <summary>
        /// �ڲ�ѯ������Ƿ��з������ʱ,ͨ��HelloServer��ȷ�Ϸ���˷������,�趨helloserver�ر��ӳ�
        /// </summary>
        public const int SOCKETREPLAYTIMEOUT = 5;

        //====�������Ƴ���==================================================================================
        //������������
        public const int TICKHEARTBEATMS = 5 * 1000;//��������heartbeat�ļ��

        public const int TICKHEARTBEATDEADMS = TICKHEARTBEATMS * 3; //�ڶ���ʱ����û���յ�tickheartbead�����½�����������

        public const int TICKHEARTBEATCHECKFREQ = 1 * 1000;//��������ά���̵߳ļ��Ƶ��
        /// <summary>
        /// ��������û���յ��������Ϣ,�����������ر�,���ڼ�������Ƿ��� ��������ʱ������30��
        /// </summary>
        public const int SENDHEARTBEATMS = 1 * 5 * 1000;//�������������ӳ�(��λ��,N����û�еõ����������κ���Ϣ,������������)
        /// <summary>
        /// �ڶ��ٸ���������û�еõ�����˵���������ر�,����Ϊ�����˵����Ӷ�ʧ,�ͻ��˻᳢�����½�������
        /// </summary>
        public const int HEARTBEATDEADMS = SENDHEARTBEATMS * 3;//(��N�������������ڷ�����û�л�Ӧ,�������Ӷ�ʧ,�������½�������)
        /// <summary>
        /// Ĭ�ϼ������������ر���� 50MS(����ά���̵߳�ˢ��Ƶ��,ÿ5MS����ϴη���������ʱ��)
        /// </summary>
        public const int DEFAULTWAIT = 500;

        //ע:�ͻ��˰�һ��Ƶ�������˷���������Ϣ,������һ�����ķ�������Ϣ������
        //�ͻ���10��/�� ��ô10���ͻ��˾���ÿ��1��,1000���ͻ��˾���ÿ��100��������Ϣ,������Ҫ�ӳ����ͼ��
        /// <summary>
        /// Ĭ�������˷���������Ϣ��� 30��(ÿ30����߷�����,�ͻ���������)
        /// </summary>
        public const int HEARTBEATPERIOD = 10;//ע�� clientsession�ı����ǰ���heartbeat�����е�

        /// <summary>
        /// �Ͽ����Ӻ�����볢������һ�η����� Ĭ��5�볢��һ������
        /// </summary>
        public const int RECONNECTDELAY = 5;

        /// <summary>
        ///�������Ӵ���
        /// </summary>
        public const int RECONNECTTIMES = 20;
        /// <summary>
        /// �ͻ�������ʱ��,����ڶ�������,û���յ��ͻ�����Ϣ,����Ϊ�ͻ��Ѿ�����,��Ҫ��������ͻ����б��Լ�SessionLoger�е����ݿ���Ϣ
        /// 2��heartbeatperiod�� ���û���յ��ͻ��˵�������Ϣ,����Ϊ����ʧЧ
        /// </summary>
        public const int CLIENTDEADTIME = HEARTBEATPERIOD * 6;

        /// <summary>
        /// ����ʱ�����һ��dead client����,4������������ڽ���һ��dead client ���
        /// </summary>
        public const int CLEARDEADSESSIONPERIOD = HEARTBEATPERIOD * 6;
    }
}
