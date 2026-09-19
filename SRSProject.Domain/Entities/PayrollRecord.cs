using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Domain.Entities
{
    public class PayrollRecord : BasicEntity<int>
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }

        public decimal BasicSalary { get; set; }
        public decimal TotalAdditions { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }

       
        public EmployeeEntity Employee { get; set; } = null!;

        public ICollection<PayrollAdjustment> Adjustments { get; set; } = new List<PayrollAdjustment>();
    }
}
