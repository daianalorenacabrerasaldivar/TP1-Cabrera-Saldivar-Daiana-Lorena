using Domain.Entity;
using Infrastructure.Persistencia.Context.config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistencia.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ApprovalRule> ApprovalRule { get; set; }
        public DbSet<Domain.Entity.ProjectProposal> ProjectProposal { get; set; }
        public DbSet<ProjectApprovalStep> ProjectApprovalStep { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<ProjectType> ProjectType { get; set; }
        public DbSet<ApprovalStatus> ApprovalStatus { get; set; }
        public DbSet<ApproverRole> ApproverRole { get; set; }
        public DbSet<User> User { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            EntityConfiguration(modelBuilder);
        }
        private static void EntityConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ApprovalRuleConfiguration());
            modelBuilder.ApplyConfiguration(new ApprovalStatusConfiguration());
            modelBuilder.ApplyConfiguration(new ApproverRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AreaConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectApprovalStepConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectProposalConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
