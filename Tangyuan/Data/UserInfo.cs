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

        internal string GetUserRoleFriendlyName()
        {
            switch (UserRole)
            {
                case Role.Webmaster:
                    return "站长";
                case UserInfo.Role.CoFounder:
                    return "联合创始人";
                case UserInfo.Role.Observer:
                    return "观察员";
                case UserInfo.Role.SchoolLeader:
                    return "校园领袖";
                case UserInfo.Role.GradeLeader:
                    return "年级领袖";
                case UserInfo.Role.ClassLeader:
                    return "班级领袖";
                case UserInfo.Role.Teacher:
                    return "老师";
                default:
                    return null;
            }
        }

        internal Color GetThemeColorOfUserRole()
        {
            switch (UserRole)
            {
                case UserInfo.Role.Webmaster:
                    return Color.Parse("#e74c3c");
                case UserInfo.Role.CoFounder:
                    return Color.Parse("#f1c40f");
                case UserInfo.Role.Observer:
                    return Color.Parse("#30336b");
                case UserInfo.Role.SchoolLeader:
                    return Color.Parse("#27ae60");
                case UserInfo.Role.GradeLeader:
                    return Color.Parse("#3498db");
                case UserInfo.Role.ClassLeader:
                    return Color.Parse("#9b59b6");
                case UserInfo.Role.Teacher:
                    return Color.Parse("#c0392b");
                default:
                    return null;
            }
        }
    }
}
