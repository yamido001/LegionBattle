using System;
using System.Collections.Generic;

namespace LBCSCommon
{
    public class BattleStopMove : BattleInstructionBase
    {
        public override BattleInstructionType InstructionType
        {
            get
            {
                return BattleInstructionType.StopMove;
            }
        }

        protected override int SerializationByteLength()
        {
            return 0;
        }

        protected override void SerializationToByte(byte[] bytes, ref int index)
        {
            
        }

        public static BattleStopMove Deserialize(byte[] data, ref int index)
        {
            return new BattleStopMove();
        }
    }
}
