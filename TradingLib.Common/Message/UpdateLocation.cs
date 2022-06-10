using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Common
{
    public class UpdateLocationInfoRequest : RequestPacket
    {

        public LocationInfo LocationInfo {get;set;}
        public UpdateLocationInfoRequest()
        {
            _type = MessageTypes.UPDATELOCATION;
            this.LocationInfo = null;
        }

        public override string ContentSerialize()
        {
            if (this.LocationInfo == null) return string.Empty;
            return LocationInfo.Serialize(this.LocationInfo);
        }

        public override void ContentDeserialize(string contentstr)
        {
            if (string.IsNullOrEmpty(contentstr))
                this.LocationInfo = null;
            else
                this.LocationInfo = LocationInfo.Deserialize(contentstr);
        }
    }
}
