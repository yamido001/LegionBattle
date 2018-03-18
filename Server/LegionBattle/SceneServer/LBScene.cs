using LegionBattle.Player;
using LBCSCommon;
using LegionBattle.ServerCommon;
using System.Collections.Generic;

namespace LegionBattle.SceneServer
{
    public class LBScene : CachedObject
    {
        const string LogTag = "LBScene";
        class LBScenePlayerInfo
        {
            public int playerId;
            public string playerName;
            public int connectionId;
            public bool isLoadFinish;

            public void ClearInfo()
            {
                playerId = CommonDefine.InvalidPlayerId;
                playerName = string.Empty;
                connectionId = -1;
                isLoadFinish = false;
            }

            public bool IsValidInfo
            {
                get
                {
                    return playerId != CommonDefine.InvalidPlayerId;
                }
            }
        }

        LBScenePlayerInfo[] mPlayerInfoArray;
        List<BattleInstructionBase> mPlayerBattleInstructionList;

        public int SceneId
        {
            get;
            private set;
        }

        public int RoomId
        {
            get;
            private set;
        }

        public bool IsPlaying
        {
            get;
            private set;
        }

        public bool IsEmpty
        {
            get{
                bool isEmpty = true;
                for (int i = 0; i < mPlayerInfoArray.Length; ++i)
                {
                    if (mPlayerInfoArray[i].IsValidInfo)
                    {
                        isEmpty = false;
                        break;
                    }
                }
                return isEmpty;
            }
        }

        long mLastSynchronizeTime;
        long mCurTime;
        long mBattleStartTime;
        short mCurFrameCount = 0;

        public LBScene()
        {
            mPlayerInfoArray = new LBScenePlayerInfo[ServerCommonDefine.RoomMaxAccountCount];
            for(int i = 0; i < mPlayerInfoArray.Length; ++i)
            {
                mPlayerInfoArray[i] = new LBScenePlayerInfo();
                mPlayerInfoArray[i].ClearInfo();
            }
            mPlayerBattleInstructionList = new List<BattleInstructionBase>(ServerCommonDefine.SceneBattleInstructionCapacity);
            IsPlaying = false;
        }

        public void SetRoomId(int roomId)
        {
            RoomId = roomId;
        }

        public bool PlayerEnterScene(LBPlayerEnterSceneInfo[] playerInfos)
        {
            for(int i = 0; i < playerInfos.Length; ++i)
            {
                if (IsPlayerInScene(playerInfos[i].playerId))
                {
                    LBLogger.Error(LogTag, "玩家已经在场景中了 " + playerInfos[i].playerId + " " + playerInfos[i].playerName);
                    return false;
                }
            }
            int emptyDataCount = GetEmptyDataCount();
            if(emptyDataCount < playerInfos.Length)
            {
                LBLogger.Error(LogTag, "当前场景空余人数为:" + emptyDataCount + "  加入场景人数:" + playerInfos.Length);
                return false;
            }
            for(int i = 0; i < playerInfos.Length; ++i)
            {
                LBPlayerEnterSceneInfo enterSceneInfo = playerInfos[i];
                LBScenePlayerInfo playerInfo = GetEmptyData();
                playerInfo.playerId = enterSceneInfo.playerId;
                playerInfo.playerName = enterSceneInfo.playerName;
                playerInfo.connectionId = enterSceneInfo.connectionId;
            }
            BroadcastEvent(RpId.AllMemberReady, null);
            return true;
        }

        public void PlayerLeaveScene(int playerId)
        {
            for (int i = 0; i < mPlayerInfoArray.Length; ++i)
            {
                if (mPlayerInfoArray[i].playerId == playerId)
                {
                    mPlayerInfoArray[i].ClearInfo();
                    break;
                }
            }
        }

        /// <summary>
        /// 客户端加载场景完成消息
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public bool LoadFinish(int playerId)
        {
            bool isSuccess = false;
            for (int i = 0; i < mPlayerInfoArray.Length; ++i)
            {
                if (mPlayerInfoArray[i].playerId == playerId)
                {
                    mPlayerInfoArray[i].isLoadFinish = true;
                    isSuccess = true;
                }
            }
            if (!isSuccess)
                return false;

            if (IsAllLoadFinish())
            {
                BattleBegin();
            }
            return true;
        }

        public bool IsAllLoadFinish()
        {
            bool isLoadFinish = true;
            for (int i = 0; i < mPlayerInfoArray.Length; ++i)
            {
                if (!mPlayerInfoArray[i].isLoadFinish)
                {
                    isLoadFinish = false;
                    break;
                }
            }
            //LBLogger.Info(LogTag, "IsAllLoadFinish " + isLoadFinish);
            return isLoadFinish;
        }

