using System;

namespace SRSProject.Application.Dtos
{
    public sealed class AttendanceReportDto
    {
        public int? EmployeeId { get; set; }
        public string? NationalId { get; set; }

        public DateOnly From { get; set; }
        public DateOnly To { get; set; }

        public int TotalDays { get; set; }
        public int LateDays { get; set; }
        public int AbsentDays { get; set; }
        public int IntimeDays { get; set; }

        // Total not-completed minutes (converted to hours when showing)
        public double NotCompleteHours { get; set; }
    }
}
