using SRSProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Infrastructure.DataContext
{
    public class AttendanceRecord :BasicEntity<int>
    {
        public int EmployeeId { get; set; }
        public DateOnly AttendanceDate { get; set; }

        public TimeOnly? CheckInTime { get; set; } 
        public TimeOnly? CheckOutTime { get; set; }

        public int? LateMinutes { get; set; }
        public int? OvertimeMinutes { get; set; }

        public AttendanceStatus Status { get; set; }

        public EmployeeEntity Employee { get; set; } = null!;
    }
}
