using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRSProject.Application.Contracts;

namespace SRSProject.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollReportController : BasicController
    {
        private readonly IPayrollReportService _reportService;

        public PayrollReportController(IPayrollReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("Monthly")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Monthly([FromQuery] int year, [FromQuery] int month)
        {
            var result = await _reportService.GenerateMonthlyReportAsync(year, month);
            return HandleResult(result);
        }
    }
}
