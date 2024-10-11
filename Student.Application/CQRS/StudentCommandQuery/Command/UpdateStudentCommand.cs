using MediatR;
using StudentApp.Application.CQRS.StudentNotification;
using StudentApp.Core.IRepositories;
using StudentApp.Infrastructure;

namespace StudentApp.Application.CQRS.StudentCommandQuery.Command
{

    public class UpdateStudentCommand : IRequest<ResultModel<int>>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhonNumber { get; set; }
    }

    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, ResultModel<int>>
    {
        #region Dependency Injection

        private readonly IStudentRepository studentRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly ExternalLogServices externalLogServices;

        public UpdateStudentCommandHandler(
            IStudentRepository studentRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            ExternalLogServices externalLogServices)
        {
            this.studentRepository = studentRepository;
            this.unitOfWork = unitOfWork;
            this.mediator = mediator;
            this.externalLogServices = externalLogServices;
        }

        #endregion

        public async Task<ResultModel<int>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var validation = Validation(request);

            if (validation.Status == Status.ValidationError)
                return validation;

            var student = await studentRepository.GetByIdAsync(request.Id);

            if (student is null)
                return ResultModel<int>.Error("ایتم مورد نظر یافت نشد");
            //throw new InvalidEntityStateException("ایتم مورد نظر یافت نشد");

            try
            {
                // برای تست logserver
                throw new Exception();

                student.FirstName = request.FirstName;
                student.LastName = request.LastName;
                student.PhonNumber = request.PhonNumber;

                studentRepository.UpdateStudent(student);
                await unitOfWork.SaveChangesAsync();

                var sendNotificationToStudent = new SendNotificationToStudent
                {
                    Message = "یک کاربر ویرایش شد",
                    UserId = request.Id
                };

                await mediator.Publish(sendNotificationToStudent, cancellationToken);
            }
            catch (Exception e)
            {
                externalLogServices.ExternalLog(e);
                return ResultModel<int>.Error("عملیات با شکست مواجه شد");
            }

            return ResultModel<int>.Sucsess(student.Id);
        }

        #region Validation
        private ResultModel<int> Validation(UpdateStudentCommand UpdateStudentCommand)
        {
            if (UpdateStudentCommand == null ||
                String.IsNullOrEmpty(UpdateStudentCommand.FirstName) ||
                String.IsNullOrEmpty(UpdateStudentCommand.LastName))
            {
                return ResultModel<int>.ValidationError("فیلد های اجباری را تکمیل کنید");
            }

            return ResultModel<int>.Sucsess();
        }

        #endregion
    }
}