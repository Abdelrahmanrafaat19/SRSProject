using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Infrastructure.Configuration
{
    internal class OfficialHolidayEntityConfiguration : IEntityTypeConfiguration<OfficialHolidayEntity>
    {
        public void Configure(EntityTypeBuilder<OfficialHolidayEntity> builder)
        {
            builder.ToTable("OfficialHolidays");
         
        }
    }
}
