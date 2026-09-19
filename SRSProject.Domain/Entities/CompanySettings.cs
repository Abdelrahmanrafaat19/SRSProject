using System;

namespace SRSProject.Domain.Entities
{
    public class CompanySettings : BasicEntity<int>
    {
        public decimal DefaultTaxRate { get; set; }
        public TimeOnly WorkStart { get; set; }
        public TimeOnly WorkEnd { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
