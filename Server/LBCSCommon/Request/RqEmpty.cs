
using System;
using System.Collections.Generic;

namespace LBCSCommon
{
    public class RqEmpty : IRequest
    {
        public static RqEmpty StaticRqEmpty = new RqEmpty();
        public Dictionary<byte, object> Serialization()
        {
            return null;
        }
    }
}
