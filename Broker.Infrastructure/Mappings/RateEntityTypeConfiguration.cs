using Broker.Domain;
using Broker.Infrastructure.Core.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Broker.Infrastructure.Mappings;

public class RateEntityTypeConfiguration : EntityTypeConfigurationBase<Rate>
{
    public override void Configure(EntityTypeBuilder<Rate> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Date)
            .IsRequired();

        // IX
        builder.HasIndex(e => e.Date)
            .IsUnique();

        // Relationships
        builder.HasMany(e => e.Values)
            .WithOne(e => e.Rate)
            .HasForeignKey(e => e.RateId);

        builder.ToTable(nameof(Rate));
    }
}