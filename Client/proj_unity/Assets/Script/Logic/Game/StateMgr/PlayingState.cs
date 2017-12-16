using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState : StateBase {

	public override void OnInit ()
	{
		base.OnInit ();
	}

	public override void OnEntered ()
	{
		base.OnEntered ();
		GameMain.Instance.GameSceneMgr.EnterScene (1);
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
}
