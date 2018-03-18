using LBCSCommon;
using System.Collections.Generic;

namespace LBCSCommon
{
    public class RqCreateRoom : IRequest
    {
        public string RoomName {
            get;
            private set;
        }

        public RqCreateRoom(string roomName)
        {
            RoomName = roomName;
        }

        public Dictionary<byte, object> Serialization()
        {
            Dictionary<byte, object> retDic = new Dictionary<byte, object>();
            retDic[1] = RoomName;
            return retDic;
        }

        public static RqCreateRoom Deserialization(Dictionary<byte, object> parameters)
        {
            object roomNameObj;
            if (!parameters.TryGetValue(1, out roomNameObj))
            {
                return null;
            }
            string roomName = roomNameObj as string;
            if (string.IsNullOrEmpty(roomName))
            {
                return null;
            }
            if (string.IsNullOrEmpty(roomName))
            {
                return null;
            }
            if(roomName.Length > CommonDefine.RoomNameLenght)
            {
                return null;
            }
            RqCreateRoom createRoom = new RqCreateRoom(roomName);
            return createRoom;
        }
    }
}
