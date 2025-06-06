using Application.Common.Interface;
using Application.Common.Presentation;
using Application.Service.ProjectProposals;
using Application.Service.StatusProjectAndStep;
using Application.Service.ValidatorsBusiness;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Dependecy
{
    public static class DependecyInjection
    {
        public static void AddDependecyInjectionApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped<IApprovalStepStatusUpdater, ApprovalStepStatusUpdater>();
            services.AddScoped<ICreateProjectProposalService, CreateProjectProposalService>();
            services.AddScoped<IApprovalAssignmentService, ProjectApprovalAssignmentRuleService>();
            services.AddScoped<IProjectApprovalStepUpdateValidator, ProjectApprovalStepUpdateValidator>();
            services.AddScoped<IGetProjectApprovalStep, GetProjectApprovalStep>();
            services.AddSingleton<IUserSessionService, UserSessionService>();
        }
    }
}