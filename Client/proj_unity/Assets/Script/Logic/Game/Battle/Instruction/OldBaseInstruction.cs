using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

namespace GameBattle
{
	public enum InstructionType
	{
		Move,
		SkillUse,
		AttrChg,
		EnterIdle,
	}

	public abstract class OldBaseInstruction{
		public int selfId;
		public abstract InstructionType InstructionType {
			get;
		}
	}

	public class OldMoveInstruction : OldBaseInstruction{
		public IntVector2 fromPos;
		public IntVector2 toPos;

		public override InstructionType InstructionType {
			get {
				return InstructionType.Move;
			}
		}
	}

	public class OldSkillUseInstruction : OldBaseInstruction{
		public int skillId;
		public int targetId;

		public override InstructionType InstructionType {
			get {
				return InstructionType.SkillUse;
			}
		}
	}

	public class OldAttrChgInstruction : OldBaseInstruction{
		public FighterAttributeType attrType;
		public int curValue;

		public override InstructionType InstructionType {
			get {
				return InstructionType.AttrChg;
			}
		}
	}

	public class OldEnterIdleInstruction : OldBaseInstruction{
		public override InstructionType InstructionType {
			get {
				return InstructionType.EnterIdle;
			}
		}
	}
}
