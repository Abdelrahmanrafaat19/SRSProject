using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using StockManagment.Application.common;
using System.Threading;
using System.Threading.Tasks;
using SRSProject.Domain.Contract;
using SRSProject.Domain.Entities;
using System.Linq;

namespace SRSProject.Application.Services
{
    public class PayrollPolicyService : IPayrollPolicyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PayrollPolicyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PayrollPolicyDto>> GetPolicyAsync(CancellationToken cancellationToken = default)
        {
            var repo = _unitOfWork.Repository<int, PayrollPolicy>();
            var list = await repo.GetAllAsync(cancellationToken);
            var policy = list.FirstOrDefault();
            if (policy == null)
            {
                policy = new PayrollPolicy
                {
                    TaxRate = 0.05m,
                    SocialSecurityRate = 0.02m,
                    CreatedAt = System.DateTime.UtcNow,
                    UpdatedAt = System.DateTime.UtcNow
                };
                await repo.AddAsync(policy, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return Result<PayrollPolicyDto>.Success(new PayrollPolicyDto
            {
                TaxRate = policy.TaxRate,
                SocialSecurityRate = policy.SocialSecurityRate
            });
        }

        public async Task<Result<PayrollPolicyDto>> UpdatePolicyAsync(PayrollPolicyDto dto, string role, CancellationToken cancellationToken = default)
        {
            if (!role?.Contains("Admin") == true)
                return Result<PayrollPolicyDto>.Failure(Error.Unauthorized("PayrollPolicy.Update", "Only admins can update payroll policy."));

            var repo = _unitOfWork.Repository<int, PayrollPolicy>();
            var list = await repo.GetAllAsync(cancellationToken);
            var policy = list.FirstOrDefault();

            if (policy == null)
            {
                policy = new PayrollPolicy
                {
                    TaxRate = dto.TaxRate,
                    SocialSecurityRate = dto.SocialSecurityRate,
                    CreatedAt = System.DateTime.UtcNow,
                    UpdatedAt = System.DateTime.UtcNow
                };
                await repo.AddAsync(policy, cancellationToken);
            }
            else
            {
                policy.TaxRate = dto.TaxRate;
                policy.SocialSecurityRate = dto.SocialSecurityRate;
                policy.UpdatedAt = System.DateTime.UtcNow;
                repo.Update(policy);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<PayrollPolicyDto>.Success(dto);
        }
    }
}
