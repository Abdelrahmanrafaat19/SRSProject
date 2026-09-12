using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Dtos
{
    public class CheckOutDtos
    {
        public int EmployeeId { get; set; }

        public DateOnly AttendanceDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public TimeOnly CheckOutTime { get; set; } = TimeOnly.FromDateTime(DateTime.Now);
    }
}
