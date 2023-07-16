using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangyuan.Data
{
    internal static class LocalSQLDataHelper
    {
        private const string databaseName = "local_tangyuan_db.db3";
        private const SQLiteOpenFlags flags =
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create |
            SQLiteOpenFlags.SharedCache;
        private static string databasePath = Path.Combine(FileSystem.AppDataDirectory, databaseName);

        private static SQLiteConnection conn;

        internal class LocalLoginInfo
        {
            public string LoginUserPhoneNumber { get; set; }
            public string LoginUserPassword { get; set; }
        }

        static LocalSQLDataHelper()
        {
            conn = new SQLiteConnection(databasePath, flags);
            conn.CreateTable<LocalLoginInfo>();
            conn.Insert(new LocalLoginInfo() { LoginUserPassword = "Fuyuxuan372819", LoginUserPhoneNumber = "18993791251" });
        }

        internal static LocalLoginInfo GetLoginInfo()
        {
            try
            {
                return conn.Table<LocalLoginInfo>().First();
            }
            catch
            {
                return null;
            }
        }
    }
}
