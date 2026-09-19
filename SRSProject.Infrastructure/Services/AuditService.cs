using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using StockManagment.Application.common;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SRSProject.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private static readonly List<AuditDto> _logs = new();

        public Task<Result<bool>> LogAsync(string action, string user, string details, CancellationToken cancellationToken = default)
        {
            _logs.Add(new AuditDto { Action = action, User = user, Details = details, Timestamp = System.DateTime.UtcNow });
            return Task.FromResult(Result<bool>.Success(true));
        }

        public Task<Result<IReadOnlyList<AuditDto>>> GetLogsAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result<IReadOnlyList<AuditDto>>.Success(_logs.ToList()));
        }
    }
}
