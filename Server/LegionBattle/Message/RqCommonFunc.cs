using LegionBattle.Player;
using LegionBattle.Room;
using LBCSCommon;
using System.Collections.Generic;

namespace LegionBattle.Message
{
    class RqCommonFunc
    {
        public static List<RpPlayerData> CreateRoomPlayerList(int roomId)
        {
            List<RpPlayerData> accountList = new List<RpPlayerData>();
            LBRoom curRoom = LBRoomManager.Instance.GetRoomById(roomId);
            for (int i = 0; i < curRoom.MemberInfoArray.Length; ++i)
            {
                LBRoom.MemberInfo roomMemberInfo = curRoom.MemberInfoArray[i];
                LBPlayer roomPlayer = LBPlayerManager.Instance.GetPlayerByPlayerId(roomMemberInfo.playerId);
                if (null == roomPlayer)
                {
                    continue;
                }
                RpPlayerData rpAccount = new RpPlayerData(roomMemberInfo.playerId, roomPlayer.PlayerName, roomMemberInfo.isReady);
                accountList.Add(rpAccount);
            }
            return accountList;
        }
    }
}
