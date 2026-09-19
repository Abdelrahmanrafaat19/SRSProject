using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRSProject.Application.Contracts;

namespace SRSProject.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : BasicController
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpPost("Log")]
        [Authorize]
        public async Task<IActionResult> Log([FromBody] Dictionary<string, string> payload)
        {
            payload.TryGetValue("action", out var action);
            payload.TryGetValue("user", out var user);
            payload.TryGetValue("details", out var details);
            var result = await _auditService.LogAsync(action ?? string.Empty, user ?? string.Empty, details ?? string.Empty);
            return HandleResult(result);
        }

        [HttpGet("GetLogs")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLogs()
        {
            var result = await _auditService.GetLogsAsync();
            return HandleResult(result);
        }
    }
}
