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

        builder.Property(x => x.Rub)
            .IsRequired();

        builder.Property(x => x.Eur)
            .IsRequired();

        builder.Property(x => x.Gbp)
            .IsRequired();

        builder.Property(x => x.Jpy)
            .IsRequired();

        builder.ToTable(nameof(Rate));
    }
}