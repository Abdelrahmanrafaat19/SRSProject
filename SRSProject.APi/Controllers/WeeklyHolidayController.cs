using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using System.Security.Claims;

namespace SRSProject.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyHolidayController : BasicController
    {
        private readonly IWeeklyHolidayService _weeklyHolidayService;

        public WeeklyHolidayController(IWeeklyHolidayService weeklyHolidayService)
        {
            _weeklyHolidayService = weeklyHolidayService;
        }

        [HttpPost("CreateWeekHoliday")]
        [Authorize]
        public async Task<IActionResult> CreateWeekHoliday([FromBody] WeeklyHolidayDto weeklyHolidayDto, CancellationToken cancellationToken)
        {
            var roles = User.FindAll(ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();
            string allRoles = string.Join(",", roles);

            var result = await _weeklyHolidayService.CreateWeekHolidayAsync(allRoles, weeklyHolidayDto, cancellationToken);
            return HandleResult(result);
        }

        [HttpGet("GetAllWeekHolidays")]
        public async Task<IActionResult> GetAllWeekHolidays(CancellationToken cancellationToken)
        {
            var result = await _weeklyHolidayService.GetAllAsync(cancellationToken);
            return HandleResult(result);
        }

        [HttpGet("IsWeeklyHoliday")]
        public async Task<IActionResult> IsWeeklyHoliday([FromQuery] DateTime date, CancellationToken cancellationToken)
        {
            var result = await _weeklyHolidayService.IsWeeklyHolidayAsync(date, cancellationToken);
            return HandleResult(result);
        }





    }
}
