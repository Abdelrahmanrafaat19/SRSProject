using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Domain.Entities
{
    public enum AttendanceStatus
    {
        Present = 1,
        Absent = 2,
        Late = 5,
        Intime = 6,
        WeeklyHoliday = 3,
        OfficialHoliday = 4
    }
}
