using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

public class PlayerProxy : DataProxy {

	public int PlayerId {
		get;
		private set;
	}

	public string PlayerName {
		get;
		private set;
	}

	protected override void OnInit ()
	{
		GameMain.Instance.SocketMgr.RegistResponseListener (RpId.PlayerLogin, OnPlayerLogin);

		PlayerId = CommonDefine.InvalidPlayerId;
		PlayerName = string.Empty;
	}

	protected override void OnDestroy ()
	{
		GameMain.Instance.SocketMgr.RemoveResponseListener (RpId.PlayerLogin, OnPlayerLogin);
	}

	#region 服务器返回消息
	void OnPlayerLogin(object parameters)
	{
		RpPlayerLogin playerLogin = parameters as RpPlayerLogin;
		if(null == playerLogin)
		{
			return;
		}
		PlayerId = playerLogin.PlayerId;
		PlayerName = playerLogin.PlayerName;
		GameMain.Instance.EventMgr.PostObjectEvent (EventId.PlayerLogin, null);
	}
	#endregion
}
