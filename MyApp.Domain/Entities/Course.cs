using System.Collections.Generic;

namespace MyApp.Domain.Entities
{
    public class Course
    {
        //initialize the collection of students
        public Course()
        {
            Students = new HashSet<StudentCourse>();
        }

        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int ProfessorId { get; set; }    //add field for FK

        public Professor Professor { get; set; }
        public ICollection<StudentCourse> Students { get; set; }
    }
}
