
namespace GameBattle.LogicLayer.Effect
{
    public class SkillEffectManager : Singleton<SkillEffectManager>
    {
        public void CreateEffect(UnitBase caster, int targetUnitId, short effectId)
        {
            UnitBase targetUnit = BattleUnitManager.Instance.GetUnitByUnitId(targetUnitId);
            if(null == targetUnit)
            {
                Logger.LogError("创建技能效果，没有找到目标单位:" + targetUnitId + "  技能效果ID: " + effectId);
                return;
            }
            BattleFiled.Instance.OnUnitDamaged(targetUnitId, effectId * 1000);
        }
    }
}
