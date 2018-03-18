using LBCSCommon;
using Photon.SocketServer;
using LegionBattle.Player;
using LegionBattle.Room;
using LegionBattle.SceneServer;

namespace LegionBattle.Message
{
    class RqBattleInstructionHandler : RqBattleInstruction
    {
        public static void OnOperateRequest(MyPeer peer, OperationRequest operationRequest)
        {
            LBPlayer curPlayer = LBPlayerManager.Instance.GetPlayerByConnectionId(peer.ConnectionId);
            if (curPlayer == null)
            {
                //LBLogger.Error("RqBattleInstructionHandler", "玩家没有登录");
                return;
            }
            byte[] byteArray = RqBattleInstruction.Deserialization(operationRequest.Parameters);
            int index = 0;
            BattleInstructionBase battleInstruction = BattleInstructionBase.Deserializetion(byteArray, ref index);
            if(null == battleInstruction)
            {
                LBLogger.Error("RqBattleInstructionHandler", "消息解析失败");
                return;
            }
            LBSceneManager.Instance.PlayerBattleInstruction(curPlayer.PlayerId, battleInstruction);
        }
    }
}
