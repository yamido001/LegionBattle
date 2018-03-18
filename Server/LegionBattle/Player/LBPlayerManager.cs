using LegionBattle.ServerCommon;
using System.Collections.Generic;
using LBCSCommon;
using LegionBattle.Room;
using LegionBattle.SceneServer;

namespace LegionBattle.Player
{
    class LBPlayerManager : SingleInstance<LBPlayerManager>
    {
        const string LogTag = "PlayerManager";
        CachedObjectManager<LBPlayer> mCacheManager;
        Dictionary<int, LBPlayer> mPlayerDic;
        Dictionary<int, LBPlayer> mPlayerConnectionDic;

        public LBPlayerManager()
        {
            mCacheManager = new CachedObjectManager<LBPlayer>(ServerCommonDefine.PreCreatePlayerCount);
            mPlayerDic = new Dictionary<int, LBPlayer>(ServerCommonDefine.PlayerDicCapacity);
            mPlayerConnectionDic = new Dictionary<int, LBPlayer>(ServerCommonDefine.PlayerDicCapacity);
        }

        public void PlayerLogin(int playerId, string playerName, int connectionId)
        {
            LBPlayer curPlayer = GetPlayerByPlayerId(playerId);
            if(null != curPlayer)
            {
                LBLogger.Error(LogTag, "当前玩家已经登录，不能再次登录 " + playerId + " " + playerName);
                return;
            }
            curPlayer = mCacheManager.GetObject();
            curPlayer.Login(playerId, playerName, connectionId);
            mPlayerDic[playerId] = curPlayer;
            mPlayerConnectionDic[connectionId] = curPlayer;

            LBPeerManager.Instance.SendMessage(connectionId, RpId.PlayerLogin, RpPlayerLogin.Serialization(playerId, playerName));
        }

        public void PlayerDisconnect(int peerConnectionId)
        {
            LBPlayer curPlayer = GetPlayerByConnectionId(peerConnectionId);
            if (null == curPlayer)
                return;
            mPlayerConnectionDic.Remove(curPlayer.ConnectionId);
            mPlayerDic.Remove(curPlayer.PlayerId);
            int playerId = curPlayer.PlayerId;
            curPlayer.Logout();
            mCacheManager.ReturnObject(curPlayer);
            LBRoomManager.Instance.PlayerLeaveRoom(playerId);
            LBSceneManager.Instance.PlayerLeaveScene(playerId);
        }

        public LBPlayer GetPlayerByPlayerId(int playerId)
        {
            LBPlayer ret = null;
            mPlayerDic.TryGetValue(playerId, out ret);
            return ret;
        }

        public LBPlayer GetPlayerByConnectionId(int playerId)
        {
            LBPlayer ret = null;
            mPlayerConnectionDic.TryGetValue(playerId, out ret);
            return ret;
        }
    }
}
