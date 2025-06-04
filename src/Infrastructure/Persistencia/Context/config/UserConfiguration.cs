using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistencia.Context.config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);
            builder.Property(user => user.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(user => user.Email)
                .IsRequired()
                .HasMaxLength(200);
            builder.HasOne(u => u.ApproverRole)
                  .WithMany()
                  .HasForeignKey(u => u.Role)
                  .IsRequired();


            UserSeed(builder);
        }

        public static void UserSeed(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
              new User { Id = 1, Name = "José Ferreyra", Email = "jferreyra@unaj.com", Role = 2 },
              new User { Id = 2, Name = "Ana Lucero", Email = "alucero@unaj.com", Role = 1 },
              new User { Id = 3, Name = "Gonzalo Molinas", Email = "gmolinas@unaj.com", Role = 2 },
              new User { Id = 4, Name = "Lucas Olivera", Email = "lolivera@unaj.com", Role = 3 },
              new User { Id = 5, Name = "Danilo Fagundez", Email = "dfagundez@unaj.com", Role = 4 },
              new User { Id = 6, Name = "Gabriel Galli", Email = "ggalli@unaj.com", Role = 4 }
          );
        }

    }
}
