///////////////////////////////////////////////////////////////////////////////////////
// 用于实现客户端与服务端的心跳机制,客户端与服务端实现双向心跳
// 1.客户端定时向服务端发送心跳信息,用于告知服务端,客户端仍然存活，如果在一定时间客户端没有向
// 服务端发送有效心跳数据则服务端会将该客户端Session数据删除
// 2.客户端会监控服务端发送回来的数据，如果在一定时间内服务端没有向客户端发送有效数据，则客户端
// 会向服务进行心跳请求，服务端正常情况下收到请求后会通知客户端，如果客户端在一定的时间间隔内仍然
// 没有收到心跳，则客户端认为该服务端已经宕机，需要重新建立到其他服务节点以恢复服务
// 
//
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.Common
{
    /// <summary>
    /// 客户端向服务端发送的心跳
    /// </summary>
    public class HeartBeat:RequestPacket
    {
        public HeartBeat()
        {
            _type = API.MessageTypes.HEARTBEAT;
        }

        public override string ContentSerialize()
        {
            return "HeartBeat";
        }
    }

    /// <summary>
    /// 客户端请求服务端返回心跳回报
    /// </summary>
    public class HeartBeatRequest : RequestPacket
    {
        public HeartBeatRequest()
        {
            _type = API.MessageTypes.HEARTBEATREQUEST;
        }

        public override string ContentSerialize()
        {
            return "HeartBeatRequest";
        }
    }

    /// <summary>
    /// 客户端心跳请求的服务端对应的回报
    /// </summary>
    public class HeartBeatResponse : RspResponsePacket
    {
        public HeartBeatResponse()
        {
            _type = API.MessageTypes.HEARTBEATRESPONSE;
        }

        public override string ResponseSerialize()
        {
            return "HeartBeatResponse";
        }
    }

    public class TickHeartBeatResponse : NotifyResponsePacket
    {
        public TickHeartBeatResponse()
        {
            _type = API.MessageTypes.TICKHEARTBEAT;
        }

        public override string  ContentSerialize()
        {
 	         return "TICKHEARTBEAT";
        }
    }
}
