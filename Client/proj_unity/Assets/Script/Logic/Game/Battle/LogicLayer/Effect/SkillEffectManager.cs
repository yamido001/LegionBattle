using LegionBattle.ServerClientCommon;
using System.Collections.Generic;

namespace GameBattle.LogicLayer.Effect
{
    public class SkillEffectManager : Singleton<SkillEffectManager>
    {
        Dictionary<int, SkillEffectBase> mEffectDic = new Dictionary<int, SkillEffectBase>();
 
        public void DestroyEffect(int effectId)
        {
            SkillEffectBase effectBase = null;
            if(mEffectDic.TryGetValue(effectId, out effectBase))
            {
                effectBase.Destroy();
            }
        }

        public int CreateTargetEffect(int targetUnitId, short effectId)
        {
            return CreateEffect(IntVector2.Zero, effectId, targetUnitId, 0, 0, 0);
        }

        public int CreateAreaEffect(IntVector2 effectPos, short effectId , short effectAngler, short param1, short param2)
        {
            return CreateEffect(effectPos, effectId, 0, effectAngler, param1, param2);
        }

        int CreateEffect(IntVector2 effectPos, short effectId, int targetUnitId, short effectAngler, short param1, short param2)
        {
            GDSKit.SkillEffect effectConfig = GDSKit.SkillEffect.GetInstance(effectId);

            SkillEffectBase createEffect = null;

            switch ((SkillEffectType)effectConfig.type)
            {
                case SkillEffectType.ChgAttr:
                    createEffect = new SkillAttrChgEffect();
                    break;
                default:
                    throw new System.NotImplementedException("未实现的技能效果类型 " + effectConfig.type);
            }
            mEffectDic[createEffect.iD] = createEffect;
            createEffect.SetInfo(effectConfig, effectPos, targetUnitId, effectAngler, param1, param2);
            createEffect.BeginWork();
            BattleFiled.Instance.OnCreateEffect(effectId, effectPos);
            return createEffect.iD;
        }
    }
}
