using System;
using System.Collections.Generic;

/// <summary>
/// 状态机基类，内部在切换状态时用到了反射，状态退出时会释放，状态切换过于频繁时慎用
/// </summary>
public class StateMechineBase{

	private StateBase mCurState;
	private Dictionary<int ,Type> mStateTypeDic = new Dictionary<int, Type> ();

	#region 公有函数，供外部调用
	public void Init()
	{
		OnInit ();
		OnRegisterState ();
	}



	public void Enter(int stateId, object param)
	{
		if (null != mCurState) {
			mCurState.OnBeforeExit();
		}
		if (!mStateTypeDic.ContainsKey (stateId)) {
			Logger.LogError("State mechine enter not registered state, id is " + stateId.ToString());
			return;
		}
		StateBase newState = System.Activator.CreateInstance(mStateTypeDic [stateId]) as StateBase;
		newState.OnInit();
		if(null != mCurState)
		{
			mCurState.OnBeforeExit();
		}
		newState.OnEntered();
		if (null != mCurState) {
			mCurState.OnDestroy();
		}
		mCurState = newState;
	}

	public void Update()
	{
		OnUpdate ();
		if (null != mCurState) {
			mCurState.OnUpdate ();
		}
	}

	public void Destroy()
	{
		if (null != mCurState) {
			mCurState.OnUpdate ();
		}
		OnDestroy ();
	}
	#endregion

	#region 内部逻辑
	protected void RegisterState(int stateId,  Type stateType)
	{
		mStateTypeDic [stateId] = stateType;
	}
	#endregion

	#region 需要继承的函数
	protected virtual void OnInit()
	{
		
	}

	protected virtual void OnRegisterState()
	{
		
	}

	protected virtual void OnUpdate()
	{
		
	}

	protected virtual void OnDestroy()
	{
		
	}
	#endregion
}
