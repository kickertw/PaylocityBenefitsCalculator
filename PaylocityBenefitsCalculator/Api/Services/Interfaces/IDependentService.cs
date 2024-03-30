using Api.Dtos.Dependent;

namespace Api.Services.Interfaces
{
    public interface IDependentService
    {
        Task<GetDependentDto?> GetDependentAsync(int dependentId);
        Task<List<GetDependentDto>> GetAllDependentsAsync();
    }
}
