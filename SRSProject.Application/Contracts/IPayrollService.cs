using SRSProject.Application.Dtos;
using StockManagment.Application.common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SRSProject.Application.Contracts
{
    public interface IPayrollService
    {
        Task<Result<PayrollRecordDto>> CreatePayrollAsync(string role, CreatePayrollDto dto, CancellationToken cancellationToken = default);
        Task<Result<PayrollRecordDto>> GetByEmployeeMonthAsync(int employeeId, int year, int month);
        Task<Result<IReadOnlyList<PayrollRecordDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<bool>> AddAdjustmentAsync(int payrollRecordId, PayrollAdjustmentDto dto, string role, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeletePayrollAsync(int payrollRecordId, string role, CancellationToken cancellationToken = default);
    }
}
