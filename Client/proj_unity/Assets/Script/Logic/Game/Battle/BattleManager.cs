using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBattle.LogicLayer;
using GameBattle.BattleView;

namespace GameBattle
{
	public class BattleManager : Singleton<BattleManager> {

		public void StartBattle(List<UnitConfigData> fighterDatas)
		{
			SceneCameraManager.Instance.MoveToPos (new Vector3(62f, 40f, 1.6f));
			SceneCameraManager.Instance.ForceTo(new Vector3(59.45f, 0f, 30f));

			BattleUnitManager.Instance.StartBattle (fighterDatas);
			OldBattlePlayerManager.Instance.Start (fighterDatas);
		}

		public void Update()
		{
			BattleUnitManager.Instance.Update ();
			OldBattlePlayerManager.Instance.Update ();
		}

		public void DestroyBattle()
		{
			BattleUnitManager.Instance.DestroyBattle ();
			OldBattlePlayerManager.Instance.Destroy ();
		}
	}

}
