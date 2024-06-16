using IdentityModel.Client;
using ITS.Application.DTOs;
using ITS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;

namespace ITS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OfficerController : ControllerBase
    {
        private readonly IOfficerService _officerService;
        private readonly IConfiguration _configuration;

        public OfficerController(IOfficerService officerService, IConfiguration configuration)
        {
            _officerService = officerService;
            _configuration = configuration;
        }
        /// <summary>
        /// Get all officer and allows to apply filter 
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns>list of officers as json string</returns>
        [HttpGet]
        public async Task<IActionResult> GetOfficer(string? searchText = null)
        {
            var officers = await _officerService.GetOfficersAsync(searchText);
            return Ok(officers);
        }
        /// <summary>
        /// verify login credentials of officestaff for token generation during login
        /// </summary>
        /// <param name="identificationNumber"></param>
        /// <param name="password"></param>
        /// <returns>return officer json</returns>
        [HttpGet]
        public async Task<IActionResult> GetOfficerLogin(string identificationNumber, string password)
        {
            var officers = await _officerService.GetOfficerLoginAsync(identificationNumber,password);
            return Ok(officers);
        }
        /// <summary>
        /// Allows adding single or bulk officer list
        /// </summary>
        /// <param name="json"> accepts json object or array string</param>
        /// <returns>newly added officer list as json</returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] string json)
        {
            var officers = await _officerService.AddOfficerAsync(json);
            return Ok(officers);
        }

        /// <summary>
        /// Allows modifiy single or bulk officer list
        /// </summary>
        /// <param name="json"> accepts json object or array string</param>
        /// <returns>modified officer list as json</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] string json)
        {

            var officers = await _officerService.UpdateOfficerAsync(json);
            return Ok(officers);
        }

        /// <summary>
        /// Allows delete single or bulk officer list
        /// </summary>
        /// <param name="json"> accepts json object or array string</param>
        /// <returns>deleted officer list as json</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] string json)
        {
            var officers = await _officerService.DeleteOfficerAsync(json);
            return Ok(officers);
        }

        /// <summary>
        /// invoked by identity server to return response if authorization is missing
        /// </summary>
        /// <param name="autoLogout"></param>
        /// <returns>401 status code wth method</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(bool autoLogout = true)
        {
            var result = JsonConvert.SerializeObject(new
            {
                error = "Unauthorized"
            });
            return Unauthorized(result);
        }

        /// <summary>
        /// login endoint for authentication
        /// </summary>
        /// <param name="login"></param>
        /// <returns>token response</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] MvLoginDto login)
        {
            
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(_configuration.GetSection("IdentityServerUrl").Value);
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                UserName = login.IdentificationNumber,
                Password = login.Password,
                ClientId = "itsWebClient",
                ClientSecret = "securePassword",
                Scope = "itsApi"
            });
            if (!tokenResponse.IsError)
            {
                return Ok(new { success = true, token = tokenResponse.AccessToken });
            }
            return Ok(new { success = false, message = tokenResponse.Error });
        }
    }
}
