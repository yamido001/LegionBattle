using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UIManager{

	class ViewConfigInfo
	{
		public string prefabName;
		public System.Type viewType;
	}

	private static string PrefabName = "UIManager";

	private Dictionary<UIViewId, ViewConfigInfo> mViewConfig = new Dictionary<UIViewId, ViewConfigInfo> ();
	private Dictionary<UIViewId, UIViewBase> mOpendViewDic = new Dictionary<UIViewId, UIViewBase>();
	private Dictionary<UIViewId, object> mOpeningViewDic = new Dictionary<UIViewId, object>();
	private HashSet<UIViewId> mWaitCloseingSet = new HashSet<UIViewId> ();
	private Transform mViewRoot;

	public void Init()
	{
		RegisterUIView ();
		GameMain.Instance.ResMgr.LoadResourceAsync (this, PrefabName, typeof(GameObject), delegate(Object obj) {
			GameObject uiMgrGameObject = GameObject.Instantiate<GameObject>(obj as GameObject);
			uiMgrGameObject.transform.SetParent(GameMain.Instance.Tf);
			uiMgrGameObject.transform.Reset();
			mViewRoot = uiMgrGameObject.transform.Find("View");
			GameMain.Instance.EventMgr.PostObjectEvent (EventId.UIManagerInitFinish, null);
		}, delegate(string obj) {
			Logger.LogError("UIManager load prefag error");	
		});
	}

	public void RegisterView(UIViewId viewId, string prefabName, System.Type viewType)
	{
		ViewConfigInfo configInfo = new ViewConfigInfo ();
		configInfo.prefabName = prefabName;
		configInfo.viewType = viewType;
		if (mViewConfig.ContainsKey (viewId)) {
			Logger.LogError ("UIManager register same viewId:" + viewId.ToString ());
		} 
		else {
			mViewConfig.Add (viewId, configInfo);
		}
	}

	public void OpenView(UIViewId viewId, object param = null, System.Action hdlOnSuccess = null)
	{
		Logger.LogInfo ("打开界面: " + viewId.ToString());
		ViewConfigInfo configInfo = null;
		if (!mViewConfig.TryGetValue (viewId, out configInfo)) {
			Logger.LogError ("UIManager open not registered view:" + viewId.ToString());
			return;
		}

		if (mOpendViewDic.ContainsKey (viewId)) {
			Logger.LogError ("UIManager open opened view,viewId:" + viewId.ToString());
			return;
		}

		if (mOpeningViewDic.ContainsKey (viewId)) {
			mOpeningViewDic [viewId] = param;
			return;
		}

		mOpeningViewDic.Add (viewId, param);
		GameMain.Instance.ResMgr.LoadResourceAsync (this, configInfo.prefabName, typeof(GameObject), delegate(Object prefab) {
			mOpeningViewDic.Remove(viewId);
			if(mWaitCloseingSet.Contains(viewId))
			{
				GameMain.Instance.ResMgr.UnloadResource(configInfo.prefabName, typeof(GameObject));
				mWaitCloseingSet.Remove(viewId);
			}
			else
			{
				UIViewBase viewBase = System.Activator.CreateInstance(mViewConfig[viewId].viewType) as UIViewBase;
				viewBase.Open(viewId, prefab as GameObject, mViewRoot);
				mOpendViewDic.Add(viewId, viewBase);
				if(null != hdlOnSuccess)
					hdlOnSuccess.Invoke();
			}
		}, delegate(string errorCode) {
			Logger.LogError("UIManager load " + configInfo.prefabName + " error, code:" + errorCode);
		});
	}

	public void CleseView(UIViewId viewId)
	{
		if (mOpendViewDic.ContainsKey (viewId)) {
			UIViewBase viewBase = mOpendViewDic [viewId];
			viewBase.Close ();
			ViewConfigInfo configInfo = mViewConfig [viewId];
			GameMain.Instance.ResMgr.UnloadResource (configInfo.prefabName, typeof(GameObject));
		} 
		else if (mOpendViewDic.ContainsKey (viewId)) {
			if (!mWaitCloseingSet.Contains (viewId)) {
				mWaitCloseingSet.Add (viewId);
			}
		} 
		else {
			Logger.LogWarning ("UIManager close not opend or opening view: " + viewId.ToString());
		}
	}

	public void Destroy()
	{

		var openedEnumerator = mOpendViewDic.GetEnumerator ();
		while (openedEnumerator.MoveNext ()) {
			
		}
		mViewConfig.Clear ();
		GameMain.Instance.ResMgr.UnloadResource (PrefabName, typeof(GameObject));
	}
}
