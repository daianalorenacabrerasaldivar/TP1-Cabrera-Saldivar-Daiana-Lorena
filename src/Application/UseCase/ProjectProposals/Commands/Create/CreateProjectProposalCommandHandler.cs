using Application.Common.Exceptions;
using Application.Common.Interface;
using Application.Mapper;
using Domain.Dto;
using MediatR;


namespace Application.UseCase.ProjectProposals.Commands.Create;
public class CreateProjectProposalCommandHandler : IRequestHandler<CreateProjectProposalCommand, ProjectProposalResponse>
{
    private readonly ICreateProjectProposalService _createProjectProposalService;

    public CreateProjectProposalCommandHandler(ICreateProjectProposalService createProjectProposalService)
    {
        _createProjectProposalService = createProjectProposalService;
    }

    public async Task<ProjectProposalResponse> Handle(CreateProjectProposalCommand request, CancellationToken cancellationToken)
    {
        var result = await _createProjectProposalService.CreateProjectWithApprovalFlowAsync(request);

        if (result.IsFailed)
            throw new CustomResponseException(result.Info);

        return MapperProposal.MapToProposalResponse(result.Value);
    }
}