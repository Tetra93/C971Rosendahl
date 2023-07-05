﻿using System;
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
        public int ID { get; set; }

        public int CourseID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set;}

        public bool Notifications { get; set; }

        public string SubmissionStatus { get; set; }

        public string CompletionStatus { get; set; }
    }
}
