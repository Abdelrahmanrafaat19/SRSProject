using SRSProject.Application.Dtos;
using SRSProject.Domain.Entities;
using StockManagment.Application.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Contracts
{
    public interface IWeeklyHolidayService
    {
        Task<Result<WeeklyHolidayDto>> CreateWeekHolidayAsync(string Role, WeeklyHolidayDto weeklyHolidayDto, CancellationToken ct=default!);
        Task<Result<IReadOnlyList<WeeklyHolidayDto>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<bool>> IsWeeklyHolidayAsync(DateTime date, CancellationToken ct = default);
    }
}
