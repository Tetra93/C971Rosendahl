﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971Rosendahl.Models
{
    public class Notes
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int CourseID { get; set; }

        public string Name { get; set; }

        public string Contents { get; set; }        
    }
}
