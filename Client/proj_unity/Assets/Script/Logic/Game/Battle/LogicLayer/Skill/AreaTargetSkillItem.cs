using GameBattle.LogicLayer.Effect;
using LBMath;

namespace GameBattle.LogicLayer.Skill
{
    public class AreaTargetSkillItem : SkillItem
    {
        protected override bool CanUseSkill(int targetUnitId, short skillAngle, int skillParam1)
        {
            return true;
        }

        protected override int OnUse(int targetUnitId, short skillAngle, int skillParam1)
        {
            int effectId;
            SkillAreaType areaType = (SkillAreaType)mSkillConfig.targetInfo.param1;
            switch (areaType)
            {
                case SkillAreaType.Line:
                    effectId = SkillEffectManager.Instance.CreateAreaEffect(mUnit.Data.camp, mUnit.Position, mSkillConfig.effectId, skillAngle, mSkillConfig.targetInfo.param2, mSkillConfig.targetInfo.param3);
                    break;
                case SkillAreaType.Circle:
                    effectId = SkillEffectManager.Instance.CreateAreaEffect(mUnit.Data.camp, IntVector2.MoveAngle(mUnit.Position, skillAngle, (short)(new IntegerFloat(skillParam1) * mSkillConfig.distance).ToInt()), mSkillConfig.effectId, skillAngle, mSkillConfig.targetInfo.param2, 0);
                    break;
                default:
                    throw new System.NotImplementedException("未实现的技能区域类型 " + areaType);
            }
            return effectId;
        }
    }
}

