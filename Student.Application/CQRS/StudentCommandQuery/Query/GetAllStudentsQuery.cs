using AutoMapper;
using MediatR;
using StudentApp.Core;
using StudentApp.Core.IRepositories;
using StudentApp.Infrastructure;

namespace StudentApp.Application.CQRS.StudentCommandQuery.Query
{

    public class GetAllStudentsQuery : IRequest<ResultModel<List<GetAllStudentsQueryResponse>>>
    {
    }

    public class GetAllStudentsQueryResponse
    {
        public int Id { get; set; }
        public string CreateDateDisplay { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhonNumber { get; set; }
    }

    public class GetAllStudentQueryHandler : IRequestHandler<GetAllStudentsQuery, ResultModel<List<GetAllStudentsQueryResponse>>>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public GetAllStudentQueryHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }

        public async Task<ResultModel<List<GetAllStudentsQueryResponse>>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            var student = await studentRepository.GetAllStudentsAsync();

            if (student == null)
                return ResultModel<List<GetAllStudentsQueryResponse>>.NotFound();

            var getStudentQueryResponse = mapper.Map<List<Student>, List<GetAllStudentsQueryResponse>>(student);

            return ResultModel<List<GetAllStudentsQueryResponse>>.Sucsess(getStudentQueryResponse);
        }
    }
}
