using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971Rosendahl.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int CourseId { get; set; }

        public int TermId { get; set; } = -1;

        public int InstructorId { get; set; } = -1;

        public string Name { set; get; }

        public int CompletionStatus { set; get; } = 4;

        public DateTime StartDate { set; get; } = DateTime.Now.Date;

        public DateTime EndDate { set; get; } = DateTime.Now.Date.AddDays(30);

        public bool DateNotifications { set; get; } = false;
        
        public string Description { set; get; }

    }
}
