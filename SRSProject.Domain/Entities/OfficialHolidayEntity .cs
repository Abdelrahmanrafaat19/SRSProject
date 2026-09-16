using SRSProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Infrastructure
{
    public class OfficialHolidayEntity : BasicEntity<int>
    {
        public string Name { get; set; } = null!;

        public int Day { get; set; }

        public int Month { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
