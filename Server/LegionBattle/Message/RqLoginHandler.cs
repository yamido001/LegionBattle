using LBCSCommon;
using Photon.SocketServer;
using LegionBattle.LoginServer;
using System.Collections.Generic;

namespace LegionBattle.Message
{
    public class RqLoginHandler : RqLogin
    {
        const string LogTag = "RqLoginHandler";

        public RqLoginHandler(string accountName, string password)
                :base(accountName, password)
        {

        }

        public static void OnOperateRequest(MyPeer peer, OperationRequest operationRequest)
        {
            LBLogger.Info(LogTag, "请求登录");
            RqLogin rqLogin = Deserialization(operationRequest.Parameters);
            if (null != rqLogin)
            {
                LBAccountManager.Instance.AccountLogin(rqLogin.AccountName, rqLogin.Password, peer.ConnectionId);
            }
            else
            {
                if (ParseErrorCode == RqLoginErrorCode.Null)
                {
                    LBLogger.Error(LogTag, "登录游戏解析失败，但是没有错误码");
                    return;
                }

                peer.SendCustomEvent(RpId.LoginResult, RpLoginResult.Serialization(false, RpLoginResult.LoginErrorCode.ParseError));
            }
        }
    }
}
