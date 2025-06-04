using Domain.Common.ResultPattern;

public interface IProjectProposalBusinessValidator
{
    Task<Result<string>> ValidateReferencesExistAsync(int areaId, int userId, int typeId, string title);
}