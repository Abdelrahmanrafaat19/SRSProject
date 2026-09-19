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
    public class CompanySettingsService : ICompanySettingsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanySettingsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CompanySettingsDto>> GetSettingsAsync(CancellationToken cancellationToken = default)
        {
            var repo = _unitOfWork.Repository<int, CompanySettings>();
            var list = await repo.GetAllAsync(cancellationToken);
            var settings = list.FirstOrDefault();
            if (settings == null)
            {
                // create default settings
                settings = new CompanySettings
                {
                    DefaultTaxRate = 0.05m,
                    WorkStart = new TimeOnly(9, 0),
                    WorkEnd = new TimeOnly(17, 0),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await repo.AddAsync(settings, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            var dto = new CompanySettingsDto
            {
                DefaultTaxRate = settings.DefaultTaxRate,
                WorkStart = settings.WorkStart,
                WorkEnd = settings.WorkEnd
            };

            return Result<CompanySettingsDto>.Success(dto);
        }

        public async Task<Result<CompanySettingsDto>> UpdateSettingsAsync(CompanySettingsDto dto, string role, CancellationToken cancellationToken = default)
        {
            if (!role?.Contains("Admin") == true)
                return Result<CompanySettingsDto>.Failure(Error.Unauthorized("Settings.Update", "Only admins can update settings."));

            var repo = _unitOfWork.Repository<int, CompanySettings>();
            var list = await repo.GetAllAsync(cancellationToken);
            var settings = list.FirstOrDefault();

            if (settings == null)
            {
                settings = new CompanySettings
                {
                    DefaultTaxRate = dto.DefaultTaxRate,
                    WorkStart = dto.WorkStart,
                    WorkEnd = dto.WorkEnd,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await repo.AddAsync(settings, cancellationToken);
            }
            else
            {
                settings.DefaultTaxRate = dto.DefaultTaxRate;
                settings.WorkStart = dto.WorkStart;
                settings.WorkEnd = dto.WorkEnd;
                settings.UpdatedAt = DateTime.UtcNow;

                repo.Update(settings);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<CompanySettingsDto>.Success(dto);
        }
    }
}
