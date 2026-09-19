using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SRSProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Infrastructure.Configuration
{
    public class PayrollRecordConfiguration : IEntityTypeConfiguration<PayrollRecord>
    {
        public void Configure(EntityTypeBuilder<PayrollRecord> builder)
        {
            builder.HasKey(x => x.Id);

            
            builder.Property(x => x.BasicSalary).HasPrecision(18, 2);
            builder.Property(x => x.TotalAdditions).HasPrecision(18, 2);
            builder.Property(x => x.TotalDeductions).HasPrecision(18, 2);
            builder.Property(x => x.NetSalary).HasPrecision(18, 2);

            
            builder.HasIndex(x => new { x.EmployeeId, x.Year, x.Month }).IsUnique();

       
            builder.HasOne(x => x.Employee)
                .WithMany(x => x.PayrollRecords)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

          
        }
    }
}
