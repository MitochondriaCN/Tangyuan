using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;
using System.Xml.Linq;
using MySqlConnector;

namespace Tangyuan.Data
{
    internal static class SQLDataHelper
    {
        private const string host = "tangyuan-maindb.mysql.database.azure.com";
        private const uint port = 3306;
        private const string database = "tangyuan";
        private const string username = "xianliticn";
        private const string passwd = "Fuyuxuan372819";

        /// <summary>
        /// 获取一个新的数据库连接对象。
        /// </summary>
        internal static MySqlConnection GetNewConnection()
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Server = host;
            sb.Database = database;
            sb.Port = port;
            sb.UserID = username;
            sb.Password = passwd;
            sb.SslMode = MySqlSslMode.None;
            MySqlConnection conn = new MySqlConnection(sb.ToString());
            return conn;
        }

        /// <summary>
        /// 发新帖
        /// </summary>
        internal static void NewPost(uint authorID, XDocument content)
        {
            uint postID;
            using (MySqlConnection conn = GetNewConnection())
            {
                conn.Open();
                MySqlDataReader r = new MySqlCommand("select max(id) from post_table", conn).ExecuteReader();
                r.Read();
                postID = r.GetUInt32(0) + 1;
            }
            using (MySqlConnection conn = GetNewConnection())//你他妈的，每一个指令都要一个新connection，烦死了
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("insert into post_table values(" + postID +
                    "," + authorID +
                    ","+GetUserInfoByID(authorID).SchoolID+
                    ",NOW()" +
                    ",0,0,'" + content.ToString(SaveOptions.DisableFormatting) + "')", conn);
                cmd.ExecuteReader();
            }
        }

        /// <summary>
        /// 获取三天内随机帖子。
        /// </summary>
        /// <param name="maxNumber">指定获取最大帖子数</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static List<PostInfo> GetRecentPosts(uint maxNumber)
        {
            using (MySqlConnection mysqlConn = GetNewConnection())
            {
                mysqlConn.Open();
                if (mysqlConn.State == ConnectionState.Open)
                {
                    List<PostInfo> posts = new List<PostInfo>();
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand("SELECT * FROM post_table where post_date>DATE_ADD(NOW(),INTERVAL -3 DAY)", mysqlConn);
                        MySqlDataReader data = cmd.ExecuteReader();
                        Random r = new Random();
                        while (data.Read())
                        {
                            if (r.Next(0, 2) == 1)
                            {
                                posts.Add(new PostInfo(
                                    data.GetUInt32("id"),
                                    data.GetUInt32("author_id"),
                                    data.GetUInt32("school_id"),
                                    data.GetDateTime("post_date").ToLocalTime(), 
                                    data.GetUInt32("likes"),
                                    data.GetUInt32("views"),
                                    XDocument.Parse(data.GetString("content"))));
                            }
                        }
                        if (posts.Count > maxNumber)
                        {
                            posts.RemoveRange(0, (int)(posts.Count - maxNumber));
                        }
                        return posts;
                    }
                    catch
                    {
                        mysqlConn.Close();
                        throw;
                    }
                }
                else
                {
                    throw new Exception("尚未连接数据库。");
                }
            }
        }

        /// <summary>
        /// 通过用户ID获取用户昵称。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static string GetUserNicknameByID(uint id)
        {
            using (MySqlConnection mysqlConn = GetNewConnection())
            {
                mysqlConn.Open();
                if (mysqlConn.State == ConnectionState.Open)
                {
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand("SELECT * FROM user_table WHERE id=" + id, mysqlConn);
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            return reader.GetString("nickname");
                        }
                        return null;
                    }
                    catch
                    {
                        mysqlConn.Close();
                        throw;
                    }
                }

                else
                {
                    throw new Exception("尚未连接数据库。");
                }
            }
        }

        /// <summary>
        /// 通过ID获取帖子。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static PostInfo GetPostByID(uint id)
        {
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                MySqlCommand cmd = new MySqlCommand("select * from post_table where id=" + id + " limit 1", c);
                MySqlDataReader r = cmd.ExecuteReader();
                if (r.Read())
                    return new PostInfo(
                        r.GetUInt32("id"),
                        r.GetUInt32("author_id"),
                        r.GetUInt32("school_id"),
                        r.GetDateTime("post_date").ToLocalTime(),
                        r.GetUInt32("likes"),
                        r.GetUInt32("views"),
                        XDocument.Parse(r.GetString("content")));
                else
                    return null;
            }
        }

        internal static void DeletePostByID(uint id)
        {
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                new MySqlCommand("delete from post_table where id=" + id, c).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 根据帖子ID获取帖子所有一级评论。
        /// </summary>
        /// <param name="postID">帖子ID</param>
        /// <returns></returns>
        internal static List<CommentInfo> GetFirstLevelCommentsByPostID(uint postID)
        {
            List<CommentInfo> comments = new List<CommentInfo>();
            using (MySqlConnection mc = GetNewConnection())
            {
                mc.Open();
                MySqlCommand cmd = new MySqlCommand("select * from comment_table where post_id=" + postID + " and is_reply=0", mc);
                MySqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    comments.Add(new CommentInfo(
                        r.GetUInt32("id"),
                        r.GetUInt32("user_id"),
                        r.GetUInt32("post_id"),
                        r.GetDateTime("date").ToLocalTime(),
                        r.GetUInt32("likes"),
                        r.GetString("content"),
                        r.GetBoolean("is_reply"),
                        r.GetUInt32("reply_id")));
                }
                return comments;
            }

        }

        internal static UserInfo GetUserInfoByID(uint userID)
        {
            using (MySqlConnection mc = GetNewConnection())
            {
                mc.Open();
                MySqlCommand cmd = new MySqlCommand("select * from user_table where id=" + userID + " limit 1", mc);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    return new UserInfo(
                        reader.GetUInt32("id"),
                        reader.GetString("passwd"),
                        reader.GetString("nickname"),
                        reader.GetString("signature"),
                        reader.GetString("phone_number"),
                        reader.GetUInt32("school_id"),
                        reader.GetString("avatar"),
                        reader.GetUInt32("grade_code"),
                        StringToUserRole(reader.GetString("role")));
                }
                return null;
            }
        }

        internal static UserInfo GetUserInfoByPhoneNumber(string phoneNumber)
        {
            using (MySqlConnection mc = GetNewConnection())
            {
                mc.Open();
                MySqlCommand cmd = new MySqlCommand("select * from user_table where phone_number=" + phoneNumber + " limit 1", mc);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    return new UserInfo(
                        reader.GetUInt32("id"),
                        reader.GetString("passwd"),
                        reader.GetString("nickname"),
                        reader.GetString("signature"),
                        reader.GetString("phone_number"),
                        reader.GetUInt32("school_id"),
                        reader.GetString("avatar"),
                        reader.GetUInt32("grade_code"),
                        StringToUserRole(reader.GetString("role")));
                }
                return null;
            }
        }

        internal static void NewComment(uint authorID, uint postID, string content)
        {
            string finalContent = System.Text.RegularExpressions.Regex.Escape(content).Replace("\'", "\\\'");
            uint commentID;
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                MySqlCommand cmd = new MySqlCommand("select max(id) from comment_table", c);
                MySqlDataReader r = cmd.ExecuteReader();
                r.Read();
                commentID = r.GetUInt32(0) + 1;
            }
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                MySqlCommand cmd = new MySqlCommand("insert into comment_table values (" +
                    commentID + "," +
                    authorID + "," +
                    postID + "," +
                    "NOW()," +
                    "0,0,0," +
                    "'" + finalContent + "')", c);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 获取所有学校信息。
        /// </summary>
        /// <returns></returns>
        internal static List<SchoolInfo> GetAllSchoolInfos()
        {
            List<SchoolInfo> schoolInfos = new List<SchoolInfo>();
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                MySqlDataReader r = new MySqlCommand("select * from school_table", c).ExecuteReader();
                while (r.Read())
                {
                    List<SchoolInfo.GradeDefinition> gds = new();
                    XDocument rawGds = XDocument.Parse(r.GetString("grade_definitions"));
                    if (rawGds.Root.Name == "TangyuanGradeDefinitions")
                    {
                        foreach (var v in rawGds.Root.Descendants("GradeDefinition"))
                        {
                            gds.Add(new SchoolInfo.GradeDefinition(
                                uint.Parse(v.Attribute("ID").Value),
                                v.Attribute("Name").Value,
                                Color.Parse(v.Attribute("ThemeColor").Value)));
                        }
                    }
                    schoolInfos.Add(new SchoolInfo(
                        r.GetUInt32("id"),
                        r.GetString("name"),
                        gds,
                        Color.Parse(r.GetString("theme_color"))));
                }
                return schoolInfos;
            }
        }

        internal static SchoolInfo GetSchoolInfoByID(uint schoolID)
        {
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                MySqlDataReader r = new MySqlCommand("select * from school_table where id=" + schoolID + " limit 1", c).ExecuteReader();
                r.Read();

                List<SchoolInfo.GradeDefinition> gds = new();
                XDocument rawGds = XDocument.Parse(r.GetString("grade_definitions"));
                if (rawGds.Root.Name == "TangyuanGradeDefinitions")
                {
                    foreach (var v in rawGds.Root.Descendants("GradeDefinition"))
                    {
                        gds.Add(new SchoolInfo.GradeDefinition(
                            uint.Parse(v.Attribute("ID").Value),
                            v.Attribute("Name").Value,
                            Color.Parse(v.Attribute("ThemeColor").Value)));
                    }
                }
                return new SchoolInfo(
                    r.GetUInt32("id"),
                    r.GetString("name"),
                    gds,
                    Color.Parse(r.GetString("theme_color")));
            }
        }

        /// <summary>
        /// 尝试注册新用户。
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="nickname"></param>
        /// <param name="schoolID"></param>
        /// <param name="gradeID"></param>
        /// <returns></returns>
        internal static bool TrySignUp(string phoneNumber, string nickname, string passwd, uint schoolID, uint gradeID,
            string avatarUri = "https://icons.veryicon.com/png/o/miscellaneous/xinjiang-tourism/user-169.png")
        {
            //校验手机号
            if (GetUserInfoByPhoneNumber(phoneNumber) == null)
            {
                using (MySqlConnection c = GetNewConnection())
                {
                    c.Open();
                    new MySqlCommand("insert into user_table values(" +
                        GetNewIDInTable("user_table") + "," +
                        "'" + passwd + "'," +
                        "'" + nickname + "'," +
                        "'该用户未填写签名'," +
                        "'" + phoneNumber + "'," +
                        schoolID + "," +
                        "'" + avatarUri + "'," +
                        gradeID + "," +
                        "'ORDINARY_STUDENT')", c)
                        .ExecuteNonQuery();
                    return true;
                }
            }
            else
                return false;
        }

        private static UserInfo.Role StringToUserRole(string rawStr)
        {
            switch (rawStr)
            {
                case "WEBMASTER":
                    return UserInfo.Role.Webmaster;
                case "COFOUNDER":
                    return UserInfo.Role.CoFounder;
                case "OBSERVER":
                    return UserInfo.Role.Observer;
                case "SCHOOL_LEADER":
                    return UserInfo.Role.SchoolLeader;
                case "GRADE_LEADER":
                    return UserInfo.Role.GradeLeader;
                case "CLASS_LEADER":
                    return UserInfo.Role.ClassLeader;
                case "ORDINARY_STUDENT":
                    return UserInfo.Role.OrdinaryStudent;
                case "TEACHER":
                    return UserInfo.Role.Teacher;
                default:
                    throw new Exception("无法将" + rawStr + "转换为任何匹配的UserInfo.Role。");
            }
        }

        private static uint GetNewIDInTable(string table_name)
        {
            uint ID;
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                MySqlCommand cmd = new MySqlCommand("select max(id) from " + table_name, c);
                MySqlDataReader r = cmd.ExecuteReader();
                r.Read();
                ID = r.GetUInt32(0) + 1;
                return ID;
            }
        }
    }
}
