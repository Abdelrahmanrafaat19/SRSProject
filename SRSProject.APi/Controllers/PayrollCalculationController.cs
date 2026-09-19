using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRSProject.Application.Contracts;

namespace SRSProject.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollCalculationController : BasicController
    {
        private readonly IPayrollCalculationService _calculationService;

        public PayrollCalculationController(IPayrollCalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        [HttpGet("Calculate")]
        [Authorize]
        public async Task<IActionResult> Calculate([FromQuery] int employeeId, [FromQuery] int year, [FromQuery] int month)
        {
            var result = await _calculationService.CalculatePayrollAsync(employeeId, year, month);
            return HandleResult(result);
        }
    }
}
