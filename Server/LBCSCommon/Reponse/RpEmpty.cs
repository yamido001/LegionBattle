
using System.Collections.Generic;

namespace LBCSCommon
{
    public class RpEmpty : ResponseBase
    {
        static RpEmpty mDeserialization = new RpEmpty();
        public static Dictionary<byte, object> Serialization()
        {
            return null;
        }

        public static RpEmpty Deserialization(Dictionary<byte, object> parameters)
        {
            return mDeserialization;
        }
    }
}
