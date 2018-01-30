using GameBattle.LogicLayer.Effect;

namespace GameBattle.LogicLayer.Skill
{
    public class UnitTargetSkillItem : SkillItem
    {
        protected override bool CanUseSkill(int targetUnitId, short skillAngle, int skillParam1)
        {
            return CheckSkillDistance(targetUnitId);
        }

        protected override int OnUse(int targetUnitId, short skillAngle, int skillParam1)
        {
            return SkillEffectManager.Instance.CreateTargetEffect(mUnit.Data.camp, targetUnitId, mSkillConfig.effectId);
        }
    }
}