        public void BroadcastEvent(RpId rpId, Dictionary<byte, object> dicParameters)
        {
            for (int i = 0; i < mPlayerInfoArray.Length; ++i)
            {
                if (mPlayerInfoArray[i].IsValidInfo)
                {
                    LBPlayer curPlayer = LBPlayerManager.Instance.GetPlayerByPlayerId(mPlayerInfoArray[i].playerId);
                    if (null != curPlayer)
                        LBPeerManager.Instance.SendMessage(curPlayer.ConnectionId, rpId, dicParameters);
                }
            }
        }

        public void Update(long deltaMs)
        {
            mCurTime += deltaMs;
            while(mCurTime > mLastSynchronizeTime + CommonDefine.BattleOneFrameMs)
            {
                //LBLogger.Info(LogTag, "CurTick " + TimeManager.Instance.CurrentTimeMs);
                mLastSynchronizeTime += CommonDefine.BattleOneFrameMs;
                SynchronizeBattleEvent();
            }
        }

        bool IsPlayerInScene(int playerId)
        {
            for(int i = 0; i < mPlayerInfoArray.Length; ++i)
            {
                if (mPlayerInfoArray[i].playerId == playerId)
                    return true;
            }
            return false;
        }

        LBScenePlayerInfo GetEmptyData()
        {
            LBScenePlayerInfo ret = null;
            for(int i = 0; i < mPlayerInfoArray.Length; ++i)
            {
                if (!mPlayerInfoArray[i].IsValidInfo)
                {
                    ret = mPlayerInfoArray[i];
                    break;
                }
            }
            return ret;
        }

        int GetEmptyDataCount()
        {
            int emptyDataCount = 0;
            for (int i = 0; i < mPlayerInfoArray.Length; ++i)
            {
                if (!mPlayerInfoArray[i].IsValidInfo)
                {
                    ++emptyDataCount;
                }
            }
            return emptyDataCount;
        }

        void BattleBegin()
        {
            if (IsPlaying)
                return;
            IsPlaying = true;
            mLastSynchronizeTime = TimeManager.Instance.CurrentTimeMs;
            mCurTime = mLastSynchronizeTime;
            mBattleStartTime = mCurTime;
            int[] sceneUnitIds = new int[mPlayerInfoArray.Length];
            for(int i = 0; i < sceneUnitIds.Length; ++i)
            {
                sceneUnitIds[i] = mPlayerInfoArray[i].playerId;
            }
            BroadcastEvent(RpId.EnterScene, RpEnterScene.Serialization(sceneUnitIds));
        }

        public bool PlayerBattleInstruction(int playerId, BattleInstructionBase instruction)
        {
            //不相信客户端，万一有外挂能让别人使用技能呢
            instruction.SceneUnitId = playerId;
            mPlayerBattleInstructionList.Add(instruction);
            return true;
        }

        void SynchronizeBattleEvent()
        {
            mCurFrameCount++;
            //LBLogger.Info(LogTag, "向前端发送战斗数据，帧数为:" + mCurFrameCount);
            if (0 == mPlayerBattleInstructionList.Count)
            {
                BroadcastEvent(RpId.BattleInstruction, RpBattleInstructionList.Serialization(mCurFrameCount, new List<BattleInstructionBase> (0), true));
                return;
            }
            int i = 0;
            List<BattleInstructionBase> instructionList = null;
            while (i < mPlayerBattleInstructionList.Count)
            {
                if(null == instructionList)
                {
                    int capacity = i + RpBattleInstructionList.MaxInstructionCount > mPlayerBattleInstructionList.Count ? mPlayerBattleInstructionList.Count - i : RpBattleInstructionList.MaxInstructionCount;
                    instructionList = new List<BattleInstructionBase>(capacity);
                }
                instructionList.Add(mPlayerBattleInstructionList[i]);
                if(instructionList.Count >= RpBattleInstructionList.MaxInstructionCount || i == mPlayerBattleInstructionList.Count - 1)
                {
                    bool isFinish = i == mPlayerBattleInstructionList.Count - 1;
                    BroadcastEvent(RpId.BattleInstruction, RpBattleInstructionList.Serialization(mCurFrameCount, instructionList, isFinish));
                }
                ++i;
            }
            mPlayerBattleInstructionList.Clear();
        }

        public override void OnWillUse()
        {
            base.OnWillUse();
            IsPlaying = false;
            mCurFrameCount = 0;
        }
    }
}
