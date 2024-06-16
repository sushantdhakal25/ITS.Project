using IdentityServer4.Validation;
using ITS.Application.DTOs;
using ITS.Application.Interfaces;
using ITS.Domain.Entities;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using static IdentityModel.OidcConstants;

namespace ITS.Application.IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        IOfficerService _officerService;
        public ResourceOwnerPasswordValidator(IOfficerService officerService)
        {
            _officerService = officerService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
           
            var officer = await _officerService.GetOfficerLoginAsync(context.UserName, context.Password);
            

            if (officer != null)
            {
                
                var optionalClaims = new List<Claim>();
                optionalClaims.Add(new Claim("officerId", officer.OfficerId.ToString()));
                optionalClaims.Add(new Claim("name", officer.Name));
                

                context.Result = new GrantValidationResult(subject: "itsClaims",
                                                            authenticationMethod: "itsClaims",
                                                            claims: optionalClaims);
            }
            else
            {
                context.Result = new GrantValidationResult(TokenErrors.UnauthorizedClient, "OfficerNotFound");
                context.Result.IsError = true;
                context.Result.Error = "IdentificationNumber or Password is incorrect";
            }
        }
    }
}