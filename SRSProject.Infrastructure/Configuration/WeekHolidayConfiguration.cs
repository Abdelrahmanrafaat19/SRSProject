using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SRSProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Infrastructure.Configuration
{
    public class WeekHolidayConfiguration : IEntityTypeConfiguration<WeekHolidayEntity>
    {
        public void Configure(EntityTypeBuilder<WeekHolidayEntity> builder)
        {
            builder.ToTable("WeeklyHolidays");
        }
    }
}
