using Microsoft.Extensions.DependencyInjection;
using StudentApp.Core.IRepositories;
using StudentApp.Infrastructure.Repositories;
using StudentApp.Infrastructure.Utility;

namespace StudentApp.Infrastructure
{
    public static class DIInfrastructure
    {
        public static void AddInfrastructureDI(this IServiceCollection services)
        {
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ExternalLogServices>();

            services.AddSingleton<DapperUtility>();
        }
    }
}
