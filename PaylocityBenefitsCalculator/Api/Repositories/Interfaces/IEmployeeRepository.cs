using Api.Models;

namespace Api.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetEmployeeAsync(int employeeId);
        Task<List<Employee>> GetAllEmployeesAsync();
    }
}
