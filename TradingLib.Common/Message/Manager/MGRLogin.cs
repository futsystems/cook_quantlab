using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    /// <summary>
    /// 管理端登入请求
    /// </summary>
    public class MGRLoginRequest:RequestPacket
    {
        public string LoginID { get; set; }
        public string Passwd { get; set; }

        public MGRLoginRequest()
        {
            _type = MessageTypes.MGR_REQ_LOGIN;
        }


        public override string ContentSerialize()
        {
            StringBuilder sb = new StringBuilder();
            char d = ',';
            sb.Append(this.LoginID);
            sb.Append(d);
            sb.Append(this.Passwd);
            return sb.ToString();
        }

        public override void ContentDeserialize(string contentstr)
        {
            string[] rec = contentstr.Split(',');
            LoginID = rec[0];
            Passwd = rec[1];
        }
    }

    public class MgrLoginResponse
    {
        public MgrLoginResponse()
        {
            LoginID = string.Empty;
            Name = string.Empty;
            ManagerType = QSEnumManagerType.MONITER;
            Mobile = string.Empty;
            QQ = string.Empty;
            MGRID = 0;
            BaseMGRFK = 0;
            UIAccess = new UIAccess();
            Domain = new DomainImpl();
        }
        /// <summary>
        /// 登入ID
        /// </summary>
        public string LoginID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 管理员类别
        /// </summary>
        public QSEnumManagerType ManagerType { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// QQ号码
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 所属主域ID
        /// </summary>
        public int BaseMGRFK { get; set; }

        /// <summary>
        /// 管理ID
        /// </summary>
        public int MGRID { get; set; }

        /// <summary>
        /// 界面授权权限
        /// </summary>
        public UIAccess UIAccess { get; set; }

        /// <summary>
        /// 域对象
        /// </summary>
        public DomainImpl Domain { get; set; }
    }

    public class RspMGRLoginResponse : RspResponsePacket
    {
        public RspMGRLoginResponse()
        {
            _type = MessageTypes.MGR_RSP_LOGIN;
            LoginResponse = new MgrLoginResponse();
        }

        public MgrLoginResponse LoginResponse { get; set; }
        public override string ResponseSerialize()
        {
            return this.LoginResponse.SerializeObject();
        }

        public override void ResponseDeserialize(string content)
        {
            this.LoginResponse = content.DeserializeObject<MgrLoginResponse>();
        }
    }
}
