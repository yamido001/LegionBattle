using LBMath;
using GameBattle.LogicLayer.Scene;

namespace GameBattle.LogicLayer.Move
{
    public class UnitMoveComponent
    {
        UnitBase mUnit;

        public void Init(UnitBase unit)
        {
            mUnit = unit;
            lattlcePos = new IntVector2(-1, -1);
            OnPosChanged(IntVector2.Zero, true);
        }

        public IntVector2 lattlcePos
        {
            get;
            set;
        }

        public void MoveAngle(short moveAngle)
        {
            //Logger.LogInfo ("进入到移动状态");
            int speed = mUnit.GetAttribute(FighterAttributeType.Speed);
            IntVector2 stopPos = IntVector2.MoveAngle(mUnit.Position, moveAngle, speed);
            stopPos = LimitPos(stopPos);

            IntVector2 fromPos = mUnit.Position;
            mUnit.Position = stopPos;
            OnPosChanged(fromPos);
            //Logger.LogError("移动 " + fromPos.ToString() + " " + mUnit.Position);
            BattleLogManager.Instance.Log("DoInBattle", BattleTimeLine.Instance.CurFrameCount + " " + mUnit.ID + " Move " + moveAngle);
        }

        /// <summary>
        /// 向目标点移动，当前有误差，移动时会来回抖动
        /// </summary>
        /// <param name="targetPos"></param>
        public void MoveToPos(IntVector2 targetPos)
        {
            if (targetPos == mUnit.Position)
                return;
            IntVector2 offset = targetPos - mUnit.Position;
            int speed = mUnit.GetAttribute(FighterAttributeType.Speed);
            IntVector2 newPos = targetPos;
            IntVector2 fromPos = mUnit.Position;
            if (offset.SqrMagnitude > speed * speed)
            {
                short angle = (short)ATanFuncTable.GetAngle(offset.x, offset.y);
                
                IntegerFloat cosValue = CosFuncByTable.CosAngle(angle);
                IntegerFloat sinValue = SinFuncByTable.SinAngle(angle);
                newPos.x = mUnit.Position.x + (cosValue * speed).ToInt();
                newPos.y = mUnit.Position.y + (sinValue * speed).ToInt();
                newPos = LimitPos(newPos);
                if (mUnit.ID == 6)
                {
                    Logger.LogError("MoveToPos " + angle + "  offset " + (newPos - fromPos).ToString());
                }
            }
            mUnit.Position = newPos;
            OnPosChanged(fromPos);
        }

        void OnPosChanged(IntVector2 fromPos, bool isInit = false)
        {
            BattleFiledLattile.Instance.UpadteUnitPos(mUnit);
            BattleFiled.Instance.OnUnitMove(mUnit.ID, fromPos, mUnit.Position);
        }

        IntVector2 LimitPos(IntVector2 pos)
        {
            IntVector2 ret = pos;
            ret = IntVector2.Clamp(pos, IntVector2.Zero, BattleSceneManager.Instance.SceneSize);
            return ret;
        }
    }
}

