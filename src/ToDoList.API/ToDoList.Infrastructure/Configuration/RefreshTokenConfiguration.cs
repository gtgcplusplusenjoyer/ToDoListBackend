using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Core.Entities.User;

namespace ToDoList.Infrastructure.Configuration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Token)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(t => t.ExpiresAt)
                .IsRequired();

            builder.Property(t => t.UserId)
                .IsRequired();



            builder.ToTable("RefreshTokens");
        }
    }
}
