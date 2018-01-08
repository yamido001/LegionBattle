using System;
using System.Collections.Generic;

/// <summary>
/// 状态机基类
/// </summary>
public abstract class PreCreateStateMechine{

	private PreCreateStateBase mCurState;
	private Dictionary<int ,PreCreateStateBase> mStateTypeDic = new Dictionary<int, PreCreateStateBase> ();

	#region 公有函数，供外部调用
	public void Init()
	{
		OnInit ();
		OnRegisterState ();
	}

	public void Enter(int stateId, object param)
	{
		if (null != mCurState) {
			mCurState.Exit();
			mCurState = null;
		}
		if (!mStateTypeDic.ContainsKey (stateId)) {
			Logger.LogError("State mechine enter not registered state, id is " + stateId.ToString());
			return;
		}
		mCurState = mStateTypeDic [stateId];
		mCurState.Entered(param);
	}

	public void Execute()
	{
		if (null != mCurState) {
			mCurState.Execute ();
		}
	}

	public void Destroy()
	{
		if (null != mCurState) {
			mCurState.Exit ();
		}
		var stateDicEnumerator = mStateTypeDic.GetEnumerator ();
		while (stateDicEnumerator.MoveNext ()) {
			stateDicEnumerator.Current.Value.Destroy ();
		}
		mStateTypeDic.Clear ();
		mStateTypeDic = null;
		OnDestroy ();
	}
	#endregion

	#region 内部逻辑
	protected void RegisterState(int stateId,  PreCreateStateBase stateObj)
	{
		mStateTypeDic [stateId] = stateObj;
	}
	#endregion

	#region 需要继承的函数
	protected abstract void OnInit();

	protected abstract void OnRegisterState ();

	protected abstract void OnDestroy ();
	#endregion
}
