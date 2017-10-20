using AsistentePagos.Core.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsistentePagos.Core.Utils
{
    public class SqLiteHelper
    {
        public string createDatabase(string path)
        {
            try
            {
                var connection = new SQLiteAsyncConnection(path);
                connection.CreateTableAsync<User>();
                return "Database created";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> insertUpdateData(User data, string path)
        {
            try
            {
                var db = new SQLiteAsyncConnection(path);
                int resultInsert = await db.InsertAsync(data);
                if ( resultInsert != 0)
                    await db.UpdateAsync(data);
                return "Single data file inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> UpdateData(User data, string path)
        {
            try
            {
                var db = new SQLiteAsyncConnection(path);
                await db.UpdateAsync(data);
                return "Single data file inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public User FindUser(string path)
        {
            try
            {
                var db = new SQLiteConnection(path);
                var queryResult = db.Query<User>("SELECT * FROM User");

                return queryResult[0];
            }
            catch (SQLiteException ex)
            {
                return null;
            }
        }


    }
}
