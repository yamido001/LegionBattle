using System;
using LBCSCommon;
using LegionBattle.ServerCommon;

namespace LegionBattle.LoginServer
{
    public class LBAccount : CachedObject
    {
        const string LogTag = "Account";

        Char[] mAccount = new Char[CommonDefine.AccountLength + 1];
        Char[] mPassword = new Char[CommonDefine.PasswrodLength + 1];

        public int AccountId
        {
            get;
            private set;
        }

        public string AccountName
        {
            get {
                return new string(mAccount);
            }
        }

        public int PeerConnectionId
        {
            get;
            private set;
        }

        public bool SetAccountInfo(string accountName, string password, int accountId, int peerConnectionId)
        {
            if (!StringUtils.CopyStringToCharArray(accountName, mAccount))
                return false;
            if (!StringUtils.CopyStringToCharArray(password, mPassword))
                return false;
            AccountId = accountId;
            PeerConnectionId = peerConnectionId;
            return true;
        }

        public bool IsCorrectPassword(string password)
        {
            return StringUtils.StringEqualsCharArray(password, mPassword);
        }

        /// <summary>
        /// 踢出当前玩家
        /// </summary>
        public void KickAccount()
        {
            LBLogger.Info(LogTag, "踢出账号 " + PeerConnectionId);
            PeerConnectionId = -1;
            LBPeerManager.Instance.KickoutPeer(PeerConnectionId);
        }

        /// <summary>
        /// 账号登录
        /// </summary>
        /// <param name="password"></param>
        /// <param name="peerId"></param>
        public void Login(int peerId)
        {
            LBLogger.Info(LogTag, "账号登录 " + " " + peerId);
            PeerConnectionId = peerId;
        }
    }
}
