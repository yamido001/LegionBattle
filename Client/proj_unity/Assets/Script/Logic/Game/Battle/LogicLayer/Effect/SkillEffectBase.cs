using System.Collections.Generic;
using LegionBattle.ServerClientCommon;

namespace GameBattle.LogicLayer.Effect
{
    public abstract class SkillEffectBase
    {
        protected GDSKit.SkillEffect mEffectConfig;
        protected IntVector2 mEffectPos;
        protected int mTargetUnitId;
        protected short mEffectAngler;
        protected short mEffectParam1;
        protected short mEffectParam2;
        
        public abstract SkillEffectType effectType 
        {
            get;
        }

        public int iD
        {
            get;
            private set;
        }

        public bool isFinish
        {
            get;
            private set;
        }

        static int SkillEffectCount = 0;

        public SkillEffectBase()
        {
            iD = ++SkillEffectCount;
        }

#region 生命周期
        public void SetInfo(GDSKit.SkillEffect effectGds, IntVector2 effectPos, int targetUnitId, short effectAngler, short param1, short param2)
        {
            mEffectConfig = effectGds;
            mEffectPos = effectPos;
            mEffectAngler = effectAngler;
            mEffectParam1 = param1;
            mEffectParam2 = param2;
            mTargetUnitId = targetUnitId;
        }

        public void BeginWork()
        {
            DoExecute();
        }

        public void Update()
        {
            if (isFinish)
                return;
            DoExecute();
        }

        public void Destroy()
        {
            
        }
#endregion

#region 子类需要继承的函数
        protected virtual void OnDestroy()
        {
            
        }

        protected abstract void DoExecute();
        #endregion

        static List<UnitBase> staticUnitList = new List<UnitBase>();

        /// <summary>
        /// 返回的list是不安全的，只能在这一次使用
        /// </summary>
        /// <returns>The area unit list.</returns>
        protected List<UnitBase> GetAreaUnitList()
        {
            staticUnitList.Clear();
            if(mEffectConfig.isAoe)   
            {
                switch ((SkillEffectAreaType)mEffectConfig.areaType)
                {
                    case SkillEffectAreaType.Circle:
                        BattleFiledLattile.Instance.GetUnitListInCircle(mEffectPos, mEffectParam1, staticUnitList);
                        break;
                    case SkillEffectAreaType.Line:
                        BattleFiledLattile.Instance.GetUnitListInLine(mEffectPos, mEffectAngler, mEffectParam1, mEffectParam2, staticUnitList);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                UnitBase targetUnit = BattleUnitManager.Instance.GetUnitByUnitId(mTargetUnitId);
                if(null != targetUnit)
                    staticUnitList.Add(targetUnit);
            }
            return staticUnitList;
        }
    }
}
