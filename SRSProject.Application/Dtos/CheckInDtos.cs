using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Dtos
{
    public sealed class CheckInDtos
    {
        public int EmployeeId { get; set; }

        public DateOnly AttendanceDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public TimeOnly CheckInTime { get; set; } = TimeOnly.FromDateTime(DateTime.Now);
    }
}
