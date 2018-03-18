using LegionBattle.LoginServer;
using LBCSCommon;
using Photon.SocketServer;

namespace LegionBattle.Message
{
    public class RqCreateAccountHandler : RqCreateAccount
    {
        const string LogTag = "RqCreateAccountHandler";

        public RqCreateAccountHandler(string accountName, string password) : base(accountName, password)
        {

        }

        public static void OnOperateRequest(MyPeer peer, OperationRequest operationRequest)
        {
            LBLogger.Info(LogTag, "请求创建账号");
            RqCreateAccount rqCreateAccount = Deserialization(operationRequest.Parameters);
            if (null != rqCreateAccount)
            {
                LBAccountManager.Instance.AccountCreate(rqCreateAccount.AccountName, rqCreateAccount.Password, peer.ConnectionId);
            }
            else
            {
                if (ParseErrorCode == RqLoginErrorCode.Null)
                {
                    LBLogger.Error(LogTag, "登录游戏解析失败，但是没有错误码");
                    return;
                }
                peer.SendCustomEvent(RpId.CreateAccountResult, RpCreateAccountResult.Serialization(false, RpCreateAccountResult.CreateAccountErrorCode.ParseError));
            }
        }
    }
}
