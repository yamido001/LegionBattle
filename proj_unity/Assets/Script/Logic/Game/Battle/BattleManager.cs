using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBattle
{
	public class BattleManager : Singleton<BattleManager> {
		
		public void StartBattle(List<FighterConfigData> fighterDatas)
		{
			SceneCameraManager.Instance.MoveToPos (new Vector3(62f, 40f, 1.6f));
			SceneCameraManager.Instance.ForceTo(new Vector3(59.45f, 0f, 30f));
			BattleServer.Instance.StartBattle (fighterDatas);
		}

		public void Update()
		{
			BattleServer.Instance.Update ();
		}

		public void DestroyBattle()
		{
			BattleServer.Instance.DestroyBattle ();
		}

	}
}

