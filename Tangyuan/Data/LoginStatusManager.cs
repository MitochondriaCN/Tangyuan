using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangyuan.Data
{
    internal static class LoginStatusManager
    {
        internal static bool IsLoggedIn { get; private set; } = false;
        internal static uint LoggedInUserID { get; private set; } = 0;

        internal static bool TryLogIn(string phoneNumber, string passwd)
        {
            UserInfo ui = SQLDataHelper.GetUserInfoByPhoneNumber(phoneNumber);
            if (ui.Password == passwd)
            {
                IsLoggedIn = true;
                LoggedInUserID = ui.UserID;
                LocalSQLDataHelper.UpdateLoginInfo(new LocalSQLDataHelper.LocalLoginInfo()
                {
                    LoginUserPassword = passwd,
                    LoginUserPhoneNumber = phoneNumber
                });
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static bool TryLogIn()
        {
            LocalSQLDataHelper.LocalLoginInfo logininfo = LocalSQLDataHelper.GetLoginInfo();
            if (logininfo != null)
            {
                if (TryLogIn(logininfo.LoginUserPhoneNumber, logininfo.LoginUserPassword))
                {
                    UserInfo ui = SQLDataHelper.GetUserInfoByPhoneNumber(logininfo.LoginUserPhoneNumber);
                    IsLoggedIn = true;
                    LoggedInUserID = ui.UserID;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
