using ITS.Api.Controllers;
using ITS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.Test.Controllers
{
    public class InmateControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly Mock<IInmateService> _mockInmateService;
        private readonly InmateController _controller;

        public InmateControllerTests(WebApplicationFactory<Program> factory)
        {
            _mockInmateService = new Mock<IInmateService>();
            _controller = new InmateController(_mockInmateService.Object);
        }

        [Fact]
        public async Task GetInmate_ShouldReturnOkResult()
        {
            // Arrange
            var searchText = "John Doe";
            var expectedResult = "[{\"inmateId\":1,\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}]";

            _mockInmateService.Setup(service => service.GetInmatesAsync(searchText))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetInmate(searchText) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedResult, result.Value);
        }

        [Fact]
        public async Task Add_ShouldReturnOkResult()
        {
            // Arrange
            var json = "{\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}";
            var expectedResult = "[{\"inmateId\":1,\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}]";

            _mockInmateService.Setup(service => service.AddInmateAsync(json))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Add(json) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedResult, result.Value);
        }

        [Fact]
        public async Task Update_ShouldReturnOkResult()
        {
            // Arrange
            var json = "{\"inmateId\":1,\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}";
            var expectedResult = "[{\"inmateId\":1,\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}]";

            _mockInmateService.Setup(service => service.UpdateInmateAsync(json))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Update(json) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedResult, result.Value);
        }

        [Fact]
        public async Task Delete_ShouldReturnOkResult()
        {
            // Arrange
            var json = "{\"inmateId\":1}";
            var expectedResult = "[{\"inmateId\":1,\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}]";

            _mockInmateService.Setup(service => service.DeleteInmateAsync(json))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Delete(json) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedResult, result.Value);
        }
    }
}
