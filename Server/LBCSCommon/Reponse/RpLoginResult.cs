using System.Collections.Generic;

namespace LBCSCommon
{
    public class RpLoginResult : ResponseBase
    {
        public enum LoginErrorCode
        {
            Null = 0,
            ParseError,
            PasswordError,
            Count,
        }

        public bool Result
        {
            get;
            private set;
        }

        public LoginErrorCode ErrorCode
        {
            get;
            private set;
        }

        public RpLoginResult(bool result, LoginErrorCode errorCode)
        {
            Result = result;
            ErrorCode = errorCode;
        }

        public static RpLoginResult Deserialization(Dictionary<byte, object> parameters)
        {
            object resultObj = null;
            if(!parameters.TryGetValue(1, out resultObj))
            {
                return null;
            }
            if(resultObj.GetType() != typeof(bool))
            {
                return null;
            }

            object errorCodeObj = null;
            if (!parameters.TryGetValue(2, out errorCodeObj))
            {
                return null;
            }
            int errorCode = (int)errorCodeObj;
            if(errorCode < (int)LoginErrorCode.Null || (int)LoginErrorCode.Count < errorCode)
            {
                return null;
            }
            return new RpLoginResult((bool)resultObj, (LoginErrorCode)errorCode);
        }

        public static Dictionary<byte, object> Serialization(bool result, LoginErrorCode errorCode)
        {
            Dictionary<byte, object> retDic = new Dictionary<byte, object>();
            retDic[1] = result;
            retDic[2] = (int)errorCode;
            return retDic;
        }
    }
}
