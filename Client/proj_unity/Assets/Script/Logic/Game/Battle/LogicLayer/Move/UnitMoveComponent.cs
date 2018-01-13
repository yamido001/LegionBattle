using LegionBattle.ServerClientCommon;
namespace GameBattle.LogicLayer.Move
{
    public class UnitMoveComponent
    {
        UnitBase mUnit;

        public UnitMoveComponent(UnitBase unit)
        {
            mUnit = unit;
            lattlcePos = new IntVector2(-1, -1);
            OnPosChanged(IntVector2.Zero, true);
        }

        public IntVector2 lattlcePos
        {
            get;
            private set;
        }

        public void MoveAngle(short moveAngle)
        {
            //Logger.LogInfo ("进入到移动状态");
            int speed = mUnit.GetAttribute(FighterAttributeType.Speed);
            IntVector2 stopPos = IntVector2.MoveAngle(mUnit.Position, moveAngle, speed);

            IntVector2 fromPos = mUnit.Position;
            mUnit.Position = stopPos;
            OnPosChanged(fromPos);
            BattleLogManager.Instance.Log("DoInBattle", BattleTimeLine.Instance.CurFrameCount + " " + mUnit.ID + " Move " + moveAngle);
        }

        void OnPosChanged(IntVector2 fromPos, bool isInit = false)
        {
            BattleFiledLattile.Instance.UpadteUnitPos(mUnit);
            BattleFiled.Instance.OnUnitMove(mUnit.ID, fromPos, mUnit.Position);
        }
    }
}

