using Api.Dtos.Employee;
using Api.Models;
using Api.Repositories.Interfaces;
using Api.Services;
using Api.Services.Interfaces;
using Moq;
using NuGet.Frameworks;

namespace UnitTests.Services
{
    public class EmployeeServiceTests
    {
        private readonly List<Employee> _expectedEmployees = new List<Employee>()
        {
            new ()
            {
                Id = 1,
                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new ()
            {
                Id = 2,
                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<Dependent>
                {
                    new ()
                    {
                        Id = 1,
                        FirstName = "Spouse",
                        LastName = "Morant",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1998, 3, 3)
                    },
                    new()
                    {
                        Id = 2,
                        FirstName = "Child1",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2020, 6, 23)
                    },
                    new()
                    {
                        Id = 3,
                        FirstName = "Child2",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2021, 5, 18)
                    }
                }
            },
            new()
            {
                Id = 3,
                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
                Dependents = new List<Dependent>
                {
                    new()
                    {
                        Id = 4,
                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1974, 1, 2)
                    }
                }
            }
        };
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository = new();
        private IEmployeeService _sut;

        public EmployeeServiceTests()
        {
            _mockEmployeeRepository.Setup(i => i.GetEmployeeAsync(It.Is<int>(id => id <= 3))).ReturnsAsync((int id) => _expectedEmployees.First(i => i.Id == id));
            _mockEmployeeRepository.Setup(i => i.GetEmployeeAsync(It.Is<int>(id => id > 3))).ReturnsAsync((Employee?)null);
            _mockEmployeeRepository.Setup(i => i.GetAllEmployeesAsync()).ReturnsAsync(_expectedEmployees);

            _sut = new EmployeeService(_mockEmployeeRepository.Object);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetEmployeeAsync_WithValidId_ReturnsEmployee(int employeeId)
        {
            // Arrange
            var expectedEmployee = _expectedEmployees.First(i => i.Id == employeeId);

            // Act
            var actualEmployee = await _sut.GetEmployeeAsync(employeeId);

            // Assert
            Assert.NotNull(actualEmployee);
            Assert.Equal(expectedEmployee.Id, actualEmployee.Id);
            Assert.Equal(expectedEmployee.FirstName, actualEmployee.FirstName);
            Assert.Equal(expectedEmployee.LastName, actualEmployee.LastName);
            Assert.Equal(expectedEmployee.DateOfBirth, actualEmployee.DateOfBirth);
            Assert.Equal(expectedEmployee.Salary, actualEmployee.Salary);
            Assert.Equal(expectedEmployee.Dependents.Count, actualEmployee.Dependents.Count);
        }

        [Fact]
        public async Task GetEmployeeAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 99;

            // Act
            var actualEmployee = await _sut.GetEmployeeAsync(invalidId);

            // Assert
            Assert.Null(actualEmployee);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_ReturnsAllEmployees()
        {
            // Arrange
            var expectedEmployeeCount = 3;

            // Act
            var actualEmployees = await _sut.GetAllEmployeesAsync();

            // Assert
            Assert.Equal(expectedEmployeeCount, actualEmployees.Count);
        }

        [Fact]
        public async void GetAllEmployeesAsync_WithNoEmployees_ReturnsEmptyList()
        {
            // Arrange
            _mockEmployeeRepository.Setup(i => i.GetAllEmployeesAsync()).ReturnsAsync(new List<Employee>());

            // Act
            var actualEmployees = await _sut.GetAllEmployeesAsync();

            // Assert
            Assert.Empty(actualEmployees);
        }
    }
}