using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StudentApp.Core
{
    public class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(t => t.Id);

            builder.Property("FirstName")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property("LastName")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property("PhonNumber")
                .HasMaxLength(20);
        }
    }
}