using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistencia.Context.config
{
    public class ProjectProposalConfiguration : IEntityTypeConfiguration<Domain.Entity.ProjectProposal>
    {
        public void Configure(EntityTypeBuilder<Domain.Entity.ProjectProposal> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Description)
                .IsRequired();

            builder.Property(p => p.EstimatedAmount)
                .IsRequired();

            builder.Property(p => p.EstimatedDuration)
                .IsRequired();

            builder.Property(p => p.CreateAt)
                .IsRequired();


            builder.HasOne(p => p.AreaEntity)
          .WithMany(a => a.ProjectProposals)
          .HasForeignKey(p => p.Area)
          .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.TypeEntity)
            .WithMany(pt => pt.ProjectProposals)
            .HasForeignKey(p => p.Type);

            builder.HasOne(p => p.User)
                .WithMany(u => u.ProjectProposals)
                .HasForeignKey(p => p.CreateBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder
            .HasOne(p => p.ApprovalStatus)
            .WithMany(a => a.ProjectProposals)
            .HasForeignKey(p => p.Status);

        }
    }
}
