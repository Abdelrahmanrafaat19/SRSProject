using SRSProject.Application.Dtos;
using StockManagment.Application.common;
using System.Threading;
using System.Threading.Tasks;

namespace SRSProject.Application.Contracts
{
    public interface IPayrollPolicyService
    {
        Task<Result<PayrollPolicyDto>> GetPolicyAsync(CancellationToken cancellationToken = default);
        Task<Result<PayrollPolicyDto>> UpdatePolicyAsync(PayrollPolicyDto dto, string role, CancellationToken cancellationToken = default);
    }
}
