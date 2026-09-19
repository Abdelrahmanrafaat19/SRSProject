using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using SRSProject.Domain.Contract;
using SRSProject.Domain.Entities;
using StockManagment.Application.common;


namespace SRSProject.Application.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PayrollService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PayrollRecordDto>> CreatePayrollAsync(string role, CreatePayrollDto dto, CancellationToken cancellationToken = default)
        {
            if (!role?.Contains("Admin") == true)
            {
                return Result<PayrollRecordDto>.Failure(Error.Unauthorized("Payroll.Create", "Only admins can create payroll records."));
            }
            var exists = await _unitOfWork.Repository<int, PayrollRecord>().AnyAsync(p => p.EmployeeId == dto.EmployeeId && p.Year == dto.Year && p.Month == dto.Month);
            if (exists)
                return Result<PayrollRecordDto>.Failure(Error.Conflict("Payroll.Create", "Payroll record already exists for this employee and period."));

            var payroll = new PayrollRecord
            {
                EmployeeId = dto.EmployeeId,
                Year = dto.Year,
                Month = dto.Month,
                BasicSalary = dto.BasicSalary,
                TotalAdditions = 0m,
                TotalDeductions = 0m,
                NetSalary = dto.BasicSalary
            };

            await _unitOfWork.Repository<int, PayrollRecord>().AddAsync(payroll, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = new PayrollRecordDto
            {
                Id = payroll.Id,
                EmployeeId = payroll.EmployeeId,
                Year = payroll.Year,
                Month = payroll.Month,
                BasicSalary = payroll.BasicSalary,
                TotalAdditions = payroll.TotalAdditions,
                TotalDeductions = payroll.TotalDeductions,
                NetSalary = payroll.NetSalary,
                Adjustments = Array.Empty<PayrollAdjustmentDto>()
            };

            return Result<PayrollRecordDto>.Success(resultDto);
        }

        public async Task<Result<bool>> AddAdjustmentAsync(int payrollRecordId, PayrollAdjustmentDto dto, string role, CancellationToken cancellationToken = default)
        {
            if (!role?.Contains("Admin") == true)
                return Result<bool>.Failure(Error.Unauthorized("Payroll.Adjustment", "Only admins can add adjustments."));

            var repo = _unitOfWork.Repository<int, PayrollRecord>();
            var payroll = (await repo.FindAsync(p => p.Id == payrollRecordId)).FirstOrDefault();
            if (payroll == null)
                return Result<bool>.Failure(Error.NotFound("Payroll.Adjustment", "Payroll record not found."));

            var adjustment = new PayrollAdjustment
            {
                PayrollRecordId = payrollRecordId,
                Type = dto.Type,
                Amount = dto.Amount,
                Reason = dto.Reason ?? string.Empty,
                CreatedAt = dto.CreatedAt == default ? DateTime.UtcNow : dto.CreatedAt
            };

            await _unitOfWork.Repository<int, PayrollAdjustment>().AddAsync(adjustment, cancellationToken);

       
            if (adjustment.Type == PayrollAdjustmentType.Addition)
                payroll.TotalAdditions += adjustment.Amount;
            else
                payroll.TotalDeductions += adjustment.Amount;

            payroll.NetSalary = payroll.BasicSalary + payroll.TotalAdditions - payroll.TotalDeductions;

            repo.Update(payroll);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }

        public async Task<Result<IReadOnlyList<PayrollRecordDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var repo = _unitOfWork.Repository<int, PayrollRecord>();
            var records = await repo.GetAllAsync(cancellationToken);

            var dtoList = records.Select(r => new PayrollRecordDto
            {
                Id = r.Id,
                EmployeeId = r.EmployeeId,
                Year = r.Year,
                Month = r.Month,
                BasicSalary = r.BasicSalary,
                TotalAdditions = r.TotalAdditions,
                TotalDeductions = r.TotalDeductions,
                NetSalary = r.NetSalary,
                Adjustments = Array.Empty<PayrollAdjustmentDto>()
            }).ToList();

            return Result<IReadOnlyList<PayrollRecordDto>>.Success(dtoList);
        }

        public async Task<Result<PayrollRecordDto>> GetByEmployeeMonthAsync(int employeeId, int year, int month)
        {
            var repo = _unitOfWork.Repository<int, PayrollRecord>();
            var records = await repo.FindAsync(p => p.EmployeeId == employeeId && p.Year == year && p.Month == month);
            var payroll = records.FirstOrDefault();
            if (payroll == null)
                return Result<PayrollRecordDto>.Failure(Error.NotFound("Payroll.Get", "Payroll record not found."));

      
            var adjRepo = _unitOfWork.Repository<int, PayrollAdjustment>();
            var adjustments = await adjRepo.FindAsync(a => a.PayrollRecordId == payroll.Id);

            var dto = new PayrollRecordDto
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

            return Result<PayrollRecordDto>.Success(dto);
        }

        public async Task<Result<bool>> DeletePayrollAsync(int payrollRecordId, string role, CancellationToken cancellationToken = default)
        {
            if (!role?.Contains("Admin") == true)
                return Result<bool>.Failure(Error.Unauthorized("Payroll.Delete", "Only admins can delete payroll records."));

            var repo = _unitOfWork.Repository<int, PayrollRecord>();
            var records = await repo.FindAsync(p => p.Id == payrollRecordId);
            var payroll = records.FirstOrDefault();
            if (payroll == null)
                return Result<bool>.Failure(Error.NotFound("Payroll.Delete", "Payroll record not found."));

           
            var adjRepo = _unitOfWork.Repository<int, PayrollAdjustment>();
            var adjustments = await adjRepo.FindAsync(a => a.PayrollRecordId == payrollRecordId);
            foreach (var a in adjustments)
                await adjRepo.Delete(a);

            repo.Delete(payroll);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
