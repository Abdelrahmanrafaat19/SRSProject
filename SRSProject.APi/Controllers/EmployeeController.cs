using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using System.Security.Claims;

namespace SRSProject.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BasicController
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController( IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        [HttpPost("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDtos data, CancellationToken cancellationToken)
        {
            var result = await _employeeService.CreateEmployee(data, cancellationToken);
            return HandleResult(result);
        }
        [HttpPost("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees([FromQuery] EmployeeSpecificationParameters parameters, CancellationToken cancellationToken)
        {

            var result = await _employeeService.GetAllAsync(parameters, cancellationToken);
            return HandleResult(result);
        }


        [HttpDelete("DeleteEmployee")]
        [Authorize]
        public async Task<IActionResult> DeleteEmployee([FromBody] string nationalId)
        {
            var roles = User.FindAll(ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();

            string allRoles = string.Join(",", roles);
            var data = new DeleteEmployeeDto
            {
                NationalID = nationalId,
                Role = allRoles

            };
            var result = await _employeeService.DeleteEmployeeAsync(data);
            return HandleResult(result);
        }


        [HttpGet("GetEmployeeByIdForEmployee")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeByIdForEmployee()
        {
            var idValue = User.FindFirstValue("EmployeeId");
            var result = await _employeeService.GetEmployeeByIdForEmployeeAsync(int.Parse(idValue));
            return HandleResult(result);
        }


        [HttpGet("GetEmployeeByIdForAdmin/{nationalId}")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeByIdForAdmin(string nationalId)
        {
            var result = await _employeeService.GetEmployeeByIdForAdminAsync(nationalId);
            return HandleResult(result);
        }


        [HttpPut("UpdateEmployee")]
        [Authorize]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeDto data)
        {
            var roles = User.FindAll(ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();

            string allRoles = string.Join(",", roles);
            var result = await _employeeService.UpdateEmployeeAsync(data, allRoles);
            return HandleResult(result);
        }
    }
}
