using System.Collections.Generic;

namespace LBCSCommon
{
    public interface IRequest
    {
        Dictionary<byte, object> Serialization();
    }
}
