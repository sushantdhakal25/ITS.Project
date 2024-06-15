using ITS.Application.Interfaces;
using ITS.Application.Services;
using ITS.Domain.Entities;
using ITS.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ITS.Test.Services
{
    public class InmateServiceTests
    {
        private readonly Mock<IInmateRepository> _mockInmateRepository;
        private readonly InmateService _inmateService;

        public InmateServiceTests()
        {
            _mockInmateRepository = new Mock<IInmateRepository>();
            _inmateService = new InmateService(_mockInmateRepository.Object);
        }

        [Fact]
        public async Task GetAllInmates_ShouldReturnAllInmates()
        {
            // Arrange
            var searchText = "John";
            var expectedResult = "[{\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}]";
            _mockInmateRepository.Setup(repo => repo.GetAsync(searchText))
                .ReturnsAsync(expectedResult);


            var result = await _inmateService.GetInmatesAsync(searchText);


            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task AddInmateAsync_ShouldReturnExpectedResult()
        {
            // Arrange
            var param = "{\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}";
            var expectedResult = "[{\"inmateId\":1,\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}]";

            _mockInmateRepository.Setup(repo => repo.AddAsync(param))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _inmateService.AddInmateAsync(param);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task UpdateInmateAsync_ShouldReturnExpectedResult()
        {
            // Arrange
            var param = "{\"inmateId\":1,\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}";
            var expectedResult = "[{\"inmateId\":1,\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}]";

            _mockInmateRepository.Setup(repo => repo.UpdateAsync(param))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _inmateService.UpdateInmateAsync(param);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task DeleteInmateAsync_ShouldReturnExpectedResult()
        {
            // Arrange
            var param = "{\"inmateId\":1}";
            var expectedResult = "[{\"inmateId\":1,\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}]";

            _mockInmateRepository.Setup(repo => repo.DeleteAsync(param))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _inmateService.DeleteInmateAsync(param);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
