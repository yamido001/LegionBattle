
using System.Collections.Generic;

namespace LBCSCommon
{
    public class RpCreateAccountResult : ResponseBase
    {
        public enum CreateAccountErrorCode
        {
            Null = 0,
            ParseError,
            AccountExist,
            Count,
        }

        public bool Result
        {
            get;
            private set;
        }

        public CreateAccountErrorCode ErrorCode
        {
            get;
            private set;
        }

        public RpCreateAccountResult(bool result, CreateAccountErrorCode errorCode)
        {
            Result = result;
            ErrorCode = errorCode;
        }

        public static RpCreateAccountResult Deserialization(Dictionary<byte, object> parameters)
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

            object errorCodeObj = null;
            if (!parameters.TryGetValue(2, out errorCodeObj))
            {
                return null;
            }
            int errorCode = (int)errorCodeObj;
            if (errorCode < (int)CreateAccountErrorCode.Null || (int)CreateAccountErrorCode.Count < errorCode)
            {
                return null;
            }
            return new RpCreateAccountResult((bool)resultObj, (CreateAccountErrorCode)errorCode);
        }

        public static Dictionary<byte, object> Serialization(bool result, CreateAccountErrorCode errorCode)
        {
            Dictionary<byte, object> retDic = new Dictionary<byte, object>();
            retDic[1] = result;
            retDic[2] = (int)errorCode;
            return retDic;
        }
    }
}
