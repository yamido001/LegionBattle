using LBCSCommon;
using LegionBattle.ServerCommon;
using LegionBattle.DataServer;
using System.Collections.Generic;
using LegionBattle.Room;

namespace LegionBattle.LoginServer
{
    public class LBAccountManager : SingleInstance<LBAccountManager>
    {
        const string LogTag = "AccountManager";

        CachedObjectManager<LBAccount> mAccountCacheMgr;
        Dictionary<int, LBAccount> mAccountIdDic;
        Dictionary<string, LBAccount> mAccountNameDic;
        Dictionary<int, LBAccount> mPeerIdDic;

        public LBAccountManager()
        {
            mAccountCacheMgr = new CachedObjectManager<LBAccount>(ServerCommonDefine.PreCreateAccount);
            mAccountIdDic = new Dictionary<int, LBAccount>(ServerCommonDefine.AccountDicCapacity);
            mAccountNameDic = new Dictionary<string, LBAccount>(ServerCommonDefine.AccountDicCapacity);
            mPeerIdDic = new Dictionary<int, LBAccount>(ServerCommonDefine.AccountDicCapacity);
        }

        LBAccount GetAccountByName(string accountName)
        {
            LBAccount curAccount = null;
            mAccountNameDic.TryGetValue(accountName, out curAccount);
            return curAccount;
        }

        public LBAccount GetAccountById(int accountId)
        {
            LBAccount curAccount = null;
            mAccountIdDic.TryGetValue(accountId, out curAccount);
            return curAccount;
        }

        public LBAccount GetAccountByPeerId(int peerId)
        {
            LBAccount curAccount = null;
            mPeerIdDic.TryGetValue(peerId, out curAccount);
            return curAccount;
        }

        public void AccountLogin(string accountName, string password, int peerConnectionId)
        {
            LBAccount curAccount = GetAccountByName(accountName);
            if(null != curAccount)
            {
                if (curAccount.IsCorrectPassword(password))
                {
                    if (peerConnectionId == curAccount.PeerConnectionId)
                    {
                        LBLogger.Error(LogTag, "当前连接重复发送登录 " + peerConnectionId);
                        return;
                    }
                    //TODO 这里的逻辑感觉有点不合理，应该抽出方法
                    mPeerIdDic.Remove(curAccount.PeerConnectionId);
                    curAccount.KickAccount();
                    curAccount.Login(peerConnectionId);
                    mPeerIdDic[curAccount.PeerConnectionId] = curAccount;
                    MyPeer peer = LBPeerManager.Instance.GetPeer(peerConnectionId);
                    if (null != peer)
                    {
                        peer.SendCustomEvent(RpId.LoginResult, RpLoginResult.Serialization(true, RpLoginResult.LoginErrorCode.ParseError));
                        LBSqlManager.Instance.PlayerLogin(curAccount.AccountName, curAccount.AccountId, peerConnectionId);
                    }
                }
                else
                {
                    //提示客户端密码错误
                    MyPeer peer = LBPeerManager.Instance.GetPeer(peerConnectionId);
                    if (null != peer)
                    {
                        peer.SendCustomEvent(RpId.LoginResult, RpLoginResult.Serialization(false, RpLoginResult.LoginErrorCode.PasswordError));
                    }
                }
            }
            else
            {
                LBSqlManager.Instance.CheckAccount(accountName, password, peerConnectionId);
            }
        }

        public void AccountLoginResult(bool success, int accountId, string accountName, string password, int peerConnectionId)
        {
            LBLogger.Info(LogTag, "数据服务器返回结果 " + success + " " + accountId + " " + peerConnectionId);
            LBAccount curAccount = GetAccountByName(accountName);
            if(null != curAccount)
            {
                //客户端发送了多条登录消息？
                LBLogger.Error(LogTag, "收到账号登录结果时，当前账号已经存,账号名 " + accountName);
                return;
            }
            curAccount = GetAccountById(accountId);
            if(null != curAccount)
            {
                //客户端发送了多条登录消息？
                LBLogger.Error(LogTag, "收到账号登录结果时，当前账号已经存。账号ID " + accountName + "  为什么mAccountNameDic中没有账号？");
                return;
            }
            MyPeer peer = LBPeerManager.Instance.GetPeer(peerConnectionId);
            if(null == peer)
            {
                //客户端主动断开连接了？
                LBLogger.Info(LogTag, "没有找到网络连接 " + peerConnectionId);
                return;
            }
            if (success)
            {
                LBAccount account = mAccountCacheMgr.GetObject();
                account.SetAccountInfo(accountName, password, accountId, peerConnectionId);
                mAccountIdDic[accountId] = account;
                mAccountNameDic[accountName] = account;
                mPeerIdDic[peerConnectionId] = account;
            }
            peer.SendCustomEvent(RpId.LoginResult, RpLoginResult.Serialization(success, RpLoginResult.LoginErrorCode.PasswordError));
        }

        public void AccountCreate(string accountName, string password, int peerConnectionId)
        {
            LBLogger.Info(LogTag, "请求创建账号");
            LBAccount curAccount = GetAccountByName(accountName);
            if (null != curAccount)
            {
                MyPeer peer = LBPeerManager.Instance.GetPeer(peerConnectionId);
                if(null != peer)
                {
                    peer.SendCustomEvent(RpId.CreateAccountResult, RpCreateAccountResult.Serialization(false, RpCreateAccountResult.CreateAccountErrorCode.AccountExist));
                }
            }
            else
            {
                LBSqlManager.Instance.CreateAccount(accountName, password, peerConnectionId);
            }
        }

        public void CreateAccountResult(bool success, int accountId, string accountName, string password, int peerConnectionId)
        {
            MyPeer peer = LBPeerManager.Instance.GetPeer(peerConnectionId);
            if (null != peer)
            {
                peer.SendCustomEvent(RpId.CreateAccountResult, RpCreateAccountResult.Serialization(success, RpCreateAccountResult.CreateAccountErrorCode.AccountExist));
            }
        }

        public void AccountDisconnect(int peerConnectionId)
        {
            //Account curAccount = GetAccountByPeerId(peerConnectionId);
            //if (null == curAccount)
            //    return;
            //RoomManager.Instance.PlayerLeaveRoom(curAccount.AccountId);
        }
    }
}
