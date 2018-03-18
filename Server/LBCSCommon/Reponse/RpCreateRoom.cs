using System.Collections.Generic;

namespace LBCSCommon
{
    public class RpCreateRoom : ResponseBase
    {
        public bool Result
        {
            get;
            private set;
        }

        public int RoomId
        {
            get;
            private set;
        }

        public string RoomName
        {
            get;
            private set;
        }
         
        public RpCreateRoom(bool result, int roomId, string roomName)
        {
            Result = result;
            RoomId = roomId;
            RoomName = roomName;
        }

        public static RpCreateRoom Deserialization(Dictionary<byte, object> parameters)
        {
            object resultObj = null;
            if (!parameters.TryGetValue(1, out resultObj))
            {
                return null;
            }
            if (resultObj.GetType() != typeof(bool))
            {
                return null;
            }

            object roomIdObj = null;
            if (!parameters.TryGetValue(2, out roomIdObj))
            {
                return null;
            }
            if (roomIdObj.GetType() != typeof(int))
            {
                return null;
            }

            object roomNameObj = null;
            if (!parameters.TryGetValue(3, out roomNameObj))
            {
                return null;
            }
            if (roomNameObj.GetType() != typeof(string))
            {
                return null;
            }

            return new RpCreateRoom((bool)resultObj, (int)roomIdObj, roomNameObj as string);
        }

        public static Dictionary<byte, object> Serialization(bool result, int roomId, string roomName)
        {
            Dictionary<byte, object> retDic = new Dictionary<byte, object>();
            retDic[1] = result;
            retDic[2] = roomId;
            retDic[3] = roomName;
            return retDic;
        }
    }
}
