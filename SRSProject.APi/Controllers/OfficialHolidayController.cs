using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;

namespace SRSProject.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficialHolidayController : BasicController
    {
        private readonly IOfficialHolidayService _officialHolidayService;
        public OfficialHolidayController(IOfficialHolidayService officialHolidayService)
        {
            _officialHolidayService = officialHolidayService;
        }
        [HttpPost("CreateOfficialHoliday")]
        [Authorize]
        public async Task<IActionResult> CreateOfficialHoliday([FromBody] OfficialHolidayDto holidayDto, CancellationToken cancellationToken)
        {
            var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();
            string allRoles = string.Join(",", roles);
            var result = await _officialHolidayService.CreateHolidayAsync(allRoles, holidayDto, cancellationToken);
            return HandleResult(result);
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Grid(CancellationToken cancellationToken)
        {
            var result = await _officialHolidayService.Grid(cancellationToken);
            return HandleResult(result);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _officialHolidayService.GetByIdAsync(id, cancellationToken);
            return HandleResult(result);
        }

        [HttpPut("UpdateOfficialHoliday")]
        [Authorize]
        public async Task<IActionResult> UpdateOfficialHoliday([FromBody] UpdateOfficialHolidayDto dto, CancellationToken cancellationToken)
        {
            var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();
            string allRoles = string.Join(",", roles);
            var result = await _officialHolidayService.UpdateHolidayAsync(dto, allRoles, cancellationToken);
            return HandleResult(result);
        }

        [HttpDelete("DeleteOfficialHoliday/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteOfficialHoliday(int id, CancellationToken cancellationToken)
        {
            var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();

            string allRoles = string.Join(",", roles);
            var result = await _officialHolidayService.DeleteHolidayAsync(id, allRoles, cancellationToken);
            return HandleResult(result);
        }
    }
}
