using StockManagment.Application.common;
using SRSProject.Application.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SRSProject.Application.Contracts
{
    public interface IAuditService
    {
        Task<Result<bool>> LogAsync(string action, string user, string details, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<AuditDto>>> GetLogsAsync(CancellationToken cancellationToken = default);
    }
}
