using System;
using System.Collections.Generic;

namespace LBCSCommon
{
    public class BattleNoTargetSkill : BattleInstructionBase
    {
        public short SkillId
        {
            get;
            private set;
        }

        public override BattleInstructionType InstructionType
        {
            get
            {
                return BattleInstructionType.NoTargetSkill;
            }
        }

        public BattleNoTargetSkill(short skillId)
        {
            SkillId = skillId;
        }

        protected override int SerializationByteLength()
        {
            return sizeof(short);
        }

        protected override void SerializationToByte(byte[] bytes, ref int index)
        {
            SerializeUtils.WriteShort(bytes, ref index, SkillId);
        }

        public static BattleNoTargetSkill Deserialize(byte[] data, ref int index)
        {
            short skillId = SerializeUtils.ReadShort(data, ref index);
            return new BattleNoTargetSkill(skillId);
        }
    }
}
