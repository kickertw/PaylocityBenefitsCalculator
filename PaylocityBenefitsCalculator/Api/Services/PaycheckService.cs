using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
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
        private const int TotalPaychecksPerYear = 26;
        private const decimal BaseBenefitCostPerMonth = 1000;
        private const decimal DependentBaseBenefitCostPerMonth = 600;
        private const decimal DependentAge50PlusBenefitAddlCostPerMonth = 200;
        private const decimal AnnualSalaryOver80kBenefitCostRate = .02m;

        public PaycheckService() { }

        /// <summary>
        /// Calculates the net pay
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>EmployePaycheckDto</returns>
        public EmployeePaycheckDto? CalculatePaycheck(GetEmployeeDto employee)
        {
            try
            {
                // 26 paychecks per year with deductions spread as evenly as possible on each paycheck.
                var employeePaycheckDto = new EmployeePaycheckDto
                {
                    EmployeeId = employee.Id,
                    BaseSalary = Math.Round(employee.Salary / TotalPaychecksPerYear, 2),
                    SalaryBenefitCost = 0,
                    BaseBenefitCost = Math.Round(BaseBenefitCostPerMonth * 12 / TotalPaychecksPerYear, 2),
                    DependentBenefitCost = 0,
                    NetPay = 0
                };

                // Employees that make more than $80,000 per year will incur an additional 2% of their yearly salary in benefits costs.
                if (employee.Salary > 80000)
                {
                    employeePaycheckDto.SalaryBenefitCost = Math.Round(employee.Salary * AnnualSalaryOver80kBenefitCostRate / TotalPaychecksPerYear, 2);
                }

                if (employee.Dependents.Any())
                {
                    employeePaycheckDto.DependentBenefitCost = CalculateDependentCosts(employee);
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
        private static decimal CalculateDependentCosts(GetEmployeeDto employee)
        {
            var today = DateTime.Now.Date;

            var dependentCount = employee.Dependents.Count;
            var dependentCountOver50 = employee.Dependents.Count(i => i.DateOfBirth <= today.AddYears(-51));

            var dependentBaseCost = DependentBaseBenefitCostPerMonth * 12 / TotalPaychecksPerYear;
            var dependentOver50BaseCost = DependentAge50PlusBenefitAddlCostPerMonth * 12 / TotalPaychecksPerYear;

            var totalDependentCost =
                (dependentCount * dependentBaseCost) +
                (dependentCountOver50 * dependentOver50BaseCost);

            return Math.Round(totalDependentCost, 2);
        }
    }
}
