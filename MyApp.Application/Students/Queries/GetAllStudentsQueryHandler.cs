using MediatR;
using Microsoft.EntityFrameworkCore;
using MyApp.Application.Students.Models;
using MyApp.Persistance;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp.Application.Students.Queries
{
    public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, StudentListViewModel>
    {
        private readonly MyAppDbContext _context;

        public GetAllStudentsQueryHandler(MyAppDbContext context)
        {
            _context = context;
        }

        public async Task<StudentListViewModel> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            // TODO: Set view model state based on user permissions.
            var model = new StudentListViewModel
            {
                Students = await _context.Students
                    .Select(StudentDto.Projection)
                    .OrderBy(p => p.StudentName)
                    .ToListAsync(cancellationToken),
                CreateEnabled = true
            };

            return model;
        }
    }
}
