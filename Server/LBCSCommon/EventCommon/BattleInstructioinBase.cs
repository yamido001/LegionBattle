using System.Collections.Generic;

namespace LBCSCommon
{
    public abstract class BattleInstructionBase
    {
        public int SceneUnitId
        {
            get;
            set;
        }
        public abstract BattleInstructionType InstructionType
        {
            get;
        }

        private const short SelfSerializationByteLength = 5;

        public int GetSerializationByteLength()
        {
            return SelfSerializationByteLength + SerializationByteLength();
        }

        public void Serialization(byte[] byteArray, ref int index)
        {
            SerializeUtils.WriteInt(byteArray, ref index, SceneUnitId);
            SerializeUtils.WriteByte(byteArray, ref index, (byte)InstructionType);
            SerializationToByte(byteArray, ref index);
        }

        public byte[] Serialization()
        {
            byte[] byteArray = new byte[GetSerializationByteLength()];
            int index = 0;
            Serialization(byteArray, ref index);
            return byteArray;
        }

        public static BattleInstructionBase Deserializetion(byte[] byteArray, ref int index)
        {
            if(byteArray.Length < SelfSerializationByteLength)
            {
                return null;
            }
            int sceneUnitId = SerializeUtils.ReadInt(byteArray, ref index);
            BattleInstructionType instructionType = (BattleInstructionType)SerializeUtils.ReadByte(byteArray, ref index);
            BattleInstructionBase ret = null;
            switch (instructionType)
            {
                case BattleInstructionType.Move:
                    ret = BattleMove.Deserialize(byteArray, ref index);
                    break;
                case BattleInstructionType.StopMove:
                    ret = BattleStopMove.Deserialize(byteArray, ref index);
                    break;
                case BattleInstructionType.NoTargetSkill:
                    ret = BattleNoTargetSkill.Deserialize(byteArray, ref index);
                    break;
                case BattleInstructionType.UnitTargetSkill:
                    ret = BattleUnitTargetSkill.Deserialize(byteArray, ref index);
                    break;
                case BattleInstructionType.AreaTargetSkill:
                    ret = BattleAreaTargetSkill.Deserialize(byteArray, ref index);
                    break;
                default:
                    
                    break;
            }
            if(null != ret)
            {
                ret.SceneUnitId = sceneUnitId;
            }
            return ret;
        }

        protected abstract void SerializationToByte(byte[] bytes, ref int index);

        protected abstract int SerializationByteLength();
    }
}
