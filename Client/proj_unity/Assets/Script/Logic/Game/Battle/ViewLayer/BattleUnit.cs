using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle
{
	namespace BattleView
	{
		public class BattleUnit{

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

			UnitActor mActor;

			public void Init(FighterConfigData data)
			{
				mActor = new UnitActor (this);
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
			public void ExecuteInstruction(BaseInstruction instruction)
			{
				switch (instruction.InstructionType) {
				case InstructionType.Move:
					ExecuteMoveInstruction (instruction as MoveInstruction);
					break;
				case InstructionType.SkillUse:
					ExecuteAttackInstruction (instruction as SkillUseInstruction);
					break;
				case InstructionType.AttrChg:
					ExecuteAttrChgInstruction (instruction as AttrChgInstruction);
					break;
				default:
					break;
				}
			}

			void ExecuteMoveInstruction(MoveInstruction moveInstruction)
			{
				Position = BattleUtils.LogicPosToScenePos (moveInstruction.toPos);
				mActor.OnMove ();
			}

			void ExecuteAttackInstruction(SkillUseInstruction attackInstruction)
			{
				mActor.PlayAnimation ("shoot");
			}

			void ExecuteAttrChgInstruction(AttrChgInstruction attrChgInstruction)
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

