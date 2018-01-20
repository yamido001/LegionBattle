using System.Collections;
using System.Collections.Generic;

namespace GameBattle.LogicLayer.Buff
{
    public class UnitBuffComponent
    {
        UnitBase mUnit;
        List<BuffItemBase> mBuffList = new List<BuffItemBase>();

        public UnitBuffComponent(UnitBase unit)
        {
            mUnit = unit;
        }

        public void AddBuff(short buffId)
        {
            GDSKit.BattleBuff buffConfig = GDSKit.BattleBuff.GetInstance(buffId);
            BuffItemBase buffItem = null;
            switch((BuffType)buffConfig.type)
            {
                case BuffType.ChgAttr:
                    buffItem = new ChgAttrBuffItem();
                    break;
                case BuffType.SetSign:
                    break;
                default:
                    throw new System.NotImplementedException("未实现的buff类型 " + buffConfig.type);
            }
            buffItem.SetInfo(mUnit, buffConfig);
            mBuffList.Add(buffItem);
        }

        public void Update()
        {
            for (int i = mBuffList.Count - 1; i >= 0; i--)
            {
                BuffItemBase buffItem = mBuffList[i];
                buffItem.Update();
                if(buffItem.IsFinish)
                {
                    buffItem.Destory();
                    mBuffList.RemoveAt(i);
                }
            }
        }

        public void Destroy()
        {
            foreach (var buffItem in mBuffList)
                buffItem.Destory();
            mBuffList.Clear();
        }
    }
}

