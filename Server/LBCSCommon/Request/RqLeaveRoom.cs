using LBCSCommon;
using System.Collections.Generic;

namespace LBCSCommon
{
    public class RqLeaveRoom : IRequest
    {

        public Dictionary<byte, object> Serialization()
        {
            Dictionary<byte, object> retDic = new Dictionary<byte, object>();
            return retDic;
        }

        public static RqLeaveRoom Deserialization(Dictionary<byte, object> parameters)
        {
            RqLeaveRoom leaveRoom = new RqLeaveRoom();
            return leaveRoom;
        }
    }
}
