using SRSProject.Application.Contracts;
using StockManagment.Application.common;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SRSProject.Infrastructure.Services
{
    public class EncryptionService : IEncryptionService
    {
        public Task<Result<string>> HashAsync(string plain, CancellationToken cancellationToken = default)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(plain);
            var hash = sha.ComputeHash(bytes);
            var hashString = Convert.ToHexString(hash);
            return Task.FromResult(Result<string>.Success(hashString));
        }

        public Task<Result<bool>> VerifyAsync(string hash, string plain, CancellationToken cancellationToken = default)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(plain);
            var computed = Convert.ToHexString(sha.ComputeHash(bytes));
            return Task.FromResult(Result<bool>.Success(string.Equals(hash, computed, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
