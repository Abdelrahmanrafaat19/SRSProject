using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Dtos
{
    public class OfficialHolidayDto
    {
        public string Name { get; set; } = null!;

        public int Day { get; set; }

        public int Month { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
