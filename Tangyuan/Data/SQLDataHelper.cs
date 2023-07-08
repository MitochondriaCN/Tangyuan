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
        static readonly MySqlConnection mysqlConn;
        static SQLDataHelper()
        {
            mysqlConn = new MySqlConnection();
        }

        /// <summary>
        /// 与数据库建立连接。在生命周期中，该方法应当只调用一次，因为只需要与一个数据库建立连接。
        /// </summary>
        /// <param name="connStr"></param>
        internal static void SetNewConnection(string host, uint port, string database, string username, string passwd)
        {
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Server = host;
            sb.Database = database;
            sb.Port = port;
            sb.UserID = username;
            sb.Password = passwd;
            sb.SslMode = MySqlSslMode.None;
            mysqlConn.ConnectionString = sb.ToString();
            try
            {
                mysqlConn.Open();
            }
            catch
            {
                mysqlConn.Close();
                throw;
            }
        }

        /// <summary>
        /// 发新帖
        /// </summary>
        /// <param name="query"></param>
        internal static void NewPost(uint authorID, XmlDocument content)
        {
            //TODO
        }

        internal static List<PostInfo> GetRecentPosts(uint number)
        {
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
}
