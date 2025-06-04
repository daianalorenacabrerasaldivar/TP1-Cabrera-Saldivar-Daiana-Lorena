using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistencia.Context.config
{
    public class ApprovalRuleConfiguration : IEntityTypeConfiguration<ApprovalRule>
    {
        public void Configure(EntityTypeBuilder<ApprovalRule> builder)
        {
            builder.HasKey(approvalRule => approvalRule.Id);

            builder.Property(approvalRule => approvalRule.MinAmount)
                   .IsRequired();

            builder.Property(approvalRule => approvalRule.MaxAmount)
                   .IsRequired();

            builder.Property(approvalRule => approvalRule.StepOrder)
                   .IsRequired();

            builder.HasOne(approvalRule => approvalRule.AreaEntity)
          .WithMany(area => area.ApprovalRules)
          .HasForeignKey(approvalRule => approvalRule.Area)
          .IsRequired(false)
          .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(approvalRule => approvalRule.TypeEntity)
            .WithMany(type => type.ApprovalRules)
            .HasForeignKey(approvalRule => approvalRule.Type)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(approvalRule => approvalRule.ApproverRole)
                   .WithMany(role => role.ApprovalRules)
                   .HasForeignKey(approvalRule => approvalRule.ApproverRoleId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            SeedApprovalRules(builder);
        }

        private void SeedApprovalRules(EntityTypeBuilder<ApprovalRule> builder)
        {
            builder.HasData(
                new ApprovalRule { Id = 1, MinAmount = 0, MaxAmount = 100000, Area = null, Type = null, StepOrder = 1, ApproverRoleId = 1 },
                new ApprovalRule { Id = 2, MinAmount = 5000, MaxAmount = 20000, Area = null, Type = null, StepOrder = 2, ApproverRoleId = 2 },
                new ApprovalRule { Id = 3, MinAmount = 0, MaxAmount = 20000, Area = 2, Type = 2, StepOrder = 1, ApproverRoleId = 2 },
                new ApprovalRule { Id = 4, MinAmount = 20000, MaxAmount = 0, Area = null, Type = null, StepOrder = 3, ApproverRoleId = 3 },
                new ApprovalRule { Id = 5, MinAmount = 5000, MaxAmount = 0, Area = 1, Type = 1, StepOrder = 2, ApproverRoleId = 2 },
                new ApprovalRule { Id = 6, MinAmount = 0, MaxAmount = 10000, Area = null, Type = 2, StepOrder = 1, ApproverRoleId = 1 },
                new ApprovalRule { Id = 7, MinAmount = 0, MaxAmount = 10000, Area = 2, Type = 1, StepOrder = 1, ApproverRoleId = 4 },
                new ApprovalRule { Id = 8, MinAmount = 10000, MaxAmount = 30000, Area = 2, Type = null, StepOrder = 2, ApproverRoleId = 2 },
                new ApprovalRule { Id = 9, MinAmount = 30000, MaxAmount = 0, Area = 3, Type = null, StepOrder = 2, ApproverRoleId = 3 },
                new ApprovalRule { Id = 10, MinAmount = 0, MaxAmount = 50000, Area = null, Type = 4, StepOrder = 1, ApproverRoleId = 4 }
            );
        }
    }
}
