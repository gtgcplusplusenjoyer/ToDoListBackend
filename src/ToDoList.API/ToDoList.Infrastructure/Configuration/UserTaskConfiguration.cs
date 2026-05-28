using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Core.Entities;
using ToDoList.Core.Enums;

namespace ToDoList.Infrastructure.Configuration
{
    public class UserTaskConfiguration : IEntityTypeConfiguration<UserTask>
    {
        public void Configure(EntityTypeBuilder<UserTask> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Description)
                .HasMaxLength(500);

            builder.Property(e => e.Status)
                .HasDefaultValue(ToDoStatus.Pending)
                .HasConversion<int>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(t => t.Priority)
                .HasDefaultValue(Priority.Medium)
                .HasConversion<int>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(t => t.DueDate)
                .IsRequired(false)
                .HasColumnType("timestamp with time zone");

            builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(t => t.UpdatedAt)
                .HasColumnType("timestamp with time zone")
                .IsRequired(false);

            builder.ToTable("UserTasks");
        }
    }
}
