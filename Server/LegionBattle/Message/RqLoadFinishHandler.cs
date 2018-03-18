using LBCSCommon;
using Photon.SocketServer;
using LegionBattle.Player;
using LegionBattle.SceneServer;

namespace LegionBattle.Message
{
    class RqLoadFinishHandler
    {
        public static void OnOperateRequest(MyPeer peer, OperationRequest operationRequest)
        {
            LBPlayer curPlayer = LBPlayerManager.Instance.GetPlayerByConnectionId(peer.ConnectionId);
            if (null == curPlayer)
            {
                LBLogger.Info("RqLoadFinish", "账号不存在 " + peer.ConnectionId);
                return;
            }
            LBSceneManager.Instance.PlayerLoadFinish(curPlayer.PlayerId);
        }
    }
}
