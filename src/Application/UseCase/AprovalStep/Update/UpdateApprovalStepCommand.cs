using Domain.Common;
using Domain.Dto;
using MediatR;

namespace Application.UseCase.AprovalStep.Update
{
    public class UpdateApprovalStepCommand : IRequest<ResponseCodeAndObject<ProjectProposalResponse>>
    {
        public Guid ProjectId { get; set; }
        public long StepId { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
        public string? Observation { get; set; }
    }


}