using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : UIViewBase {

	Text mTip;
	Button mBtnPlayGame;

	public override void OnOpend ()
	{
		base.OnOpend ();

		mTip = PrefabTf.Find ("textTip").GetComponent<Text>();
		mTip.text = string.Empty;
		AddObjectEventListener(EventId.LoadingViewTips, delegate(object param) {
			Tupple<float, string> curProcess = param as Tupple<float, string>;
			mTip.text = curProcess.item2 + "(" + curProcess.item1.ToString() + "%)";
		});
	}
}
