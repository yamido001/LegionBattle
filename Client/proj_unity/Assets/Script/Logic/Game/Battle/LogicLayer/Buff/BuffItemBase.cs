
namespace GameBattle.LogicLayer.Buff
{
    public abstract class BuffItemBase
    {
        protected UnitBase mUnit;
        protected GDSKit.BattleBuff mBuffConfig;
        int mCurFrameCount;

        public bool IsFinish
        {
            get;
            private set;
        }

        public void SetInfo(UnitBase unit, GDSKit.BattleBuff buffConfig)
        {
            mBuffConfig = buffConfig;
            mUnit = unit;
        }

        public void Update()
        {
            if (mBuffConfig.effectType == (byte)BuffEffectWayType.Once)
                return;
            ++mCurFrameCount;

            if(mCurFrameCount % mBuffConfig.effectTypeParam == 0)
            {
                OnEffect();
            }
            if(mCurFrameCount > mBuffConfig.time)
            {
                OnFinish();
                IsFinish = true;
            }
        }

        public void Destory()
        {
            BattleFiled.Instance.OnBuffExpire(mUnit.ID, mBuffConfig.id);
        }

        protected virtual void OnEffect()
        {
            
        }

        protected virtual void OnFinish()
        {
            
        }
    }
}


