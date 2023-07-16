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
        private const string host = "81.68.124.30";
        private const uint port = 3306;
        private const string database = "tangyuan";
        private const string username = "tangyuan";
        private const string passwd = "Fuyuxuan372819";

        /// <summary>
        /// 获取一个新的数据库连接对象。
        /// </summary>
        /// <param name="connStr"></param>
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
        /// <param name="query"></param>
        internal static void NewPost(uint authorID, XmlDocument content)
        {
            //TODO
        }

        /// <summary>
        /// 获取最近帖子。
        /// </summary>
        /// <param name="number">指定获取帖子数</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal static List<PostInfo> GetRecentPosts(uint number)
        {
            using (MySqlConnection mysqlConn = GetNewConnection())
            {
                mysqlConn.Open();
                if (mysqlConn.State == ConnectionState.Open)
                {
                    List<PostInfo> posts = new List<PostInfo>();
                    try
                    {
                        MySqlCommand cmd = new MySqlCommand("SELECT * FROM post_table order by post_date desc", mysqlConn);
                        MySqlDataReader data = cmd.ExecuteReader();
                        while (data.Read())
                        {
                            posts.Add(new PostInfo(data.GetUInt32("id"), data.GetUInt32("author_id"), data.GetDateTime("post_date"), data.GetUInt32("likes"),
                                data.GetUInt32("views"), XDocument.Parse(data.GetString("content"))));
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
                    return new PostInfo(r.GetUInt32("id"), r.GetUInt32("author_id"), r.GetDateTime("post_date"),
                        r.GetUInt32("likes"), r.GetUInt32("views"), XDocument.Parse(r.GetString("content")));
                else
                    return null;
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
                        r.GetDateTime("date"),
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
                        reader.GetString("phone_number"),
                        reader.GetUInt32("school_id"),
                        reader.GetString("avatar"),
                        reader.GetUInt32("grade_code"));
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
                        reader.GetString("phone_number"),
                        reader.GetUInt32("school_id"),
                        reader.GetString("avatar"),
                        reader.GetUInt32("grade_code"));
                }
                return null;
            }
        }
    }
}
