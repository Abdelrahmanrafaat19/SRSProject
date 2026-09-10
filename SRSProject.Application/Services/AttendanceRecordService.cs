

using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using SRSProject.Domain.Contract;
using SRSProject.Domain.Entities;
using SRSProject.Infrastructure.DataContext;
using StockManagment.Application.common;

namespace SRSProject.Application.Services
{
    public class AttendanceRecordService : IAttendanceRecordService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceRecordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> CheckInAsync(CheckInDtos data)
        {
            DateTime nineAm = DateTime.Today.AddHours(9);
            var countOFMinutesLate = (data.CheckInTime.ToTimeSpan() - nineAm.TimeOfDay).TotalMinutes;
            await _unitOfWork.Repository<int, AttendanceRecord>().AddAsync(new AttendanceRecord()
            {
                AttendanceDate = data.AttendanceDate,
                CheckInTime = data.CheckInTime,
                EmployeeId = data.EmployeeId,
                Status = countOFMinutesLate > 0 ? AttendanceStatus.Late  : AttendanceStatus.Intime,
            });

            var result = await _unitOfWork.SaveChangesAsync();
            if (result > 0){
                return Result<bool>.Success(true);
            }
            return Result<bool>.Failure(Error.Failure("ErrorType" , "Not Checkin"));
        }
    }
}
