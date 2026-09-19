using SRSProject.Application.Dtos;
using StockManagment.Application.common;
using System.Threading;
using System.Threading.Tasks;

namespace SRSProject.Application.Contracts
{
    public interface ICompanySettingsService
    {
        Task<Result<CompanySettingsDto>> GetSettingsAsync(CancellationToken cancellationToken = default);
        Task<Result<CompanySettingsDto>> UpdateSettingsAsync(CompanySettingsDto dto, string role, CancellationToken cancellationToken = default);
    }
}
