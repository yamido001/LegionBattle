using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle
{
	namespace BattleView
	{
		public class OldBattleUnit{

			public int ID {
				get;
				private set;
			}

			public Vector3 Position {
				get;
				private set;
			}

			public bool IsDead
			{
				get;
				private set;
			}

			OldUnitActor mActor;

			public void Init(UnitConfigData data)
			{
				mActor = new OldUnitActor (this);
				ID = data.id;
				IsDead = false;
			}

			public void Update()
			{
				if (IsDead)
					return;
				mActor.Update ();
			}

			public void Destroy()
			{
				mActor.Destroy ();
			}

			#region 执行指令函数
			public void ExecuteInstruction(OldBaseInstruction instruction)
			{
				switch (instruction.InstructionType) {
				case InstructionType.Move:
					ExecuteMoveInstruction (instruction as OldMoveInstruction);
					break;
				case InstructionType.SkillUse:
					ExecuteAttackInstruction (instruction as OldSkillUseInstruction);
					break;
				case InstructionType.AttrChg:
					ExecuteAttrChgInstruction (instruction as OldAttrChgInstruction);
					break;
				default:
					break;
				}
			}

			void ExecuteMoveInstruction(OldMoveInstruction moveInstruction)
			{
				Position = BattleUtils.LogicPosToScenePos (moveInstruction.toPos);
				mActor.OnMove ();
			}

			void ExecuteAttackInstruction(OldSkillUseInstruction attackInstruction)
			{
				mActor.PlayAnimation ("shoot");
			}

			void ExecuteAttrChgInstruction(OldAttrChgInstruction attrChgInstruction)
			{
				switch (attrChgInstruction.attrType) {
				case FighterAttributeType.Attack:
					break;
				case FighterAttributeType.AttackRange:
					break;
				case FighterAttributeType.Life:
					if (attrChgInstruction.curValue <= 0) {
						IsDead = true;
					}
					break;
				case FighterAttributeType.Speed:
					break;
				default:
					break;
				}
			}
			#endregion
		}
	}
}

