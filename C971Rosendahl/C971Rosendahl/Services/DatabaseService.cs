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

        public static async Task<List<Term>> GetTerms()
        {
            await Init();

            List<Term> terms = await _db.Table<Term>().ToListAsync();

            return terms;
        }

        public static async Task RemoveTerm(int id)
        {
            await Init();
            List<Course> courses = (List<Course>)await GetCourseByTerm(id);
            foreach (Course course in courses)
            {
                await RemoveCourse(course.CourseId);
            }
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

        public static async Task UpdateCourse(int courseId, int instructorId, string name, DateTime StartDate, DateTime EndDate, bool notifications, string description)
        {
            await Init();
            var courseQuery = await _db.Table<Course>()
                .Where(i => i.CourseId == courseId)
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

        public static async Task UpdateCourse(int courseId, int completionStatus)
        {
            await Init();
            var courseQuery = await _db.Table<Course>()
                .Where(i => i.CourseId == courseId)
                .FirstOrDefaultAsync();

            if (courseQuery != null)
            {
                courseQuery.CompletionStatus = completionStatus;

                await _db.UpdateAsync(courseQuery);
            }
        }
        
        public static async Task UpdateCourse(int courseId, bool notifications)
        {
            await Init();
            var courseQuery = await _db.Table<Course>()
                .Where(i => i.CourseId == courseId)
                .FirstOrDefaultAsync();

            if (courseQuery != null)
            {
                courseQuery.DateNotifications = notifications;

                await _db.UpdateAsync(courseQuery);
            }
        }

        public static async Task<List<Course>> GetCourseByTerm(int termId)
        {
            await Init();
            
            List<Course> courses = await _db.Table<Course>()
                .Where (i =>  i.TermId == termId)
                .ToListAsync();

            return courses;
        }

        public static async Task<Course> GetSpecificCourse(int courseId)
        {
            await Init();

            Course course = await _db.Table<Course>()
                .Where(i => i.CourseId == courseId)
                .FirstOrDefaultAsync();

            return course;
        }

        public static async Task<List<Course>> GetCourse()
        {
            await Init();

            List<Course> courses = await _db.Table<Course>().ToListAsync();

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

        public static async Task<List<Instructor>> GetInstructor()
        {
            await Init();

            var instructors = await _db.Table<Instructor>().ToListAsync();

            return instructors;
        }

        public static async Task<Instructor> GetInstructorById(int id)
        {
            await Init();

            Instructor instructorSearch = await _db.Table<Instructor>()
                .Where (i => i.InstructorId == id)
                .FirstOrDefaultAsync();

            return instructorSearch;
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

        public static async Task<List<Note>> GetNote()
        {
            await Init();

            var notes = await _db.Table<Note>().ToListAsync();

            return notes;
        }

        public static async Task<List<Note>> GetNotesById(int id)
        {
            await Init();

            var notes = await _db.Table<Note>()
                .Where (i => i.CourseID == id)
                .ToListAsync();

            return notes;
        }

        public static async Task RemoveNote(int id)
        {
            await Init();

            await _db.DeleteAsync<Note>(id);
        }

        #endregion

        #region Assessment

        public static async Task AddAssessment(int courseId, string name, string type, string description, DateTime dueDate, bool notifications, string submissionStatus, string completionStatus)
        {
            await Init();
            var assessment = new Assessment()
            {
                CourseId = courseId,
                Name = name,
                Type = type,
                Description = description,
                DueDate = dueDate,
                Notifications = notifications,
                SubmissionStatus = submissionStatus,
                CompletionStatus = completionStatus

            };
            await _db.InsertAsync(assessment);

        }

        public static async Task UpdateAssessment(int assessmentId, string name, string description, DateTime dueDate, string submissionStatus, string completionStatus)
        {
            await Init();
            var assessmentQuery = await _db.Table<Assessment>()
                .Where(i => i.AssessmentId == assessmentId)
                .FirstOrDefaultAsync();

            if (assessmentQuery != null)
            {
                assessmentQuery.Name = name;
                assessmentQuery.Description = description;
                assessmentQuery.DueDate = dueDate;
                assessmentQuery.SubmissionStatus = submissionStatus;
                assessmentQuery.CompletionStatus = completionStatus;

                await _db.UpdateAsync(assessmentQuery);
            }
        }

        public static async Task UpdateAssessment(int assessmentId, bool notifications)
        {
            await Init();
            var assessmentQuery = await _db.Table<Assessment>()
                .Where(i => i.AssessmentId == assessmentId)
                .FirstOrDefaultAsync();

            if (assessmentQuery != null)
            {
                assessmentQuery.Notifications = notifications;

                await _db.UpdateAsync(assessmentQuery);
            }
        }

        public static async Task<List<Assessment>> GetAssessment(int courseId)
        {
            await Init();

            var assessments = await _db.Table<Assessment>()
                .Where(i => i.CourseId == courseId)
                .ToListAsync();

            return assessments;
        }

        public static async Task<List<Assessment>> GetAssessment()
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

        #region Load Sample Data

        public static async Task LoadSampleData()
        {
            await Init();

            Instructor instructor1 = new Instructor
            {
                Name = "Jacob Rosendahl",
                Phone = "123-456-7890",
                Email = "jrosendahl@fakemail.com"
            };

            await _db.InsertAsync(instructor1);

            Term term1 = new Term
            {
                Name = "Term 1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(180)

            };

            await _db.InsertAsync(term1);

            Course course1 = new Course
            {
                Name = "Mobile Application Development Using C# - C971",
                CompletionStatus = 1,
                Description = "Mobile Application Development Using C# introduces " +
                "students to programming for mobile devices. " +
                "Building on students’ previous knowledge of programming in C#, " +
                "this course investigates Xamarin.Forms and how it can be used to " +
                "build a mobile application. This course explores a broad range of topics, " +
                "including mobile user interface design and development; building applications " +
                "that adapt to different mobile devices and platforms; managing data using a " +
                "local database; and consuming REST-based web services. There are several " +
                "prerequisites for this course: Software I and II, and UI Design.",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,//DateTime.Now.AddDays(30),
                DateNotifications = true,
                TermId = 1,
                InstructorId = 1
            };

            await _db.InsertAsync(course1);

            Course course2 = new Course
            {
                Name = "Software I – C# - C968",
                CompletionStatus = 4,
                Description = "Software I - C# builds object-oriented " +
                "programming expertise and introduces powerful new tools " +
                "for C# application development. You will learn about and " +
                "put into action: class design, exception handling, and " +
                "other object-oriented principles and constructs to develop " +
                "software that meets business requirements. This course " +
                "requires foundational knowledge of object-oriented programming. " +
                "Scripting and Programming: Foundations and Scripting and " +
                "Programming: Applications are prerequisites for this course.",
                StartDate = DateTime.Now.AddDays(30),
                EndDate = DateTime.Now,//DateTime.Now.AddDays(60),
                DateNotifications = true,
                TermId = 1,
                InstructorId = 1
            };

            await _db.InsertAsync(course2);

            Course course3 = new Course
            {
                Name = "Software II – Advanced C# - C969",
                CompletionStatus = 4,
                Description = "Software II - Advanced C# refines " +
                "object-oriented programming expertise and builds database " +
                "and file server application development skills. You will " +
                "learn about and put into action lambda expressions, " +
                "collections, and input/output to develop software with " +
                "C# that meets business requirements. This course requires " +
                "intermediate expertise in object-oriented programming and " +
                "the C# language. The prerequisite for this course is Software I - C#.",
                StartDate = DateTime.Now.AddDays(60),
                EndDate = DateTime.Now,//DateTime.Now.AddDays(90),
                DateNotifications = true,
                TermId = 1,
                InstructorId = 1
            };

            await _db.InsertAsync(course3);

            Course course4 = new Course
            {
                Name = "Advanced Data Management - D191",
                CompletionStatus = 4,
                Description = "Advanced Data Management enables " +
                "organizations to extract and analyze raw data. " +
                "Skillful data management allows organizations to " +
                "discover and explore data in ways that uncover " +
                "trends, issues, and their root causes. In turn, " +
                "businesses are better equipped to capitalize on " +
                "opportunities and more accurately plan for the future. " +
                "As organizations continue to extract larger and more " +
                "detailed volumes of data, the need is rapidly growing " +
                "for IT professionals possessing data management skills. " +
                "These skills include performing advanced relational " +
                "data modeling as well as designing data marts, lakes, " +
                "and warehouses. This course will empower software " +
                "developers with the skills to build business logic at " +
                "the database layer to employ more stability and higher " +
                "data-processing speeds. Data analysts will gain the " +
                "ability to automate common tasks to summarize and " +
                "integrate data as they prepare it for analysis. Data " +
                "Management is a prerequisite for this course.",
                StartDate = DateTime.Now.AddDays(90),
                EndDate = DateTime.Now.AddDays(120),
                TermId = 1,
                InstructorId = 1
            };

            await _db.InsertAsync(course4);

            Course course5 = new Course
            {
                Name = "Network and Security - Foundations - C172",
                CompletionStatus = 4,
                Description = "Network and Security - Foundations " +
                "introduces students to the components of a computer " +
                "network and the concept and role of communication " +
                "protocols. The course covers widely used categorical " +
                "classifications of networks (e.g., LAN, MAN, WAN, " +
                "WLAN, PAN, SAN, CAN, and VPN) as well as network " +
                "topologies, physical devices, and layered abstraction. " +
                "The course also introduces students to basic concepts " +
                "of security, covering vulnerabilities of networks and " +
                "mitigation techniques, security of physical media, and " +
                "security policies and procedures. This course has no prerequisites.",
                StartDate = DateTime.Now.AddDays(120),
                EndDate = DateTime.Now.AddDays(150),
                TermId = 1,
                InstructorId = 1
            };

            await _db.InsertAsync(course5);

            Course course6 = new Course
            {
                Name = "Data Structures and Algorithms I - C949",
                CompletionStatus = 4,
                Description = "Data Structures and Algorithms I " +
                "covers the fundamentals of dynamic data " +
                "structures, such as bags, lists, stacks, queues, " +
                "trees, hash tables, and their associated algorithms. " +
                "With Python software as the basis, the course " +
                "discusses object-oriented design and abstract data " +
                "types as a design paradigm. The course emphasizes " +
                "problem solving and techniques for designing " +
                "efficient, maintainable software applications. " +
                "Students will implement simple applications using the techniques learned.",
                StartDate = DateTime.Now.AddDays(150),
                EndDate = DateTime.Now.AddDays(180),
                TermId = 1,
                InstructorId = 1
            };

            await _db.InsertAsync(course6);

            Note note = new Note
            {
                CourseID = 1,
                Name = "Test",
                Contents = "This is a note"
            };

            await _db.InsertAsync(note);

            Assessment assessment = new Assessment
            {
                CourseId = 1,
                Name = "Objective Assessment 1",
                Type = "Objective Assesment",
                Description = "This is an exam to demonstrate your understanding of Xamarin.Forms.",
                DueDate = DateTime.Now,
                Notifications = true,
                SubmissionStatus = "Not Submitted",
                CompletionStatus = "Not Completed"
            };

            await _db.InsertAsync(assessment);

            Assessment assessment1 = new Assessment
            {
                CourseId = 1,
                Name = "Performance Assessment 1",
                Type = "Performance Assessment",
                Description = "This performance assessment will demonstrate your understanding of Xamarin.Forms through the creation of your own mobile application.",
                DueDate = DateTime.Now.AddDays(21),
                Notifications = true,
                SubmissionStatus = "Submitted",
                CompletionStatus = "Not Completed"
            };

            await _db.InsertAsync(assessment1);

            DegreePlan.terms = (List<Term>)await GetTerms();
            DegreePlan.courses = await GetCourse();
            Settings.FirstRun = false;  
        }

        public static async Task ClearSampleData()
        {
            await Init();

            await _db.DropTableAsync<Term>();
            await _db.DropTableAsync<Course>();
            await _db.DropTableAsync<Assessment>();
            await _db.DropTableAsync<Instructor>();
            await _db.DropTableAsync<Note>();

            _db = null;
            Settings.FirstRun = true;
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
