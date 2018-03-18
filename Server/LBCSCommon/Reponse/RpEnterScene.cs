using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LBCSCommon
{
    public class RpEnterScene : ResponseBase
    {
        public int[] SceneUnit
        {
            get;
            private set;
        }

        public RpEnterScene(int[] sceneUnits)
        {
            SceneUnit = sceneUnits;
        }

        public static RpEnterScene Deserialization(Dictionary<byte, object> parameters)
        {
            object byteArrayObj = null;
            if (!parameters.TryGetValue(1, out byteArrayObj))
            {
                return null;
            }
            if (byteArrayObj.GetType() != typeof(byte[]))
            {
                return null;
            }
            byte[] byteArray = byteArrayObj as byte[];
            if (byteArray.Length < 2)
                return null;
            int index = 0;
            short count = SerializeUtils.ReadShort(byteArray, ref index);
            if (count * 4 + index != byteArray.Length)
                return null;
            int[] sceneUnits = new int[count];
            for (int i = 0; i < count; ++i)
            {
                sceneUnits[i] = SerializeUtils.ReadInt(byteArray, ref index);
            }
            return new RpEnterScene(sceneUnits);
        }

        public static Dictionary<byte, object> Serialization(int[] sceneUnits)
        {
            byte[] byteArray = new byte[sceneUnits.Length * 4 + 2];
            int index = 0;
            SerializeUtils.WriteShort(byteArray, ref index, (short)sceneUnits.Length);
            for(int i = 0; i < sceneUnits.Length; ++i)
            {
                SerializeUtils.WriteInt(byteArray, ref index, sceneUnits[i]);
            }
            Dictionary<byte, object> retDic = new Dictionary<byte, object>();
            retDic[1] = byteArray;
            return retDic;
        }
    }
}
