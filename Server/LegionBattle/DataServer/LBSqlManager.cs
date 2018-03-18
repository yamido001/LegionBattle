using System.Data.SqlClient;
using System.Text;
using System;
using LegionBattle.LoginServer;
using LBCSCommon;
using LegionBattle.Player;

namespace LegionBattle.DataServer
{
    class LBSqlManager : SingleInstance<LBSqlManager>
    {
        const string LogTag = "SqlManager";
        SqlConnection mConnection;
        StringBuilder mCommondStringBuilder;

        public LBSqlManager()
        {
            CreateConnection();
            OpenSql();
            mCommondStringBuilder = new StringBuilder();
        }

        void CreateConnection()
        {
            SqlConnectionStringBuilder commondStrBuilder = new SqlConnectionStringBuilder();
            commondStrBuilder.DataSource = "PC201506191722";
            commondStrBuilder.InitialCatalog = "LegionBattle";
            commondStrBuilder.IntegratedSecurity = true;

            string connectStr = commondStrBuilder.ToString();
            mConnection = new SqlConnection(connectStr);
        }

        bool OpenSql()
        {
            if (mConnection.State == System.Data.ConnectionState.Open)
                return true;
            mConnection.Open();
            if (mConnection.State == System.Data.ConnectionState.Open)
                return true;
            LBLogger.Error(LogTag, "数据库连接不上");
            return false;
        }

        int GetAccountId(string accountName, string password)
        {
            mCommondStringBuilder.Clear();
            mCommondStringBuilder.Append("SELECT AccountId FROM dbo.Account Where AccountName = '");
            mCommondStringBuilder.Append(accountName);
            mCommondStringBuilder.Append("' AND Password = '");
            mCommondStringBuilder.Append(password);
            mCommondStringBuilder.Append("'");

            string sqlStr = mCommondStringBuilder.ToString();
            int accountId = -1;

            try
            {
                SqlCommand checkCommond = new SqlCommand(sqlStr, mConnection);
                SqlDataReader sqlReader = checkCommond.ExecuteReader();
                if (sqlReader.Read())
                {
                    accountId = sqlReader.GetInt32(0);
                }
                sqlReader.Close();
                LBLogger.Info(LogTag, "账号登录验证成功 " + accountId + "  " + sqlStr);
            }
            catch (Exception ex)
            {
                LBLogger.Error(LogTag, "账号登录验证异常：" + accountName + " " + password + " " + sqlStr + " Ex:" + ex.Message);
            }
            finally
            {

            }
            return accountId;
        }

        public void CheckAccount(string accountName, string password, int peerConnectionId)
        {
            if (!OpenSql())
                return;

            int accountId = GetAccountId(accountName, password);
            bool loginSuccess = accountId != -1;
            LBAccountManager.Instance.AccountLoginResult(loginSuccess, accountId, accountName, password, peerConnectionId);
            if (loginSuccess)
            {
                PlayerLogin(accountName, accountId, peerConnectionId);
            }
        }

        public void CreateAccount(string accountName, string password, int peerConnectionId)
        {
            if (!OpenSql())
                return;

            int accountId = GetAccountId(accountName, password);
            if(-1 != accountId)
            {
                LBAccountManager.Instance.CreateAccountResult(false, accountId, accountName, password, peerConnectionId);
                return;
            }

            mCommondStringBuilder.Clear();
            mCommondStringBuilder.Append("INSERT INTO[dbo].[Account] ([AccountName],[Password]) VALUES('");
            mCommondStringBuilder.Append(accountName);
            mCommondStringBuilder.Append("','");
            mCommondStringBuilder.Append(password);
            mCommondStringBuilder.Append("')");

            string sqlStr = mCommondStringBuilder.ToString();
            bool insertRet = false;

            try
            {
                SqlCommand sqlCommand = new SqlCommand(sqlStr, mConnection);
                int ret = sqlCommand.ExecuteNonQuery();
                insertRet = ret != 0;
                LBLogger.Info(LogTag, "账号创建成功 " + ret + "  " + sqlStr);
            }
            catch (Exception ex)
            {
                LBLogger.Error(LogTag, "账号创建异常：" + accountName + " " + password + " " + sqlStr + " " + ex.Message);
            }

            if(insertRet)
            {
                accountId = GetAccountId(accountName, password);
                insertRet = accountId != -1;
            }
            LBAccountManager.Instance.CreateAccountResult(insertRet, accountId, accountName, password, peerConnectionId);
        }

        #region Player数据
        public void PlayerLogin(string accountName, int accountId, int peerConnectionId)
        {
            int playerId;
            string playerName;
            if(!GetPlayerInfo(accountId, out playerId, out playerName))
            {
                playerName = accountName;
                if (!CratePlayer(playerName, accountId, out playerId))
                {
                    LBLogger.Error(LogTag, "创建玩家失败, " + playerName + " " + accountId);
                    return;
                }
            }
            LBPlayerManager.Instance.PlayerLogin(playerId, playerName, peerConnectionId);
        }

        bool GetPlayerInfo(int accountId, out int playerId, out string playerName)
        {
            playerId = CommonDefine.InvalidPlayerId;
            playerName = string.Empty;
            if (!OpenSql())
                return false;
            
            mCommondStringBuilder.Clear();
            mCommondStringBuilder.Append("SELECT PlayerId, PlayerName FROM dbo.Player Where AccountId = ");
            mCommondStringBuilder.Append(accountId.ToString());

            string sqlStr = mCommondStringBuilder.ToString();
            try
            {
                SqlCommand checkCommond = new SqlCommand(sqlStr, mConnection);
                SqlDataReader sqlReader = checkCommond.ExecuteReader();
                if (sqlReader.Read())
                {
                    playerId = sqlReader.GetInt32(0);
                    LBLogger.Info(LogTag, "玩家登录执行 Read success " + playerId);
                    playerName = sqlReader.GetString(1);
                }
                sqlReader.Close();
                LBLogger.Info(LogTag, "玩家登录执行 " + playerId + "  " + sqlStr);
            }
            catch (Exception ex)
            {
                LBLogger.Error(LogTag, "玩家登录验证异常：" + accountId + " " + sqlStr + " Ex:" + ex.Message);
            }


            return playerId != CommonDefine.InvalidPlayerId;
        }

        bool CratePlayer(string playerName, int accountId, out int playerId)
        {
            playerId = CommonDefine.InvalidPlayerId;
            if (!OpenSql())
                return false;

            mCommondStringBuilder.Clear();
            mCommondStringBuilder.Append("INSERT INTO[dbo].[Player] ([PlayerName],[accountId]) VALUES('");
            mCommondStringBuilder.Append(playerName);
            mCommondStringBuilder.Append("',");
            mCommondStringBuilder.Append(accountId);
            mCommondStringBuilder.Append(")");

            string sqlStr = mCommondStringBuilder.ToString();
            try
            {
                SqlCommand checkCommond = new SqlCommand(sqlStr, mConnection);
                SqlDataReader sqlReader = checkCommond.ExecuteReader();
                if (sqlReader.Read())
                {
                    playerId = sqlReader.GetInt32(0);
                }
                sqlReader.Close();
                LBLogger.Info(LogTag, "创建玩家执行 " + playerId + "  " + sqlStr);
            }
            catch (Exception ex)
            {
                LBLogger.Error(LogTag, "创建玩家异常：" + accountId + " " + sqlStr + " Ex:" + ex.Message);
            }
            return playerId != CommonDefine.InvalidPlayerId;
        }
        #endregion
    }
}
