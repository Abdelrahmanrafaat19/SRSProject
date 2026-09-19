using SRSProject.Application.Dtos;
using StockManagment.Application.common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SRSProject.Application.Contracts
{
    public interface IPayrollReportService
    {
        Task<Result<IReadOnlyList<PayrollRecordDto>>> GenerateMonthlyReportAsync(int year, int month, CancellationToken cancellationToken = default);
    }
}
