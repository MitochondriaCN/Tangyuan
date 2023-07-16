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
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
