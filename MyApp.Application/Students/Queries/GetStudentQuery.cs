using MediatR;
using MyApp.Application.Students.Models;

namespace MyApp.Application.Students.Queries
{
    public class GetStudentQuery : IRequest<StudentViewModel>
    {
        public GetStudentQuery()
        {
        }

        public GetStudentQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
