using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LBCSCommon;

public class SocketManager{

	PhotonConnection mConnection;
	PhotonConnection.ConnectionState mLastConnectionState = PhotonConnection.ConnectionState.Null;
	Dictionary<RpId, System.Action<ResponseBase>> mResponseDic = new Dictionary<RpId, System.Action<ResponseBase>>();

	public void Connect(string ip, int port)
	{
		if (null == mConnection) {
			mConnection = new PhotonConnection ();
		}
		mConnection.Connect (ip, port);
	}

	public void RegistResponseListener(RpId responseId, System.Action<ResponseBase> action)
	{
		if (mResponseDic.ContainsKey (responseId)) {
			Logger.LogError ("注册重复的消息处理");
			return;
		}
		mResponseDic.Add (responseId, action);
	}

	public void RemoveResponseListener(RpId responseId, System.Action<ResponseBase> action)
	{
		mResponseDic.Remove (responseId);
	}

	public void Update()
	{
		if (null == mConnection)
			return;
		mConnection.Update ();
		DealWith ();
		PhotonConnection.ConnectionState curConnectionState = mConnection.ConnectState;
		if (curConnectionState == mLastConnectionState)
			return;
		mLastConnectionState = curConnectionState;
		OnConnectionStateChange ();
	}

	public void SendMessage(RqId requestId, IRequest request)
	{
		if (null == mConnection) {
			Logger.LogError ("当前没有连接，无法发送消息 " + requestId.ToString());
			return;
		}
		mConnection.SendMessage (requestId, request);
	}

	public void OnDestroy()
	{
		if (null == mConnection)
			return;
		mConnection.OnDestroy ();
	}

	void DealWith()
	{
		for (int i = 0; i < mConnection.MessageList.Count; ++i) {
			PhotonConnection.ResponseMessageData reponseMsg = mConnection.MessageList [i];
			System.Action<ResponseBase> actionFunc = null;
			if (mResponseDic.TryGetValue (reponseMsg.responseId, out actionFunc)) {
				actionFunc.Invoke (reponseMsg.responseData);
			} else {
				Logger.LogInfo ("收到消息，但是没有监听 " + reponseMsg.responseId);
			}
		}
		mConnection.MessageList.Clear ();
	}

	void OnConnectionStateChange()
	{
		switch (mLastConnectionState) {
		case PhotonConnection.ConnectionState.Connected:
			GameMain.Instance.EventMgr.PostObjectEvent (EventId.SocketConnected, null);
			break;
		case PhotonConnection.ConnectionState.Connecting:
			break;
		case PhotonConnection.ConnectionState.Disconnect:
			GameMain.Instance.EventMgr.PostObjectEvent (EventId.SocketDisconnect, null);
			break;
		default:
			break;
		}
	}
}
