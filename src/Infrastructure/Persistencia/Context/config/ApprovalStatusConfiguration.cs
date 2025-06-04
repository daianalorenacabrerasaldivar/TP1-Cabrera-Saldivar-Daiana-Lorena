using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistencia.Context.config
{
    public class ApprovalStatusConfiguration : IEntityTypeConfiguration<ApprovalStatus>
    {
        public void Configure(EntityTypeBuilder<ApprovalStatus> builder)
        {
            builder.HasKey(status => status.Id);
            builder.Property(status => status.Name)
                .IsRequired()
                .HasMaxLength(25);

            builder.HasData(
                    new ApprovalStatus { Id = 1, Name = "Pending" },
                    new ApprovalStatus { Id = 2, Name = "Approved" },
                    new ApprovalStatus { Id = 3, Name = "Rejected" },
                    new ApprovalStatus { Id = 4, Name = "Observed" }
                    );
        }
    }
}
