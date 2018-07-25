using MediatR;
using MyApp.Application.Students.Models;
using MyApp.Application.Students.Queries;
using MyApp.Domain.Entities;
using MyApp.Persistance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.Application.Students.Commands
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, StudentViewModel>
    {
        private readonly MyAppDbContext _context;
        private readonly IMediator _mediator;

        public CreateStudentCommandHandler(
            MyAppDbContext context,
            IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<StudentViewModel> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var entity = new Student
            {
                StudentId = request.StudentId,
                StudentName = request.StudentName
            };

            _context.Students.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return await _mediator.Send(new GetStudentQuery(entity.StudentId), cancellationToken);
        }
    }
}
