
namespace GameBattle.LogicLayer.Skill
{
    public abstract class SkillItem
    {
        protected GDSKit.SkillConfig mSkillConfig;
        protected UnitBase mUnit;
        int mSkillUseFrame;
        System.Action<short> mHdlOnFinish;

        public short skillId
        {
            get;
            private set;
        }

        public bool isInCd
        {
            get;
            private set;
        }

        public bool isCasting
        {
            get;
            private set;
        }

        public void SetSkillInfo(UnitBase myUnit, GDSKit.SkillConfig skillConfig, System.Action<short> hdlOnFinish)
        {
            mUnit = myUnit;
            mSkillConfig = skillConfig;
            mHdlOnFinish = hdlOnFinish;
            this.skillId = skillConfig.id;
        }

        public void Update()
        {
            ++mSkillUseFrame;
            if(isCasting && mSkillUseFrame > mSkillConfig.consumeTime)
            {
                isCasting = false;
                mHdlOnFinish.Invoke(skillId);
            }
            if(mSkillUseFrame > mSkillConfig.cd)
            {
                isInCd = false;
            }
            OnUpdate();
        }

        public virtual void TryUseSkill(int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            
        }

        public void BreakSkill()
        {
            isCasting = false;
            isInCd = false;
        }

        public void Destroy()
        {
            
        }

        protected virtual void OnUse(int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            mSkillUseFrame = 0;
            isCasting = true;
            isInCd = true;
        }

        protected virtual void OnUpdate()
        {

        }

        protected bool CheckSkillDistance(int targetUnitId)
        {
            UnitBase targetUnitBase = BattleUnitManager.Instance.GetUnitByUnitId(targetUnitId);
            if((targetUnitBase.Position - mUnit.Position).SqrMagnitude > mSkillConfig.skillDistance * mSkillConfig.skillDistance)
            {
                return false;
            }
            return true;
        }
    }
}


