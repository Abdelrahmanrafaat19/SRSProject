using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Dtos
{
    public class UpdateEmployeeDto
    {
        public string NationalID { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal? BasicSalary { get; set; }
        public TimeSpan? ExpectedCheckInTime { get; set; }
        public TimeSpan? ExpectedCheckOutTime { get; set; }
        public bool? IsActive { get; set; }
    }
}
