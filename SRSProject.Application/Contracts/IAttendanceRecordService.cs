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
    }
}
