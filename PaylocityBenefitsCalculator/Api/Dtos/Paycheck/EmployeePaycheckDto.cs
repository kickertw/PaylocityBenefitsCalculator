namespace Api.Dtos.Paycheck
{
    public class EmployeePaycheckDto
    {
        public int EmployeeId { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal BaseBenefitCost { get; set; }
        public decimal DependentBenefitCost { get; set; }
        public decimal Above80kBenefitCost { get; set; }
        public decimal TotalPaycheckAmount { get; set; }
    }
}
