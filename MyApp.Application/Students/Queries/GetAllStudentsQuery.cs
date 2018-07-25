using MediatR;
using MyApp.Application.Students.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Application.Students.Queries
{
    public class GetAllStudentsQuery : IRequest<StudentListViewModel>
    {
    }
}
