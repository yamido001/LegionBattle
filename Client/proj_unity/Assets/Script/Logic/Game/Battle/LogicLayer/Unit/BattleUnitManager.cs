using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LBCSCommon;

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

			public UnitBase GetUnitByUnitId(int id)
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

			public void OnUnitDead(int unitId)
			{
				UnitBase targetUnit = GetUnitByUnitId (unitId);
				if (null == targetUnit) {
					return;
				}
                BattleFiledLattile.Instance.RemoveUnit(targetUnit);
				if (targetUnit.Data.IsAttack)
					mAttackFighterList.Remove (targetUnit);
				else
					mDefenderFighterList.Remove (targetUnit);
				targetUnit.OnDead ();
                mFighterDic.Remove(unitId);
            }
		}
	}
}

