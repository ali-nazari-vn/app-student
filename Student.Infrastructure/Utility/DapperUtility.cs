using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace StudentApp.Infrastructure.Utility
{
    public class DapperUtility
    {
        private readonly IConfiguration configuration;
        public DapperUtility(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(configuration.GetConnectionString("StudentAppConnectionString"));
        }
    }
}
