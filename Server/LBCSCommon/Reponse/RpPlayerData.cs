using log4net.Core;
using System.Collections.Generic;

namespace LBCSCommon
{
    public class RpPlayerData
    {
        public int PlayerId
        {
            get;set;
        }

        public string PlayerName
        {
            get;set;
        }

        public bool IsReady
        {
            get;set;
        }


        public RpPlayerData(int playerId, string playerName, bool isReady)
        {
            PlayerId = playerId;
            PlayerName = playerName;
            IsReady = isReady;
        }

        public static byte[] Serialize(object x)
        {
            RpPlayerData rpData = (RpPlayerData)x;
            byte[] res = new byte[4 + 2 + rpData.PlayerName.Length + 1];
            int index = 0;
            SerializeUtils.WriteInt(res, ref index, rpData.PlayerId);
            SerializeUtils.WriteString(res, ref index, rpData.PlayerName);
            SerializeUtils.WriteBool(res, ref index, rpData.IsReady);
            return res;
        }

        public static object Deserialize(byte[] data)
        {
            int index = 0;
            int accountId = SerializeUtils.ReadInt(data, ref index);
            string accountName = SerializeUtils.ReadString(data, ref index);
            bool isReady = SerializeUtils.ReadBool(data, ref index);
            RpPlayerData rpData = new RpPlayerData(accountId, accountName, isReady);
            return rpData;
        }
    }
}
