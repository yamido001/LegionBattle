using System.Collections;
using System.Collections.Generic;
using LegionBattle.ServerClientCommon;
namespace GameBattle.LogicLayer
{
	public class UnitAIInstructionMove : PreCreateStateBase {

		UnitBase mUnitBase;
		UnitAIComponent mAIComponent;
		short mMoveAngle;

		public UnitAIInstructionMove(UnitBase unitBase, UnitAIComponent unitAIComp)
		{
			mUnitBase = unitBase;
			mAIComponent = unitAIComp;
		}

		public override void Entered (object param)
		{
			BattleMove moveInstruction = param as BattleMove;
			mMoveAngle = moveInstruction.MoveAngle;
			Logger.LogInfo ("设置移动指令  " + mMoveAngle);
		}

		public override void Execute ()
		{
			mUnitBase.MoveAngle (mMoveAngle);
		}
	}
}


