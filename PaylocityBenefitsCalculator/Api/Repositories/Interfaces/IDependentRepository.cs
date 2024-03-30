using Api.Models;

namespace Api.Repositories.Interfaces
{
    public interface IDependentRepository
    {
        Task<Dependent?> GetDependentAsync(int dependentId);
        Task<List<Dependent>> GetAllDependentsAsync();
    }
}
