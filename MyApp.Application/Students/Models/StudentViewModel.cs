using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Application.Students.Models
{
    public class StudentViewModel
    {
        public StudentDto Student { get; set; }
        public bool EditEnabled { get; set; }
        public bool DeleteEnabled { get; set; }
    }
}
