using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : UIViewBase {

	Text mTip;

	public override void OnOpend (object openParam)
	{
		base.OnOpend (openParam);

		mTip = PrefabTf.Find ("textTip").GetComponent<Text>();
		mTip.text = string.Empty;
		AddObjectEventListener(EventId.LoadingViewTips, delegate(object eventParam) {
			Tupple<float, string> curProcess = eventParam as Tupple<float, string>;
			mTip.text = curProcess.item2 + "(" + curProcess.item1.ToString() + "%)";
		});
	}
}
