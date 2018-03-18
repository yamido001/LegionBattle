using System;
using System.Collections.Generic;

namespace LBCSCommon
{
    public class BattleMove : BattleInstructionBase
    {
        public short MoveAngle
        {
            get;
            private set;
        }

        public override BattleInstructionType InstructionType
        {
            get
            {
                return BattleInstructionType.Move;
            }
        }

        public BattleMove(short moveAngle)
        {
            this.MoveAngle = moveAngle;
        }

        protected override int SerializationByteLength()
        {
            return sizeof(short);
        }

        protected override void SerializationToByte(byte[] bytes, ref int index)
        {
            SerializeUtils.WriteShort(bytes, ref index, MoveAngle);
        }

        public static BattleMove Deserialize(byte[] data, ref int index)
        {
            short movaAngle = SerializeUtils.ReadShort(data, ref index);
            return new BattleMove(movaAngle);
        }
    }
}
