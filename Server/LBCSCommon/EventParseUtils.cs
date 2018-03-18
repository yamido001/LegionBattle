using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LBCSCommon
{
    public class EventParseUtils
    {
        public static bool ParseValue<T>(Dictionary<byte, object> parametersDic, byte key, out T value)
        {
            value = default(T);
            object valueObj = null;
            if (!parametersDic.TryGetValue(1, out valueObj))
            {
                return false;
            }
            if (parametersDic.GetType() != typeof(T))
            {
                return false;
            }
            value = (T)valueObj;
            return true;
        }
    }
}
