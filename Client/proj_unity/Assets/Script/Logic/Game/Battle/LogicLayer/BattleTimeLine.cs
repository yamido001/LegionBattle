using System.Collections;
using System.Collections.Generic;
using GameBattle;
using GameBattle.LogicLayer;

public class BattleTimeLine : Singleton<BattleTimeLine>{

	#region 生命周期
	public void StartBattle(List<UnitConfigData> fighterDatas)
	{
		BattleUnitManager.Instance.StartBattle (fighterDatas);
	}

	public void Update()
	{
		//GameMain.Instance.TimeMgr.CurTimeMs;
	}

	public void Destroy()
	{
		BattleUnitManager.Instance.DestroyBattle ();
	}
	#endregion

	void PlayOneFrame()
	{
		
	}
}
