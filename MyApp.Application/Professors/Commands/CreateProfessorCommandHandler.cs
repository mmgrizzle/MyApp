using MediatR;
using MyApp.Application.Professors.Models;
using MyApp.Application.Professors.Queries;
using MyApp.Domain.Entities;
using MyApp.Persistance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.Application.Professors.Commands
{
    public class CreateProfessorCommandHandler : IRequestHandler<CreateProfessorCommand, ProfessorViewModel>
    {
        private readonly MyAppDbContext _context;
        private readonly IMediator _mediator;

        public CreateProfessorCommandHandler(
            MyAppDbContext context,
            IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<ProfessorViewModel> Handle(CreateProfessorCommand request, CancellationToken cancellationToken)
        {
            var entity = new Professor
            {
                ProfessorId = request.ProfessorId,
                ProfessorName = request.ProfessorName
            };

            _context.Professors.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return await _mediator.Send(new GetProfessorQuery(entity.ProfessorId), cancellationToken);
        }
    }
}
