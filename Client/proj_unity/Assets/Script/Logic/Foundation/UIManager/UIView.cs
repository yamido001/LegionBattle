using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIViewBase{

	protected GameObject PrefabGO {
		get;
		private set;
	}

	protected Transform PrefabTf {
		get;
		private set;
	}

	protected UIViewId ViewId {
		get;
		private set;
	}

	HashSet<EventId> mAddedObjectEventSet;

	/// <summary>
	/// 只能被UIMgr调用
	/// </summary>
	/// <param name="viewId">View identifier.</param>
	/// <param name="prefab">Prefab.</param>
	/// <param name="parentTf">Parent tf.</param>
	public void Open(UIViewId viewId, GameObject prefab, Transform parentTf, object param)
	{
		ViewId = viewId;
		PrefabGO = GameObject.Instantiate(prefab);
		PrefabTf = PrefabGO.transform;
		PrefabTf.SetParent(parentTf);
		PrefabTf.Reset ();
		OnOpend (param);
	}

	/// <summary>
	/// 只能被UIMgr调用
	/// </summary>
	public void Close()
	{
		RemoveAllEventListener ();
		OnClose ();
		GameObject.Destroy (PrefabGO);
	}

	protected void AddObjectEventListener(EventId eventId, System.Action<object> hdl)
	{
		if (null == mAddedObjectEventSet) {
			mAddedObjectEventSet = new HashSet<EventId> ();
		}
		if (mAddedObjectEventSet.Contains (eventId)) {
			Logger.LogError (ViewId.ToString() + " 注册重复的事件监听，" + eventId.ToString());
			return;
		}
		mAddedObjectEventSet.Add (eventId);
		GameMain.Instance.EventMgr.RegisterObjectEvent (eventId, this, hdl);
	}

	void RemoveAllEventListener()
	{
		if (null == mAddedObjectEventSet)
			return;
		var enumerator = mAddedObjectEventSet.GetEnumerator ();
		while (enumerator.MoveNext ()) {
			GameMain.Instance.EventMgr.RemoveObjectEvent (enumerator.Current, this);
		}
	}

	#region 需要子类继承的函数
	public virtual void OnOpend(object param)
	{
		
	}

	public virtual void OnClose()
	{
		
	}
	#endregion

}
