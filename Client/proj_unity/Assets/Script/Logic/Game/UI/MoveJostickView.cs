using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LegionBattle.ServerClientCommon;
using UnityEngine.EventSystems;

public class MoveJostickView : UIViewBase {

	JostickController mJostickController;
	int mTimerId;
	bool mIsDirty = false;

	public override void OnOpend ()
	{
		base.OnOpend ();
		mJostickController = new JostickController ();
		mJostickController.Init (PrefabTf.Find ("SimpleTouchJoystick"), delegate() {
			mIsDirty = true;
		});

		mTimerId = TimerManager.Instance.RepeatCall (ulong.MaxValue, delegate() {
			if(!mIsDirty)
				return;
			if(mJostickController.touchPresent)
			{
				GameBattle.LogicLayer.OperatorManager.Instance.MoveJoystick(mJostickController.movementVector);
			}
			else
			{
				GameBattle.LogicLayer.OperatorManager.Instance.StopMove();
			}
			mIsDirty = false;
		});
	}

	public override void OnClose ()
	{
		base.OnClose ();
		mJostickController.Destroy ();
		TimerManager.Instance.DestroyTimer (mTimerId);
	}
}
