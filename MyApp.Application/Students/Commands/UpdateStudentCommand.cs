using MediatR;
using MyApp.Application.Students.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Application.Students.Commands
{
    public class UpdateStudentCommand : IRequest<StudentDto>
    {
        public StudentDto Student { get; set; }
    }
}
