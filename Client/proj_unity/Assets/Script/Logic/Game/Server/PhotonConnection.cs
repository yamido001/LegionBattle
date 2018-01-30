using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using LBCSCommon;

public class PhotonConnection : IPhotonPeerListener {

	public enum ConnectionState
	{
		Null,
		Idle,
		Connecting,
		Connected,
		Disconnect,
	}

	public class ResponseMessageData
	{
		public RpId responseId;
		public ResponseBase responseData;

		public ResponseMessageData(RpId responseId, ResponseBase responseData)
		{
			this.responseId = responseId;
			this.responseData = responseData;
		}
	}

	public delegate ResponseBase ReponseDeserialization(Dictionary<byte, object> parameters);

	PhotonPeer mPeer;
	List<ResponseMessageData> mReponseMessageList = new List<ResponseMessageData>();
	Dictionary<RpId, ReponseDeserialization> mDeserializationFuncDic = new Dictionary<RpId, ReponseDeserialization>();

	public PhotonConnection()
	{
		MessageList = new List<ResponseMessageData> ();
		ConnectState = ConnectionState.Idle;
		RegistReponseType (RpId.LoginResult, RpLoginResult.Deserialization);
		RegistReponseType (RpId.CreateAccountResult, RpCreateAccountResult.Deserialization);
		RegistReponseType (RpId.CreateRoom, RpCreateRoom.Deserialization);
		RegistReponseType (RpId.EnterRoom, RpEnterRoom.Deserialization);
		RegistReponseType (RpId.LeaveRoom, RpLeaveRoom.Deserialization);
		RegistReponseType (RpId.RoomAccountInfo, RpRoomMemberInfo.Deserialization);
		RegistReponseType (RpId.AllMemberReady, RpEmpty.Deserialization);
		RegistReponseType (RpId.PlayerLogin, RpPlayerLogin.Deserialization);
		RegistReponseType (RpId.EnterScene, RpEnterScene.Deserialization);
		RegistReponseType (RpId.BattleInstruction, RpBattleInstructionList.Deserialization);
		PhotonPeer.RegisterType (typeof(RpPlayerData), (byte)ProtocolSerializeType.AccountData, RpPlayerData.Serialize, RpPlayerData.Deserialize);
	}

	public ConnectionState ConnectState {
		get;
		private set;
	}

	public List<ResponseMessageData> MessageList {
		get;
		private set;
	}

	public void Connect(string ip, int port)
	{
		Logger.LogInfo ("连接服务器 " + ip + " " + port);
		if (null == mPeer) {
			mPeer = new PhotonPeer (this, ConnectionProtocol.Udp);
		}
		mPeer.Connect (ip + ":" + port, "LegionBattle");
		ConnectState = ConnectionState.Connecting;
	}

	public void Disconnect()
	{
		mPeer.Disconnect ();
		mPeer = null;
		ConnectState = ConnectionState.Idle;
	}

	public void Update()
	{
		mPeer.Service ();
		MessageList.AddRange (mReponseMessageList);
		mReponseMessageList.Clear ();
	}

	public void SendMessage(RqId requestId, IRequest request)
	{
		mPeer.OpCustom ((byte)requestId, request.Serialization (), true);
	}

	public void RegistReponseType(RpId rpId, ReponseDeserialization reponseDeserial)
	{
		if (mDeserializationFuncDic.ContainsKey (rpId)) {
			Logger.LogError ("注册重复的消息序列化方法 " + rpId.ToString());
			return;
		}
		mDeserializationFuncDic [rpId] = reponseDeserial;
	}

	public void OnDestroy()
	{
		mPeer.Disconnect ();
	}

	/// <summary>
	/// 为网路底层准备的，逻辑层不能调用
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnEvent (EventData eventData)
	{
		RpId rpId = (RpId)eventData.Code;
		ReponseDeserialization deserializationFunc = null;

		if (mDeserializationFuncDic.TryGetValue (rpId, out deserializationFunc)) {
			Logger.LogInfo ("收到消息 " + rpId);
			ResponseBase responseMsg = deserializationFunc.Invoke (eventData.Parameters);
			if (null != responseMsg)
				mReponseMessageList.Add (new ResponseMessageData(rpId, responseMsg));
			else
				Logger.LogError ("消息解析失败 " + rpId);
		} else {
			Logger.LogError ("收到未注册的消息 " + rpId.ToString());
		}
	}

	/// <summary>
	/// 为网路底层准备的，逻辑层不能调用
	/// </summary>
	/// <param name="statusCode">Status code.</param>
	public void OnStatusChanged(StatusCode statusCode)
	{
		switch (statusCode) {
		case StatusCode.Connect:
			ConnectState = ConnectionState.Connected;
			break;
		case StatusCode.Disconnect:
		case StatusCode.ExceptionOnConnect:
			ConnectState = ConnectionState.Disconnect;
			break;
		default:
			{
				Logger.LogError ("未处理的网络连接状态 " + statusCode.ToString());
			}
			break;
		}
	}

	/// <summary>
	/// 为网路底层准备的，逻辑层不能调用
	/// </summary>
	/// <param name="level">Level.</param>
	/// <param name="message">Message.</param>
	public void DebugReturn (DebugLevel level, string message)
	{

	}

	/// <summary>
	/// 为网路底层准备的，逻辑层不能调用
	/// </summary>
	/// <param name="operationResponse">Operation response.</param>
	public void OnOperationResponse (OperationResponse operationResponse)
	{

	}
}
