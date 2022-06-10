using System;
using System.Collections.Generic;
using TradingLib.API;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;



namespace TradingLib.Common
{
    /// <summary>
    /// 消息
    /// 固定头格式的消息
    /// |--4 Length--|--4 Type--|--content--|
    /// 头部为8字节长度
    /// 4字节为消息体长度 消息长度包含头部长度
    /// 4字节为消息类型
    /// 所有消息序列化成字符串进行传输 涉及到前置C++消息转换部分
    /// 如果兼容二进制原生协议?
    /// </summary>
    public struct Message
    {
        public static Message NewMessage()
        {
            Message m = new Message();
            m.Tag = string.Empty;
            m.Type = MessageTypes.UNKNOWN_MESSAGE;
            m.Content = string.Empty;
            m.ByteLength = 0;
            m.Data = null;
            return m;
        }
        public Message(MessageTypes type, string content,byte[] data, int len)
        {
            Type = type;
            Content = content;
            Data = data;
            ByteLength = len;
            CodecType = EnumMessageCodeType.BINARY;
            Tag = string.Empty;
        }

        //public Message(MessageTypes type, string body)
        //{
        //    Content = body;
        //    Type = type;
        //    Tag = string.Empty;
        //    ByteLength = 0;
        //    Data = null;
        //}
        //public Message(MessageTypes type, byte[] data)
        //{
        //    Type = type;
        //    Data = data;
        //    Tag = string.Empty;
        //    ByteLength = 0;
        //    Content = string.Empty;
        //}
        //public string ToJsonString()
        //{
        //    return "{" + Type.ToString() + ":" + Content + "}";
        //}

        public string Content;
        public byte[] Data;
        public MessageTypes Type;
        public int ByteLength;
        public EnumMessageCodeType CodecType;
        public bool isValid { get { return Type != MessageTypes.UNKNOWN_MESSAGE; } }
        public const int SIZE = 1024;


        public static bool sendmessage(Message m, ref byte[] data)
        {
            return sendmessage(m.Type, m.Content, ref data);
        }
        public static byte[] sendmessage(Message m)
        {
            return sendmessage(m.Type, m.Content);
        }
        public string Tag;
        public static byte[] sendmessage(MessageTypes type, string msg)
        {
            byte[] data = new byte[SIZE];
            if (sendmessage(type, msg, ref data))
                return data;
            return new byte[0];
        }
        // 4 bytes for length, 4 bytes for type
        const int HEADERSIZE = 8;
        const int LENGTHOFFSET = 0;
        const int TYPEOFFSET = 4;



        public static bool sendmessage(MessageTypes type, string msg, ref byte[] data)
        {
            try
            {
                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                //System.Text.UnicodeEncoding encoding = new System.Text.UnicodeEncoding();
                byte[] message = encoding.GetBytes(msg);
                int size = HEADERSIZE+message.Length;
                data = new byte[size];
                byte[] sizebyte = BitConverter.GetBytes(size);
                byte[] typebyte = BitConverter.GetBytes((int)type);
                Array.Copy(sizebyte, 0, data, LENGTHOFFSET, sizebyte.Length);
                Array.Copy(typebyte, 0, data, TYPEOFFSET, typebyte.Length);
                Array.Copy(message, 0, data, HEADERSIZE, message.Length);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;

            
        }

        /// <summary>
        /// 这里的data为接受到的数据大小 完整数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Message[] gotmessages(ref byte[] data, ref int offset)
        {

            // save original length
            int orglen = data.Length;
            // prepare to hold a sequence of messages from a buffer
            List<Message> msgs = new List<Message>();
            // keep track of length of all read messages
            int totallen = 0;
            bool msgok = true;
            int start = 0;
            // prepare vars to hold per-message attributes
            MessageTypes mt = MessageTypes.UNKNOWN_MESSAGE;
            string mc = string.Empty;
            byte[] md = null;
            int ml = 0;
            bool done = false;
            // fetch all messages we can get
            while (msgok)
            {
                // fetch a message and record success
                msgok = gotmessage(start, data, ref mt, ref mc,ref md, ref ml, ref done);
                // if we got a message
                if (msgok) 
                {
                    // save it
                    msgs.Add(new Message(mt,mc,md,ml));
                    // save total length of messages we've read
                    totallen += ml;
                }
                // update next start
                start += ml;
            }
            // set flag if partial message is left
            bool partial = totallen != data.Length;
            // set index of any partial message
            if (!done && partial)
            {
                // copy
                int partialidx = totallen;
                // save partial length as offset
                offset = data.Length - partialidx;
                // move any partial data to front of buffer
                byte[] pdata = new byte[orglen];
                Array.Copy(data, partialidx, pdata, 0,offset );
                data = pdata;
            }
            else
            {
                //reset buffer and offset
                data = new byte[data.Length];
                offset = 0;
            }
            // return messages we found
            return msgs.ToArray();
        }

        public static Message gotmessage(byte[] data, int start)
        {
            // prepare to get message parameters
            MessageTypes type = MessageTypes.UNKNOWN_MESSAGE;
            string body = string.Empty;
            byte[] msgdata = null;
            int len = 0;
            bool done = false;
            // fetch message and return it if successful
            if (gotmessage(start, data, ref type, ref body, ref msgdata, ref len, ref done))
                return new Message(type, body, msgdata, len);
            // return invalid message otherwise
            return NewMessage();
        }
        public static Message gotmessage(byte[] data)
        {
            return gotmessage(data, 0);
        }
        //public static bool gotmessage(byte[] data, ref MessageTypes type, ref string msg,ref byte[] msgdata, ref int msglen, ref bool done)
        //{
        //    return gotmessage(0, data, ref type, ref msg,ref msgdata,ref msglen, ref done);
        //}


        public static bool gotmessage(int startidx, byte[] data, ref MessageTypes type, ref string msg, ref byte[] msgdata,ref int msglen, ref bool done)
        {
            try
            {
                // ensure we have enough room to store header
                if (startidx+HEADERSIZE>=data.Length)
                    return false;
                // get type from message
                type = (MessageTypes)BitConverter.ToInt32(data, startidx+TYPEOFFSET);
                // get length of message
                msglen = BitConverter.ToInt32(data, startidx+LENGTHOFFSET);
                // ensure it's valid message
                if (msglen < HEADERSIZE)
                {
                    done = (msglen == 0) && (type == MessageTypes.OK);
                    return false;
                }
                // ensure we have enough data for message body
                if (startidx + msglen > data.Length)
                    return false;

                switch (type)
                {
                    case MessageTypes.BIN_BARRESPONSE:
                    case MessageTypes.XQRYTRADSPLITRESPONSE:
                    case MessageTypes.XQRYPRICEVOLRESPONSE:
                    case MessageTypes.XQRYMINUTEDATARESPONSE:
                    case MessageTypes.MGR_MD_UPLOADBARDATA:
                        {
                            msg = string.Empty;
                            msgdata = new byte[msglen - HEADERSIZE];
                            Array.Copy(data, startidx + HEADERSIZE, msgdata, 0, msglen - HEADERSIZE);
                            return true;
                        }
                    default:
                        {
                            msgdata = null;
                            // decode message to string
                            msg = System.Text.Encoding.UTF8.GetString(data, startidx + HEADERSIZE, msglen - HEADERSIZE);
                            return true;
                        }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
