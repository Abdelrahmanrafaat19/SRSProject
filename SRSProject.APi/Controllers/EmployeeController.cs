using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;

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
    }
}
