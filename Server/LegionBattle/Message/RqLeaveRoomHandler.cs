using LBCSCommon;
using Photon.SocketServer;
using LegionBattle.Room;
using LegionBattle.Player;

namespace LegionBattle.Message
{
    public class RqLeaveRoomHandler : RqLeaveRoom
    {
        const string LogTag = "RqLoginHandler";

        public static void OnOperateRequest(MyPeer peer, OperationRequest operationRequest)
        {
            RqLeaveRoom rqEnterRoom = Deserialization(operationRequest.Parameters);
            if(null == rqEnterRoom)
            {
                LBLogger.Info(LogTag, "请求离开Room,解析失败");
                peer.SendCustomEvent(RpId.LeaveRoom, RpLeaveRoom.Serialization(false));
            }
            else
            {
                LBPlayer curPlayer = LBPlayerManager.Instance.GetPlayerByConnectionId(peer.ConnectionId);
                if(null == curPlayer)
                {
                    LBLogger.Info(LogTag, "请求离开Room，当前玩家不存在");
                    peer.SendCustomEvent(RpId.LeaveRoom, RpLeaveRoom.Serialization(false));
                }
                else
                {
                    if(!LBRoomManager.Instance.IsPlayerInRoom(curPlayer.PlayerId))
                    {
                        LBLogger.Info(LogTag, "请求离开房间，但是当前不在房间中 ");
                        peer.SendCustomEvent(RpId.LeaveRoom, RpLeaveRoom.Serialization(false));
                    }
                    else
                    {
                        bool leaveResult = LBRoomManager.Instance.PlayerLeaveRoom(curPlayer.PlayerId);
                        if (leaveResult)
                        {
                            LBLogger.Info(LogTag, "请求离开房间成功");
                            peer.SendCustomEvent(RpId.LeaveRoom, RpLeaveRoom.Serialization(true));
                        }
                        else
                        {
                            LBLogger.Info(LogTag, "请求离开失败");
                            peer.SendCustomEvent(RpId.LeaveRoom, RpLeaveRoom.Serialization(false));
                        }
                    }
                }
            }
        }
    }
}
