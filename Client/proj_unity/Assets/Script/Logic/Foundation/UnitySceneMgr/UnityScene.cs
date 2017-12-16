using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityScene{

	SceneConfigInfo mConfigInfo;
	List<string> mLoadedResList;
	int mCurIndex = 0;
	int mErrorCount = 0;
	Tupple<int, int> mPostEventParam;

	public string SceneId
	{
		private set;
		get;
	}

	public UnityScene(string sceneId)
	{
		SceneId = sceneId;
	}

	public void OnWillEntered()
	{
		
	}

	public void OnEntered()
	{
		GameMain.Instance.ResMgr.LoadResourceAsync (this, SceneId, typeof(TextAsset), delegate(Object obj) {

			TextAsset configAsset = obj as TextAsset;
			mConfigInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<SceneConfigInfo>(configAsset.text);
			mLoadedResList = new List<string>();
			LoadScenePrefab();

		}, delegate(string errorCode) {
			Debug.LogError("加载场景，未找到配置文件：" + SceneId);
		});
	}

	public void OnWillExited()
	{
		
	}

	public void OnExited ()
	{
		if (mLoadedResList != null) {
			for (int i = 0; i < mLoadedResList.Count; ++i) {
				GameMain.Instance.ResMgr.UnloadResource(mLoadedResList[i], typeof(GameObject));
			}
		}
	}

	private void ShowProcessTip()
	{
		if (null == mPostEventParam) {
			mPostEventParam = new Tupple<int, int> ();
		}
		mPostEventParam.item1 = mCurIndex;
		mPostEventParam.item2 = mConfigInfo.prefabList.Count;
		GameMain.Instance.EventMgr.PostObjectEvent (EventId.UnitySceneLoadingInfo, mPostEventParam);
	}

	private void LoadScenePrefab()
	{
		if (null == mConfigInfo)
			return;
		ShowProcessTip ();
		if (mCurIndex + mErrorCount >= mConfigInfo.prefabList.Count) {
			GameMain.Instance.EventMgr.PostObjectEvent (EventId.UnitySceneLoaded, null);
			return;
		}
		ScenePrefabInfo prefabInfo = mConfigInfo.prefabList[mCurIndex];
		GameMain.Instance.ResMgr.LoadResourceAsync (this, prefabInfo.prefabName, typeof(GameObject), delegate(Object prefab) {
			GameObject prefabObj = GameObject.Instantiate(prefab) as GameObject;
			prefabObj.transform.localPosition = prefabInfo.position;
			prefabObj.transform.localScale = prefabInfo.scale;
			prefabObj.transform.localEulerAngles = prefabInfo.eularAngles;
			++mCurIndex;

			mLoadedResList.Add(prefabInfo.prefabName);
			LoadScenePrefab();
		}, delegate(string errorCode) {
			Debug.LogError("加载场景单位失败： " + errorCode);
			++mErrorCount;
		});
	}
}
