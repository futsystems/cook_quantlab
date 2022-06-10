///////////////////////////////////////////////////////////////////////////////////////
// 查询合约
// 
//
///////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 注册合约实时行情数据
    /// </summary>
    public class RegisterSymbolTickRequest:RequestPacket
    {
        public RegisterSymbolTickRequest()
        {
            _type = API.MessageTypes.REGISTERSYMTICK;
            this.Exchange = string.Empty;
            this.SymbolList = new List<string>();
        }


        public string Exchange { get; set; }
        /// <summary>
        /// 合约
        /// </summary>
        public List<string> SymbolList { get; set; }



        public override string ContentSerialize()
        {
            string str = string.Empty;
            if (this.SymbolList != null && this.SymbolList.Count > 0)
            {
                str = string.Join(" ", this.SymbolList.ToArray());
            }
            return this.Exchange +","+str;
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            if (rec.Length == 2)
            {
                this.Exchange = rec[0];
                this.SymbolList.Clear();
                foreach (var symbol in rec[1].Split(' '))
                {
                    if (string.IsNullOrEmpty(symbol)) continue;
                    this.SymbolList.Add(symbol);
                }
            }
        }
    }

    /// <summary>
    /// 注销合约实时行情数据
    /// </summary>
    public class UnregisterSymbolTickRequest : RequestPacket
    {
        public UnregisterSymbolTickRequest()
        {
            _type = MessageTypes.UNREGISTERSYMTICK;
            this.Exchange = string.Empty;
            this.SymbolList = new List<string>();
        }

        public string Exchange { get; set; }
        /// <summary>
        /// 合约
        /// </summary>
        public List<string> SymbolList { get; set; }



        public override string ContentSerialize()
        {
            string str = string.Empty;
            if (this.SymbolList != null && this.SymbolList.Count > 0)
            {
                str = string.Join(" ", this.SymbolList.ToArray());
            }
            return this.Exchange + "," + str;
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            if (rec.Length == 2)
            {
                this.Exchange = rec[0];
                this.SymbolList.Clear();
                foreach (var symbol in rec[1].Split(' '))
                {
                    if (string.IsNullOrEmpty(symbol)) continue;
                    this.SymbolList.Add(symbol);
                }
            }
        }

    }


}
