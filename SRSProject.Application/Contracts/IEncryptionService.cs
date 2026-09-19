using StockManagment.Application.common;
using System.Threading;
using System.Threading.Tasks;

namespace SRSProject.Application.Contracts
{
    public interface IEncryptionService
    {
        Task<Result<string>> HashAsync(string plain, CancellationToken cancellationToken = default);
        Task<Result<bool>> VerifyAsync(string hash, string plain, CancellationToken cancellationToken = default);
    }
}
