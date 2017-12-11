using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : StateMechineBase{

	protected override void OnInit ()
	{
		base.OnInit ();
	}

	protected override void OnRegisterState ()
	{
		RegisterState ((int)GameStateId.StartUp, typeof(StartUpState));
		RegisterState ((int)GameStateId.Download, typeof(DownloadState));
		RegisterState ((int)GameStateId.Playing, typeof(PlayingState));
	}

	public void EnterState(GameStateId stateId, object param)
	{
		Enter ((int)stateId, param);
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
	}

	protected override void OnUpdate ()
	{
		base.OnUpdate ();
	}
}
