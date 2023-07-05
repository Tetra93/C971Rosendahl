using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

namespace C971Rosendahl.Models
{
    internal class Assessment
    {
        public static string Name { get; set; }

        public static string Description { get; set; }

        public static DateTime StartDate { get; set; }

        public static DateTime EndDate { get; set;}

        public static bool Notifications { get; set; }

        public static string SubmissionStatus { get; set; }

        public static string CompletionStatus { get; set; }


    }
}
