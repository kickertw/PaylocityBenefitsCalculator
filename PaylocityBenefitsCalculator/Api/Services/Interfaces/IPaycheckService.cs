using Api.Dtos.Employee;
using Api.Dtos.Paycheck;

namespace Api.Services.Interfaces
{
    public interface IPaycheckService
    {
        EmployeePaycheckDto? CalculatePaycheck(GetEmployeeDto employee);
    }
}
