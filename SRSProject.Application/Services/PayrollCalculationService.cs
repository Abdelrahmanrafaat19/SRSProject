using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using SRSProject.Domain.Contract;
using SRSProject.Domain.Entities;
using StockManagment.Application.common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SRSProject.Application.Services
{
    public class PayrollCalculationService : IPayrollCalculationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PayrollCalculationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PayrollRecordDto>> CalculatePayrollAsync(int employeeId, int year, int month, CancellationToken cancellationToken = default)
        {
            // Try to find existing payroll
            var repo = _unitOfWork.Repository<int, PayrollRecord>();
            var records = await repo.FindAsync(p => p.EmployeeId == employeeId && p.Year == year && p.Month == month, cancellationToken);
            var payroll = records.FirstOrDefault();

            if (payroll == null)
            {
                // Try to create a calculation from employee basic salary
                var empRepo = _unitOfWork.Repository<int, EmployeeEntity>();
                var emp = (await empRepo.FindAsync(e => e.Id == employeeId, cancellationToken)).FirstOrDefault();
                if (emp == null)
                    return Result<PayrollRecordDto>.Failure(Error.NotFound("Payroll.Calculate", "Employee not found"));

                var basic = emp.BasicSalary;
                var dto = new PayrollRecordDto
                {
                    Id = 0,
                    EmployeeId = employeeId,
                    Year = year,
                    Month = month,
                    BasicSalary = basic,
                    TotalAdditions = 0m,
                    TotalDeductions = 0m,
                    NetSalary = basic,
                    Adjustments = System.Array.Empty<PayrollAdjustmentDto>()
                };

                return Result<PayrollRecordDto>.Success(dto);
            }

            var adjRepo = _unitOfWork.Repository<int, PayrollAdjustment>();
            var adjustments = await adjRepo.FindAsync(a => a.PayrollRecordId == payroll.Id, cancellationToken);

            var dtoResult = new PayrollRecordDto
            {
                Id = payroll.Id,
                EmployeeId = payroll.EmployeeId,
                Year = payroll.Year,
                Month = payroll.Month,
                BasicSalary = payroll.BasicSalary,
                TotalAdditions = payroll.TotalAdditions,
                TotalDeductions = payroll.TotalDeductions,
                NetSalary = payroll.NetSalary,
                Adjustments = adjustments.Select(a => new PayrollAdjustmentDto
                {
                    Id = a.Id,
                    Type = a.Type,
                    Amount = a.Amount,
                    Reason = a.Reason,
                    CreatedAt = a.CreatedAt
                }).ToList()
            };

            return Result<PayrollRecordDto>.Success(dtoResult);
        }
    }
}
