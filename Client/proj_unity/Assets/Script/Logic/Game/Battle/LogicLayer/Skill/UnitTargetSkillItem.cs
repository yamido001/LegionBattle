using GameBattle.LogicLayer.Effect;

namespace GameBattle.LogicLayer.Skill
{
    public class UnitTargetSkillItem : SkillItem
    {
        protected override bool CanUseSkill(int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            return CheckSkillDistance(targetUnitId);
        }

        protected override int OnUse(int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            return SkillEffectManager.Instance.CreateTargetEffect(targetUnitId, mSkillConfig.effectId);
        }
    }
}

