using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Dtos
{
    public  class CreateEmployeeDtos
    {
        public string FullName { get; set; } = default!;

        public string NationalID { get; set; } = default!;

        public DateOnly? BirthDate { get; set; } = default!;

        public decimal? BasicSalary { get; set; } = default!;

        public TimeOnly ExpectedCheckInTime { get; set; }
        public TimeOnly ExpectedCheckOutTime { get; set; }

    }
}
