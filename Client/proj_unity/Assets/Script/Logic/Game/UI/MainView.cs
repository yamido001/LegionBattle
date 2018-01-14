using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainView : UIViewBase {

	Text mTitle;
	Button mBtnPlayGame;

	public override void OnOpend (object openParam)
	{
		base.OnOpend (openParam);
		mTitle = PrefabTf.Find ("textTitle").GetComponent<Text>();
		mBtnPlayGame = PrefabTf.Find ("btnNewGame").GetComponent<Button> ();
		EventTriggerListener.Get (mBtnPlayGame.gameObject).onClick = OnPlayClicked;

		mTitle.text = "哈哈主界面";
	}

	public override void OnClose ()
	{
		base.OnClose ();
	}

	void OnPlayClicked(GameObject obj)
	{
		mBtnPlayGame.interactable = false;
		GameMain.Instance.UIMgr.OpenView (UIViewId.Loading, null, delegate() {
			GameMain.Instance.UIMgr.CleseView (ViewId);
			GameMain.Instance.StateMgr.EnterState(GameStateId.Playing, null);
		});
	}
}
