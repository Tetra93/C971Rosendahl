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

        public int TermID { get; set; }

        public string Name { set; get; }

        public DateTime StartDate { set; get; }

        public DateTime EndDate { set; get; }

        public bool DateNotifications { set; get; }
        
        public string Description { set; get; }

        public string Notes { set; get; }



    }
}
