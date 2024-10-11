using AutoMapper;
using MediatR;
using StudentApp.Core;
using StudentApp.Core.IRepositories;
using StudentApp.Infrastructure;

namespace StudentApp.Application.CQRS.StudentCommandQuery.Query
{
    public class GetByIdStudentQuery : IRequest<ResultModel<GetAllStudentsQueryResponse>>
    {
        public int Id { get; set; }
    }

    public class GetStudentQueryResponse
    {
        public int Id { get; set; }
        public string CreateDateDisplay { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhonNumber { get; set; }
    }

    public class GetStudentQueryHandler : IRequestHandler<GetByIdStudentQuery, ResultModel<GetAllStudentsQueryResponse>>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public GetStudentQueryHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }

        public async Task<ResultModel<GetAllStudentsQueryResponse>> Handle(GetByIdStudentQuery request, CancellationToken cancellationToken)
        {
            var student = await studentRepository.GetByIdAsync(request.Id);

            if (student == null)
                return ResultModel<GetAllStudentsQueryResponse>.NotFound();

            var getStudentQueryResponse = mapper.Map<Student, GetAllStudentsQueryResponse>(student);

            return ResultModel<GetAllStudentsQueryResponse>.Sucsess(getStudentQueryResponse);
        }
    }
}
