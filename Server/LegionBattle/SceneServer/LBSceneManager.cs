using System.Collections.Generic;
using LegionBattle.ServerCommon;
using LBCSCommon;

namespace LegionBattle.SceneServer
{
    public class LBSceneManager : SingleInstance<LBSceneManager>
    {
        const string LogTag = "LBSceneManager";
        CachedObjectManager<LBScene> mCachedSceneMgr;
        Dictionary<int, LBScene> mSceneDic;
        Dictionary<int, int> mRoomIdSceneIdDic;
        Dictionary<int, int> mPlayerIdSceneIdDic;

        public LBSceneManager()
        {
            mCachedSceneMgr = new CachedObjectManager<LBScene>(ServerCommonDefine.PreCreateSceneCount);
            mSceneDic = new Dictionary<int, LBScene>(ServerCommonDefine.SceneDicCapacity);
            mRoomIdSceneIdDic = new Dictionary<int, int>(ServerCommonDefine.SceneDicCapacity);
            mPlayerIdSceneIdDic = new Dictionary<int, int>(ServerCommonDefine.SceneDicCapacity);
        }
        
        public bool PlayersEnterScene(int roomId, LBPlayerEnterSceneInfo[] playerInfos)
        {
            for (int i = 0; i < playerInfos.Length; ++i)
            {
                if (null != GetSceneByPlayerId(playerInfos[i].playerId))
                    return false;
            }

            LBScene curScene = GetSceneByRoomId(roomId);
            if(null == curScene)
            {
                curScene = mCachedSceneMgr.GetObject();
                curScene.SetRoomId(roomId);
                mSceneDic[curScene.SceneId] = curScene;
                mRoomIdSceneIdDic[roomId] = curScene.SceneId;
            }
            if(!curScene.PlayerEnterScene(playerInfos))
            {
                LBLogger.Error(LogTag, "玩家进入场景");
                FreeScene(curScene.SceneId);
                return false;
            }
            for(int i = 0; i < playerInfos.Length; ++i)
            {
                mPlayerIdSceneIdDic[playerInfos[i].playerId] = curScene.SceneId;
            }
            return true;
        }

        public bool PlayerLoadFinish(int playerId)
        {
            LBScene scene = GetSceneByPlayerId(playerId);
            if (scene == null)
            {
                LBLogger.Error(LogTag, "玩家通知加載成功，但是沒有找到對應的場景 " + playerId);
                return false;
            }
            return scene.LoadFinish(playerId);
        }

        public void PlayerLeaveScene(int playerId)
        {
            LBScene scene = GetSceneByPlayerId(playerId);
            if (scene == null)
            {
                return;
            }
            scene.PlayerLeaveScene(playerId);
            mPlayerIdSceneIdDic.Remove(playerId);
            if (scene.IsEmpty)
            {
                FreeScene(scene.SceneId);
            }
        }

        public bool PlayerBattleInstruction(int playerId, BattleInstructionBase instruction)
        {
            LBScene scene = GetSceneByPlayerId(playerId);
            if (scene == null)
            {
                LBLogger.Error(LogTag, "收到玩家的战斗指令，但是没有找到对应的场景 " + playerId);
                return false;
            }
            return scene.PlayerBattleInstruction(playerId, instruction);
        }

        public LBScene GetSceneByPlayerId(int playerId)
        {
            int sceneId = 0;
            if (!mPlayerIdSceneIdDic.TryGetValue(playerId, out sceneId))
            {
                LBLogger.Info(LogTag, "通過玩家id沒有找到對應的場景 " + playerId);
                return null;
            }
                
            return GetSceneBySceneId(sceneId);
        }

        public void UpdatTick(long deltaMs)
        {
            var dicEnumerator = mSceneDic.GetEnumerator();
            while(dicEnumerator.MoveNext())
            {
                dicEnumerator.Current.Value.Update(deltaMs);
            }
        }

        void FreeScene(int sceneId)
        {
            LBScene curScene = GetSceneBySceneId(sceneId);
            if (null == curScene)
                return;
            mSceneDic.Remove(curScene.SceneId);
            mRoomIdSceneIdDic.Remove(curScene.RoomId);
            mCachedSceneMgr.ReturnObject(curScene);
        }

        LBScene GetSceneBySceneId(int sceneId)
        {
            LBScene ret = null;
            mSceneDic.TryGetValue(sceneId, out ret);
            return ret;
        }

        LBScene GetSceneByRoomId(int roomId)
        {
            int sceneId;
            if(!mRoomIdSceneIdDic.TryGetValue(roomId, out sceneId))
            {
                LBLogger.Info(LogTag, "通过房间号没有找到对应的场景 " + roomId);
                return null;
            }
            return GetSceneBySceneId(sceneId);
        }
    }
}
