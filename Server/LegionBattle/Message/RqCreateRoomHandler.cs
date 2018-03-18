using LBCSCommon;
using Photon.SocketServer;
using LegionBattle.Room;
using LegionBattle.Player;

namespace LegionBattle.Message
{
    public class RqCreateRoomHandler : RqCreateRoom
    {
        const string LogTag = "RqLoginHandler";
        public RqCreateRoomHandler(string roomName)
                : base(roomName)
        {

        }

        public static void OnOperateRequest(MyPeer peer, OperationRequest operationRequest)
        {
            RqCreateRoom rqCreateRoom = Deserialization(operationRequest.Parameters);
            if(null == rqCreateRoom)
            {
                LBLogger.Info(LogTag, "请求创建房间，消息解析失败");
                peer.SendCustomEvent(RpId.CreateRoom, RpCreateRoom.Serialization(false, CommonDefine.InvalidRoomId, string.Empty));
            }
            else
            {
                LBPlayer curPlayer = LBPlayerManager.Instance.GetPlayerByConnectionId(peer.ConnectionId);
                if(null == curPlayer)
                {
                    LBLogger.Info(LogTag, "请求创建房间，账号未登陆，连接ID：" + peer.ConnectionId);
                    peer.SendCustomEvent(RpId.CreateRoom, RpCreateRoom.Serialization(false, CommonDefine.InvalidRoomId, string.Empty));
                }
                else
                {
                    if(LBRoomManager.Instance.IsPlayerInRoom(curPlayer.PlayerId))
                    {
                        LBLogger.Info(LogTag, "无法创建，因为当前已经处于房间");
                        peer.SendCustomEvent(RpId.CreateRoom, RpCreateRoom.Serialization(false, CommonDefine.InvalidRoomId, string.Empty));
                    }
                    else
                    {
                        int roomId = LBRoomManager.Instance.CreateRoom(rqCreateRoom.RoomName);
                        if (CommonDefine.InvalidRoomId == roomId)
                        {
                            LBLogger.Info(LogTag, "创建房间失败");
                            peer.SendCustomEvent(RpId.CreateRoom, RpCreateRoom.Serialization(false, CommonDefine.InvalidRoomId, string.Empty));
                        }
                        else
                        {
                            LBLogger.Info(LogTag, "创建房间成功 " + roomId);
                            peer.SendCustomEvent(RpId.CreateRoom, RpCreateRoom.Serialization(true, roomId, rqCreateRoom.RoomName));
                        }
                    }
                }
            }
        }
    }
}
