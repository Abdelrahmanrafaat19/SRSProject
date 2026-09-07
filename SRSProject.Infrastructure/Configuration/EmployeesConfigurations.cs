using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SRSProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Infrastructure.Configuration
{
    public class EmployeeConfiguration
       : IEntityTypeConfiguration<EmployeeEntity>
    {
        public void Configure(
            EntityTypeBuilder<EmployeeEntity> builder)
        {
            
            


            builder.HasIndex(employee => employee.NationalId)
                .IsUnique()
                .HasDatabaseName("IX_Employees_NationalId");

            

        }
    }
}
