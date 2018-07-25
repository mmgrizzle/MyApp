using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Application.Students.Models
{
    public class StudentListViewModel
    {
        public IEnumerable<StudentDto> Students { get; set; }
        public bool CreateEnabled { get; set; }
    }
}
