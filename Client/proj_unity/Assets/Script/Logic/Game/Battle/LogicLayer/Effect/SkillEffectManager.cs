using LegionBattle.ServerClientCommon;
using System.Collections.Generic;

namespace GameBattle.LogicLayer.Effect
{
    public class SkillEffectManager : Singleton<SkillEffectManager>
    {
        Dictionary<int, SkillEffectBase> mEffectDic = new Dictionary<int, SkillEffectBase>();
        List<int> mRemoveList = new List<int>();

        public void DestroyEffect(int effectId)
        {
            SkillEffectBase effectBase = null;
            if(mEffectDic.TryGetValue(effectId, out effectBase))
            {
                effectBase.Destroy();
                mEffectDic.Remove(effectId);
            }
        }

        public void Update()
        {
            foreach (var item in mEffectDic)
            {
                item.Value.Update();
                if(item.Value.isFinish)
                {
                    item.Value.Destroy();
                    mRemoveList.Add(item.Key);
                }
            }
            foreach (var removeId in mRemoveList)
                mEffectDic.Remove(removeId);
        }

        public int CreateTargetEffect(CampType casterCampType, int targetUnitId, short effectId)
        {
            return CreateEffect(casterCampType, IntVector2.Zero, effectId, targetUnitId, 0, 0, 0);
        }

        public int CreateAreaEffect(CampType casterCampType, IntVector2 effectPos, short effectId , short effectAngler, short param1, short param2)
        {
            return CreateEffect(casterCampType, effectPos, effectId, 0, effectAngler, param1, param2);
        }

        int CreateEffect(CampType casterCampType, IntVector2 effectPos, short effectId, int targetUnitId, short effectAngler, short param1, short param2)
        {
            UnityEngine.Debug.Log("創建技能效果 " + casterCampType.ToString() + " " + effectPos.ToString() + " " + effectAngler + " " + param1 + " " + param2);
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
            createEffect.SetInfo(effectConfig, effectPos, targetUnitId, effectAngler, param1, param2, casterCampType);
            createEffect.BeginWork();
            BattleFiled.Instance.OnCreateEffect(effectId, effectPos);
            return createEffect.iD;
        }
    }
}
