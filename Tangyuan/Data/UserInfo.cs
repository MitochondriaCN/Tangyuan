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
        internal string Signature { get; private set; }
        internal string PhoneNumber { get; private set; }
        internal uint SchoolID { get; private set; }
        internal string Avatar { get; private set; }
        internal uint GradeID { get; private set; }
        internal Role UserRole { get; private set; }

        internal enum Role
        {
            Webmaster,//站长
            CoFounder,//联合创始人
            Observer,//观察员
            SchoolLeader,//学校领袖
            GradeLeader,//年级领袖
            ClassLeader,//班级领袖
            OrdinaryStudent,//普通学生
            Teacher//老师
        }
        internal UserInfo(uint userID, string passwd, string nickname, string signature, string phoneNumber, uint schoolID, string avatar, uint gradeID, Role userRole)
        {
            UserID = userID;
            Password = passwd;
            Nickname = nickname;
            Signature = signature;
            PhoneNumber = phoneNumber;
            SchoolID = schoolID;
            Avatar = avatar;
            GradeID = gradeID;
            UserRole = userRole;
        }
    }
}
