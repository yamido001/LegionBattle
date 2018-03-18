using System;

namespace LBCSCommon
{
    public class BattleAreaTargetSkill : BattleInstructionBase
    {
        public short SkillId
        {
            get;
            private set;
        }

        public short SkillAngle
        {
            get;
            private set;
        }

        public Int32 SkillParam1
        {
            get;
            private set;
        }

        public override BattleInstructionType InstructionType
        {
            get
            {
                return BattleInstructionType.AreaTargetSkill;
            }
        }

        public BattleAreaTargetSkill(short skillId, short skillAngle, Int32 param1)
        {
            this.SkillAngle = skillAngle;
            this.SkillId = skillId;
            SkillParam1 = param1;
        }

        protected override int SerializationByteLength()
        {
            return sizeof(short) * 2 + sizeof(int);
        }

        protected override void SerializationToByte(byte[] bytes, ref int index)
        {
            SerializeUtils.WriteShort(bytes, ref index, SkillId);
            SerializeUtils.WriteShort(bytes, ref index, SkillAngle);
            SerializeUtils.WriteInt(bytes, ref index, SkillParam1);
        }

        public static BattleAreaTargetSkill Deserialize(byte[] data, ref int index)
        {
            short skillId = SerializeUtils.ReadShort(data, ref index);
            short skillAngle = SerializeUtils.ReadShort(data, ref index);
            short skillParam1 = SerializeUtils.ReadShort(data, ref index);
            return new BattleAreaTargetSkill(skillId, skillAngle, skillParam1);
        }
    }
}
