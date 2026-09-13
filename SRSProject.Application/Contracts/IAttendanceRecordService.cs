using SRSProject.Application.Dtos;
using StockManagment.Application.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Contracts
{
    public interface IAttendanceRecordService
    {
        public Task<Result<bool>> CheckInAsync(CheckInDtos data);
        public Task<Result<bool>> CheckOutAsync(CheckOutDtos data);
        public Task<Result<AttendanceReportDto>> GetReportForEmployeeAsync(
            int? employeeId,
            DateOnly? from,
            DateOnly? to);

        public Task<Result<AttendanceReportDto>> GetReportForAdminAsync(
            string? NationalID,
            DateOnly? from,
            DateOnly? to);
    }
}
