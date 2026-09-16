using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Domain.Entities
{
    public class WeekHolidayEntity : BasicEntity <int>
    {
        public required string NameOfHoliday { get; set; }
        public required List<DayOfWeek> DaysOfHoliday { get; set; }
    }
}
