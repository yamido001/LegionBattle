using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

namespace GameBattle{

	namespace LogicLayer
	{
		/// <summary>
		/// 逻辑层战斗基础单位
		/// </summary>
		public class UnitBase{

			UnitAIComponent mAI;
			//FighterSkillComponent mSkill;

			public UnitConfigData Data {
				get;
				private set;
			}

			public int ID {
				get;
				private set;
			}

			public bool IsDead
			{
				get;
				private set;
			}

			public IntVector2 Position {
				get;
				private set;
			}

			int[] mAttributes = new int[(int)FighterAttributeType.Count];

			public void Init(UnitConfigData data)
			{
				Data = data;

				mAttributes [(int)FighterAttributeType.Life] = data.life;
				mAttributes [(int)FighterAttributeType.Speed] = data.speed;
				mAttributes [(int)FighterAttributeType.Attack] = data.attack;
				mAttributes [(int)FighterAttributeType.AttackRange] = data.attackRange;

				Position = Data.borthPos;
				ID = Data.id;
				IsDead = false;

				//mSkill = new FighterSkillComponent (this, Data.skillList);
				mAI = new UnitAIComponent (this);
				mAI.Init ();
			}

			public void SetBattleInstruction(BattleInstructionBase instruction)
			{
				mAI.SetBattleInstruction (instruction);
			}

			public int GetAttribute(FighterAttributeType attrType)
			{
				return mAttributes[(int)attrType];
			}

			public void SetAttribute(FighterAttributeType attrType, int value)
			{
				mAttributes [(int)attrType] = value;
				//InstructionManager.Instance.CreateAttrChgInstruction (ID, attrType, value);
			}

			public void Update()
			{
				if (IsDead) {
					return;
				}
				mAI.Execute ();
				//mSkill.Update ();
			}

			public void Attack(int skillId, int targetId)
			{
				//mSkill.UseSkill (skillId, targetId);
			}

			public void MoveAngle(short moveAngle)
			{
				int speed = GetAttribute(FighterAttributeType.Speed);
				IntVector2 stopPos = IntVector2.MoveAngle (Position, moveAngle, speed);

				Position = stopPos;
				//InstructionManager.Instance.CreateMoveInstruction (ID, oldPos, Position);
			}

			public void EnterIdle()
			{
				
			}

			public void Destroy()
			{
				mAI.Destroy();
				mAI = null;
			}

			protected bool IsSameCamp(UnitConfigData fighterData)
			{
				return fighterData.camp == Data.camp;
			}

			public void OnDead()
			{
				if (IsDead)
					return;
				IsDead = true;
				//InstructionManager.Instance.CreateAttrChgInstruction (ID, FighterAttributeType.Life, 0);
			}

			public void OnUseSkill(int skillId, int targetId)
			{
				//mSkill.OnUseSkill (skillId, targetId);

				//InstructionManager.Instance.CreateSkillUseInstruction (ID, skillId, targetId);
			}
		}
	}
}
