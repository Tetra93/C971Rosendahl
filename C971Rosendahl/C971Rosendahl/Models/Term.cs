using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971Rosendahl.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set;}
    }
}
