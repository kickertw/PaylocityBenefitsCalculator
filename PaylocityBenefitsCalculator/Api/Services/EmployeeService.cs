using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepo;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepo = employeeRepository;
        }

        public async Task<GetEmployeeDto?> GetEmployeeAsync(int employeeId)
        {
            var employee = await _employeeRepo.GetEmployeeAsync(employeeId);
            if (employee is not null)
            {
                return ConvertToEmployeeDto(employee);
            }

            return null;
        }

        public async Task<List<GetEmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepo.GetAllEmployeesAsync();
            return employees.Select(i => ConvertToEmployeeDto(i)).ToList();
        }

        private static GetEmployeeDto ConvertToEmployeeDto(Employee employee)
        {
            return new GetEmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth,
                Salary = employee.Salary,
                Dependents = employee.Dependents.Select(i => ConvertToDependentDto(i)).ToList()
            };
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