using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using SRSProject.Domain.Contract;
using SRSProject.Domain.Entities;
using StockManagment.Application.common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SRSProject.Application.Services
{
    public class WeeklyHolidayService : IWeeklyHolidayService
    {
        private readonly IUnitOfWork _uniteOfWork;
        public WeeklyHolidayService(IUnitOfWork unitOfWork)
        {
            _uniteOfWork = unitOfWork;
        }
        public async Task<Result<WeeklyHolidayDto>> CreateWeekHolidayAsync(string Role, WeeklyHolidayDto weeklyHolidayDto, CancellationToken ct = default)
        {
            if (!Role.Contains("Admin"))
            {
                return Result<WeeklyHolidayDto>.Failure(Error.Unauthorized("Unauthorized", "You do not have permission to create a weekly holiday."));
            }

            await _uniteOfWork.Repository<int, WeekHolidayEntity>()
                .AddAsync(
                new WeekHolidayEntity()
                {
                    NameOfHoliday = weeklyHolidayDto.NameOfHoliday,
                    DaysOfHoliday = weeklyHolidayDto.DaysOfHoliday
                });
            var countOfProcess = await _uniteOfWork.SaveChangesAsync(ct);
            if (countOfProcess == 0)
            {
                return Result<WeeklyHolidayDto>.Failure(Error.Failure("CreateFailed", "Failed to create the weekly holiday."));

            }
            else
            {
                return Result<WeeklyHolidayDto>.Success(weeklyHolidayDto);
            }
        }

        public async Task<Result<IReadOnlyList<WeeklyHolidayDto>>> GetAllAsync(CancellationToken ct = default)
        {
            var result = await _uniteOfWork.Repository<int, WeekHolidayEntity>().GetAllAsync(ct);
            var data = result.Select(x => new WeeklyHolidayDto
            {
                NameOfHoliday = x.NameOfHoliday,
                DaysOfHoliday = x.DaysOfHoliday
            }).ToList();

            return Result<IReadOnlyList<WeeklyHolidayDto>>.Success(data);
        }

        public async Task<Result<bool>> IsWeeklyHolidayAsync(DateTime date, CancellationToken ct = default)
        {
            var result = await _uniteOfWork.Repository<int, WeekHolidayEntity>().GetAllAsync(ct);
            if (result == null || !result.Any())
                return Result<bool>.Success(false);

            var day = date.DayOfWeek;
            var isHoliday = result.Any(w => w.DaysOfHoliday != null && w.DaysOfHoliday.Contains(day));
            return Result<bool>.Success(isHoliday);
        }
    }
}
