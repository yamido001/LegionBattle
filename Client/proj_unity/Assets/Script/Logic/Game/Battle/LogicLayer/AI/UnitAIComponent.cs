using LBCSCommon;
using LBMath;

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
                RegisterState ((int)UnitAIStateType.RandomMove, new UnitAIRandomMove(mSelf, this));
                if (mSelf.Data.isRandomMove)
                {
                    Enter((int)UnitAIStateType.RandomMove, null);
                }
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
				case BattleInstructionType.StopMove:
					Enter ((int)UnitAIStateType.Idle, instruction);
					break;
				case BattleInstructionType.AreaTargetSkill:
				case BattleInstructionType.NoTargetSkill:
				case BattleInstructionType.UnitTargetSkill:
					Enter ((int)UnitAIStateType.InstructionSkill, instruction);
					break;
				default:
					break;
				}
			}
		}	
	}
}

