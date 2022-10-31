using Broker.Common;
using Broker.Domain;
using Broker.Infrastructure.Core.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Broker.Infrastructure.Mappings;

public class RateValueEntityTypeConfiguration : EntityTypeConfigurationBase<RateValue>
{
    public override void Configure(EntityTypeBuilder<RateValue> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.SourceCurrencyCodeType)
            .HasConversion(new EnumToStringConverter<CurrencyCodeType>())
            .IsRequired();

        builder.Property(x => x.TargetCurrencyCodeType)
            .HasConversion(new EnumToStringConverter<CurrencyCodeType>())
            .IsRequired();

        builder.Property(x => x.Value)
            .HasPrecision(18, 6)
            .IsRequired();

        builder.ToTable(nameof(RateValue));
    }
}