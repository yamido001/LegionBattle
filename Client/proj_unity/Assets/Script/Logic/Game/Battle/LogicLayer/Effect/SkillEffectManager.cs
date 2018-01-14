
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
            GDSKit.SkillEffect effectConfig = GDSKit.SkillEffect.GetInstance(effectId);
            switch((SkillEffectType)effectConfig.type)
            {
                case SkillEffectType.Damage:
                    BattleFiled.Instance.OnUnitDamaged(targetUnitId, effectConfig.param1);
                    break;
                default:
                    throw new System.NotImplementedException("没有实现的效果类型 " + effectConfig.type);
            }           
        }
    }
}
