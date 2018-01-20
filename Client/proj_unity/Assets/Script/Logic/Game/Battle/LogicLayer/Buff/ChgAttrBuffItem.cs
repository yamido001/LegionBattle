namespace GameBattle.LogicLayer.Buff
{
    public class ChgAttrBuffItem : BuffItemBase
    {
        int mTotalChgValue = 0;
        protected override void OnEffect()
        {
            BattleFiled.Instance.ChangeUnitAttr(mUnit, (FighterAttributeType)mBuffConfig.param1, mBuffConfig.param2);
            mTotalChgValue += mBuffConfig.param2;
        }

        protected override void OnFinish()
        {
            if(mBuffConfig.needClear)
            {
                BattleFiled.Instance.ChangeUnitAttr(mUnit, (FighterAttributeType)mBuffConfig.param1, -mTotalChgValue);
            }
        }
    }
}

