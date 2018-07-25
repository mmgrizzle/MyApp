using MediatR;
using MyApp.Application.Exceptions;
using MyApp.Application.Students.Models;
using MyApp.Domain.Entities;
using MyApp.Persistance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.Application.Students.Commands
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, StudentDto>
    {
        private readonly MyAppDbContext _context;

        public UpdateStudentCommandHandler(MyAppDbContext context)
        {
            _context = context;
        }

        public async Task<StudentDto> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var student = request.Student;

            var entity = await _context.Students
                .FindAsync(student.StudentId);

            if (entity == null)
            {
                throw new EntityNotFoundException(nameof(Student), student.StudentId);
            }

            entity.StudentId = student.StudentId;
            entity.StudentName = student.StudentName;

            await _context.SaveChangesAsync(cancellationToken);

            return StudentDto.Create(entity);
        }
    }
}
