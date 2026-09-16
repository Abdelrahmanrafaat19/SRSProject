using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using SRSProject.Domain.Contract;
using SRSProject.Infrastructure;
using StockManagment.Application.common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SRSProject.Application.Services
{
    public class OfficialHolidayService : IOfficialHolidayService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OfficialHolidayService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<OfficialHolidayDto>> CreateHolidayAsync(string Role, OfficialHolidayDto holidayDto, CancellationToken cancellationToken = default)
        {

            if (!Role.Contains("Admin"))
            {
                return Result<OfficialHolidayDto>.Failure(Error.Unauthorized("ErrorType.Unauthorized", "Donot Have permission to create Offcial Holiday"));
            }

            await _unitOfWork.Repository<int, OfficialHolidayEntity>().AddAsync(new OfficialHolidayEntity
            {
                Name = holidayDto.Name,
                Day = holidayDto.Day,
                Month = holidayDto.Month,
                IsActive = holidayDto.IsActive
            }, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
            {
                return Result<OfficialHolidayDto>.Failure(Error.Failure("ErrorType.Failure", "Failed to create Official Holiday"));

            }

            return Result<OfficialHolidayDto>.Success(holidayDto);
        }

        public async Task<Result<IReadOnlyList<OfficialHolidayDto>>> Grid(CancellationToken cancellationToken = default)
        {
            var data = await _unitOfWork.Repository<int, OfficialHolidayEntity>().GetAllAsync(cancellationToken);
            var dtos = data.Select(x => new OfficialHolidayDto
            {
                Name = x.Name,
                Day = x.Day,
                Month = x.Month,
                IsActive = x.IsActive
            }).ToList();

            return Result<IReadOnlyList<OfficialHolidayDto>>.Success(dtos);
        }

        public async Task<Result<OfficialHolidayDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _unitOfWork.Repository<int, OfficialHolidayEntity>().GetByIdAsync(id, cancellationToken);
            if (entity is null)
            {
                return Result<OfficialHolidayDto>.Failure(Error.Validation("OfficialHoliday.GetById", "Official holiday not found"));
            }

            var dto = new OfficialHolidayDto
            {
                Name = entity.Name,
                Day = entity.Day,
                Month = entity.Month,
                IsActive = entity.IsActive
            };

            return Result<OfficialHolidayDto>.Success(dto);
        }

        public async Task<Result<bool>> DeleteHolidayAsync(int id, string role, CancellationToken cancellationToken = default)
        {
            if (!role.Contains("Admin"))
            {
                return Result<bool>.Failure(Error.Unauthorized("OfficialHoliday.Delete", "Donot Have permission to delete Official Holiday"));
            }

            var entity = await _unitOfWork.Repository<int, OfficialHolidayEntity>().GetByIdAsync(id, cancellationToken);
            if (entity is null)
            {
                return Result<bool>.Failure(Error.Validation("OfficialHoliday.Delete", "Official holiday not found"));
            }

            await _unitOfWork.Repository<int, OfficialHolidayEntity>().Delete(entity);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
            {
                return Result<bool>.Failure(Error.Failure("OfficialHoliday.Delete", "Failed to delete Official Holiday"));
            }

            return Result<bool>.Success(true);
        }

        public async Task<Result<OfficialHolidayDto>> UpdateHolidayAsync(UpdateOfficialHolidayDto dto, string role, CancellationToken cancellationToken = default)
        {
            if (!role.Contains("Admin"))
            {
                return Result<OfficialHolidayDto>.Failure(Error.Unauthorized("OfficialHoliday.Update", "Donot Have permission to update Official Holiday"));
            }

            var entity = await _unitOfWork.Repository<int, OfficialHolidayEntity>().GetByIdAsync(dto.Id, cancellationToken);
            if (entity is null)
            {
                return Result<OfficialHolidayDto>.Failure(Error.Validation("OfficialHoliday.Update", "Official holiday not found"));
            }

            entity.Name = dto.Name;
            entity.Day = dto.Day;
            entity.Month = dto.Month;
            entity.IsActive = dto.IsActive;

            _unitOfWork.Repository<int, OfficialHolidayEntity>().Update(entity);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result == 0)
            {
                return Result<OfficialHolidayDto>.Failure(Error.Failure("OfficialHoliday.Update", "Failed to update Official Holiday"));
            }

            var updatedDto = new OfficialHolidayDto
            {
                Name = entity.Name,
                Day = entity.Day,
                Month = entity.Month,
                IsActive = entity.IsActive
            };

            return Result<OfficialHolidayDto>.Success(updatedDto);
        }
    }
}
