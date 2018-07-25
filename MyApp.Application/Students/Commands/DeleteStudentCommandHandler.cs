using MediatR;
using MyApp.Application.Exceptions;
using MyApp.Domain.Entities;
using MyApp.Persistance;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.Application.Students.Commands
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand> 
    {
        private readonly MyAppDbContext _context;

        public DeleteStudentCommandHandler(MyAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Students.FindAsync(request.Id);

            if (entity == null)
            {
                throw new EntityNotFoundException(nameof(Student), request.Id);
            }

            var hasCourses = _context.StudentCourses.Any(c => c.StudentId == entity.StudentId);          
            if (hasCourses)
            {
                foreach (var item in _context.StudentCourses)
                {
                    if(item.StudentId == request.Id)
                    {
                        _context.StudentCourses.Remove(item);
                    }
                }
            }
            var hasReports = _context.Reports.Any(c => c.StudentId == entity.StudentId);
            if (hasReports)
            {
                foreach (var item in _context.Reports)
                {
                    if (item.StudentId == request.Id)
                    {
                        _context.Reports.Remove(item);
                    }
                }
            }

            _context.Students.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
