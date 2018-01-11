using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LegionBattle.ServerClientCommon;
using UnityEngine.EventSystems;

public class SkillView : UIViewBase {

	ScrollRect mScroolRect;
	RectTransform joystickArea;

	bool touchPresent = false;
	Vector2 movementVector;
	int mTimerId;
	bool mIsDirty = false;

	public override void OnOpend ()
	{
		base.OnOpend ();
		mScroolRect = PrefabTf.Find ("SimpleTouchJoystick").GetComponent<ScrollRect> ();
		joystickArea = PrefabTf.Find ("SimpleTouchJoystick/Viewport/Content").GetComponent<RectTransform> ();
		EventTrigger eventTrigger = mScroolRect.GetComponent<EventTrigger> ();

		mScroolRect.onValueChanged.AddListener (OnValueChanged);

		EventTrigger.Entry onDragEntry = new EventTrigger.Entry ();
		onDragEntry.eventID = EventTriggerType.BeginDrag;
		onDragEntry.callback.AddListener(BeginDrag);
		eventTrigger.triggers.Add (onDragEntry);

		EventTrigger.Entry endDragEntry = new EventTrigger.Entry ();
		endDragEntry.eventID = EventTriggerType.EndDrag;
		endDragEntry.callback.AddListener (EndDrag);
		eventTrigger.triggers.Add (endDragEntry);

		mTimerId = TimerManager.Instance.RepeatCall (ulong.MaxValue, delegate() {
			if(!mIsDirty)
				return;
			if(touchPresent)
			{
				GameBattle.LogicLayer.OperatorManager.Instance.MoveJoystick(movementVector);
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
		mScroolRect.onValueChanged.RemoveListener (OnValueChanged);
		TimerManager.Instance.DestroyTimer (mTimerId);
	}


	void BeginDrag(BaseEventData eventData)
	{
		touchPresent = true;
		mIsDirty = true;
	}

	void EndDrag(BaseEventData eventData)
	{
		touchPresent = false;
		movementVector = joystickArea.anchoredPosition = Vector2.zero;
		mIsDirty = true;
	}

	void OnValueChanged(Vector2 value)
	{
		if(touchPresent)
		{
			// convert the value between 1 0 to -1 +1
			movementVector.x = ((1 - value.x) - 0.5f) * 2f;
			movementVector.y = ((1 - value.y) - 0.5f) * 2f;
			mIsDirty = true;
		}
	}

}
