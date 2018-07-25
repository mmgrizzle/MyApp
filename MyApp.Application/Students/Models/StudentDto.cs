using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MyApp.Application.Students.Models
{
    public class StudentDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }

        public static Expression<Func<Student, StudentDto>> Projection
        {
            get
            {
                return s => new StudentDto
                {
                    StudentId = s.StudentId,
                    StudentName = s.StudentName,
                };
            }
        }

        public static StudentDto Create(Student student)
        {
            return Projection.Compile().Invoke(student);
        }
    }
}
