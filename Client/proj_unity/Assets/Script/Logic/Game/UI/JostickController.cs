using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JostickController{

	ScrollRect mScroolRect;
	RectTransform joystickArea;
	System.Action mHdlOnChange;
    System.Action mHdlOnBegin;
    System.Action mHdlOnEnd;

	public bool touchPresent
	{
		get;
		private set;
	}

	public Vector2 movementVector {
		get;
		private set;
	}

	public void Init(Transform scroolTf, System.Action hdlOnChange, System.Action hdlOnBegin, System.Action hdlOnEnd)
	{
		mHdlOnChange = hdlOnChange;
        mHdlOnBegin = hdlOnBegin;
        mHdlOnEnd = hdlOnEnd;
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
            Vector2 centerPos = new Vector2(0.5f, 0.5f);
            Vector2 offset = centerPos - value;
            if(offset.magnitude > 0.5f)
            {
                offset = offset.normalized * 0.5f;
                mScroolRect.normalizedPosition = centerPos - offset;
            }
			movementVector = offset * 2f;
            if(null != mHdlOnChange)
			    mHdlOnChange.Invoke ();
		}
	}

	void BeginDrag(BaseEventData eventData)
	{
		touchPresent = true;
        if(null != mHdlOnBegin)
		    mHdlOnBegin.Invoke ();
	}

	void EndDrag(BaseEventData eventData)
	{
		touchPresent = false;
        if (null != mHdlOnEnd)
            mHdlOnEnd.Invoke();
        mScroolRect.normalizedPosition = new Vector2(0.5f, 0.5f);
        movementVector = Vector2.zero;
	}
}
