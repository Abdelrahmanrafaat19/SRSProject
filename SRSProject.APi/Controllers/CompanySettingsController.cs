using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using System.Linq;
using System.Security.Claims;

namespace SRSProject.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanySettingsController : BasicController
    {
        private readonly ICompanySettingsService _settingsService;

        public CompanySettingsController(ICompanySettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var result = await _settingsService.GetSettingsAsync();
            return HandleResult(result);
        }

        [HttpPut("Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] CompanySettingsDto dto)
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            var allRoles = string.Join(",", roles);
            var result = await _settingsService.UpdateSettingsAsync(dto, allRoles);
            return HandleResult(result);
        }
    }
}
