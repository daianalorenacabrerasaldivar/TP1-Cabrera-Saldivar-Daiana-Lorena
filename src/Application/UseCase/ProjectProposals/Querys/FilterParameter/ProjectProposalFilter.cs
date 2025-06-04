using Domain.Common.ResultPattern;
using MediatR;

namespace Application.UseCase.ProjectProposals.Querys.FilterParameter
{
    public class ProjectProposalFilter : IRequest<Result<List<GetProjectResponse>>>
    {
        public string? Title { get; set; }
        public int? Status { get; set; }
        public int? Applicant { get; set; }
        public int? ApprovalUser { get; set; }
    }
}
