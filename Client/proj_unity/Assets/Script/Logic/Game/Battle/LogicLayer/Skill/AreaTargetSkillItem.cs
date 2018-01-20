using GameBattle.LogicLayer.Effect;

namespace GameBattle.LogicLayer.Skill
{
    public class AreaTargetSkillItem : SkillItem
    {
        protected override bool CanUseSkill(int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            return true;
        }

        protected override int OnUse(int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            int effectId;
            SkillAreaType areaType = (SkillAreaType)mSkillConfig.targetInfo.param1;
            switch (areaType)
            {
                case SkillAreaType.Line:
                    effectId = SkillEffectManager.Instance.CreateAreaEffect(mUnit.Position, mSkillConfig.effectId, skillAngle, mSkillConfig.targetInfo.param2, mSkillConfig.targetInfo.param3);
                    break;
                case SkillAreaType.Circle:
                    effectId = SkillEffectManager.Instance.CreateAreaEffect(mUnit.Position, mSkillConfig.effectId, skillAngle, (short)(skillParam1 * mSkillConfig.distance / skillParam2), 0);
                    break;
                default:
                    throw new System.NotImplementedException("未实现的技能区域类型 " + areaType);
            }
            return effectId;
        }
    }
}

