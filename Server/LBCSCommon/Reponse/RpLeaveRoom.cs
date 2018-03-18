using System.Collections.Generic;

namespace LBCSCommon
{
    public class RpLeaveRoom : ResponseBase
    {
        public bool Result
        {
            get;
            private set;
        }
         
        public RpLeaveRoom(bool result)
        {
            Result = result;
        }

        public static RpLeaveRoom Deserialization(Dictionary<byte, object> parameters)
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
            return new RpLeaveRoom((bool)resultObj);
        }

        public static Dictionary<byte, object> Serialization(bool result)
        {
            Dictionary<byte, object> retDic = new Dictionary<byte, object>();
            retDic[1] = result;
            return retDic;
        }
    }
}
