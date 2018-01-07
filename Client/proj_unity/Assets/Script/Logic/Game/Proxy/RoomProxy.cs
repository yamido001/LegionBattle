using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LegionBattle.ServerClientCommon;

public class RoomProxy : DataProxy {

	protected override void OnInit ()
	{
		GameMain.Instance.SocketMgr.RegistResponseListener (RpId.CreateRoom, OnCreateRoomResponse);
		GameMain.Instance.SocketMgr.RegistResponseListener (RpId.EnterRoom, OnEnterRoomResponse);
		GameMain.Instance.SocketMgr.RegistResponseListener (RpId.LeaveRoom, OnLeaveRoomResponse);
		GameMain.Instance.SocketMgr.RegistResponseListener (RpId.RoomAccountInfo, OnRoomMemberInfoResponse);
		GameMain.Instance.SocketMgr.RegistResponseListener (RpId.AllMemberReady, OnAllMemberReady);
		GameMain.Instance.SocketMgr.RegistResponseListener (RpId.EnterScene, OnGameBegin);
		CurRoomId = CommonDefine.InvalidRoomId;
	}

	protected override void OnDestroy ()
	{
		GameMain.Instance.SocketMgr.RemoveResponseListener (RpId.CreateRoom, OnCreateRoomResponse);
		GameMain.Instance.SocketMgr.RemoveResponseListener (RpId.EnterRoom, OnEnterRoomResponse);
		GameMain.Instance.SocketMgr.RemoveResponseListener (RpId.LeaveRoom, OnLeaveRoomResponse);
		GameMain.Instance.SocketMgr.RemoveResponseListener (RpId.RoomAccountInfo, OnRoomMemberInfoResponse);
		GameMain.Instance.SocketMgr.RemoveResponseListener (RpId.AllMemberReady, OnAllMemberReady);
		GameMain.Instance.SocketMgr.RemoveResponseListener (RpId.EnterScene, OnGameBegin);
	}

	public int CurRoomId
	{
		get;
		private set;
	}

	public string CurRoomName {
		get;
		private set;
	}

	public bool IsInRoom{
		get{
			return CurRoomId != CommonDefine.InvalidRoomId;
		}
	}

	public List<RpPlayerData> RoomAccountList
	{
		get;
		private set;
	}

	#region 服务器范返回消息
	void OnCreateRoomResponse(ResponseBase response)
	{
		RpCreateRoom createRoomResult = response as RpCreateRoom;
		if (createRoomResult.Result) {
			GameMain.Instance.EventMgr.PostObjectEvent (EventId.CreateRoomSuccess, createRoomResult.RoomId);	
		} else {
			GameMain.Instance.EventMgr.PostObjectEvent (EventId.CreateRoomError, null);
		}
	}

	void OnEnterRoomResponse(ResponseBase response)
	{
		RpEnterRoom enterRoomResult = response as RpEnterRoom;
		if (enterRoomResult.Result) {
			CurRoomId = enterRoomResult.RoomId;
			CurRoomName = enterRoomResult.RoomName;
			GameMain.Instance.EventMgr.PostObjectEvent (EventId.EnterRoomSuccess, null);	
		} else {
			GameMain.Instance.EventMgr.PostObjectEvent (EventId.EnterRoomError, null);
		}
	}

	void OnRoomMemberInfoResponse(ResponseBase response)
	{
		RpRoomMemberInfo roomMemberInfo = response as RpRoomMemberInfo;
		RoomAccountList = roomMemberInfo.AccountList;
		GameMain.Instance.EventMgr.PostObjectEvent (EventId.RoomMemberInfoRefresh, null);
	}

	void OnLeaveRoomResponse(ResponseBase response)
	{
		RoomAccountList = null;
		RpLeaveRoom leaveRoomResult = response as RpLeaveRoom;
		if (leaveRoomResult.Result) {
			CurRoomId = CommonDefine.InvalidRoomId;
			GameMain.Instance.EventMgr.PostObjectEvent (EventId.LeaveRoomSuccess, null);
		} else {
			GameMain.Instance.EventMgr.PostObjectEvent (EventId.LeaveRoomError, null);
		}
	}

	void OnAllMemberReady(ResponseBase response)
	{
		GameMain.Instance.EventMgr.PostObjectEvent (EventId.AllMemberReady, null);
	}

	void OnGameBegin(ResponseBase response)
	{
		RpEnterScene enterScene = response as RpEnterScene;
		GameMain.Instance.EventMgr.PostObjectEvent (EventId.GameBegin, enterScene.SceneUnit);
	}
	#endregion

	#region 客户端请求消息
	public void CreateRoom(string roomName)
	{
		RqCreateRoom createRequest = new RqCreateRoom (roomName);
		GameMain.Instance.SocketMgr.SendMessage (RqId.CreateRoom, createRequest);
	}

	public void EnterRoom(int roomId)
	{
		RqEnterRoom enterRoomRequest = new RqEnterRoom (roomId);
		GameMain.Instance.SocketMgr.SendMessage (RqId.EnterRoom, enterRoomRequest);
	}

	public void LeaveRoom()
	{
		RqLeaveRoom leaveRoomRequest = new RqLeaveRoom ();
		GameMain.Instance.SocketMgr.SendMessage (RqId.LeaveRoom, leaveRoomRequest);
	}

	public void ReadyInRoom()
	{
		GameMain.Instance.SocketMgr.SendMessage (RqId.RoomReadyPlay, RqEmpty.StaticRqEmpty);
	}

	public void LoadFinish()
	{
		GameMain.Instance.SocketMgr.SendMessage (RqId.LoadFinish, RqEmpty.StaticRqEmpty);
	}
	#endregion
}
