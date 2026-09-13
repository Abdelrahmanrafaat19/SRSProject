using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using System.Threading.Tasks;

namespace SRSProject.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BasicController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("CreateAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminDto data)
        {
            var result = await _userService.CreateAdminAsync(data);
            return HandleResult(result);
        }

        [HttpPut("UpdateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto data)
        {
            var result = await _userService.UpdateUserAsync(data);
            return HandleResult(result);
        }

        [HttpPut("SetActive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetActive([FromQuery] string nationalId, [FromQuery] bool isActive)
        {
            var result = await _userService.SetActiveAsync(nationalId, isActive);
            return HandleResult(result);
        }

        [HttpPut("AssignRoles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRoles([FromQuery] string nationalId, [FromBody] string[] roles)
        {
            var result = await _userService.AssignRolesAsync(nationalId, roles);
            return HandleResult(result);
        }

        [HttpGet("GetUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers([FromQuery] UserQueryParameters parameters)
        {
            var result = await _userService.GetUsersAsync(parameters);
            return HandleResult(result);
        }
    }
}
