using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataProxy{

	bool mHasInit = false;
	public void Init()
	{
		if (mHasInit) {
			Logger.LogError (this.GetType() + "初始化多次");
			return;
		}
		OnInit ();
		mHasInit = true;
	}

	public void Destroy()
	{
		if(mHasInit)
			OnDestroy ();
	}

	protected abstract void OnInit ();
	protected abstract void OnDestroy ();
}
