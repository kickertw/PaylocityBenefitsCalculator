using Api.Dtos.Employee;
using Api.Dtos.Dependent;
using Api.Services;

namespace UnitTests.Services
{
    public class PaycheckServiceTests
    {
        // Inputs are:
        //      - annual salary
        //      - paycheck salary (before any costs)
        //      - paycheck cost when salary > 80k
        public static TheoryData<decimal, decimal, decimal> SalaryInputData => new()
        {
            { 26000m, 1000m, 0m },
            { 80000m, 3076.92m, 0m },
            { 260000m, 10000m, 200m }
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
            decimal expectedSalary,
            decimal expectedAbove80kCost)
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
            Assert.Equal(expectedAbove80kCost, paycheck.SalaryBenefitCost);
            Assert.Equal(expectedSalary, paycheck.BaseSalary);
        }

        [Fact]
        public void CalculatePaycheck_ReturnsCorrectBaseCost()
        {
            // Arrange
            var expectedCost = Math.Round(1000m * 12 / 26, 2);
            var sut = new PaycheckService();
            var employee = new GetEmployeeDto()
            {
                Salary = 1
            };

            // Act
            var paycheck = sut.CalculatePaycheck(employee);


            // Assert
            Assert.NotNull(paycheck);
            Assert.Equal(expectedCost, paycheck.BaseBenefitCost);
        }

        [Theory]
        [MemberData(nameof(DependentsInputData))]
        public void CalculatePaycheck_With50AndUnderDependents_ReturnsCorrectCost(DateTime birthdate)
        {
            // Arrange
            var expectedCost = Math.Round(600m * 12 / 26, 2);
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
            Assert.Equal(expectedCost, paycheck.DependentBenefitCost);
        }

        [Theory]
        [MemberData(nameof(DependentsOver50InputData))]
        public void CalculatePaycheck_WithOver50Dependents_ReturnsCorrectCost(DateTime birthdate)
        {
            // Arrange
            var expectedCost = Math.Round(800m * 12 / 26, 2);
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
            Assert.Equal(expectedCost, paycheck.DependentBenefitCost);
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

        /// <summary>
        /// For Salary of $80k, 2 depedents (1 under and 1 over 50)
        /// paycheck salary = 80,000 / 26            = 3076.92
        /// base cost       = -1,000 * 12 / 26       = 461.54
        /// dependent cost  = -(600 + 800) * 12 / 26 = 646.15
        /// salary cost     = 0
        /// Net                                      = 1,969.23
        /// </summary>
        [Fact]
        public void CalculatePaycheck_NetPay_ReturnsCorrectCost()
        {
            // Arrange
            var expectedNetPay = 1969.23m;
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
            Assert.Equal(expectedNetPay, paycheck.NetPay);
        }
    }
}
