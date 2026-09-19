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
    public class PayrollController : BasicController
    {
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        [HttpPost("CreatePayroll")]
        [Authorize]
        public async Task<IActionResult> CreatePayroll([FromBody] CreatePayrollDto dto)
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            string allRoles = string.Join(",", roles);
            var result = await _payrollService.CreatePayrollAsync(allRoles, dto);
            return HandleResult(result);
        }

        [HttpGet("GetByEmployeeMonth")]
        [Authorize]
        public async Task<IActionResult> GetByEmployeeMonth([FromQuery] int employeeId, [FromQuery] int year, [FromQuery] int month)
        {
            var result = await _payrollService.GetByEmployeeMonthAsync(employeeId, year, month);
            return HandleResult(result);
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _payrollService.GetAllAsync();
            return HandleResult(result);
        }

        [HttpPost("AddAdjustment/{payrollRecordId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAdjustment(int payrollRecordId, [FromBody] PayrollAdjustmentDto dto)
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            string allRoles = string.Join(",", roles);
            var result = await _payrollService.AddAdjustmentAsync(payrollRecordId, dto, allRoles);
            return HandleResult(result);
        }

        [HttpDelete("DeletePayroll/{payrollRecordId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePayroll(int payrollRecordId)
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            string allRoles = string.Join(",", roles);
            var result = await _payrollService.DeletePayrollAsync(payrollRecordId, allRoles);
            return HandleResult(result);
        }
    }
}
