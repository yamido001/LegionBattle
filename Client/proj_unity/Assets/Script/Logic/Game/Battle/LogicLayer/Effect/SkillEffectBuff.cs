using System.Collections.Generic;

namespace GameBattle.LogicLayer.Effect
{
    public class SkillEffectBuff : SkillEffectBase
    {
        public override SkillEffectType effectType
        {
            get
            {
                return SkillEffectType.Buff;
            }
        }

        protected override void DoExecute()
        {
            List<UnitBase> targetUnitList = GetAreaUnitList();
            for (int i = 0; i < targetUnitList.Count; ++i)
            {
                BattleFiled.Instance.AddBuffer(targetUnitList[i], (short)mEffectConfig.param1);
            }
        }
    }
}


