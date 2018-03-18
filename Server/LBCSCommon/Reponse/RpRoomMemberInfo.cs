using System.Collections.Generic;

namespace LBCSCommon
{
    public class RpRoomMemberInfo : ResponseBase
    {
        public List<RpPlayerData> AccountList
        {
            get;
            private set;
        }

        public RpRoomMemberInfo(List<RpPlayerData> accountList)
        {
            AccountList = accountList;
        }

        public static RpRoomMemberInfo Deserialization(Dictionary<byte, object> parameters)
        {
            object accountListObj = null;
            if (!parameters.TryGetValue(1, out accountListObj))
            {
                return null;
            }
            if (null != accountListObj && accountListObj.GetType() != typeof(object[]))
            {
                return null;
            }
            List<RpPlayerData> accountList = null;
            if (accountListObj != null)
            {
                object[] accountArray = accountListObj as object[];
                accountList = new List<RpPlayerData>(accountArray.Length);
                for (int i = 0; i < accountArray.Length; ++i)
                {
                    accountList.Add(accountArray[i] as RpPlayerData);
                }
            }
            return new RpRoomMemberInfo(accountList);
        }

        public static Dictionary<byte, object> Serialization(List<RpPlayerData> accountList)
        {
            Dictionary<byte, object> retDic = new Dictionary<byte, object>();
            retDic[1] = accountList;
            return retDic;
        }
    }
}
