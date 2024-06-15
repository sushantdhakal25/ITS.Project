using ITS.Domain.Entities;
using ITS.Domain.HelperInterface;
using ITS.Infrastructure.Repositories;
using Moq;
using MySql.Data.MySqlClient;

namespace ITS.Test.Repositories
{
    public class InmateRepositoryTests
    {
        private readonly Mock<IDataAccessHelper> _mockDataAccessHelper;
        private readonly InmateRepository _repository;

        public InmateRepositoryTests()
        {
            _mockDataAccessHelper = new Mock<IDataAccessHelper>();
            _repository = new InmateRepository(_mockDataAccessHelper.Object);
        }

        [Fact]
        public async Task GetInmates_ShouldReturnAllInmates()
        {
            // Arrange
            var searchText = "John";
            var expectedJsonResult = "[{\"identificationNumber\":\"I1\",\"name\":\"John Green\",\"currentFacilityId\":1}]";

            var expectedMvResults = new List<MvResult>
            {
                new MvResult { Json = expectedJsonResult }
            };

            _mockDataAccessHelper.Setup(helper => helper.ExecuteRetrievalProcedureAsync<MvResult>(
                "SpInmateSel", It.IsAny<IDictionary<string, object>>()))
                .ReturnsAsync(expectedMvResults);

            // Act
            var result = await _repository.GetAsync(searchText);

            // Assert
            Assert.Equal(expectedJsonResult, result);
        }


        [Fact]
        public async Task AddAsync_ShouldReturnExpectedResult()
        {
            // Arrange
            var param = "{\"identificationNumber\":\"I1\",\"name\":\"John Doee\",\"currentFacilityId\":1}";
            var expectedResult = "[{\"inmateId\":1,\"identificationNumber\":\"I1\",\"name\":\"John Doee\",\"currentFacilityId\":1}]";

            _mockDataAccessHelper.Setup(helper => helper.ExecuteActionProcedureAsync(
                "SpInmateInsert", It.Is<IDictionary<string, object>>(d => d["@param"].ToString() == param), "@result"))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _repository.AddAsync(param);

            // Assert
            Assert.Equal(expectedResult, result);
        }


        [Fact]
        public async Task UpdateAsync_ShouldReturnExpectedResult()
        {
            // Arrange
            var param = "{\"inmateId\":1,\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}";
            var expectedResult = "[{\"inmateId\":1,\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}]";

            _mockDataAccessHelper.Setup(helper => helper.ExecuteActionProcedureAsync(
                "SpInmateUpdate", It.Is<IDictionary<string, object>>(d => d["@param"].ToString() == param), "@result"))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _repository.UpdateAsync(param);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldHandleNullParameter()
        {
            string param = null;
            string expectedResult = null;
            var result = await _repository.UpdateAsync(param);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnExpectedResult()
        {
            var param = "{\"inmateId\":1}";
            var expectedResult = "[{\"inmateId\":1,\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}]";

            _mockDataAccessHelper.Setup(helper => helper.ExecuteActionProcedureAsync(
                "SpInmateDelete", It.Is<IDictionary<string, object>>(d => d["@param"].ToString() == param), "@result"))
                .ReturnsAsync(expectedResult);

            var result = await _repository.DeleteAsync(param);

            Assert.Equal(expectedResult, result);
        }
    }
}
