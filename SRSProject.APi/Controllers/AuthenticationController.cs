
using Microsoft.AspNetCore.Mvc;
using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos.Identity;
using SRSProject.Application.Dtos.Identity.loginDtos;

namespace SRSProject.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : BasicController
    {
        private readonly IAuthenticationService _authenticationService;


        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInPutDtos loginDto)
        {
            var result= await _authenticationService.LoginAsync(loginDto);
            return HandleResult(result);
        }


        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var result = await _authenticationService.ChangePasswordAsync(changePasswordDto.NationID, changePasswordDto.Password);
            return HandleResult(result);
        }
    }
}
