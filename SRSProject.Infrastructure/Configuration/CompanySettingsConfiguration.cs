using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SRSProject.Domain.Entities;

namespace SRSProject.Infrastructure.Configuration
{
    internal class CompanySettingsConfiguration : IEntityTypeConfiguration<CompanySettings>
    {
        public void Configure(EntityTypeBuilder<CompanySettings> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DefaultTaxRate).HasPrecision(18, 4);

            builder.Property(x => x.WorkStart);
            builder.Property(x => x.WorkEnd);

            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
