using System.Collections;
using System.Collections.Generic;
using LBCSCommon;
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
            mUnitBase.skillComp.BreakSkill();
			BattleMove moveInstruction = param as BattleMove;
			mMoveAngle = moveInstruction.MoveAngle;
			Logger.LogInfo ("设置移动指令  " + mMoveAngle);
		}

		public override void Execute ()
		{
			mUnitBase.moveComp.MoveAngle (mMoveAngle);
		}
	}
}


