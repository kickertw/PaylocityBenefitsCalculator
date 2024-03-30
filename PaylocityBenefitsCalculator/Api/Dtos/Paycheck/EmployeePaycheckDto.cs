namespace Api.Dtos.Paycheck
{
    public class EmployeePaycheckDto
    {
        public int EmployeeId { get; set; }

        /// <summary>
        /// Raw salary to be paid out per paycheck
        /// </summary>

        public decimal BaseSalary { get; set; }
        /// <summary>
        /// Employee benefit cost per paycheck
        /// </summary>

        public decimal BaseBenefitCost { get; set; }
        /// <summary>
        /// Total dependent benefit cost per paycheck
        /// </summary>
        public decimal DependentBenefitCost { get; set; }

        /// <summary>
        /// Total salary benefit cost per paycheck
        /// Example: If annual salary > $80,000 => benefit cost equals 2% of salary
        ///          Then an employee w/ a salary of $100k has a benefit cost  of $2k
        /// </summary>
        public decimal SalaryBenefitCost { get; set; }
        public decimal NetPay { get; set; }
    }
}
