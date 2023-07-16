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

        private static SQLiteAsyncConnection conn;

        internal class LocalLoginInfo
        {
            internal string LoginUserPhoneNumber { get; private set; }
            internal string LoginUserPassword { get; private set; }
        }

        static LocalSQLDataHelper()
        {
            conn = new SQLiteAsyncConnection(databasePath, flags);
            conn.CreateTableAsync(typeof(LocalLoginInfo));
        }

        internal static async Task<LocalLoginInfo> GetLoginInfoAsync()
        {
            try
            {
                return await conn.Table<LocalLoginInfo>().FirstAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}
