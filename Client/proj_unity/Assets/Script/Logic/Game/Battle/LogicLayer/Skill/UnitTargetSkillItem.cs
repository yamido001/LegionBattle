using GameBattle.LogicLayer.Effect;

namespace GameBattle.LogicLayer.Skill
{
    public class UnitTargetSkillItem : SkillItem
    {
        public override void TryUseSkill(int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            if (!CheckSkillDistance(targetUnitId))
                return;
            OnUse(targetUnitId, skillAngle, skillParam1, skillParam2);
        }

        protected override void OnUse(int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            base.OnUse(targetUnitId, skillAngle, skillParam1, skillParam2);
            SkillEffectManager.Instance.CreateEffect(mUnit, targetUnitId, mSkillConfig.effectId);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}

