using MediatR;
using MyApp.Application.Professors.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Application.Professors.Commands
{
    public class CreateProfessorCommand : IRequest<ProfessorViewModel>
    {
        public int ProfessorId { get; set; }
        public string ProfessorName { get; set; }
    }
}
