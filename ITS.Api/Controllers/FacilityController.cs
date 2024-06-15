using ITS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITS.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private readonly IFacilityService _facilityService;

        public FacilityController(IFacilityService facilityService)
        {
            _facilityService = facilityService;
        }

        /// <summary>
        /// Get All facilities, Also can Query with Search text
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns>list of all or filtered facilities as json</returns>
        [HttpGet]
        public async Task<IActionResult> GetFacility(string? searchText = null)
        {
            var Facilitys = await _facilityService.GetFacilitiesAsync(searchText);
            return Ok(Facilitys);
        }

        /// <summary>
        /// insert single/bulk facility
        /// </summary>
        /// <param name="json"></param>
        /// <returns>latest inserted facilities list as json</returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] string json)
        {
            var Facilitys = await _facilityService.AddFacilityAsync(json);
            return Ok(Facilitys);
        }

        /// <summary>
        /// Change existing inmate information in bulk/single
        /// </summary>
        /// <param name="json"></param>
        /// <returns>modified facility list as json</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] string json)
        {

            var Facilitys = await _facilityService.UpdateFacilityAsync(json);
            return Ok(Facilitys);
        }

        /// <summary>
        /// Remove existing facility bulk/single
        /// </summary>
        /// <param name="json"></param>
        /// <returns>deleted facility list as json</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] string json)
        {
            var Facilitys = await _facilityService.DeleteFacilityAsync(json);
            return Ok(Facilitys);
        }
    }
}
