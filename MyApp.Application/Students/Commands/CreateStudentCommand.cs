using MediatR;
using MyApp.Application.Students.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Application.Students.Commands
{
    public class CreateStudentCommand : IRequest<StudentViewModel>
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
    }
}
