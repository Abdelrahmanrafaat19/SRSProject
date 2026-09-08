using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SRSProject.Infrastructure.DataContext.IDentityEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Infrastructure.Configuration
{
    internal class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");
            builder.HasOne(u => u.Employee).WithOne().HasForeignKey<UserEntity>(u => u.EmployeeId);
        }
    }
}
