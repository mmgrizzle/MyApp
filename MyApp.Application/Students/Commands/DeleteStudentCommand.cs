using MediatR;

namespace MyApp.Application.Students.Commands
{
    public class DeleteStudentCommand : IRequest
    {
        public DeleteStudentCommand()
        {
        }

        public DeleteStudentCommand(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
