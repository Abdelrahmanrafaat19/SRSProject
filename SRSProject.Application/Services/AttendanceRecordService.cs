

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
        private readonly IOfficialHolidayService _officialHolidayService;
        private readonly IWeeklyHolidayService _weeklyHolidayService;

        public AttendanceRecordService(IUnitOfWork unitOfWork, IOfficialHolidayService officialHolidayService, IWeeklyHolidayService weeklyHolidayService)
        {
            _unitOfWork = unitOfWork;
            _officialHolidayService = officialHolidayService;
            _weeklyHolidayService = weeklyHolidayService;
        }

        public async Task<Result<AttendanceReportDto>> GetReportForEmployeeAsync(int? employeeId, DateOnly? from, DateOnly? to)
        {
            var officalDays = (await _officialHolidayService.Grid(CancellationToken.None)).Value.Select(h => new
            {
                Day = h.Day,
                Month = h.Month,
            }).ToList();
            // load weekly holiday definitions (days of week)
            var weeklyDtos = (await _weeklyHolidayService.GetAllAsync(CancellationToken.None)).Value ?? new List<WeeklyHolidayDto>();
            var weeklyDaysSet = weeklyDtos.SelectMany(w => w.DaysOfHoliday ?? new List<DayOfWeek>()).Distinct().ToHashSet();
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
                        var isOfficialHoliday = officalDays.Any(h => h.Day == day.Day && h.Month == day.Month);
                        var attendance = recs.FirstOrDefault(r => r.AttendanceDate == day);

                        if (isOfficialHoliday)
                        {
                            report.OfficalDays++;
                            continue;
                        }

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

                var isWeeklyHoliday = weeklyDaysSet.Contains(day.DayOfWeek);
                var isOfficialHoliday = officalDays.Any(h => h.Day == day.Day && h.Month == day.Month);
                var attendance = records.FirstOrDefault(r => r.AttendanceDate == day);

                // Official holiday takes precedence
                if (isOfficialHoliday)
                {
                    report.OfficalDays++;
                    continue;
                }

                if (isWeeklyHoliday)
                {
                    report.WeeklyHolidayDays++;
                    continue;
                }

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
            // Validate official and weekly holidays
            var officalDays = (await _officialHolidayService.Grid(CancellationToken.None)).Value;
            var isOfficialHoliday = officalDays.Any(h => h.Day == data.AttendanceDate.Day && h.Month == data.AttendanceDate.Month);
            var weeklyDtos = (await _weeklyHolidayService.GetAllAsync(CancellationToken.None)).Value ?? new List<WeeklyHolidayDto>();
            var weeklyDaysSet = weeklyDtos.SelectMany(w => w.DaysOfHoliday ?? new List<DayOfWeek>()).Distinct().ToHashSet();
            var isWeeklyHoliday = weeklyDaysSet.Contains(data.AttendanceDate.DayOfWeek);
        

            var repo = _unitOfWork.Repository<int, AttendanceRecord>();
            var exists = await repo.AnyAsync(a => a.EmployeeId == data.EmployeeId && a.AttendanceDate == data.AttendanceDate);
            if (exists)
            {
                return Result<bool>.Failure(Error.Failure("Validation", "Already checked in for this date"));
            }
            if (isOfficialHoliday)
            {
                // On official holidays we treat as intime (no late)
                var attendance = new AttendanceRecord()
                {
                    AttendanceDate = data.AttendanceDate,
                    CheckInTime = null,
                    EmployeeId = data.EmployeeId,
                    LateMinutes = 0,
                    Status = AttendanceStatus.Intime,
                };

                await repo.AddAsync(attendance);
            }
            else if (isWeeklyHoliday)
            {
                // On weekly holidays record the holiday status for the employee
                var attendance = new AttendanceRecord()
                {
                    AttendanceDate = data.AttendanceDate,
                    CheckInTime = null,
                    EmployeeId = data.EmployeeId,
                    LateMinutes = 0,
                    Status = AttendanceStatus.WeeklyHoliday,
                };

                await repo.AddAsync(attendance);
            }
            else
            {
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
            }

            var result = await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(Error.Failure("ErrorType", "Not Checkin"));
        }

        public async Task<Result<bool>> CheckOutAsync(CheckOutDtos data)
        {
            // Validate official holiday: do not allow check-out on official holidays
            var officalDays = (await _officialHolidayService.Grid(CancellationToken.None)).Value;
            var isOfficialHoliday = officalDays.Any(h => h.Day == data.AttendanceDate.Day && h.Month == data.AttendanceDate.Month);
            var weeklyDtos = (await _weeklyHolidayService.GetAllAsync(CancellationToken.None)).Value ?? new List<WeeklyHolidayDto>();
            var weeklyDaysSet = weeklyDtos.SelectMany(w => w.DaysOfHoliday ?? new List<DayOfWeek>()).Distinct().ToHashSet();
            var isWeeklyHoliday = weeklyDaysSet.Contains(data.AttendanceDate.DayOfWeek);


            var repo = _unitOfWork.Repository<int, AttendanceRecord>();

            var records = await repo.FindAsync(a => a.EmployeeId == data.EmployeeId && a.AttendanceDate == data.AttendanceDate);
            var attendance = records.FirstOrDefault();

            if (isOfficialHoliday)
            {
                // No checkout needed on official holidays
                return Result<bool>.Success(true);
            }

            if (isWeeklyHoliday)
            {
                // No checkout needed on weekly holidays
                // Optionally create record if missing
                if (attendance == null)
                {
                    await repo.AddAsync(new AttendanceRecord
                    {
                        AttendanceDate = data.AttendanceDate,
                        EmployeeId = data.EmployeeId,
                        Status = AttendanceStatus.WeeklyHoliday
                    });
                    await _unitOfWork.SaveChangesAsync();
                }

                return Result<bool>.Success(true);
            }

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
            else if (attendance.LateMinutes.HasValue && attendance.LateMinutes > 0&&!isOfficialHoliday)
            {
                attendance.Status = AttendanceStatus.Late;
            }
            else if (notCompleteMinutes > 0&&!isOfficialHoliday)
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
