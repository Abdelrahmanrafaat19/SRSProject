using System;

namespace SRSProject.Application.Dtos
{
    public sealed class CompanySettingsDto
    {
        public decimal DefaultTaxRate { get; set; }
        public TimeOnly WorkStart { get; set; } = new TimeOnly(9,0);
        public TimeOnly WorkEnd { get; set; } = new TimeOnly(17,0);
    }
}
