using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Services.Interfaces;

namespace Api.Services
{
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
        /// <returns></returns>
        public EmployeePaycheckDto? CalculatePaycheck(GetEmployeeDto employee)
        {
            var today = DateTime.Now.Date;

            try
            {
                // 26 paychecks per year with deductions spread as evenly as possible on each paycheck.
                var employeePaycheckDto = new EmployeePaycheckDto
                {
                    EmployeeId = employee.Id,
                    BaseSalary = Math.Round(employee.Salary / TotalPaychecksPerYear, 2),
                    Above80kBenefitCost = 0,
                    BaseBenefitCost = BaseBenefitCostPerMonth,
                    DependentBenefitCost = 0,
                    TotalPaycheckAmount = 0
                };

                // Employees that make more than $80,000 per year will incur an additional 2% of their yearly salary in benefits costs.
                if (employee.Salary > 80000)
                {
                    employeePaycheckDto.Above80kBenefitCost = Math.Round(employee.Salary * AnnualSalaryOver80kBenefitCostRate / TotalPaychecksPerYear, 2);
                    employeePaycheckDto.BaseSalary -= employeePaycheckDto.Above80kBenefitCost;
                }

                // Each dependent represents an additional $600 cost per month (for benefits)
                // Dependents that are over 50 years old will incur an additional $200 per month
                if (employee.Dependents.Any())
                {
                    var dependentCount = employee.Dependents.Count;
                    var dependentCountOver50 = employee.Dependents.Count(i => i.DateOfBirth < today.AddYears(-51));

                    employeePaycheckDto.DependentBenefitCost =
                        (dependentCount * DependentBaseBenefitCostPerMonth) +
                        (dependentCountOver50 * DependentAge50PlusBenefitAddlCostPerMonth);
                }

                // Calculate total paycheck amount
                employeePaycheckDto.TotalPaycheckAmount =
                    employeePaycheckDto.BaseSalary -
                    employeePaycheckDto.Above80kBenefitCost -
                    employeePaycheckDto.DependentBenefitCost -
                    employeePaycheckDto.BaseBenefitCost;
                employeePaycheckDto.TotalPaycheckAmount = Math.Round(employeePaycheckDto.TotalPaycheckAmount, 2);

                return employeePaycheckDto;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
