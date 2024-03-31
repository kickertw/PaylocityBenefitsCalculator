using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Models;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services
{
    /// <summary>
    /// Service to calculate an employee's paycheck
    /// Current requirements are:
    ///     - Able to calculate and view a paycheck for an employee given the following rules:
	///     - 26 paychecks per year with deductions spread as evenly as possible on each paycheck.
	///     - Employees have a base cost of $1,000 per month (for benefits)
	///     - Each dependent represents an additional $600 cost per month(for benefits)
	///     - Employees that make more than $80,000 per year will incur an additional 2% of their yearly salary in benefits costs.
	///     - Dependents that are over 50 years old will incur an additional $200 per month
    /// </summary>
    public class PaycheckService : IPaycheckService
    {
        private readonly IAppConfigurationRepository _appConfigRepository;

        public PaycheckService(IAppConfigurationRepository appConfigurationRepository)
        {
            _appConfigRepository = appConfigurationRepository;
        }

        /// <summary>
        /// Calculates the net pay
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>EmployePaycheckDto</returns>
        public async Task<EmployeePaycheckDto?> CalculatePaycheckAsync(GetEmployeeDto employee)
        {
            try
            {
                var appConfig = await _appConfigRepository.GetAppConfigurationAsync();
                if (appConfig is null)
                {
                    return null;
                }

                // 26 paychecks per year with deductions spread as evenly as possible on each paycheck.
                var employeePaycheckDto = new EmployeePaycheckDto
                {
                    EmployeeId = employee.Id,
                    BaseSalary = Math.Round(employee.Salary / appConfig.TotalPaychecksPerYear, 2),
                    SalaryBenefitCost = 0,
                    BaseBenefitCost = Math.Round(appConfig.BaseBenefitMonthlyCost * 12 / appConfig.TotalPaychecksPerYear, 2),
                    DependentBenefitCost = 0,
                    NetPay = 0
                };

                // Employees that make more than $80,000 per year will incur an additional 2% of their yearly salary in benefits costs.
                if (employee.Salary > appConfig.AnnualSalaryBenefitCostThreshold)
                {
                    employeePaycheckDto.SalaryBenefitCost = Math.Round(employee.Salary * appConfig.AnnualSalaryCostRate / appConfig.TotalPaychecksPerYear, 2);
                }

                if (employee.Dependents.Any())
                {
                    employeePaycheckDto.DependentBenefitCost = CalculateDependentCosts(employee, appConfig);
                }

                // Calculate total paycheck amount by taking
                // the base salary to be paid and subtracting all other costs
                employeePaycheckDto.NetPay = employeePaycheckDto.BaseSalary - employeePaycheckDto.BaseBenefitCost - employeePaycheckDto.SalaryBenefitCost - employeePaycheckDto.DependentBenefitCost;

                return employeePaycheckDto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Calculate the dependent benefits cost
        ///     - Each dependent represents an additional $600 cost per month (for benefits)
        ///     - Dependents that are over 50 years old will incur an additional $200 per month
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>Total cost of dependents</returns>
        private decimal CalculateDependentCosts(GetEmployeeDto employee, AppConfiguration appConfig)
        {
            var today = DateTime.Now.Date;

            var dependentCount = employee.Dependents.Count;
            var dependentCountOverThreshold = employee.Dependents.Count(i => i.DateOfBirth <= today.AddYears(-1 * appConfig.DependentAdditionalBenefitCostAgeThreshold));

            var dependentBaseCost = appConfig.DependentBaseBenefitMonthlyCost * 12 / appConfig.TotalPaychecksPerYear;
            var dependentAdditionalCost = appConfig.DependentAdditionalBenefitMonthlyCost * 12 / appConfig.TotalPaychecksPerYear;

            var totalDependentCost =
                (dependentCount * dependentBaseCost) +
                (dependentCountOverThreshold * dependentAdditionalCost);

            return Math.Round(totalDependentCost, 2);
        }
    }
}
