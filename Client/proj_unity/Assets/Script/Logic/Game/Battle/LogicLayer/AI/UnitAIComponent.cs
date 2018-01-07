using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

namespace GameBattle{

	namespace LogicLayer
	{
		public class UnitAIComponent : PreCreateStateMechine {

			UnitBase mSelf;
			IntVector2 mMovePos;

			#region 生命周期
			public UnitAIComponent(UnitBase fighter)
			{
				mSelf = fighter;
			}

			protected override void OnInit ()
			{
				
			}

			protected override void OnRegisterState ()
			{
				RegisterState ((int)UnitAIStateType.InstructionMove, new UnitAIInstructionMove (mSelf, this));
				RegisterState ((int)UnitAIStateType.InstructionSkill, new UnitAIInstructionAttack (mSelf, this));
				RegisterState ((int)UnitAIStateType.Idle, new UnitAIIdle (mSelf, this));
			}

			protected override void OnDestroy ()
			{
				
			}
			#endregion

			public void SetBattleInstruction(BattleInstructionBase instruction)
			{
				switch (instruction.InstructionType) {
				case BattleInstructionType.Move:
					Enter ((int)UnitAIStateType.InstructionMove, instruction);
					break;
				case BattleInstructionType.Skill:
					break;
				default:
					break;
				}
			}
		}	
	}
}

