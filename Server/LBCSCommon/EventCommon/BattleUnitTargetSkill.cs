using System;

namespace LBCSCommon
{
    public class BattleUnitTargetSkill : BattleInstructionBase
    {
        public short SkillId
        {
            get;
            private set;
        }

        public int TargetUnitId
        {
            get;
            private set;
        }

        public override BattleInstructionType InstructionType
        {
            get
            {
                return BattleInstructionType.UnitTargetSkill;
            }
        }

        public BattleUnitTargetSkill(int targetUnitId, short skillId)
        {
            this.TargetUnitId = targetUnitId;
            this.SkillId = skillId;
        }

        protected override int SerializationByteLength()
        {
            return sizeof(short) + sizeof(int);
        }

        protected override void SerializationToByte(byte[] bytes, ref int index)
        {
            SerializeUtils.WriteShort(bytes, ref index, SkillId);
            SerializeUtils.WriteInt(bytes, ref index, TargetUnitId);
        }

        public static BattleUnitTargetSkill Deserialize(byte[] data, ref int index)
        {
            short skillId = SerializeUtils.ReadShort(data, ref index);
            int targetUnitId = SerializeUtils.ReadInt(data, ref index);
            return new BattleUnitTargetSkill(targetUnitId, skillId);
        }
    }
}
