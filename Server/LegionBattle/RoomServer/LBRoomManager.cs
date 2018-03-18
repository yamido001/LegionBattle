using System.Collections.Generic;
using LegionBattle.ServerCommon;

namespace LegionBattle.Room
{
    public class LBRoomManager : SingleInstance<LBRoomManager>
    {
        const string LogTag = "RoomManager";
        CachedObjectManager<LBRoom> mCachedRoom;
        Dictionary<int, LBRoom> mRoomDic;
        Dictionary<int, int> mPlayerIdRoomIdDic;

        public LBRoomManager()
        {
            mCachedRoom = new CachedObjectManager<LBRoom>(ServerCommonDefine.PreCreateRoomCount);
            mRoomDic = new Dictionary<int, LBRoom>(ServerCommonDefine.RoomDicCapacity);
            mPlayerIdRoomIdDic = new Dictionary<int, int>();
        }

        public int CreateRoom(string roomName)
        {
            LBRoom newRoom = mCachedRoom.GetObject();
            LBLogger.Info(LogTag, "创建完房间 " + newRoom.RoomId);
            mRoomDic[newRoom.RoomId] = newRoom;
            newRoom.SetRoomName(roomName);
            return newRoom.RoomId;
        }

        public bool PlayerEnterRoom(int roomId, int playerId)
        {
            LBRoom room = GetRoomById(roomId);
            if (room == null)
            {
                LBLogger.Error(LogTag, "房间不存在 " + roomId);
                return false;
            }
            bool enterRoomResult = room.EnterRoom(playerId);
            if (enterRoomResult)
            {
                mPlayerIdRoomIdDic[playerId] = roomId;
            }
            return enterRoomResult;
        }

        public bool PlayerLeaveRoom(int playerId)
        {
            LBRoom room = GetRoomByPlayerId(playerId);
            if (room == null)
            {
                return false;
            }
            bool leaveRoomResult = room.LeaveRoom(playerId);
            if(leaveRoomResult)
            {
                mPlayerIdRoomIdDic.Remove(playerId);
            }
            return leaveRoomResult;
        }

        public bool PlayerRoomReady(int playerId)
        {
            LBRoom room = GetRoomByPlayerId(playerId);
            if (room == null)
            {
                return false;
            }
            return room.ReadyInRoom(playerId);
        }

        public bool IsAllMemberReady(int roomId)
        {
            LBRoom room = GetRoomById(roomId);
            if (null == room)
                return false;
            return room.IsAllMemberReady();
        }

        public bool IsPlayerInRoom(int playerId)
        {
            return mPlayerIdRoomIdDic.ContainsKey(playerId);
        }

        public int GetRoomIdByPlayerId(int playerId)
        {
            int roomId = 0;
            mPlayerIdRoomIdDic.TryGetValue(playerId, out roomId);
            return roomId;
        }

        public LBRoom GetRoomByPlayerId(int playerId)
        {
            int roomId = GetRoomIdByPlayerId(playerId);
            return GetRoomById(roomId);
        }

        public LBRoom GetRoomById(int roomId)
        {
            LBRoom ret = null;
            mRoomDic.TryGetValue(roomId, out ret);
            return ret;
        }
    }
}
