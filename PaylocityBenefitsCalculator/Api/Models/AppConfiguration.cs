namespace Api.Models;

public class AppConfiguration
{
    public int Id { get; set; }
    public int TotalPaychecksPerYear { get; set; }
    public decimal BaseBenefitMonthlyCost { get; set; }
    public decimal DependentBaseBenefitMonthlyCost { get; set; }
    public int DependentAdditionalBenefitCostAgeThreshold { get; set; }
    public decimal DependentAdditionalBenefitMonthlyCost { get; set; }
    public decimal AnnualSalaryBenefitCostThreshold { get; set; }
    public decimal AnnualSalaryCostRate { get; set; }
}

