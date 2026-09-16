using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Dtos
{
    public class WeeklyHolidayDto
    {
        public required string NameOfHoliday { get; set; }
        public required List<DayOfWeek> DaysOfHoliday { get; set; }
    }
}
