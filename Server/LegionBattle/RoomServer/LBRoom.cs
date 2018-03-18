using System.Collections.Generic;
using LegionBattle.ServerCommon;
using LegionBattle.Player;
using LBCSCommon;
using LegionBattle.Message;
using LegionBattle.SceneServer;

namespace LegionBattle.Room
{
    public class LBRoom : CachedObject
    {
        public class MemberInfo
        {
            public int playerId;
            public bool isReady;
            public bool isLoadFinish;

            public void ClearInfo()
            {
                playerId = CommonDefine.InvalidPlayerId;
                isReady = false;
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


        const string LogTag = "Room";
        static int RoomCount;

        public LBRoom()
        {
            MemberInfoArray = new MemberInfo[ServerCommonDefine.RoomMaxAccountCount];
            for (int i = 0; i < MemberInfoArray.Length; ++i)
            {
                MemberInfoArray[i] = new MemberInfo();
                MemberInfoArray[i].ClearInfo();
            }
            RefreshRoomId();
        }

        public int RoomId
        {
            get;
            private set;
        }

        public string RoomName
        {
            get;
            private set;
        }

        public MemberInfo[] MemberInfoArray
        {
            get;
            private set;
        }

        public bool IsPlaying
        {
            get;
            private set;
        }

        public void SetRoomName(string roomName)
        {
            RoomName = roomName;
        }

        /// <summary>
        /// 账号进入房间
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public bool EnterRoom(int playerId)
        {
            LBLogger.Error(LogTag, "请求进入房间 " + playerId);
            int emptyIndex = FindEmptyMemberIndex();
            if (emptyIndex < 0)
                return false;
            MemberInfoArray[emptyIndex].playerId = playerId;

            LBPlayer curPlayer = LBPlayerManager.Instance.GetPlayerByPlayerId(playerId);
            if (null != curPlayer) {
                LBLogger.Error(LogTag, "发起进入房间消息 " + playerId + " " + curPlayer.ConnectionId);
                LBPeerManager.Instance.SendMessage(curPlayer.ConnectionId, RpId.EnterRoom, RpEnterRoom.Serialization(true, RoomId, RoomName));
            }
            BroadcastEvent(RpId.RoomAccountInfo, RpRoomMemberInfo.Serialization(RqCommonFunc.CreateRoomPlayerList(RoomId)));
            return true;
        }

        /// <summary>
        /// 账号离开房间
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public bool LeaveRoom(int playerId)
        {
            for(int i = 0; i < MemberInfoArray.Length; ++i)
            {
                if(MemberInfoArray[i].playerId == playerId)
                {
                    MemberInfoArray[i].ClearInfo();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 账号在房间内发起准备
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public bool ReadyInRoom(int accountId)
        {
            bool isSuccess = false;
            for (int i = 0; i < MemberInfoArray.Length; ++i)
            {
                if (MemberInfoArray[i].playerId == accountId)
                {
                    MemberInfoArray[i].isReady = true;
                    isSuccess = true;
                }
            }
            if (!isSuccess)
                return false;

            if (IsAllMemberReady())
            {
                bool isNotFind = false;
                LBPlayerEnterSceneInfo[] playerInfos = new LBPlayerEnterSceneInfo[MemberInfoArray.Length];
                for (int i = 0; i < MemberInfoArray.Length; ++i)
                {
                    LBPlayer curPlayer = LBPlayerManager.Instance.GetPlayerByPlayerId(MemberInfoArray[i].playerId);
                    if(null == curPlayer)
                    {
                        //异常情况，应该是bug
                        LBLogger.Error(LogTag, "准备进入场景时找不到对应的玩家 " + MemberInfoArray[i].playerId);
                        isNotFind = true;
                        break;
                    }
                    playerInfos[i] = new LBPlayerEnterSceneInfo();
                    playerInfos[i].playerId = curPlayer.PlayerId;
                    playerInfos[i].playerName = curPlayer.PlayerName;
                    playerInfos[i].connectionId = curPlayer.ConnectionId;
                }
                if(isNotFind)
                {
                    //异常处理情况
                }
                else
                {
                    if(LBSceneManager.Instance.PlayersEnterScene(RoomId, playerInfos))
                    {
                        IsPlaying = true;
                    }
                    else
                    {
                        LBLogger.Error(LogTag, "进入场景失败，房间ID：" + RoomId);
                    }
                }
            }
            else
            {
                BroadcastEvent(RpId.RoomAccountInfo, RpRoomMemberInfo.Serialization(RqCommonFunc.CreateRoomPlayerList(RoomId)));
            }
            return true;
        }

        public bool IsAllMemberReady()
        {
            bool isReady = true;
            for(int i = 0; i < MemberInfoArray.Length; ++i)
            {
                if (!MemberInfoArray[i].isReady)
                {
                    isReady = false;
                    break;
                }   
            }
            return isReady;
        }

        public void BroadcastEvent(RpId rpId, Dictionary<byte, object> dicParameters)
        {
            for (int i = 0; i < MemberInfoArray.Length; ++i)
            {
                if (MemberInfoArray[i].IsValidInfo)
                {
                    LBPlayer curPlayer = LBPlayerManager.Instance.GetPlayerByPlayerId(MemberInfoArray[i].playerId);
                    if (null != curPlayer)
                        LBPeerManager.Instance.SendMessage(curPlayer.ConnectionId, rpId, dicParameters);
                }
            }
        }

        #region 继承的虚函数
        public override void OnWillUse()
        {
            base.OnWillUse();
            RefreshRoomId();
        }

        public override void OnWillRecycle()
        {
            base.OnWillRecycle();
            for(int i = 0; i < MemberInfoArray.Length; ++i)
            {
                MemberInfoArray[i].ClearInfo();
            }
            RoomName = string.Empty;
            IsPlaying = false;
        }
        #endregion

        #region 数据操作函数
        /// <summary>
        /// 刷新房间号
        /// </summary>
        void RefreshRoomId()
        {
            RoomId = ++RoomCount;
            LBLogger.Info(LogTag, "刷新房间ID " + RoomId);
        }

        /// <summary>
        /// 找到空位的房间成员数组的索引
        /// </summary>
        /// <returns></returns>
        int FindEmptyMemberIndex()
        {
            int findIndex = -1;
            for (int i = 0; i < MemberInfoArray.Length; ++i)
            {
                if (MemberInfoArray[i].IsValidInfo)
                    continue;
                findIndex = i;
                break;
            }
            return findIndex;
        }
        #endregion
    }
}
