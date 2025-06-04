using Application.Common.Interface;
using Application.Common.Interface.Infrastructure;
using Application.UseCase.ProjectProposals.Commands.Create;
using Domain.Common.ResultPattern;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.ProjectProposals
{
    public class CreateProjectProposalService : ICreateProjectProposalService
    {
        private readonly IRepositoryCommand _repositoryCommand;
        private readonly IApprovalAssignmentService _approvalAssignmentService;
        private readonly IRepositoryQuery _repositoryQuery;

        public CreateProjectProposalService(IRepositoryCommand repositoryCommand, IRepositoryQuery repositoryQuery, IApprovalAssignmentService approvalAssignmentService)
        {
            _repositoryCommand = repositoryCommand;
            _approvalAssignmentService = approvalAssignmentService;
            _repositoryQuery = repositoryQuery;
        }

        public async Task<Result<ProjectProposal>> CreateProjectWithApprovalFlowAsync(CreateProjectProposalCommand projectProposal)
        {
            if (projectProposal == null)
                return new Failed<ProjectProposal>("La propuesta de proyecto no puede ser nula.");

            var createdProjectProposal = new ProjectProposal
            {
                Title = projectProposal.Title,
                Description = projectProposal.Description,
                EstimatedAmount = projectProposal.Amount,
                EstimatedDuration = projectProposal.Duration,
                Area = projectProposal.Area,
                Type = projectProposal.Type,
                CreateAt = DateTime.UtcNow,
                CreateBy = projectProposal.User
            };

            _repositoryCommand.Add(createdProjectProposal);

            var steps = await _approvalAssignmentService.GetApprovalStepsForProposalAsync(createdProjectProposal);
            if (steps.IsFailed)
                return new Failed<ProjectProposal>($"Error al obtener los pasos de aprobación: {steps.Info}");
            foreach (var step in steps.Value)
                _repositoryCommand.Add(step);

            // Guardar todo junto
            var resultProject = await _repositoryCommand.SaveAsync();
            if (resultProject.IsFailed)
                return new Failed<ProjectProposal>($"Error al guardar el proyecto.");

            var projectProposalDetail = await _repositoryQuery.Query<ProjectProposal>()
                .Include(p => p.AreaEntity)
                .Include(p => p.TypeEntity)
                .Include(p => p.ApprovalStatus)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == createdProjectProposal.Id);
            if (projectProposalDetail == null)
                return new Failed<ProjectProposal>("No se encontró la propuesta de proyecto creada.");

            return new Success<ProjectProposal>(projectProposalDetail);
        }
    }
}