using AutoMapper;
using MediatR;
using StudentApp.Application.CQRS.StudentNotification;
using StudentApp.Core;
using StudentApp.Core.IRepositories;
using StudentApp.Infrastructure;

namespace StudentApp.Application.CQRS.StudentCommandQuery.Command
{
    public class CreateStudentCommand : IRequest<ResultModel<int>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhonNumber { get; set; }
    }

    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, ResultModel<int>>
    {
        #region Dependency Injection

        private readonly IStudentRepository studentRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public CreateStudentCommandHandler(
            IStudentRepository studentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMediator mediator)
        {
            this.studentRepository = studentRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.mediator = mediator;
        }

        #endregion

        public async Task<ResultModel<int>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var validation = Validation(request);

            if (validation.Status == Status.ValidationError)
                return validation;

            var student = mapper.Map<CreateStudentCommand, Student>(request);

            await studentRepository.InsertStudentAsync(student);
            await unitOfWork.SaveChangesAsync();

            var sendNotificationToStudent = new SendNotificationToStudent
            {
                Message = "یک کاربر ایجاد شد",
                UserId = student.Id
            };

            await mediator.Publish(sendNotificationToStudent, cancellationToken);

            return ResultModel<int>.Sucsess(student.Id);
        }

        #region Validation
        private ResultModel<int> Validation(CreateStudentCommand createStudentCommand)
        {
            if (createStudentCommand == null ||
                String.IsNullOrEmpty(createStudentCommand.FirstName) ||
                String.IsNullOrEmpty(createStudentCommand.LastName))
            {
                return ResultModel<int>.ValidationError("فیلد های اجباری را تکمیل کنید");
            }

            return ResultModel<int>.Sucsess();
        }

        #endregion
    }
}