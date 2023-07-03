using C971Rosendahl.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace C971Rosendahl.Services
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection _db;
        private static SQLiteConnection _dbConnection;

        #region DegreePlan

        #endregion

        #region Courses

        #endregion

        static async Task Init()
        {
            if (_db != null)
            {
                return;
            }

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "DegreePlan.db");

            _db = new SQLiteAsyncConnection(databasePath);
            _dbConnection = new SQLiteConnection(databasePath);

            await _db.CreateTableAsync<DegreePlan>();

        }
    }
}
