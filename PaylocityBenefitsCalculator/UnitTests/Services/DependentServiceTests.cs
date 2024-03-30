using Api.Models;
using Api.Repositories.Interfaces;
using Api.Services;
using Api.Services.Interfaces;
using Moq;

namespace UnitTests.Services
{
    public class DependentServiceTests
    {
        private readonly List<Dependent> _expectedDependents = new List<Dependent>()
        {
            new ()
            {
                Id = 1,
                FirstName = "Dependent",
                LastName = "One",
                Relationship = Relationship.Spouse,
                DateOfBirth = new DateTime(1998, 3, 3)
            },
            new()
            {
                Id = 2,
                FirstName = "Dependent",
                LastName = "Two",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2020, 6, 23)
            },
            new()
            {
                Id = 3,
                FirstName = "Dependent",
                LastName = "Three",
                Relationship = Relationship.Child,
                DateOfBirth = new DateTime(2021, 5, 18)
            }
        };

        private readonly Mock<IDependentRepository> _mockDependentRepository = new();
        private IDependentService _sut;

        public DependentServiceTests()
        {
            _mockDependentRepository.Setup(i => i.GetDependentAsync(It.Is<int>(id => id <= 3))).ReturnsAsync((int id) => _expectedDependents.First(i => i.Id == id));
            _mockDependentRepository.Setup(i => i.GetDependentAsync(It.Is<int>(id => id > 3))).ReturnsAsync((Dependent?)null);
            _mockDependentRepository.Setup(i => i.GetAllDependentsAsync()).ReturnsAsync(_expectedDependents);

            _sut = new DependentService(_mockDependentRepository.Object);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetDependentAsync_WithValidId_ReturnsDependent(int dependentId)
        {
            // Arrange
            var expectedDependent = _expectedDependents.First(i => i.Id == dependentId);

            // Act
            var actualDependent = await _sut.GetDependentAsync(dependentId);

            // Assert
            Assert.NotNull(actualDependent);
            Assert.Equal(expectedDependent.Id, actualDependent.Id);
            Assert.Equal(expectedDependent.FirstName, actualDependent.FirstName);
            Assert.Equal(expectedDependent.LastName, actualDependent.LastName);
            Assert.Equal(expectedDependent.DateOfBirth, actualDependent.DateOfBirth);
            Assert.Equal(expectedDependent.Relationship, actualDependent.Relationship);
        }

        [Fact]
        public async Task GetDependentAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = 99;

            // Act
            var actualDependent = await _sut.GetDependentAsync(invalidId);

            // Assert
            Assert.Null(actualDependent);
        }

        [Fact]
        public async Task GetAllDependentsAsync_ReturnsAllDependents()
        {
            // Arrange
            var expectedCount = 3;

            // Act
            var actualDepedents = await _sut.GetAllDependentsAsync();

            // Assert
            Assert.Equal(expectedCount, actualDepedents.Count);
        }

        [Fact]
        public async Task GetAllDependentsAsync_WithNoDependents_ReturnsEmptyList()
        {
            // Arrange
            _mockDependentRepository.Setup(i => i.GetAllDependentsAsync()).ReturnsAsync(new List<Dependent>());

            // Act
            var actualDepedents = await _sut.GetAllDependentsAsync();

            // Assert
            Assert.Empty(actualDepedents);
        }
    }
}
