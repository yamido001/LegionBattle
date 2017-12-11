using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle{

	public class FighterAI {

		enum AIState
		{
			Idle,
			MoveTarget,
			Attack,
		}

		AIState mState = AIState.Idle;
		Fighter mAttackTarget = null;
		Fighter mMoveTarget = null;
		Fighter mSelf;

		#region 生命周期
		public FighterAI(Fighter fighter)
		{
			mSelf = fighter;
		}

		public void Update()
		{
			if (mSelf.ID == 10) {
				int a = 12;
				++a;
			}
			switch (mState) {
			case AIState.Idle:
				UpdateIdle ();
				break;
			case AIState.MoveTarget:
				UpdateMoveToTarget ();
				break;
			case AIState.Attack:
				UpdateAttack ();
				break;
			default:
				break;
			}
		}

		public void Destroy()
		{

		}
		#endregion

		#region 不同状态
		void UpdateIdle()
		{
			FindAttackTarget ();
		}

		void UpdateAttack()
		{
			if (mAttackTarget.IsDead) {
				FindAttackTarget ();
				return;
			}

			float dis = BattleUtils.FastDistance (mSelf.Position, mAttackTarget.Position);
			if (dis > mSelf.GetAttribute (FighterAttributeType.AttackRange)) {
				SetMoveTarget (mAttackTarget);
			} else {
				DoAttack ();
			}
		}

		void UpdateMoveToTarget()
		{
			if (mMoveTarget.IsDead) {
				FindAttackTarget ();		
			} 
			else {
				float dis = BattleUtils.FastDistance (mSelf.Position, mMoveTarget.Position);
				if (dis < mSelf.GetAttribute (FighterAttributeType.AttackRange)) {
					SetAttackTarget (mMoveTarget);
				} else {
					DoMove ();
				}
			}
		}

		#endregion

		#region 通用接口
		void FindAttackTarget()
		{
			List<Fighter> enemyFighters = BattleServer.Instance.GetEnemyFighterList (mSelf.Data.id);
			Fighter nearestEnemy = GetNearestFighter (enemyFighters);
			if (null != nearestEnemy)
				SetAttackTarget (nearestEnemy);
			else
				DoIdle ();
		}

		void SetAttackTarget(Fighter target)
		{
			ClearMoveTarget ();
			mAttackTarget = target;
			float dis = BattleUtils.FastDistance (mAttackTarget.Position, mSelf.Position);
			if (dis > mSelf.GetAttribute(FighterAttributeType.AttackRange)) {
				SetMoveTarget (target);
			} else {
				DoAttack ();
			}
		}

		void ClearAttackTarget()
		{
			mAttackTarget = null;
		}

		void DoAttack()
		{
			mState = AIState.Attack;
			mSelf.Attack (1, mAttackTarget.ID);
		}

		void DoMove()
		{
			mState = AIState.MoveTarget;
			mSelf.MoveTo (mMoveTarget.Position);
		}

		void SetMoveTarget(Fighter target)
		{
			ClearAttackTarget ();
			mMoveTarget = target;
			DoMove ();
		}

		void ClearMoveTarget()
		{
			mMoveTarget = null;
		}

		void DoIdle()
		{
			mState = AIState.Idle;
			mSelf.PlayAnimation ("idle");
		}

		Fighter GetNearestFighter(List<Fighter> fighters)
		{
			Fighter ret = null;
			float curDis = 0f;
			for (int i = 0; i < fighters.Count; ++i) {
				Fighter findFighter = fighters [i];
				if (ret == null ) {
					ret = findFighter;
					curDis = BattleUtils.FastDistance (mSelf.Position, findFighter.Position);
					continue;
				}
				float dis = BattleUtils.FastDistance (mSelf.Position, findFighter.Position);
				if (dis < curDis) {
					curDis = dis;
					ret = findFighter;
				}
			}
			return ret;
		}
		#endregion
	}
}

