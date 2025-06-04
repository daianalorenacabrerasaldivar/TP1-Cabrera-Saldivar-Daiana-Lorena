using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistencia.Context.config
{
    public class ProjectApprovalStepConfiguration : IEntityTypeConfiguration<ProjectApprovalStep>
    {
        public void Configure(EntityTypeBuilder<ProjectApprovalStep> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.ProjectProposal)
                   .WithMany(p => p.ApprovalSteps)
                   .HasForeignKey(p => p.ProjectProposalId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.ApproverUser)
                   .WithMany(u => u.ProjectApprovalSteps)
                   .HasForeignKey(p => p.ApproverUserId)
                   .IsRequired(false);

            builder.HasOne(p => p.ApproverRole)
                   .WithMany(role => role.ProjectApprovalSteps)
                   .HasForeignKey(p => p.ApproverRoleId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.ApprovalStatus)
                   .WithMany(s => s.ApprovalSteps)
                   .HasForeignKey(p => p.Status)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.StepOrder)
                   .IsRequired();

            builder.Property(p => p.DecisionDate)
                   .IsRequired(false);

            builder.Property(p => p.Observations)
                   .HasMaxLength(500)
                   .IsRequired(false);
        }
    }
}
