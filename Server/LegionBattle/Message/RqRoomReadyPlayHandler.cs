using LBCSCommon;
using Photon.SocketServer;
using LegionBattle.Player;
using LegionBattle.Room;

namespace LegionBattle.Message
{
    class RqRoomReadyPlayHandler
    {
        public static void OnOperateRequest(MyPeer peer, OperationRequest operationRequest)
        {
            LBPlayer curPlayer = LBPlayerManager.Instance.GetPlayerByConnectionId(peer.ConnectionId);
            if (curPlayer == null)
                return;
            if(LBRoomManager.Instance.PlayerRoomReady(curPlayer.PlayerId))
            {
                
            }
            else
            {
                LBLogger.Info("RqRoomReadyPlayHandler", "发起准备，失败 " + peer.ConnectionId);
            }
        }
    }
}
