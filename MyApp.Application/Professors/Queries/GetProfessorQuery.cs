using MediatR;
using MyApp.Application.Professors.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Application.Professors.Queries
{
    public class GetProfessorQuery : IRequest<ProfessorViewModel>
    {
        public GetProfessorQuery()
        {
        }
        
        public GetProfessorQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
