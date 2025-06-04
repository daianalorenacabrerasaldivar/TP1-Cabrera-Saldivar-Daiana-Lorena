using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistencia.Context.config
{
    public class AreaConfiguration : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.HasKey(area => area.Id);
            builder.Property(area => area.Name)
                .IsRequired()
                .HasMaxLength(25);


            builder.HasData(
                    new Area { Id = 1, Name = "Finanzas" },
                    new Area { Id = 2, Name = "Tecnología" },
                    new Area { Id = 3, Name = "Recursos Humanos" },
                    new Area { Id = 4, Name = "Operaciones" }
                    );

        }
    }
}
