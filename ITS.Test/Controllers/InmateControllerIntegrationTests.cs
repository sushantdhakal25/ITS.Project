using ITS.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace ITS.Test.Controllers
{
    public class InmateControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public InmateControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetInmate_ShouldReturnOkResponse()
        {
            var response = await _client.GetAsync("/api/inmate/getinmate");

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(responseBody);
        }

        [Fact]
        public async Task AddInmate_ShouldReturnOkResponse()
        {
            var inmate = new Inmate
            {
                IdentificationNumber = "I7",
                Name = "John Doee",
                CurrentFacilityId = 2
            };
            var json = JsonConvert.SerializeObject(new List<Inmate> { inmate });
            var response = await _client.PostAsJsonAsync("/api/inmate/add", json);

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(responseBody);
            Assert.Contains("inmateId", responseBody);
        }

        [Fact]
        public async Task UpdateInmate_ShouldReturnOkResponse()
        {
            var json = "[{\"inmateId\":1,\"identificationNumber\":\"I1\",\"name\":\"John Doe\",\"currentFacilityId\":1}]";

            var response = await _client.PutAsJsonAsync("/api/inmate/update", json);

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(responseBody);
        }

        [Fact]
        public async Task DeleteInmateWithInvalidParameter_ShouldReturnBadResponse()
        {

            var json = "";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/inmate/delete")
            {
                Content = content
            };
            var response = await _client.SendAsync(request);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

        }
    }
}
