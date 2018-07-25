using System.Collections.Generic;

namespace MyApp.Domain.Entities
{
    public class Student
    {
        //initialize the collection of courses & reports
        public Student()
        {
            Courses = new HashSet<StudentCourse>();
            Reports = new HashSet<Report>();
        }       

        public int StudentId { get; set; }
        public string StudentName { get; set; }

        public ICollection<StudentCourse> Courses { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
