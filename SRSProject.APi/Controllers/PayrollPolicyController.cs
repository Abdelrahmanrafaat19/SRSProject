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
    public class PayrollPolicyController : BasicController
    {
        private readonly IPayrollPolicyService _policyService;

        public PayrollPolicyController(IPayrollPolicyService policyService)
        {
            _policyService = policyService;
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var result = await _policyService.GetPolicyAsync();
            return HandleResult(result);
        }

        [HttpPut("Update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] PayrollPolicyDto dto)
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            var allRoles = string.Join(",", roles);
            var result = await _policyService.UpdatePolicyAsync(dto, allRoles);
            return HandleResult(result);
        }
    }
}
