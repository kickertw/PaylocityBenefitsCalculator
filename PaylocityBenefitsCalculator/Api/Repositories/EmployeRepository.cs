using Api.Data;
using Api.Models;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly PaylocityDbContext _paylocityDbContext;

        public EmployeeRepository(PaylocityDbContext context)
        {
            _paylocityDbContext = context;
        }

        public async Task<Employee?> GetEmployeeAsync(int employeeId)
        {
            var employee = await _paylocityDbContext.Employees
                .Include(i => i.Dependents)
                .FirstOrDefaultAsync(i => i.Id == employeeId);
            return employee;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            var allEmployees = await _paylocityDbContext.Employees
                .Include(i => i.Dependents)
                .ToListAsync();
            return allEmployees;
        }
    }
}
