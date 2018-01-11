﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JostickController{

	ScrollRect mScroolRect;
	RectTransform joystickArea;
	System.Action mHdlOnChange;

	public bool touchPresent
	{
		get;
		private set;
	}

	public Vector2 movementVector {
		get;
		private set;
	}

	public void Init(Transform scroolTf, System.Action hdlOnChange)
	{
		mHdlOnChange = hdlOnChange;
		mScroolRect = scroolTf.GetComponent<ScrollRect> ();
		joystickArea = scroolTf.Find ("Viewport/Content").GetComponent<RectTransform> ();
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
	}

	public void Destroy()
	{
		mScroolRect.onValueChanged.RemoveListener (OnValueChanged);
	}

	void OnValueChanged(Vector2 value)
	{
		if(touchPresent)
		{
			// convert the value between 1 0 to -1 +1
			movementVector = new Vector2(((1 - value.x) - 0.5f) * 2f, ((1 - value.y) - 0.5f) * 2f);
			mHdlOnChange.Invoke ();
		}
	}

	void BeginDrag(BaseEventData eventData)
	{
		touchPresent = true;
		mHdlOnChange.Invoke ();
	}

	void EndDrag(BaseEventData eventData)
	{
		touchPresent = false;
		movementVector = joystickArea.anchoredPosition = Vector2.zero;
		mHdlOnChange.Invoke ();
	}
}