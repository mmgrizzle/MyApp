using MediatR;
using Microsoft.EntityFrameworkCore;
using MyApp.Application.Exceptions;
using MyApp.Application.Students.Models;
using MyApp.Domain.Entities;
using MyApp.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.Application.Students.Queries
{
    public class GetStudentQueryHandler : IRequestHandler<GetStudentQuery, StudentViewModel>
    {
        private readonly MyAppDbContext _context;

        public GetStudentQueryHandler(MyAppDbContext context)
        {
            _context = context;
        }

        public async Task<StudentViewModel> Handle(GetStudentQuery request, CancellationToken cancellationToken)
        {
            var student = await _context.Students
                .Where(p => p.StudentId == request.Id)
                .Select(StudentDto.Projection)
                .SingleOrDefaultAsync(cancellationToken);

            if (student == null)
            {
                throw new EntityNotFoundException(nameof(Student), request.Id);
            }

            // TODO: Set view model state based on user permissions.
            var model = new StudentViewModel
            {
                Student = student,
                EditEnabled = true,
                DeleteEnabled = false
            };

            return model;
        }
    }
}
