using Api.Dtos.Employee;
using Api.Dtos.Dependent;
using Api.Services;

namespace UnitTests.Services
{
    public class PaycheckServiceTests
    {
        // Inputs are:
        //      - annual salary
        //      - paycheck cost when salary > 80k
        //      - paycheck salary
        public static TheoryData<decimal, decimal, decimal> SalaryInputData => new()
        {
            { 26000m, 0m, 1000m },
            { 80000m, 0m, 3076.92m },
            { 260000m, 200m , 9800m }
        };

        public static TheoryData<DateTime> DependentsInputData => new()
        {
            { DateTime.Today },
            { DateTime.Today.AddYears(-50) }
        };

        public static TheoryData<DateTime> DependentsOver50InputData => new()
        {
            { DateTime.Today.AddYears(-51) },
            { DateTime.Today.AddYears(-70) }
        };

        public PaycheckServiceTests() { }

        [Theory]
        [MemberData(nameof(SalaryInputData))]
        public void CalculatePaycheck_Salary_ReturnsCorrectCost(
            decimal annualSalary,
            decimal expectedAbove80kCost,
            decimal expectedSalary)
        {
            // Arrange
            var sut = new PaycheckService();
            var employee = new GetEmployeeDto()
            {
                Salary = annualSalary
            };

            // Act
            var paycheck = sut.CalculatePaycheck(employee);


            // Assert
            Assert.NotNull(paycheck);
            Assert.Equal(expectedAbove80kCost, paycheck.Above80kBenefitCost);
            Assert.Equal(expectedSalary, paycheck.BaseSalary);
        }

        [Theory]
        [MemberData(nameof(DependentsInputData))]
        public void CalculatePaycheck_50AndUnderDependents_ReturnsCorrectCost(DateTime birthdate)
        {
            // Arrange
            var sut = new PaycheckService();
            var employee = new GetEmployeeDto()
            {
                Salary = 1,
                Dependents = new List<GetDependentDto>()
                {
                    new()
                    {
                        DateOfBirth = birthdate
                    }
                }
            };

            // Act
            var paycheck = sut.CalculatePaycheck(employee);

            // Assert
            Assert.NotNull(paycheck);
            Assert.Equal(600m, paycheck.DependentBenefitCost);
        }

        [Theory]
        [MemberData(nameof(DependentsOver50InputData))]
        public void CalculatePaycheck_Over50Dependents_ReturnsCorrectCost(DateTime birthdate)
        {
            // Arrange
            var sut = new PaycheckService();
            var employee = new GetEmployeeDto()
            {
                Salary = 1,
                Dependents = new List<GetDependentDto>()
                {
                    new()
                    {
                        DateOfBirth = birthdate
                    }
                }
            };

            // Act
            var paycheck = sut.CalculatePaycheck(employee);

            // Assert
            Assert.NotNull(paycheck);
            Assert.Equal(800m, paycheck.DependentBenefitCost);
        }

        [Fact]
        public void CalculatePaycheck_NoDependents_ReturnsNoCost()
        {
            // Arrange
            var sut = new PaycheckService();
            var employee = new GetEmployeeDto()
            {
                Salary = 1,
                Dependents = new List<GetDependentDto>()
            };

            // Act
            var paycheck = sut.CalculatePaycheck(employee);

            // Assert
            Assert.NotNull(paycheck);
            Assert.Equal(0, paycheck.DependentBenefitCost);
        }

        [Fact]
        public void CalculatePaycheck_NetPay_ReturnsCorrectCost()
        {
            // Arrange
            var sut = new PaycheckService();
            var employee = new GetEmployeeDto()
            {
                Salary = 80000m,
                Dependents = new List<GetDependentDto>()
                {
                    new()
                    {
                        DateOfBirth = DateTime.Today.AddYears(-51)
                    },
                    new()
                    {
                        DateOfBirth = DateTime.Today
                    }
                }
            };

            // Act
            var paycheck = sut.CalculatePaycheck(employee);

            // Assert
            Assert.NotNull(paycheck);
            Assert.Equal(676.92m, paycheck.NetPay);
        }
    }
}
