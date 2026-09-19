using System;

namespace SRSProject.Domain.Entities
{
    public class PayrollPolicy : BasicEntity<int>
    {
        public decimal TaxRate { get; set; }
        public decimal SocialSecurityRate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
