using System.Collections.Generic;

namespace MyApp.Domain.Entities
{
    public class Professor
    {
        public Professor()
        {
            Courses = new HashSet<Course>();
        }

        public int ProfessorId { get; set; }
        public string ProfessorName { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
