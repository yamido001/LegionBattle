
using System;
using System.Collections.Generic;

namespace LBCSCommon
{
    public class RqBattleInstruction : IRequest
    {
        public BattleInstructionBase instruction
        {
            get;
            set;
        }

        public Dictionary<byte, object> Serialization()
        {
            Dictionary<byte, object> retDic = new Dictionary<byte, object>();
            retDic[1] = instruction.Serialization();
            return retDic;
        }

        public static byte[] Deserialization(Dictionary<byte, object> parameters)
        {
            return (byte[])parameters[1];
        }
    }
}
