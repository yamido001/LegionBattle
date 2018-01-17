using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;
using GameBattle.LogicLayer.Effect;

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
            List<UnitBase> areaUnitList = null;
            switch (areaType)
            {
                case SkillAreaType.Line:
                    areaUnitList = BattleFiledLattile.Instance.GetUnitListInLine(mUnit.Position, skillAngle, mSkillConfig.targetInfo.param2, mSkillConfig.targetInfo.param3);
                    break;
                case SkillAreaType.Circle:
                    IntVector2 cricleCenterPos = IntVector2.MoveAngle(mUnit.Position, skillAngle, skillParam1 * mSkillConfig.distance / skillParam2);
                    areaUnitList = BattleFiledLattile.Instance.GetUnitListInCircle(cricleCenterPos, mSkillConfig.targetInfo.param2);
                    break;
                default:
                    throw new System.NotImplementedException("未实现的技能区域类型 " + areaType);
            }
            for(int i = 0; i < areaUnitList.Count; ++i)
            {
                UnitBase targetUnit = areaUnitList[i];
                if (targetUnit.Data.camp == mUnit.Data.camp)
                    continue;
                SkillEffectManager.Instance.CreateEffect(mUnit, targetUnit.ID, mSkillConfig.effectId);
            }
        }
        
        protected override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}

