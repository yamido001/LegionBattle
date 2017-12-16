using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle{

	namespace LogicLayer
	{
		/// <summary>
		/// 逻辑层战斗基础单位
		/// </summary>
		public class Fighter{

			FighterAIComponent mAI;
			FighterSkillComponent mSkill;

			public FighterConfigData Data {
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

			public BattlePosition Position {
				get;
				private set;
			}

			int[] mAttributes = new int[(int)FighterAttributeType.Count];

			public void Init(FighterConfigData data)
			{
				Data = data;

				mAttributes [(int)FighterAttributeType.Life] = data.life;
				mAttributes [(int)FighterAttributeType.Speed] = data.speed;
				mAttributes [(int)FighterAttributeType.Attack] = data.attack;
				mAttributes [(int)FighterAttributeType.AttackRange] = data.attackRange;

				Position = Data.borthPos;
				ID = Data.id;
				IsDead = false;

				mSkill = new FighterSkillComponent (this, Data.skillList);
				mAI = new FighterAIComponent (this);
			}

			public int GetAttribute(FighterAttributeType attrType)
			{
				return mAttributes[(int)attrType];
			}

			public void SetAttribute(FighterAttributeType attrType, int value)
			{
				mAttributes [(int)attrType] = value;
				InstructionManager.Instance.CreateAttrChgInstruction (ID, attrType, value);
			}

			public void Update()
			{
				if (IsDead) {
					return;
				}
				mAI.Update ();
				mSkill.Update ();
			}

			public void Attack(int skillId, int targetId)
			{
				mSkill.UseSkill (skillId, targetId);
			}

			public void MoveTo(BattlePosition targetPos)
			{
				BattlePosition oldPos = Position;
				
				int dis = (Position - targetPos).magnitude;
				int speed = GetAttribute(FighterAttributeType.Speed);
				if (dis < speed) {
					//移动到目标点
					Position = targetPos;
				}
				else
				{
					Position = BattlePosition.Lerp (Position, targetPos, speed, dis);
				}
				InstructionManager.Instance.CreateMoveInstruction (ID, oldPos, Position);
			}

			public void Destroy()
			{
				mAI.Destroy();
				mAI = null;
			}

			protected bool IsSameCamp(FighterConfigData fighterData)
			{
				return fighterData.camp == Data.camp;
			}

			public void OnDead()
			{
				if (IsDead)
					return;
				IsDead = true;
				InstructionManager.Instance.CreateAttrChgInstruction (ID, FighterAttributeType.Life, 0);
			}

			public void OnUseSkill(int skillId, int targetId)
			{
				mSkill.OnUseSkill (skillId, targetId);

				InstructionManager.Instance.CreateSkillUseInstruction (ID, skillId, targetId);
			}
		}
	}
}
