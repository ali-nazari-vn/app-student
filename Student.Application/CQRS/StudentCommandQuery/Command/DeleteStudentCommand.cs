using MediatR;
using StudentApp.Application.CQRS.StudentNotification;
using StudentApp.Core.IRepositories;
using StudentApp.Infrastructure;

namespace StudentApp.Application.CQRS.StudentCommandQuery.Command
{
    public class DeleteStudentCommand : IRequest<ResultModel<bool>>
    {
        public int Id { get; set; }
    }

    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, ResultModel<bool>>
    {
        #region Dependency Injection

        private readonly IStudentRepository studentRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public DeleteStudentCommandHandler(
            IStudentRepository studentRepository,
            IUnitOfWork unitOfWork, 
            IMediator mediator)
        {
            this.studentRepository = studentRepository;
            this.unitOfWork = unitOfWork;
            this.mediator = mediator;
        }

        #endregion

        public async Task<ResultModel<bool>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await studentRepository.GetByIdAsync(request.Id);

            if (student is null)
                return ResultModel<bool>.Error("ایتم مورد نظر یافت نشد");

            studentRepository.DeleteStudent(student);
            await unitOfWork.SaveChangesAsync();

            var sendNotificationToStudent = new SendNotificationToStudent
            {
                Message = "یک کاربر حذف شد",
                UserId = request.Id
            };

            await mediator.Publish(sendNotificationToStudent, cancellationToken);

            return ResultModel<bool>.Sucsess(true);
        }
    }
}
