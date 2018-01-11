using System.Collections;
using System.Collections.Generic;
using LegionBattle.ServerClientCommon;

namespace GameBattle.LogicLayer
{
	public class UnitAIInstructionAttack : PreCreateStateBase {

		public UnitAIInstructionAttack(UnitBase unitBase, UnitAIComponent unitAIComp)
		{
			
		}

		public override void Entered (object param)
		{
			BattleInstructionBase instruction = param as BattleInstructionBase;
			switch (instruction.InstructionType) {
			case BattleInstructionType.AreaTargetSkill:
				break;
			case BattleInstructionType.NoTargetSkill:
				break;
			case BattleInstructionType.UnitTargetSkill:
				break;
			default:
				break;
			}
		}

	}
}
