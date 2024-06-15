using ITS.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITS.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransferController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        /// <summary>
        /// Get All Transfer, Also can Query with Search text
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns>All transfer made or with specific filter</returns>
        [HttpGet]
        public async Task<IActionResult> GetTransfer(string? searchText = null)
        {
            var transfers = await _transferService.GetTransfersAsync(searchText);
            return Ok(transfers);
        }

        /// <summary>
        /// Transfer single/bulk inmate
        /// </summary>
        /// <param name="json"></param>
        /// <returns> list of transfer made</returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] string json)
        {
            var transfers = await _transferService.AddTransferAsync(json);
            return Ok(transfers);
        }

        /// <summary>
        /// Change existing transfer in bulk/single
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] string json)
        {
            var transfers = await _transferService.UpdateTransferAsync(json);
            return Ok(transfers);
        }

        /// <summary>
        /// Remove Transfer in bulk/single
        /// </summary>
        /// <param name="json"></param>
        /// <returns>Removed transfer information array as json</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] string json)
        {
            var transfers = await _transferService.DeleteTransferAsync(json);
            return Ok(transfers);
        }
    }
}
