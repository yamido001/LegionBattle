using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

public class LoginProxy : DataProxy {

	protected override void OnInit ()
	{
		GameMain.Instance.SocketMgr.RegistResponseListener (RpId.LoginResult, OnLoginResponse);
		GameMain.Instance.SocketMgr.RegistResponseListener (RpId.CreateAccountResult, OnCreateAccountResponse);
	}

	protected override void OnDestroy ()
	{
		GameMain.Instance.SocketMgr.RemoveResponseListener (RpId.LoginResult, OnLoginResponse);
		GameMain.Instance.SocketMgr.RemoveResponseListener (RpId.CreateAccountResult, OnCreateAccountResponse);
	}

	#region 服务器范返回消息
	void OnLoginResponse(ResponseBase response)
	{
		RpLoginResult loginResult = response as RpLoginResult;
		if (loginResult.Result) {
			GameMain.Instance.EventMgr.PostObjectEvent (EventId.LoginSuccess, null);	
		} else {
			GameMain.Instance.EventMgr.PostObjectEvent (EventId.LoginError, loginResult.ErrorCode);
		}
	}

	void OnCreateAccountResponse(ResponseBase response)
	{
		RpCreateAccountResult createAccountResult = response as RpCreateAccountResult;
		if (createAccountResult.Result) {
			GameMain.Instance.EventMgr.PostObjectEvent (EventId.CreateAccountSuccess, null);	
		} else {
			GameMain.Instance.EventMgr.PostObjectEvent (EventId.CreateAccountError, createAccountResult.ErrorCode);
		}
	}
	#endregion

	#region 客户端请求消息
	public void Login(string accountName, string password)
	{
		RqLogin loginRequest = new RqLogin (accountName, password);
		GameMain.Instance.SocketMgr.SendMessage (RqId.Login, loginRequest);
	}

	public void CreateAccount(string accountName, string password)
	{
		RqCreateAccount createAccountRequest = new RqCreateAccount (accountName, password);
		GameMain.Instance.SocketMgr.SendMessage (RqId.CreateAccount, createAccountRequest);
	}
	#endregion
}
