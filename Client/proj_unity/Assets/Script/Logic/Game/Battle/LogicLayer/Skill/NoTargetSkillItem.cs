using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBattle.LogicLayer.Effect;

namespace GameBattle.LogicLayer.Skill
{
    public class NoTargetSkillItem : SkillItem
    {
        protected override bool CanUseSkill(int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            return true;
        }

        protected override int OnUse(int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            return SkillEffectManager.Instance.CreateTargetEffect(mUnit.Data.camp, mUnit.ID, mSkillConfig.effectId);
        }
    }
}

