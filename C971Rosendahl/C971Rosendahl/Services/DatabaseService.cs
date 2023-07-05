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
        //private static SQLiteConnection _dbConnection;

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

        public static async Task<IEnumerable<Term>> GetTerms()
        {
            await Init();
            var terms = await _db.Table<Term>().ToListAsync();
            return terms;
        }

        public static async Task RemoveTerm(int id)
        {
            await Init();
            await _db.DeleteAsync<Term>(id);
        }

        #endregion

        #region Courses

        public static async Task AddCourse(int termId, int instructorId, string name,  DateTime StartDate, DateTime EndDate, bool notifications, string description)
        {
            await Init();
            var course = new Course()
            {
                TermId = termId,
                InstructorId = instructorId,
                Name = name,
                StartDate = StartDate,
                EndDate = EndDate,
                DateNotifications = notifications,
                Description = description

            };
            await _db.InsertAsync(course);

        }

        public static async Task UpdateCourse(int termId, int instructorId, string name, DateTime StartDate, DateTime EndDate, bool notifications, string description)
        {
            await Init();
            var courseQuery = await _db.Table<Course>()
                .Where(i => i.TermId == termId)
                .FirstOrDefaultAsync();

            if (courseQuery != null) 
            {
                courseQuery.InstructorId = instructorId;
                courseQuery.Name = name;
                courseQuery.StartDate = StartDate;
                courseQuery.EndDate = EndDate;
                courseQuery.DateNotifications = notifications;
                courseQuery.Description = description;

                await _db.UpdateAsync(courseQuery);
            }
        }

        public static async Task<IEnumerable<Course>> GetCourse(int termId)
        {
            await Init();
            
            var courses = await _db.Table<Course>()
                .Where (i =>  i.TermId == termId)
                .ToListAsync();

            return courses;
        }

        public static async Task<IEnumerable<Course>> GetCourse()
        {
            await Init();

            var courses = await _db.Table<Course>().ToListAsync();

            return courses;
        }

        public static async Task RemoveCourse(int id)
        {
            await Init();

            await _db.DeleteAsync<Course>(id);
        }

        #endregion

        #region Instructor

        public static async Task AddInstructor(string name, string phone, string email)
        {
            await Init();
            var instructor = new Instructor()
            {
                Name = name,
                Phone = phone,
                Email = email

            };
            await _db.InsertAsync(instructor);

        }

        public static async Task UpdateInstructor(int instructorId, string name, string phone, string email)
        {
            await Init();
            var instructorQuery = await _db.Table<Instructor>()
                .Where(i => i.InstructorId == instructorId)
                .FirstOrDefaultAsync();

            if (instructorQuery != null)
            {
                instructorQuery.Name = name;
                instructorQuery.Phone = phone;
                instructorQuery.Email = email;

                await _db.UpdateAsync(instructorQuery);
            }
        }

        public static async Task<IEnumerable<Instructor>> GetInstructor()
        {
            await Init();

            var instructors = await _db.Table<Instructor>().ToListAsync();

            return instructors;
        }

        public static async Task RemoveInstructor(int id)
        {
            await Init();

            await _db.DeleteAsync<Instructor>(id);
        }

        #endregion

        #region Notes

        public static async Task AddNote(int courseId, string name, string contents)
        {
            await Init();
            var note = new Note()
            {
                CourseID = courseId,
                Name = name,
                Contents = contents

            };
            await _db.InsertAsync(note);

        }

        public static async Task UpdateNote(int noteId, string name, string contents)
        {
            await Init();
            var noteQuery = await _db.Table<Note>()
                .Where(i => i.NoteId == noteId)
                .FirstOrDefaultAsync();

            if (noteQuery != null)
            {
                noteQuery.Name = name;
                noteQuery.Contents = contents;

                await _db.UpdateAsync(noteQuery);
            }
        }

        public static async Task<IEnumerable<Note>> GetNote()
        {
            await Init();

            var notes = await _db.Table<Note>().ToListAsync();

            return notes;
        }

        public static async Task RemoveNote(int id)
        {
            await Init();

            await _db.DeleteAsync<Note>(id);
        }

        #endregion

        #region Assessment

        public static async Task AddAssessment(int courseId, string name, string type, string description, DateTime startDate, DateTime endDate, bool notifications, string submissionStatus, string completionStatus)
        {
            await Init();
            var assessment = new Assessment()
            {
                CourseId = courseId,
                Name = name,
                Type = type,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                Notifications = notifications,
                SubmissionStatus = submissionStatus,
                CompletionStatus = completionStatus

            };
            await _db.InsertAsync(assessment);

        }

        public static async Task UpdateAssessment(int assessmentId, string name, string type, string description, DateTime startDate, DateTime endDate, bool notifications, string submissionStatus, string completionStatus)
        {
            await Init();
            var assessmentQuery = await _db.Table<Assessment>()
                .Where(i => i.AssessmentId == assessmentId)
                .FirstOrDefaultAsync();

            if (assessmentQuery != null)
            {
                assessmentQuery.Name = name;
                assessmentQuery.Type = type;
                assessmentQuery.Description = description;
                assessmentQuery.StartDate = startDate;
                assessmentQuery.EndDate = endDate;
                assessmentQuery.Notifications = notifications;
                assessmentQuery.SubmissionStatus = submissionStatus;
                assessmentQuery.CompletionStatus = completionStatus;

                await _db.UpdateAsync(assessmentQuery);
            }
        }

        public static async Task<IEnumerable<Assessment>> GetAssessment(int courseId)
        {
            await Init();

            var assessments = await _db.Table<Assessment>()
                .Where(i => i.CourseId == courseId)
                .ToListAsync();

            return assessments;
        }

        public static async Task<IEnumerable<Assessment>> GetAssessment()
        {
            await Init();

            var assessments = await _db.Table<Assessment>().ToListAsync();

            return assessments;
        }

        public static async Task RemoveAssessment(int id)
        {
            await Init();

            await _db.DeleteAsync<Assessment>(id);
        }

        #endregion

        static async Task Init()
        {
            if (_db != null)
            {
                return;
            }

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "DegreePlan.db");

            _db = new SQLiteAsyncConnection(databasePath);
           // _dbConnection = new SQLiteConnection(databasePath);

            await _db.CreateTableAsync<Term>();
            await _db.CreateTableAsync<Course>();
            await _db.CreateTableAsync<Assessment>();
            await _db.CreateTableAsync<Instructor>();
            await _db.CreateTableAsync<Note>();

        }
    }
}
