using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SRSProject.Application.Contracts;

namespace SRSProject.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptionController : BasicController
    {
        private readonly IEncryptionService _encryptionService;

        public EncryptionController(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        [HttpPost("Hash")]
        [Authorize]
        public async Task<IActionResult> Hash([FromBody] string plain)
        {
            var result = await _encryptionService.HashAsync(plain);
            return HandleResult(result);
        }

        [HttpPost("Verify")]
        [Authorize]
        public async Task<IActionResult> Verify([FromBody] Dictionary<string, string> payload)
        {
            payload.TryGetValue("hash", out var hash);
            payload.TryGetValue("plain", out var plain);
            var result = await _encryptionService.VerifyAsync(hash ?? string.Empty, plain ?? string.Empty);
            return HandleResult(result);
        }
    }
}
