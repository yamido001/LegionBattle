using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBattle.LogicLayer.Effect;

namespace GameBattle.LogicLayer.Skill
{
    public class NoTargetSkillItem : SkillItem
    {
        public override void TryUseSkill(int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            //没有目标的技能，不需要检查距离，这种技能的效果只能作用给自身
            OnUse(targetUnitId, skillAngle, skillParam1, skillParam2);
        }

        protected override void OnUse(int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            base.OnUse(targetUnitId, skillAngle, skillParam1, skillParam2);
            SkillEffectManager.Instance.CreateEffect(mUnit, mUnit.ID, mSkillConfig.effectId);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}

