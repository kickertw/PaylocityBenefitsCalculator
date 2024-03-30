using Api.Dtos.Employee;

namespace Api.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<GetEmployeeDto?> GetEmployeeAsync(int employeeId);
        Task<List<GetEmployeeDto>> GetAllEmployeesAsync();
    }
}
