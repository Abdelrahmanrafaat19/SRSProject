

using SRSProject.Application.Contracts;
using SRSProject.Application.Dtos;
using SRSProject.Domain.Contract;
using SRSProject.Domain.Entities;
using SRSProject.Infrastructure.DataContext;
using StockManagment.Application.common;
using System.Linq;

namespace SRSProject.Application.Services
{
    public class AttendanceRecordService : IAttendanceRecordService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceRecordService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AttendanceReportDto>> GetReportForEmployeeAsync(int? employeeId, DateOnly? from, DateOnly? to)
        {
            
            var toDate = to ?? DateOnly.FromDateTime(DateTime.Now);
            var fromDate = from ?? toDate;

            var report = new AttendanceReportDto
            {
                EmployeeId = employeeId,
                From = fromDate,
                To = toDate
            };

            if (employeeId is null)
            {
                
                var repoAll = _unitOfWork.Repository<int, AttendanceRecord>();
                var recordsAll = await repoAll.FindAsync(a => a.AttendanceDate >= fromDate && a.AttendanceDate <= toDate);

                var daysAll = new List<DateOnly>();
                for (var d = fromDate; d <= toDate; d = d.AddDays(1)) daysAll.Add(d);

                report.TotalDays = daysAll.Count;

                
                var expectedEndAll = new TimeOnly(17, 0);

                
                var grouped = recordsAll.GroupBy(r => r.EmployeeId);
                foreach (var group in grouped)
                {
                    var recs = group.ToList();
                    foreach (var day in daysAll)
                    {
                        var attendance = recs.FirstOrDefault(r => r.AttendanceDate == day);
                        if (attendance == null || !attendance.CheckInTime.HasValue)
                        {
                            report.AbsentDays++;
                            continue;
                        }

                        if (attendance.LateMinutes.HasValue && attendance.LateMinutes > 0)
                        {
                            report.LateDays++;
                        }
                        else if (attendance.Status == AttendanceStatus.Intime)
                        {
                            report.IntimeDays++;
                        }

                        TimeOnly? checkOut = attendance.CheckOutTime;
                        if (checkOut.HasValue)
                        {
                            if (checkOut.Value < expectedEndAll)
                            {
                                var minutes = (expectedEndAll.ToTimeSpan() - checkOut.Value.ToTimeSpan()).TotalMinutes;
                                report.NotCompleteHours += minutes / 60.0;
                            }
                        }
                        else
                        {
                            var minutes = (expectedEndAll.ToTimeSpan() - attendance.CheckInTime!.Value.ToTimeSpan()).TotalMinutes;
                            if (minutes > 0)
                                report.NotCompleteHours += minutes / 60.0;
                        }
                    }
                }

                report.NotCompleteHours = Math.Round(report.NotCompleteHours, 2);
                return Result<AttendanceReportDto>.Success(report);
            }

            var repo = _unitOfWork.Repository<int, AttendanceRecord>();
            var empRepo = _unitOfWork.Repository<int, EmployeeEntity>();

            var employee = (await empRepo.FindAsync(e => e.Id == employeeId)).FirstOrDefault();

            var expectedEnd = employee?.ExpectedCheckOutTime ?? new TimeOnly(17, 0);

            var records = await repo.FindAsync(a => a.EmployeeId == employeeId && a.AttendanceDate >= fromDate && a.AttendanceDate <= toDate);

           
            var days = new List<DateOnly>();
            for (var d = fromDate; d <= toDate; d = d.AddDays(1)) days.Add(d);

            report.TotalDays = days.Count;

            foreach (var day in days)
            {
                var attendance = records.FirstOrDefault(r => r.AttendanceDate == day);

                if (attendance == null || !attendance.CheckInTime.HasValue)
                {
                    report.AbsentDays++;
                    continue;
                }

                if (attendance.LateMinutes.HasValue && attendance.LateMinutes > 0)
                {
                    report.LateDays++;
                }
                else if (attendance.Status == AttendanceStatus.Intime)
                {
                    report.IntimeDays++;
                }

                TimeOnly? checkOut = attendance.CheckOutTime;
                if (checkOut.HasValue)
                {
                    if (checkOut.Value < expectedEnd)
                    {
                        var minutes = (expectedEnd.ToTimeSpan() - checkOut.Value.ToTimeSpan()).TotalMinutes;
                        report.NotCompleteHours += minutes / 60.0;
                    }
                }
                else
                {
                    var minutes = (expectedEnd.ToTimeSpan() - attendance.CheckInTime!.Value.ToTimeSpan()).TotalMinutes;
                    if (minutes > 0)
                        report.NotCompleteHours += minutes / 60.0;
                }
            }

