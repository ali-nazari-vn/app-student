using StudentApp.Core.Context;

namespace StudentApp.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Dependency Injection

        private readonly StudentAppContext studentContext;

        public UnitOfWork(StudentAppContext studentContext)
        {
            this.studentContext = studentContext;
        }

        #endregion

        #region methods

        public void Dispose()
        {
            studentContext.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            await studentContext.SaveChangesAsync();
        }

        #endregion
    }
}
