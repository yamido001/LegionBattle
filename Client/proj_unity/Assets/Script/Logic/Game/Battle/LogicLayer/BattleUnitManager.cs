using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

namespace GameBattle
{
	namespace LogicLayer
	{
		/// <summary>
		/// 前端战斗管理器
		/// </summary>
		public class BattleUnitManager : Singleton<BattleUnitManager> {

			List<UnitBase> mFighters = new List<UnitBase>();
			Dictionary<int, UnitBase> mFighterDic = new Dictionary<int, UnitBase>();
			List<UnitBase> mAttackFighterList = new List<UnitBase>();
			List<UnitBase> mDefenderFighterList = new List<UnitBase>();

			#region 生命周期函数
			public void StartBattle(List<UnitConfigData> fighterDatas)
			{
				SceneCameraManager.Instance.MoveToPos (new Vector3(62f, 40f, 1.6f));
				SceneCameraManager.Instance.ForceTo(new Vector3(59.45f, 0f, 30f));

				for (int i = 0; i < fighterDatas.Count; ++i) {
					UnitBase fighter = new UnitBase ();
					fighter.Init (fighterDatas [i]);
					mFighters.Add (fighter);
					mFighterDic.Add (fighter.ID, fighter);
					if (fighter.Data.IsAttack) {
						mAttackFighterList.Add (fighter);
					} else {
						mDefenderFighterList.Add (fighter);
					}
				}
			}

			public void SetBattleInstruction(BattleInstructionBase instruction)
			{
				UnitBase battleUnit = GetUnitByUnitId (instruction.SceneUnitId);
				if (null != battleUnit) {
					battleUnit.SetBattleInstruction (instruction);
				}
			}

			public void Update()
			{
				for (int i = 0; i < mFighters.Count; ++i) {
					mFighters [i].Update ();
				}
				for (int i = mFighters.Count - 1; i >= 0; --i) {
					if (!mFighters [i].IsDead)
						continue;
					mFighters [i].Destroy ();
					mFighters.RemoveAt (i);
				}
			}

			public void DestroyBattle()
			{
				for (int i = 0; i < mFighters.Count; ++i) {
					mFighters [i].Destroy ();
				}
				mFighters = null;
			}
			#endregion

			#region 回调通知
			System.Action<int, IntVector2, IntVector2> mOnMoveListener;
			public void AddUnitMoveListener(System.Action<int, IntVector2, IntVector2> listener)
			{
				mOnMoveListener = listener;
			}

			public void OnUnitMove(int unitId, IntVector2 fromPos, IntVector2 toPos)
			{
				if (null != mOnMoveListener)
					mOnMoveListener.Invoke (unitId, fromPos, toPos);
			}

			System.Action<int> mOnEnterIdleListener;
			public void AddUnitEnterIdleListener(System.Action<int> listener)
			{
				mOnEnterIdleListener = listener;
			}

			public void OnUnitEnterIdle(int unitId)
			{
				if (null != mOnEnterIdleListener)
					mOnEnterIdleListener.Invoke (unitId);
			}
			#endregion

			UnitBase GetUnitByUnitId(int id)
			{
				UnitBase unit = null;
				mFighterDic.TryGetValue (id, out unit);
				return unit;
			}

			public List<UnitBase> GetEnemyFighterList(int id)
			{
				UnitBase requestFighter = GetUnitByUnitId (id);
				return requestFighter.Data.IsAttack ? mDefenderFighterList : mAttackFighterList;
			}

			public void FighterTryUseSkill(int fighterId, int skillId, int targetFighterId)
			{
				UnitBase skillUser = GetUnitByUnitId (fighterId);
				if (skillUser == null || skillUser.IsDead) {
					return;
				}
				UnitBase targetFighter = GetUnitByUnitId (targetFighterId);
				if (null == targetFighter || targetFighter.IsDead) {
					return;
				}
				GDSKit.SkillConfig skillConfig = GDSKit.SkillConfig.GetInstance (skillId);
				int damage = skillUser.GetAttribute(FighterAttributeType.Attack) * skillConfig.attackPercentage / 100;
				int preLife = targetFighter.GetAttribute (FighterAttributeType.Life);
				preLife -= damage;

				if (preLife <= 0) {
					if (targetFighter.Data.IsAttack)
						mAttackFighterList.Remove (targetFighter);
					else
						mDefenderFighterList.Remove (targetFighter);
					targetFighter.OnDead ();
				} else {
					targetFighter.SetAttribute (FighterAttributeType.Life, preLife);
				}

				skillUser.OnUseSkill (skillId, targetFighterId);
			}
		}
	}
}