            report.NotCompleteHours = Math.Round(report.NotCompleteHours, 2);

            if (employee is not null)
                report.NationalId = employee.NationalId;

            return Result<AttendanceReportDto>.Success(report);
        }

        public async Task<Result<AttendanceReportDto>> GetReportForAdminAsync(string? NationalID, DateOnly? from, DateOnly? to)
        {
          

            var toDate = to ?? DateOnly.FromDateTime(DateTime.Now);
            var fromDate = from ?? toDate;

            if (string.IsNullOrWhiteSpace(NationalID))
            {
                return Result<AttendanceReportDto>.Success(new AttendanceReportDto { From = fromDate, To = toDate });
            }

            var empRepo = _unitOfWork.Repository<int, EmployeeEntity>();
            var employees = await empRepo.FindAsync(e => e.NationalId == NationalID);
            var employee = employees.FirstOrDefault();
            if (employee == null)
            {
                return Result<AttendanceReportDto>.Success(new AttendanceReportDto { From = fromDate, To = toDate });
            }

            return await GetReportForEmployeeAsync(employee.Id, fromDate, toDate);
        }
        public async Task<Result<bool>> CheckInAsync(CheckInDtos data)
        {
            var repo = _unitOfWork.Repository<int, AttendanceRecord>();
            var exists = await repo.AnyAsync(a => a.EmployeeId == data.EmployeeId && a.AttendanceDate == data.AttendanceDate);
            if (exists)
            {
                return Result<bool>.Failure(Error.Failure("Validation", "Already checked in for this date"));
            }
            var workStart = data.AttendanceDate.ToDateTime(new TimeOnly(9, 0));
            var minutesLate = (data.CheckInTime.ToTimeSpan() - workStart.TimeOfDay).TotalMinutes;
            var attendance = new AttendanceRecord()
            {
                AttendanceDate = data.AttendanceDate,
                CheckInTime = data.CheckInTime,
                EmployeeId = data.EmployeeId,
                LateMinutes = minutesLate > 0 ? (int)minutesLate : 0,
                Status = minutesLate > 0 ? AttendanceStatus.Late : AttendanceStatus.Intime,
            };

            await repo.AddAsync(attendance);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(Error.Failure("ErrorType", "Not Checkin"));
        }

        public async Task<Result<bool>> CheckOutAsync(CheckOutDtos data)
        {
            var repo = _unitOfWork.Repository<int, AttendanceRecord>();

            var records = await repo.FindAsync(a => a.EmployeeId == data.EmployeeId && a.AttendanceDate == data.AttendanceDate);
            var attendance = records.FirstOrDefault();

            if (attendance == null)
            {
             
                return Result<bool>.Failure(Error.Failure("Validation", "No check-in record found for this employee and date"));
            }

            var workEnd = new TimeOnly(17, 0);
            var checkOutTime = data.CheckOutTime;

            var overtimeMinutes = 0;
            var notCompleteMinutes = 0;

            var diff = (checkOutTime.ToTimeSpan() - workEnd.ToTimeSpan()).TotalMinutes;
            if (diff > 0)
            {
                overtimeMinutes = (int)diff;
            }
            else if (diff < 0)
            {
                notCompleteMinutes = (int)(-diff);
            }

            attendance.CheckOutTime = data.CheckOutTime;
            attendance.OvertimeMinutes = overtimeMinutes;


            if (!attendance.CheckInTime.HasValue)
            {
                attendance.Status = AttendanceStatus.Absent;
            }
            else if (attendance.LateMinutes.HasValue && attendance.LateMinutes > 0)
            {
                attendance.Status = AttendanceStatus.Late;
            }
            else if (notCompleteMinutes > 0)
            {
                attendance.Status = AttendanceStatus.NotCompleteYourTime;
            }
            else
            {
                attendance.Status = AttendanceStatus.Intime;
            }

            repo.Update(attendance);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result > 0)
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(Error.Failure("ErrorType", "Not Checkout"));
        }
    }
}
