using GameBattle.LogicLayer.Effect;

namespace GameBattle.LogicLayer.Skill
{
    public abstract class SkillItem
    {
        protected GDSKit.SkillConfig mSkillConfig;
        protected UnitBase mUnit;
        int mSkillUseFrame;
        System.Action<short> mHdlOnFinish;
        int mSkillEffectId;

        int mTargetUnitId;
        short mSkillAngle;
        short mSkillParam1;
        short mSkillParam2;

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

        private bool isPreparation
        {
            get;
            set;
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
            if (!isCasting)
            {
                if (mSkillUseFrame > mSkillConfig.cd)
                {
                    isInCd = false;
                }
                return;
            }
            if (isPreparation && mSkillUseFrame > mSkillConfig.prepareTime)
            {
                isPreparation = false;
                OnSkill();

            }
            if(!isPreparation && mSkillUseFrame > mSkillConfig.prepareTime + mSkillConfig.continousTime)
            {
                isCasting = false;
                mHdlOnFinish.Invoke(skillId);
            }
        }

        public void UseSkill(int targetUnitId, short skillAngle, short skillParam1, short skillParam2)
        {
            if (!CanUseSkill(targetUnitId, skillAngle, skillParam1, skillParam2))
                return;
            mSkillAngle = skillAngle;
            mSkillParam1 = skillParam1;
            mSkillParam2 = skillParam2;
            mTargetUnitId = targetUnitId;

            mSkillUseFrame = 0;
            isCasting = true;
            isInCd = true;
            if(mSkillConfig.prepareTime > 0)
            {
                isPreparation = true;
            }
            else
            {
                isPreparation = false;
                OnSkill();
            }
        }

        public void BreakSkill()
        {
            isCasting = false;
            if (mSkillConfig.continousTime > 0)
            {
                SkillEffectManager.Instance.DestroyEffect(mSkillEffectId);
            }
        }

        public void Destroy()
        {

        }

        void OnSkill()
        {
            if (!CanUseSkill(mTargetUnitId, mSkillAngle, mSkillParam1, mSkillParam2))
                return;
            mSkillEffectId = OnUse(mTargetUnitId, mSkillAngle, mSkillParam1, mSkillParam2);
        }

        #region 需要子类继承的函数
        protected abstract bool CanUseSkill(int targetUnitId, short skillAngle, short skillParam1, short skillParam2);

        protected abstract int OnUse(int targetUnitId, short skillAngle, short skillParam1, short skillParam2);
        #endregion

        protected bool CheckSkillDistance(int targetUnitId)
        {
            UnitBase targetUnitBase = BattleUnitManager.Instance.GetUnitByUnitId(targetUnitId);
            if ((targetUnitBase.Position - mUnit.Position).SqrMagnitude > mSkillConfig.distance * mSkillConfig.distance)
            {
                return false;
            }
            return true;
        }
    }
}


