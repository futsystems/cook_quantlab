using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;


using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 原生CTP的TLSocketBase实现
    /// </summary>
    public class TCPSocket : TLSocketBase
    {

        protected NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        Socket _socket;
        Thread _recvThread = null;

        /// <summary>
        /// Socket是否处于连接状态
        /// </summary>
        public override bool IsConnected { get { return _socket != null; } }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="msg"></param>
        public override void Send(byte[] msg)
        {
            try
            {
                if (_socket == null)
                    throw new InvalidOperationException("Socket is null");
                if (_socket.Connected)
                {
                    _socket.Send(msg);
                }
            }
            catch (Exception ex)
            {
                logger.Error("socket send data error:" + ex.ToString());
            }
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        public override void Connect()
        {
            try
            {
                if (this.Server == null)
                {
                    throw new NullReferenceException("Need Set Server First");
                }
                if (this.IsConnected)
                {
                    logger.Warn("Socket already connected");
                    return;
                }
                logger.Info(string.Format("Connect to server:{0}", this.Server.ToString()));
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendBufferSize = BUFFERSIZE;
                socket.ReceiveBufferSize = BUFFERSIZE; //注默认接受数据BufferSize 8192 如果服务端发送一个大的Bar数据包 会导致数据接受异常
                socket.Connect(this.Server);

                if (socket.Connected)
                {
                    _socket = socket;
                }
                else
                {
                    _socket = null;
                    return;
                }

                _recvThread = new Thread(RecvProcess);
                _recvThread.IsBackground = true;
                _recvThread.Start();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("socket connect to server:{0} error:{1}", this.Server, ex));
            }


        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public override void Disconnect()
        {
            if (!this.IsConnected)
            {
                logger.Warn("Socket not connected");
                return;
            }
            logger.Info(string.Format("Disconnect from server:{0}", this.Server != null ? this.Server.ToString() : "Null"));
            SafeCloseSocket();
        }


        void SafeCloseSocket()
        {
            if (!IsConnected)
                return;
            try
            {
                _socket.Close();
            }
            catch (Exception ex)
            {
                logger.Error("Socket Close Error:" + ex.ToString());
            }
            _socket = null;
        }


        const int BUFFERSIZE = 65535;
        void RecvProcess()
        {
            byte[] buffer = new byte[BUFFERSIZE];
            int bufferOffset = 0;
            try
            {
                while (this.IsConnected)
                {
                    /* 此处buffer为接受数据缓存大小,数据包有可能只到达一部分
                     * 在没有比较gotmessage解析数据长度是否超过ret+bufferoffset长度时 可能出现对不完整的数据包解析 造成数据错误
                     * 
                     * gotmessages函数要求提供的pdata为实际数据长度,在函数内部通过 int orglen = data.Length; 来确定原来数据块的大小
                     * 
                     * 解决方案
                     * 1.socket获得数据后将数据复制到对应的pdata中(包含所有有效数据)
                     * 2.通过gotmessage解析后 如果还存有部分数据包内容 则将数据重新复制回buffer缓冲
                     * 
                     * */

                    int ret = _socket.Receive(buffer, bufferOffset, buffer.Length - bufferOffset, SocketFlags.None);
                    if (ret > 0)
                    {
                        //logger.Info(string.Format("buffer size:{0}", buffer.Length));
                        byte[] pdata = new byte[ret + bufferOffset];
                        Array.Copy(buffer, 0, pdata, 0, ret + bufferOffset);

                        //logger.Debug("socket recv bytes:" + ret + "raw data:" + HexToString(buffer, ret));
                        Message[] messagelist = Message.gotmessages(ref pdata, ref bufferOffset);//消息不完整则会将数据copy到头部并且设定bufferoffset用于下一次读取数据时进行自动拼接
                        if (bufferOffset != 0)
                        {
                            Array.Copy(pdata, 0, buffer, 0, bufferOffset);
                        }
                        int gotlen = 0;
                        int j = 0;
                        foreach (var msg in messagelist)
                        {
                            gotlen += msg.ByteLength;
                            j++;
                            if (msg.Type == MessageTypes.TICKNOTIFY)
                            {
                                int x = 0;
                            }
                            HandleMessage(msg);
                            //logger.Info(string.Format("Recv Message Type:{0} Content:{1}", msg.Type, msg.Content));
                        }
                        //logger.Debug(string.Format("buffer len:{0} buffer offset:{1} ret len:{2} parse len:{3} cnt:{4}", buffer.Length, bufferoffset, ret, gotlen, j));

                    }
                    else if (ret <= 0) // socket was shutdown
                    {
                        SafeCloseSocket();
                    }
                }
            }
            catch (Exception ex)
            {
                SafeCloseSocket();
                logger.Error(string.Format("RecvProcess Error:{0}", ex.ToString()));
            }
            logger.Info("Recv Thread Stopped");
        }

          

        bool IsSocketConnected(Socket client)
        {
            int err;
            return IsSocketConnected(client, out err);
        }

        bool IsSocketConnected(Socket client, out int errorcode)
        {
            errorcode = 0;
            if (client == null) return false;
            bool blockingState = client.Blocking;

            try
            {
                byte[] tmp = new byte[1];

                client.Blocking = false;
                client.Send(tmp, 0, 0);
            }
            catch (SocketException e)
            {
                // 10035 == WSAEWOULDBLOCK
                if (e.NativeErrorCode.Equals(10035))
                {
                    logger.Info("connected but send blocked.");
                    return true;
                }
                else
                {
                    errorcode = e.NativeErrorCode;
                    logger.Info("disconnected, error: " + errorcode);
                    return false;
                }
            }
            finally
            {
                client.Blocking = blockingState;
            }
            return client.Connected;
        }

        /// <summary>
        /// 查询远端服务状态
        /// </summary>
        /// <param name="apiType"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public override RspQryServiceResponse QryService(QSEnumAPIType apiType, string version)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(this.Server);
            QryServiceRequest request = RequestTemplate<QryServiceRequest>.CliSendRequest(0);
            request.APIType = apiType;
            request.APIVersion = version;

            byte[] nrequest = Message.sendmessage(request.Type, request.Content);
            s.Send(nrequest);

            byte[] tmp = new byte[s.ReceiveBufferSize];
            int len = s.Receive(tmp);
            byte[] data = new byte[len];
            Array.Copy(tmp, 0, data, 0, len);
            Message message = Message.gotmessage(data, 0);

            RspQryServiceResponse response = null;
            if (message.isValid && message.Type == MessageTypes.SERVICERESPONSE)
            {
                response = ResponseTemplate<RspQryServiceResponse>.CliRecvResponse(message);

            }
            return response;
        }
    }

}
