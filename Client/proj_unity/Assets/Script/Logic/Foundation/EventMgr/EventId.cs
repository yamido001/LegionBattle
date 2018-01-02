using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventId{
	DownLoadFinish,
	StreamingCopyFinish,
	ResourceManagerInitFinish,
	ResourceManagerReUpdateFinish,
	UIManagerInitFinish,
	GameSceneMgrInitFinish,
	UnitySceneLoadingInfo,
	UnitySceneLoaded,
	LoadingViewTips,
	SocketConnected,
	SocketDisconnect,
	LoginError,
	LoginSuccess,
	PlayerLogin,
	CreateAccountError,
	CreateAccountSuccess,
	CreateRoomSuccess,
	CreateRoomError,
	EnterRoomSuccess,
	EnterRoomError,
	LeaveRoomSuccess,
	LeaveRoomError,
	RoomMemberInfoRefresh,
	AllMemberReady,
	GameBegin,
}
