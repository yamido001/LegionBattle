using LBCSCommon;
using Photon.SocketServer;
using LegionBattle.Room;
using LegionBattle.Player;
using System.Collections.Generic;

namespace LegionBattle.Message
{
    public class RqEnterRoomHandler : RqEnterRoom
    {
        const string LogTag = "RqLoginHandler";
        public RqEnterRoomHandler(int roomId)
                : base(roomId)
        {

        }

        public static void OnOperateRequest(MyPeer peer, OperationRequest operationRequest)
        {
            RqEnterRoom rqEnterRoom = Deserialization(operationRequest.Parameters);
            if(null == rqEnterRoom)
            {
                LBLogger.Info(LogTag, "解析请求进入房间消息失败");
            }
            else
            {
                LBPlayer curPlayer = LBPlayerManager.Instance.GetPlayerByConnectionId(peer.ConnectionId);
                LBLogger.Info(LogTag, "临时日志   " + curPlayer.PlayerId + "  " + curPlayer.ConnectionId);
                if(null == curPlayer)
                {
                    LBLogger.Info(LogTag, "不存在的账号请求进入房间，连接id:" + peer.ConnectionId);
                }
                else
                {
                    if(LBRoomManager.Instance.IsPlayerInRoom(curPlayer.PlayerId))
                    {
                        LBLogger.Info(LogTag, "请求进入房间，但是已经在房间中");
                    }
                    else
                    {
                        if(!LBRoomManager.Instance.PlayerEnterRoom(rqEnterRoom.RoomId, curPlayer.PlayerId))
                        {
                            LBLogger.Info(LogTag, "请求进入房间失败");
                        }
                    }
                }
            }
        }
    }
}
