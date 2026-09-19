using System;

namespace SRSProject.Application.Dtos
{
    public sealed class AuditDto
    {
        public string Action { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
