using Api.Dtos.Dependent;
using Api.Models;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services
{
    public class DependentService : IDependentService
    {
        private readonly IDependentRepository _dependentRepo;
        public DependentService(IDependentRepository dependentRepository)
        {
            _dependentRepo = dependentRepository;
        }

        public async Task<GetDependentDto?> GetDependentAsync(int dependentId)
        {
            var dependent = await _dependentRepo.GetDependentAsync(dependentId);
            if (dependent is not null)
            {
                return ConvertToDependentDto(dependent);
            }

            return null;
        }

        public async Task<List<GetDependentDto>> GetAllDependentsAsync()
        {
            var dependents = await _dependentRepo.GetAllDependentsAsync();
            return dependents.Select(i => ConvertToDependentDto(i)).ToList();
        }

        private static GetDependentDto ConvertToDependentDto(Dependent dependent)
        {
            return new GetDependentDto
            {
                Id = dependent.Id,
                FirstName = dependent.FirstName,
                LastName = dependent.LastName,
                DateOfBirth = dependent.DateOfBirth,
                Relationship = dependent.Relationship,
            };
        }
    }
}