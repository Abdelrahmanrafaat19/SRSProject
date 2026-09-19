using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Domain.Entities
{
    public class PayrollAdjustment : BasicEntity<int>
    {
        public int Id { get; set; }

        public int PayrollRecordId { get; set; }

        public PayrollAdjustmentType Type { get; set; }
        public decimal Amount { get; set; }

        public string Reason { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public PayrollRecord PayrollRecord { get; set; } = null!;
    }
}
