using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginState : StateBase {

	enum StateId
	{
		Null,
		Connecting,
		Login,
	}

	StateId mCurState = StateId.Null;

	public override void OnInit ()
	{
		if (Lancher.Instance != null)
			Lancher.Instance.SetTips ("进入登录状态,等待游戏的初始化");
		base.OnInit ();
	}

	public override void OnEntered ()
	{
		base.OnEntered ();
		GameMain.Instance.EventMgr.RegisterObjectEvent (EventId.SocketConnected, this, delegate(object obj) {
			OpenLoginView();
		});

		GameMain.Instance.EventMgr.RegisterObjectEvent (EventId.SocketDisconnect, this, delegate(object obj) {
			ConnectSocket();
		});

		GameMain.Instance.EventMgr.RegisterObjectEvent (EventId.PlayerLogin, this, delegate(object obj) {
			GameMain.Instance.UIMgr.OpenView(UIViewId.RoomView, null, delegate {
				GameMain.Instance.UIMgr.CleseView(UIViewId.Login);
			});
		});

		GameMain.Instance.EventMgr.RegisterObjectEvent (EventId.AllMemberReady, this, delegate(object obj) {

			GameMain.Instance.UIMgr.OpenView (UIViewId.Loading, null, delegate() {
				GameMain.Instance.UIMgr.CleseView (UIViewId.RoomView);
				GameMain.Instance.StateMgr.EnterState(GameStateId.Playing, null);
			});
		});

		ConnectSocket ();
	}

	void ConnectSocket()
	{
		SetCurState (StateId.Connecting);
		GameMain.Instance.SocketMgr.Connect("localhost", 5055);
	}

	void OpenLoginView()
	{
		SetCurState (StateId.Login);
		GameMain.Instance.UIMgr.OpenView(UIViewId.Login, null, delegate() {
			Lancher.Instance.RemoveLancher();
		});
	}

	void SetCurState(StateId curState)
	{
		if (mCurState == curState)
			return;
		switch (curState) {
		case StateId.Connecting:
			break;
		case StateId.Login:
			GameMain.Instance.UIMgr.CleseView (UIViewId.Login);
			break;
		default:
			break;
		}
		mCurState = curState;
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
		GameMain.Instance.EventMgr.RemoveIntEvent (EventId.SocketConnected, this);
		GameMain.Instance.EventMgr.RemoveIntEvent (EventId.SocketDisconnect, this);
		base.OnDestroy ();
	}

}
