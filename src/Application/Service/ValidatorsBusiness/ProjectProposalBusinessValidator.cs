using Application.Common.Interface.Infrastructure;
using Domain.Common.ResultPattern;
using Domain.Dto;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Application.Service.ValidatorsBusiness
{
    public class ProjectProposalBusinessValidator : IProjectProposalBusinessValidator
    {
        private readonly IRepositoryQuery _queryRepository;

        public ProjectProposalBusinessValidator(IRepositoryQuery queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<Result<string>> ValidateReferencesExistAsync(int areaId, int userId, int typeId, string title)
        {
            var areaExists = await _queryRepository.Query<Area>()
                .AnyAsync(a => a.Id == areaId);
            if (!areaExists)
                return new Failed<string>($"No existe un área con el id {areaId}.");

            var userExists = await _queryRepository.Query<User>()
                .AnyAsync(u => u.Id == userId);
            if (!userExists)
                return new Failed<string>($"No existe un usuario con el id {userId}.");

            var typeExists = await _queryRepository.Query<ProjectType>()
                .AnyAsync(t => t.Id == typeId);
            if (!typeExists)
                return new Failed<string>($"No existe un tipo de proyecto con el id {typeId}.");


            var titleExists = await _queryRepository.Query<ProjectProposalResponse>()
                .AnyAsync(t => t.Title == title);

            if (titleExists)
                return new Failed<string>($"Ya existe una propuesta con el título '{title}'.");

            return new Success<string>("Validación exitosa.");
        }

    }
}