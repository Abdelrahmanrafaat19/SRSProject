using SRSProject.Application.Dtos;
using StockManagment.Application.common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SRSProject.Application.Contracts
{
    public interface IOfficialHolidayService
    {
        Task<Result<OfficialHolidayDto>> CreateHolidayAsync(string Role, OfficialHolidayDto holidayDto, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<OfficialHolidayDto>>> Grid(CancellationToken cancellationToken = default);
        Task<Result<OfficialHolidayDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteHolidayAsync(int id, string role, CancellationToken cancellationToken = default);
        Task<Result<OfficialHolidayDto>> UpdateHolidayAsync(UpdateOfficialHolidayDto dto, string role, CancellationToken cancellationToken = default);
    }
}
