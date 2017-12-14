using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameBattle.LogicLayer;
using GameBattle.BattleView;

namespace GameBattle
{
	public class BattleManager : Singleton<BattleManager> {

		public void StartBattle(List<FighterConfigData> fighterDatas)
		{
			SceneCameraManager.Instance.MoveToPos (new Vector3(62f, 40f, 1.6f));
			SceneCameraManager.Instance.ForceTo(new Vector3(59.45f, 0f, 30f));

			BattleFighterManager.Instance.StartBattle (fighterDatas);
			BattlePlayerManager.Instance.Start (fighterDatas);
		}

		public void Update()
		{
			BattleFighterManager.Instance.Update ();
			BattlePlayerManager.Instance.Update ();
		}

		public void DestroyBattle()
		{
			BattleFighterManager.Instance.DestroyBattle ();
			BattlePlayerManager.Instance.Destroy ();
		}
	}

}
