namespace StudentApp.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync();
    }
}
