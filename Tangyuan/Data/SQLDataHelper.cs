﻿using System.Data;
using System.Xml.Linq;
using MySqlConnector;

namespace Tangyuan.Data
{
    /// <summary>
    /// SQL数据帮助类。除本类外，软件内所有的时间均应使用本地时。
    /// </summary>
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
            sb.ConnectionTimeout = 10;
            MySqlConnection conn = new MySqlConnection(sb.ToString());
            return conn;
        }

        /// <summary>
        /// 发新帖
        /// </summary>
        internal static uint NewPost(uint authorID, XDocument content)
        {
            uint postID = GetNewIDInTable("post_table");
            using (MySqlConnection conn = GetNewConnection())//你他妈的，每一个指令都要一个新connection，烦死了
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("insert into post_table values(" + postID +
                    "," + authorID +
                    "," + GetUserInfoByID(authorID).SchoolID +
                    ",NOW()" +
                    ",0,0,'" + content.ToString(SaveOptions.DisableFormatting) + "',0,0)", conn);
                cmd.ExecuteReader();
                return postID;
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
                                    XDocument.Parse(data.GetString("content")),
                                    data.GetBoolean("is_in_collection"),
                                    data.GetUInt32("collection_id")));
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
        /// 获取用户正在关注的用户ID
        /// </summary>
        /// <param name="id">要获取关注用户的用户ID</param>
        /// <returns></returns>
        internal static uint[] GetFollowingsByUserID(uint id)
        {
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                MySqlDataReader r = new MySqlCommand("select follow_id from follow_table where user_id=" + id, c).ExecuteReader();
                List<uint> ids = new();
                while (r.Read())
                {
                    ids.Add(r.GetUInt32(0));
                }
                return ids.ToArray();
            }
        }

        /// <summary>
        /// 获取用户粉丝ID
        /// </summary>
        /// <param name="id">要获取粉丝的用户ID</param>
        /// <returns></returns>
        internal static uint[] GetFansByUserID(uint id)
        {
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                MySqlDataReader r = new MySqlCommand("select user_id from follow_table where follow_id=" + id, c).ExecuteReader();
                List<uint> ids = new();
                while (r.Read())
                {
                    ids.Add(r.GetUInt32(0));
                }
                return ids.ToArray();
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
        /// 获取指定用户指定日期（不含）之前发表的帖子，时间由近到远。
        /// </summary>
        /// <param name="userID">指定用户ID</param>
        /// <param name="time">指定日期，应当使用本地时</param>
        /// <param name="limit">获取帖子上限</param>
        /// <returns>指定用户发表的帖子</returns>
        internal static List<PostInfo> GetPostsByUserID(uint userID, DateTime time, uint limit = 50)
        {
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                MySqlCommand m = new MySqlCommand(
                    "select * from post_table where author_id=" + userID + " && post_date < '" + time.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                    "order by post_date desc limit " + limit,
                    c);
                MySqlDataReader r = m.ExecuteReader();
                List<PostInfo> lst = new();
                while (r.Read())
                {
                    lst.Add(GetPostByID(r.GetUInt32(0)));
                }
                return lst;
            }
        }

        /// <summary>
        /// 设置新关注关系。
        /// </summary>
        /// <param name="srcUserID"></param>
        /// <param name="targetUserID"></param>
        internal static void NewFollowRelation(uint srcUserID, uint targetUserID)
        {
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                new MySqlCommand("insert into follow_table (id,user_id,follow_id) values (" +
                    GetNewIDInTable("follow_table") + "," +
                    srcUserID + "," +
                    targetUserID + ")", c).ExecuteNonQuery();
            }
            
        }

        /// <summary>
        /// 删除现有关注关系。
        /// </summary>
        /// <param name="srcUserID"></param>
        /// <param name="targetUserID"></param>
        internal static void DeleteFollowRelation(uint srcUserID, uint targetUserID)
        {
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                new MySqlCommand("delete from follow_table where user_id=" + srcUserID + " and follow_id=" + targetUserID, c).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 获取指定用户的贴子总数
        /// </summary>
        /// <param name="userID">指定用户ID</param>
        /// <returns></returns>
        internal static uint GetCountOfPostOfUser(uint userID)
        {
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                MySqlDataReader r = new MySqlCommand("select count(*) from post_table where author_id=" + userID, c).ExecuteReader();
                r.Read();
                return r.GetUInt32(0);
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
                        XDocument.Parse(r.GetString("content")),
                        r.GetBoolean("is_in_collection"),
                        r.GetUInt32("collection_id"));
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

        internal static CollectionInfo GetCollectionByID(uint id)
        {
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                MySqlDataReader r = new MySqlCommand("select * from collection_table where id=" + id + " limit 1", c).ExecuteReader();
                while (r.Read())
                {
                    return new CollectionInfo(
                        r.GetUInt32("id"),
                        r.GetUInt32("author_id"),
                        r.GetString("collection_name"),
                        r.GetString("description"));
                }
                return null;
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

        /// <summary>
        /// 为指定帖子加上指定数量的阅读数。
        /// </summary>
        /// <param name="postID">指定帖子ID</param>
        /// <param name="value">增加的阅读数（可为负）</param>
        internal static void AddPostViewByID(uint postID, int value = 1)
        {
            using(MySqlConnection mc = GetNewConnection()) 
            {
                mc.Open();
                new MySqlCommand("update post_table set views=views" + (value >= 0 ? "+" + value : value) + " where id=" + postID, mc)
                    .ExecuteNonQueryAsync();
            }
        }

        internal static void AddPostLikeByID(uint postID, int value = 1)
        {
            using (MySqlConnection mc = GetNewConnection())
            {
                mc.Open();
                new MySqlCommand("update post_table set likes=likes" + (value >= 0 ? "+" + value : value) + " where id=" + postID, mc)
                    .ExecuteNonQueryAsync();
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
        /// 更新用户信息
        /// </summary>
        /// <param name="ui">用户信息（必须是已经存在的用户）</param>
        internal static void UpdateUser(UserInfo ui)
        {
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                new MySqlCommand("update user_table set nickname='" + ui.Nickname + "'," +
                    "signature='" + ui.Signature + "'," +
                    "avatar='" + ui.Avatar + "' " +
                    "where id=" + ui.UserID, c).ExecuteNonQuery();
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
        /// 根据ID获取商品信息。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static ProductInfo GetProductInfoByID(uint id)
        {
            using (MySqlConnection c = GetNewConnection())
            {
                c.Open();
                MySqlDataReader r = new MySqlCommand("select * from product_table where id=" + id + " limit 1", c).ExecuteReader();
                while (r.Read())
                {
                    return new ProductInfo(r.GetUInt32("id"),
                        r.GetUInt32("publisher_id"),
                        r.GetUInt32("category_id"),
                        r.GetUInt32("views"),
                        r.GetBoolean("take_off_after_selling"),
                        XDocument.Parse(r.GetString("content")));
                }
                return null;
            }
        }

        internal static List<ProductInfo> GetProductInfosRandomly(uint limit)
        {
            MySqlDataReader r = GetRandomly("product_table", "id", limit);
            List<ProductInfo> pis = new();
            while (r.Read())
            {
                pis.Add(GetProductInfoByID(r.GetUInt32("id")));
            }
            return pis;
        }

        /// <summary>
        /// 随机获取指定表中数据。该算法来自于CSDN。
        /// </summary>
        /// <param name="table">指定表名</param>
        /// <param name="primaryKey">主键名</param>
        /// <param name="limit">最多条目数</param>
        /// <returns></returns>
        private static MySqlDataReader GetRandomly(string table, string primaryKey = "id", uint limit = 100)
        {
            MySqlConnection c = GetNewConnection();
            c.Open();
            return new MySqlCommand("select * from " + table + " as t1 where t1." + primaryKey + ">=" +
                "(RAND()*(SELECT MAX(" + primaryKey + ") FROM " + table + ")) LIMIT " + limit,
                c).ExecuteReader();
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
