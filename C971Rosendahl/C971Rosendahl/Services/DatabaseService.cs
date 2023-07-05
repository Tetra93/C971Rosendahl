using C971Rosendahl.Models;
using C971Rosendahl.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Course = C971Rosendahl.Models.Course;

namespace C971Rosendahl.Services
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection _db;
        private static SQLiteConnection _dbConnection;

        #region Terms

        public static async Task AddTerm(string Name, DateTime StartDate, DateTime EndDate)
        {
            await Init();
            var term = new Term()
            {
                Name = Name,
                StartDate = StartDate,
                EndDate = EndDate
            };
            await _db.InsertAsync(term);

        }

        public static async Task UpdateTerm(int id, string Name, DateTime StartDate, DateTime EndDate)
        {
            await Init();
            var termQuery = await _db.Table<Term>()
                    .Where(i => i.Id == id)
                    .FirstOrDefaultAsync();

            if (termQuery != null) 
            {
                termQuery.Name = Name;
                termQuery.StartDate = StartDate;
                termQuery.EndDate = EndDate;

                await _db.UpdateAsync(termQuery);
            }
        }

        #endregion

        #region Courses

        #endregion

        #region Instructor

        #endregion

        #region Notes

        #endregion

        #region Assessment

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

            await _db.CreateTableAsync<Term>();
            await _db.CreateTableAsync<Course>();
            await _db.CreateTableAsync<Assessment>();
            await _db.CreateTableAsync<Instructor>();
            await _db.CreateTableAsync<Notes>();

        }
    }
}
