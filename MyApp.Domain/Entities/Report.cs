using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Domain.Entities
{
    public class Report
    {
        public int ReportId { get; set; }
        public int StudentId { get; set; }

        public Student Student { get; set; }
    }
}
