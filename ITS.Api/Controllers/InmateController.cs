using ITS.Application.DTOs;
using ITS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITS.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InmateController : ControllerBase
    {
        private readonly IInmateService _inmateService;

        public InmateController(IInmateService inmateService)
        {
            _inmateService = inmateService;
        }

        /// <summary>
        /// Get All Inmate, Also can Query with Search text
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns>All inmate array based on searchText</returns>
        [HttpGet]
        public async Task<IActionResult> GetInmate(string? searchText = null)
        {
            var inmates = await _inmateService.GetInmatesAsync(searchText);
            return Ok(inmates);
        }

        /// <summary>
        /// insert single/bulk inmate
        /// </summary>
        /// <param name="json"></param>
        /// <returns>Newly added inmate array as json</returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MvInsertDto json)
        {
            var inmates = await _inmateService.AddInmateAsync(json.Json);
            return Ok(inmates);
        }

        /// <summary>
        /// Change existing inmate information in bulk/single
        /// </summary>
        /// <param name="json"></param>
        /// <returns>Modified inmate array as json</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MvInsertDto json)
        {

            var inmates = await _inmateService.UpdateInmateAsync(json.Json);
            return Ok(inmates);
        }

        /// <summary>
        /// Remove existing inmate bulk/single
        /// </summary>
        /// <param name="json"></param>
        /// <returns>Deleted inmate array as json</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] MvInsertDto json)
        {
            var inmates = await _inmateService.DeleteInmateAsync(json.Json);
            return Ok(inmates);
        }
    }
}
