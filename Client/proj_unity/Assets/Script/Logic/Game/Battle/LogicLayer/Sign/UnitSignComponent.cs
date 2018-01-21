using System.Collections;
using System.Collections.Generic;

namespace GameBattle.LogicLayer.Sign
{
    public class UnitSignComponent
    {
        Dictionary<int, short> mSignDic = new Dictionary<int, short>();

        public void AddSign(BattleSign sign)
        {
            int signInt = (int)sign;
            short curCount = 0;
            if(mSignDic.TryGetValue(signInt, out curCount))
            {
                mSignDic[signInt] = ++curCount;
            }
            else
            {
                mSignDic[signInt] = 1;
            }
        }

        public void RemoveSign(BattleSign sign)
        {
            int signInt = (int)sign;
            short curCount = 0;
            if (mSignDic.TryGetValue(signInt, out curCount))
            {
                --curCount;
                if(curCount <= 0)
                {
                    mSignDic.Remove(signInt);
                }
                else
                {
                    mSignDic[signInt] = curCount;
                }
            }
        }

        public bool CanMove()
        {
            return true;
        }
    }
}

