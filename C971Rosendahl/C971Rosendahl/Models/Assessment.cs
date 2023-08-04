using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using SQLite;

namespace C971Rosendahl.Models
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int AssessmentId { get; set; }

        public int CourseId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set;}

        public bool Notifications { get; set; } = false;

        public string CompletionStatus { get; set; }
    }
}
