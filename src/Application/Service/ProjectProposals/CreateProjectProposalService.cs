using Application.Common.Interface;
using Application.Common.Interface.Infrastructure;
using Application.UseCase.ProjectProposals.Commands.Create;
using Domain.Common.ResultPattern;
using Domain.Entity;

namespace Application.Service.ProjectProposals
{
    public class CreateProjectProposalService : ICreateProjectProposalService
    {
        private readonly IRepositoryCommand _repositoryCommand;
        private readonly IApprovalAssignmentService _approvalAssignmentService;
        private readonly IProjectProposalQuery _projectProposalQuery;

        public CreateProjectProposalService(IRepositoryCommand repositoryCommand, IProjectProposalQuery projectProposalQuery, IApprovalAssignmentService approvalAssignmentService)
        {
            _repositoryCommand = repositoryCommand;
            _approvalAssignmentService = approvalAssignmentService;
            _projectProposalQuery = projectProposalQuery;
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


            var projectProposalDetail = await _projectProposalQuery.GetProjectProposalByIdAsync(createdProjectProposal.Id);
            if (projectProposalDetail == null)
                return new Failed<ProjectProposal>("No se encontró la propuesta de proyecto creada.");

            return new Success<ProjectProposal>(projectProposalDetail);
        }
    }
}