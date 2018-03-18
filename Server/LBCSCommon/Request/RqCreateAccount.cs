using System;
using System.Collections.Generic;

namespace LBCSCommon
{
    public class RqCreateAccount : IRequest
    {
        public string AccountName
        {
            get;
            private set;
        }

        public string Password
        {
            get;
            private set;
        }

        public enum RqLoginErrorCode
        {
            Null,
            AccountIsNullOrEmpty,
            PasswordIsNullOrEmpty,
            AccountNameLength,
            PasswordLength,
        }

        public static RqLoginErrorCode ParseErrorCode = RqLoginErrorCode.Null;

        public RqCreateAccount(string accountName, string password)
        {
            AccountName = accountName;
            Password = password;
        }

        public static RqCreateAccount Deserialization(Dictionary<byte, object> parameters)
        {
            object accountObj;
            object passwordObj;
            if (!parameters.TryGetValue(1, out accountObj))
            {
                ParseErrorCode = RqLoginErrorCode.AccountIsNullOrEmpty;
                return null;
            }
            if (!parameters.TryGetValue(2, out passwordObj))
            {
                ParseErrorCode = RqLoginErrorCode.PasswordIsNullOrEmpty;
                return null;
            }
            string accountName = accountObj as string;
            string password = passwordObj as string;
            if (string.IsNullOrEmpty(accountName))
            {
                ParseErrorCode = RqLoginErrorCode.AccountIsNullOrEmpty;
                return null;
            }
            if (string.IsNullOrEmpty(password))
            {
                ParseErrorCode = RqLoginErrorCode.PasswordIsNullOrEmpty;
                return null;
            }
            if (!IsCorrectAccountName(accountName))
            {
                ParseErrorCode = RqLoginErrorCode.AccountNameLength;
                return null;
            }
            if (!IsCorrectPassword(password))
            {
                ParseErrorCode = RqLoginErrorCode.PasswordLength;
                return null;
            }
            RqCreateAccount rqLogin = new RqCreateAccount(accountName, password);
            return rqLogin;
        }

        public Dictionary<byte, object> Serialization()
        {
            Dictionary<byte, object> retDic = new Dictionary<byte, object>();
            retDic[1] = AccountName;
            retDic[2] = Password;
            return retDic;
        }

        static bool IsCorrectAccountName(string accountName)
        {
            return accountName.Length < CommonDefine.AccountLength;
        }

        static bool IsCorrectPassword(string password)
        {
            return password.Length < CommonDefine.AccountLength;
        }
    }
}
