using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDSKit;
using GameBattle;

/// <summary>
/// 游戏逻辑场景，包含战斗、Unity场景等
/// </summary>
public class GameSceneManager{
	
	public int CurSceneId
	{
		get;
		private set;
	}

	bool mHasUnitySceneLoaded = false;
	int mUnityScenePerTop = 70;

	Tupple<float, string> mLoadingEventParam;

	GameScene mCurSceneInfo;


	public void Init()
	{
		GameMain.Instance.EventMgr.PostObjectEvent (EventId.GameSceneMgrInitFinish, null);
		GameMain.Instance.EventMgr.RegisterObjectEvent (EventId.UnitySceneLoadingInfo, this, OnUnitySceneLoadProgress);
		GameMain.Instance.EventMgr.RegisterObjectEvent (EventId.UnitySceneLoaded, this, OnUnitySceneLoaded);
	}

	public void Destroy()
	{
		GameMain.Instance.EventMgr.RemoveObjectEvent (EventId.UnitySceneLoadingInfo, this);
		GameMain.Instance.EventMgr.RemoveObjectEvent (EventId.UnitySceneLoaded, this);
		BattleManager.Instance.DestroyBattle ();
	}

	public void EnterScene(int sceneId)
	{
		CurSceneId = sceneId;
		if (null == mLoadingEventParam)
			mLoadingEventParam = new Tupple<float, string> ();
		
		mHasUnitySceneLoaded = false;

		mCurSceneInfo = GameScene.GetInstance (CurSceneId);
		Utils.Assert (mCurSceneInfo != null, "在场景配置表中，没有找到id值为：" + sceneId.ToString () + " 的文件");
		GameMain.Instance.SceneMgr.EnterScene (mCurSceneInfo.unitySceneId, null);
	}

	public void Update(float dt)
	{
		BattleManager.Instance.Update ();
	}

	public void LateUpdate()
	{
		
	}

	#region Unity的静态场景
	void OnUnitySceneLoadProgress(object obj)
	{
		Tupple<int, int> param = obj as Tupple<int, int>;
		UpdateLoadingView ((float)param.item1 / param.item2 * mUnityScenePerTop, "加载中");
	}

	void OnUnitySceneLoaded(object obj)
	{
		mHasUnitySceneLoaded = true;
		SceneCameraManager.Instance.OnEnterScene ();
		EnterBattle ();
		GameMain.Instance.UIMgr.CleseView (UIViewId.Loading);
	}

	void EnterBattle()
	{
		List<FighterConfigData> fighters = new List<FighterConfigData>();
		List<BattleTest> testGdsList = BattleTest.GetAllList ();
		for (int i = 0; i < testGdsList.Count; ++i) {
			FighterConfigData fighterData = new FighterConfigData ();
			BattleTest gdsInfo = testGdsList [i];
			fighterData.id = gdsInfo.id;
			fighterData.borthPos.x = gdsInfo.borthPos.x;
			fighterData.borthPos.y = gdsInfo.borthPos.y;
			fighterData.camp = gdsInfo.camp;
			fighterData.life = gdsInfo.life;
			fighterData.speed = gdsInfo.speed;
			fighterData.attack = gdsInfo.attack;
			fighterData.attackRange = gdsInfo.attackRange;
			fighterData.skillList = gdsInfo.skillList.ToArray();
			fighters.Add (fighterData);
		}
		BattleManager.Instance.StartBattle (fighters);
	}
	#endregion

	void UpdateLoadingView(float progress, string tipesKey)
	{
		mLoadingEventParam.item1 = progress;
		mLoadingEventParam.item2 = GameMain.Instance.LanguageMgr.GetLanguage (tipesKey);
		GameMain.Instance.EventMgr.PostObjectEvent (EventId.LoadingViewTips, mLoadingEventParam);
	}
}
