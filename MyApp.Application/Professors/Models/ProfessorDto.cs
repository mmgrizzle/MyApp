using MyApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MyApp.Application.Professors.Models
{
    public class ProfessorDto
    {
        public int ProfessorId { get; set; }
        public string ProfessorName { get; set; }

        public static Expression<Func<Professor, ProfessorDto>> Projection
        {
            get
            {
                return p => new ProfessorDto
                {
                    ProfessorId = p.ProfessorId,
                    ProfessorName = p.ProfessorName
                };
            }
        }

        public static ProfessorDto Create(Professor professor)
        {
            return Projection.Compile().Invoke(professor);
        }
    }
}
