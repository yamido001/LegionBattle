using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle{

	namespace LogicLayer
	{
		public class FighterAIComponent {

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
			public FighterAIComponent(Fighter fighter)
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

				long sqrDis = (mSelf.Position - mAttackTarget.Position).SqrMagnitude;
				int attackRange = mSelf.GetAttribute (FighterAttributeType.AttackRange);
				if (sqrDis > attackRange * attackRange) {
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
					long sqrDis = (mSelf.Position - mMoveTarget.Position).SqrMagnitude;
					int attackRange = mSelf.GetAttribute (FighterAttributeType.AttackRange);
					if (sqrDis < attackRange * attackRange) {
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
				List<Fighter> enemyFighters = BattleFighterManager.Instance.GetEnemyFighterList (mSelf.Data.id);
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
				long sqrDis = (mAttackTarget.Position - mSelf.Position).SqrMagnitude;
				int attackRange = mSelf.GetAttribute (FighterAttributeType.AttackRange);
				if (sqrDis > attackRange * attackRange) {
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
			}

			Fighter GetNearestFighter(List<Fighter> fighters)
			{
				Fighter ret = null;
				long curSqrDis = 0;
				for (int i = 0; i < fighters.Count; ++i) {
					Fighter findFighter = fighters [i];
					if (ret == null ) {
						ret = findFighter;
						curSqrDis = (mSelf.Position - findFighter.Position).SqrMagnitude;
						continue;
					}
					long sqrDis = (mSelf.Position - findFighter.Position).SqrMagnitude;
					if (sqrDis < curSqrDis) {
						curSqrDis = sqrDis;
						ret = findFighter;
					}
				}
				return ret;
			}
			#endregion
		}	
	}
}

