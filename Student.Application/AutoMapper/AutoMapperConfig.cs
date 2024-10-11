using AutoMapper;
using StudentApp.Application.CQRS.StudentCommandQuery.Command;
using StudentApp.Application.CQRS.StudentCommandQuery.Query;
using StudentApp.Core;
using StudentApp.Infrastructure.Utility;

namespace StudentApp.Application
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<CreateStudentCommand, Student>();
            CreateMap<Student, CreateStudentCommand>();

            CreateMap<Student, GetAllStudentsQueryResponse>()
                .ForMember(dest => dest.CreateDateDisplay, opt => opt.MapFrom(src => src.CreateDate.ToShamsi()));

        }
    }
}
