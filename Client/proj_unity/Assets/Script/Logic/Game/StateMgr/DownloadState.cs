using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadState : StateBase {

	public override void OnInit ()
	{
		if (Lancher.Instance != null)
			Lancher.Instance.SetTips ("进入DownloadState状态,等待游戏的初始化");
		base.OnInit ();
	}

	public override void OnEntered ()
	{
		base.OnEntered ();
		GameMain.Instance.EventMgr.RegisterObjectEvent (EventId.DownLoadFinish, this, delegate(object param) {
			Lancher.Instance.SetTips("加载中.");
			GameMain.Instance.ResMgr.OnResourceUpdate();
		});
		GameMain.Instance.EventMgr.RegisterObjectEvent (EventId.ResourceManagerReUpdateFinish, this, delegate(object param) {
			Lancher.Instance.SetTips("加载中..");
			GameMain.Instance.UIMgrInit();
		});
		GameMain.Instance.EventMgr.RegisterObjectEvent (EventId.UIManagerInitFinish, this, delegate(object param) {
			Lancher.Instance.SetTips("加载中...");
			GameMain.Instance.FileDepInit(delegate() {
				Lancher.Instance.SetTips("加载中....");
				GameMain.Instance.UIMgr.OpenView(UIViewId.MainView, null, delegate() {
					Lancher.Instance.RemoveLancher();
				});
			});
		});
		GameMain.Instance.FileMgr.BeginDownload (null);
	}

	public override void OnUpdate ()
	{
		base.OnUpdate ();
	}

	public override void OnBeforeExit ()
	{
		base.OnBeforeExit ();
	}

	public override void OnDestroy ()
	{
		GameMain.Instance.EventMgr.RemoveObjectEvent (EventId.DownLoadFinish, this);
		GameMain.Instance.EventMgr.RemoveObjectEvent (EventId.ResourceManagerReUpdateFinish, this);
		GameMain.Instance.EventMgr.RemoveObjectEvent (EventId.UIManagerInitFinish, this);
		base.OnDestroy ();
	}

}
