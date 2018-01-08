using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

namespace GameBattle.BattleView
{
	public class BattleActorManager : Singleton<BattleActorManager>{

		Dictionary<int, BattleActorBase> mBattleActorDic = new Dictionary<int, BattleActorBase>();

		public void StartBattle(List<UnitConfigData> fighterDatas)
		{
			for (int i = 0; i < fighterDatas.Count; ++i) {
				BattleActorBase actor = new BattleActorBase (fighterDatas[i].id);
				mBattleActorDic [actor.Id] = actor;
			}
		}

		public void Update()
		{
			var enumerator = mBattleActorDic.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				enumerator.Current.Value.Update ();
			}
		}

		public void DestroyBattle()
		{
			var enumerator = mBattleActorDic.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				enumerator.Current.Value.Destroy ();
			}
			mBattleActorDic.Clear ();
		}

		BattleActorBase GetActorById(int unitId)
		{
			BattleActorBase ret = null;
			mBattleActorDic.TryGetValue (unitId, out ret);
			return ret;
		}

		public void OnUnitMove(int unitId, IntVector2 fromPos, IntVector2 toPos)
		{
			BattleActorBase actor = GetActorById (unitId);
			if (null != actor) {
				actor.OnMove (fromPos, toPos);
			}
		}

		public void OnUnitEnterIdle(int unitId)
		{
			BattleActorBase actor = GetActorById (unitId);
			if (null != actor) {
				actor.OnEnterIdle ();
			}
		}
	}
}

