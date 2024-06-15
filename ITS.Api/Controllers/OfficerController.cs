using ITS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITS.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OfficerController : ControllerBase
    {
        private readonly IOfficerService _officerService;

        public OfficerController(IOfficerService officerService)
        {
            _officerService = officerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOfficer(string? searchText = null)
        {
            var officers = await _officerService.GetOfficersAsync(searchText);
            return Ok(officers);
        }

        [HttpGet]
        public async Task<IActionResult> GetOfficerLogin(string identificationNumber, string password)
        {
            var officers = await _officerService.GetOfficerLoginAsync(identificationNumber,password);
            return Ok(officers);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] string json)
        {
            var officers = await _officerService.AddOfficerAsync(json);
            return Ok(officers);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] string json)
        {

            var officers = await _officerService.UpdateOfficerAsync(json);
            return Ok(officers);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] string json)
        {
            var officers = await _officerService.DeleteOfficerAsync(json);
            return Ok(officers);
        }
    }
}
