using Microsoft.EntityFrameworkCore;

namespace StudentApp.Core.Context
{
    public class StudentAppContext : DbContext
    {
        public StudentAppContext(DbContextOptions<StudentAppContext> options) : base(options)
        { }

        #region dbset

        public DbSet<Student> Students { get; set; }

        #endregion

        #region OnModelCreating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
