using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpState : StateBase {

	//下面为执行步骤
	//1.不依赖于文件的各个模块的初始化
	//2.从Streaming目录向Persisten目录拷贝
	//3.跳转到下载状态

	public override void OnInit ()
	{
		if (Lancher.Instance != null)
			Lancher.Instance.SetTips ("进入StartUp状态,等待游戏的初始化");
		//这里之所以不适用EventMgr，是因为这个时候EventMgr还没有初始化完毕 
		GameMain.Instance.NotFileDepInit (delegate() {
			GameMain.Instance.EventMgr.RegisterObjectEvent(EventId.StreamingCopyFinish, this, OnStreamingCopyFinish);
			GameMain.Instance.FileMgr.BeginStreamingCopy();
		});
		base.OnInit ();
	}

	public override void OnEntered ()
	{
		base.OnEntered ();
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
		base.OnDestroy ();
	}
	
	void OnStreamingCopyFinish(object param)
	{
		GameMain.Instance.EventMgr.RemoveObjectEvent (EventId.StreamingCopyFinish, this);
		GameMain.Instance.StateMgr.EnterState (GameStateId.Download, null);
	}
}