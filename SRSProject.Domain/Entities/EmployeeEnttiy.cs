using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Domain.Entities
{
    public class EmployeeEntity : BasicEntity<int>
    {

        public string FullName { get; set; } = default!;
        public string NationalId { get; set; } = default!;

        public DateOnly BirthDate { get; set; }
        public DateOnly ContractDate { get; set; }=DateOnly.FromDateTime(DateTime.Now);

        public decimal BasicSalary { get; set; }

        public TimeOnly ExpectedCheckInTime { get; set; }
        public TimeOnly ExpectedCheckOutTime { get; set; }

        public bool IsActive { get; set; } = true; 

    }
}
