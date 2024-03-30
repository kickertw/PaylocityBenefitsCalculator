using Api.Data;
using Api.Models;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class DependentRepository : IDependentRepository
    {
        private readonly PaylocityDbContext _paylocityDbContext;

        public DependentRepository(PaylocityDbContext context)
        {
            _paylocityDbContext = context;
        }

        public async Task<Dependent?> GetDependentAsync(int dependentId)
        {
            var dependent = await _paylocityDbContext.Dependents.FirstOrDefaultAsync(i => i.Id == dependentId);
            return dependent;
        }

        public async Task<List<Dependent>> GetAllDependentsAsync()
        {
            var allDependents = await _paylocityDbContext.Dependents.ToListAsync();
            return allDependents;
        }
    }
}
