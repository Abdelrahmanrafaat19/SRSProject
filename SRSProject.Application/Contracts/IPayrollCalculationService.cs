using SRSProject.Application.Dtos;
using StockManagment.Application.common;
using System.Threading;
using System.Threading.Tasks;

namespace SRSProject.Application.Contracts
{
    public interface IPayrollCalculationService
    {
        Task<Result<PayrollRecordDto>> CalculatePayrollAsync(int employeeId, int year, int month, CancellationToken cancellationToken = default);
    }
}
