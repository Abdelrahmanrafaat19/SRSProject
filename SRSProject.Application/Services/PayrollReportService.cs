using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using SRSProject.Domain.Contract;
using StockManagment.Application.common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SRSProject.Application.Services
{
    public class PayrollReportService : IPayrollReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PayrollReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IReadOnlyList<PayrollRecordDto>>> GenerateMonthlyReportAsync(int year, int month, CancellationToken cancellationToken = default)
        {
            var repo = _unitOfWork.Repository<int, Domain.Entities.PayrollRecord>();
            var records = await repo.FindAsync(p => p.Year == year && p.Month == month, cancellationToken);

            var list = records.Select(r => new PayrollRecordDto
            {
                Id = r.Id,
                EmployeeId = r.EmployeeId,
                Year = r.Year,
                Month = r.Month,
                BasicSalary = r.BasicSalary,
                TotalAdditions = r.TotalAdditions,
                TotalDeductions = r.TotalDeductions,
                NetSalary = r.NetSalary,
                Adjustments = System.Array.Empty<PayrollAdjustmentDto>()
            }).ToList();

            return Result<IReadOnlyList<PayrollRecordDto>>.Success(list);
        }
    }
}
