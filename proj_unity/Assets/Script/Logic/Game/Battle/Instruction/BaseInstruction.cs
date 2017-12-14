using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle
{
	public enum InstructionType
	{
		Move,
		SkillUse,
		AttrChg,
		EnterIdle,
	}

	public abstract class BaseInstruction{
		public int selfId;
		public abstract InstructionType InstructionType {
			get;
		}
	}

	public class MoveInstruction : BaseInstruction{
		public BattlePosition fromPos;
		public BattlePosition toPos;

		public override InstructionType InstructionType {
			get {
				return InstructionType.Move;
			}
		}
	}

	public class SkillUseInstruction : BaseInstruction{
		public int skillId;
		public int targetId;

		public override InstructionType InstructionType {
			get {
				return InstructionType.SkillUse;
			}
		}
	}

	public class AttrChgInstruction : BaseInstruction{
		public FighterAttributeType attrType;
		public int curValue;

		public override InstructionType InstructionType {
			get {
				return InstructionType.AttrChg;
			}
		}
	}

	public class EnterIdleInstruction : BaseInstruction{
		public override InstructionType InstructionType {
			get {
				return InstructionType.EnterIdle;
			}
		}
	}
}
