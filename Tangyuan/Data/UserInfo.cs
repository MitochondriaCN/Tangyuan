using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangyuan.Data
{
    internal class UserInfo
    {
        internal uint UserID { get; private set; }
        internal string Password { get; private set; }
        internal string Nickname { get; private set; }
        internal string PhoneNumber { get; private set; }
        internal uint SchoolID { get; private set; }
        internal string Avatar { get; private set; }
        internal uint GradeCode { get; private set; }
        internal UserInfo(uint userID, string passwd, string nickname, string phoneNumber, uint schoolID, string avatar, uint gradeCode)
        {
            UserID = userID;
            Password = passwd;
            Nickname = nickname;
            PhoneNumber = phoneNumber;
            SchoolID = schoolID;
            Avatar = avatar;
            GradeCode = gradeCode;
        }
    }
}
