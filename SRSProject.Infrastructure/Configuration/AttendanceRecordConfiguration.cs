using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SRSProject.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Infrastructure.Configuration
{
    public class AttendanceRecordConfiguration : IEntityTypeConfiguration<AttendanceRecord>
    {
        public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
        {

            builder.Property(record => record.Status)
            .HasConversion<int>();

            builder.HasOne(record => record.Employee)
            .WithMany(employee => employee.AttendanceRecords)
            .HasForeignKey(record => record.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
