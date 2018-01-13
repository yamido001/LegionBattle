using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle.LogicLayer.Skill
{
    public class AreaTargetSkillItem : SkillItem
    {
        public override void TryUseSkill(int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            OnUse(targetUnitId, skillAngle, skillParam1, skillParam2);
        }

        protected override void OnUse(int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            base.OnUse(targetUnitId, skillAngle, skillParam1, skillParam2);
            SkillAreaType areaType = (SkillAreaType)mSkillConfig.targetInfo.param1;
            switch (areaType)
            {
                case SkillAreaType.Line:
                    break;
                case SkillAreaType.Circle:
                    break;
                default:
                    throw new System.NotImplementedException("未实现的技能区域类型 " + areaType);
            }
        }
        
        protected override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}

