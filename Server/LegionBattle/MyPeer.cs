using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using LBCSCommon;
using LegionBattle.Message;
using System.Collections.Generic;
using LegionBattle.ServerCommon;

namespace LegionBattle
{
    public class MyPeer : ClientPeer
    {
        const string LogTag = "MyPeer";
        public MyPeer(InitRequest initRequest) : base(initRequest)
        {
            LBLogger.Info(LogTag, "游戏服务器创建连接  " + ConnectionId.ToString());
            LBPeerManager.Instance.AddPeer(ConnectionId, this);
        }

        #region 注册处理回调
        static Dictionary<byte, System.Action<MyPeer, OperationRequest>> mPeerHandlerDic = new Dictionary<byte, System.Action<MyPeer, OperationRequest>>();
        public static void RegisterHandlerFunc(RqId requestId, System.Action<MyPeer, OperationRequest> invokeFunc)
        {
            byte rqIdByte = (byte)requestId;
            if (mPeerHandlerDic.ContainsKey(rqIdByte))
            {
                LBLogger.Error(LogTag, "注册重复的事件处理 " + requestId.ToString());
                return;
            }
            mPeerHandlerDic[rqIdByte] = invokeFunc;
        }
        #endregion

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            LBLogger.Info(LogTag, "连接  关闭 " + ConnectionId.ToString());
            LBPeerManager.Instance.RemovePeer(ConnectionId);
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            System.Action<MyPeer, OperationRequest> invokeAction;
            if(mPeerHandlerDic.TryGetValue(operationRequest.OperationCode, out invokeAction))
            {
                LBLogger.Info(LogTag, "OnOperationRequest " + ((RqId)operationRequest.OperationCode).ToString());
                invokeAction.Invoke(this, operationRequest);
            }
            else
            {
                LBLogger.Error(LogTag, "消息处理没有注册 " + operationRequest.OperationCode);
            }
        }

        protected override void OnMessage(object message, SendParameters sendParameters)
        {
            LBLogger.Info(LogTag, "连接  OnMessage");
            base.OnMessage(message, sendParameters);
        }

        public void SendCustomEvent(RpId rpId, Dictionary<byte, object> parameters)
        {
            EventData loginResult = new EventData((byte)rpId, parameters);
            //LBLogger.Info(LogTag, ConnectionId + " 向游戏前端发送数据 " + rpId.ToString());
            SendEvent(loginResult, new SendParameters());
        }
    }
}
