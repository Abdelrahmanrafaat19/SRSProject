using SRSProject.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SRSProject.Application.Dtos
{
    public sealed class PayrollRecordDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal TotalAdditions { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public IReadOnlyList<PayrollAdjustmentDto> Adjustments { get; set; } = new List<PayrollAdjustmentDto>();
    }

    public sealed class CreatePayrollDto
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal BasicSalary { get; set; }
    }

    public sealed class PayrollAdjustmentDto
    {
        public int Id { get; set; }
        public PayrollAdjustmentType Type { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
