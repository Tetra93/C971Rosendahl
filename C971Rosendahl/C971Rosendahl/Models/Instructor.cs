using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971Rosendahl.Models
{
    public class Instructor
    {
        [PrimaryKey, AutoIncrement]
        public int InstructorId { get; set; }

        public string Name { set; get; }

        public string Phone { set; get; }

        public string EmailAddress { set; get; }
    }
}
