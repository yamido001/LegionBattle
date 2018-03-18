using LBCSCommon;
using LegionBattle.ServerCommon;

namespace LegionBattle.Player
{
    class LBPlayer : CachedObject
    {
        public string PlayerName
        {
            get;
            private set;
        }

        public int PlayerId
        {
            get;
            private set;
        }

        public int ConnectionId
        {
            get;
            private set;
        }

        #region 对外接口
        public void Login(int playerId, string playerName, int connectionId)
        {
            PlayerName = playerName;
            PlayerId = playerId;
            ConnectionId = connectionId;
        }

        public void Logout()
        {

        }
        #endregion

        #region 数据成员函数
        #endregion

        #region 继承的虚函数
        public override void OnWillUse()
        {
            base.OnWillUse();
            PlayerName = string.Empty;
            PlayerId = CommonDefine.InvalidPlayerId;
        }
        #endregion

    }
}
