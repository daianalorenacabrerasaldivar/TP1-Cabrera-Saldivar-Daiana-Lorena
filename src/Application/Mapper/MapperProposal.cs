using Domain.Dto;
using Domain.Entity;

namespace Application.Mapper
{
    public class MapperProposal
    {
        public static ProjectProposalResponse MapToProposalResponse(ProjectProposal proposal)
        {
            if (proposal == null)
                throw new ArgumentNullException(nameof(proposal), "El proyecto no puede ser nulo");

            return new ProjectProposalResponse
            {
                Id = proposal.Id,
                Title = proposal.Title,
                Description = proposal.Description,
                Amount = proposal.EstimatedAmount,
                Duration = proposal.EstimatedDuration,
                Area = new AreaDto
                {
                    Id = proposal.Area,
                    Name = proposal.AreaEntity?.Name ?? string.Empty
                },
                Status = new StatusDto
                {
                    Id = proposal.Status,
                    Name = proposal.ApprovalStatus?.Name ?? string.Empty
                },
                Type = new ProjectTypeDto
                {
                    Id = proposal.Type,
                    Name = proposal.TypeEntity?.Name ?? string.Empty
                },
                User = new UserDto
                {
                    Id = proposal.CreateBy,
                    Name = proposal.User?.Name ?? string.Empty,
                    Email = proposal.User?.Email ?? string.Empty,
                    Role = new RoleDto
                    {
                        Id = proposal.User?.Role ?? 0,
                        Name = proposal.User?.Name ?? string.Empty
                    }
                },
                Steps = MapSteps(proposal.ApprovalSteps)
            };
        }

        private static List<AprovalStepDto> MapSteps(ICollection<ProjectApprovalStep>? steps)
        {
            if (steps == null) return new List<AprovalStepDto>();

            return steps.Select(step => new AprovalStepDto
            {
                Id = step.Id,
                StepOrder = step.StepOrder,
                DecisionDate = step.DecisionDate,
                Observations = step.Observations,
                ApproverUser = step.ApproverUser != null ? new UserDto
                {
                    Id = step.ApproverUser.Id,
                    Name = step.ApproverUser.Name,
                    Email = step.ApproverUser.Email,
                    Role = new RoleDto
                    {
                        Id = step.ApproverUser.Role,
                        Name = step.ApproverUser.ApproverRole?.Name ?? string.Empty
                    }
                } : null,
                ApproverRole = new RoleDto
                {
                    Id = step.ApproverRoleId,
                    Name = step.ApproverRole?.Name ?? string.Empty
                },
                Status = new StatusDto
                {
                    Id = step.Status,
                    Name = step.ApprovalStatus?.Name ?? string.Empty
                }
            }).ToList();
        }
    }
}
