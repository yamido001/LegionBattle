using System;
using System.Collections.Generic;

namespace LBCSCommon
{
    public class RpPlayerLogin : ResponseBase
    {
        public int PlayerId
        {
            get;
            private set;
        }

        public string PlayerName
        {
            get;
            private set;
        }

        public RpPlayerLogin(int playerId, string playerName)
        {
            PlayerId = playerId;
            PlayerName = playerName;
        }

        public static RpPlayerLogin Deserialization(Dictionary<byte, object> parameters)
        {
            object playerIdObj = null;
            if (!parameters.TryGetValue(1, out playerIdObj))
            {
                return null;
            }
            if (playerIdObj.GetType() != typeof(int))
            {
                return null;
            }

            object playerNameObj = null;
            if (!parameters.TryGetValue(2, out playerNameObj))
            {
                return null;
            }
            if(playerNameObj.GetType() != typeof(string))
            {
                return null;
            }
            return new RpPlayerLogin((int)playerIdObj, (string)playerNameObj);
        }

        public static Dictionary<byte, object> Serialization(int playerId, string playerName)
        {
            Dictionary<byte, object> retDic = new Dictionary<byte, object>();
            retDic[1] = playerId;
            retDic[2] = playerName;
            return retDic;
        }
    }
}
