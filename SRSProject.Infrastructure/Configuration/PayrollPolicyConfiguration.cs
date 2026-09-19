using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SRSProject.Domain.Entities;

namespace SRSProject.Infrastructure.Configuration
{
    internal class PayrollPolicyConfiguration : IEntityTypeConfiguration<PayrollPolicy>
    {
        public void Configure(EntityTypeBuilder<PayrollPolicy> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TaxRate).HasPrecision(18, 4);
            builder.Property(x => x.SocialSecurityRate).HasPrecision(18, 4);

            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
