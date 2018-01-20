using System.Collections.Generic;

namespace GameBattle.LogicLayer.Effect
{
    public class SkillAttrChgEffect : SkillEffectBase
    {
        public override SkillEffectType effectType
        {
            get
            {
                return SkillEffectType.ChgAttr;
            }
        }

        protected override void DoExecute()
        {
            List<UnitBase> targetUnitList = GetAreaUnitList();
            for (int i = 0; i < targetUnitList.Count; ++i)
            {
                BattleFiled.Instance.ChangeUnitAttr(targetUnitList[i], (FighterAttributeType)mEffectConfig.param1, mEffectConfig.param2);
            }
        }
    }
}


