using LBCSCommon;
using System.Collections.Generic;

namespace LBCSCommon
{
    public class RqEnterRoom : IRequest
    {
        public int RoomId
        {
            get;
            private set;
        }

        public RqEnterRoom(int roomId)
        {
            RoomId = roomId;
        }

        public Dictionary<byte, object> Serialization()
        {
            Dictionary<byte, object> retDic = new Dictionary<byte, object>();
            retDic[1] = RoomId;
            return retDic;
        }

        public static RqEnterRoom Deserialization(Dictionary<byte, object> parameters)
        {
            object roomIdObj;
            if (!parameters.TryGetValue(1, out roomIdObj))
            {
                return null;
            }
            if (roomIdObj.GetType() != typeof(int))
                return null;
            RqEnterRoom createRoom = new RqEnterRoom((int)roomIdObj);
            return createRoom;
        }
    }
}
