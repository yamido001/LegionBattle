using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle{
	
	public class Fighter{

		FighterAI mAI;
		FighterActor mActor;
		FighterSkill mSkill;

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

		public Vector2 Position {
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

			mSkill = new FighterSkill (this, Data.skillList);
			mAI = new FighterAI (this);
			mActor = new FighterActor (this);
		}

		public int GetAttribute(FighterAttributeType attrType)
		{
			return mAttributes[(int)attrType];
		}

		public void SetAttribute(FighterAttributeType attrType, int value)
		{
			mAttributes [(int)attrType] = value;
		}

		public void Update()
		{
			if (IsDead) {
				return;
			}
			mAI.Update ();
			mSkill.Update ();
			mActor.Update ();
		}

		public void Attack(int skillId, int targetId)
		{
			mSkill.UseSkill (skillId, targetId);
		}

		public void MoveTo(Vector2 pos)
		{
			float dis = BattleUtils.FastDistance (Position, pos);
			float moveDis = GetAttribute(FighterAttributeType.Speed) / 30f;
			if (dis < GetAttribute(FighterAttributeType.Speed)) {
				//移动到目标点
				Position = pos;
			}
			else
			{
				Position = Vector2.Lerp (Position, pos, moveDis / dis);
			}
			mActor.OnMove ();
			mActor.PlayAnimation ("run");
		}

		public void PlayAnimation(string animation)
		{
			mActor.PlayAnimation (animation);
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

		#region 服务器返回消息
		public void OnDead()
		{
			if (IsDead)
				return;
			IsDead = true;
			mActor.Destroy ();
			mActor = null;
		}

		public void OnUseSkill(int skillId, int targetId)
		{
			mSkill.OnUseSkill (skillId, targetId);
		}
		#endregion
	}
}
