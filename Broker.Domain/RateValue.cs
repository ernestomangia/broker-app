using Broker.Common;
using Broker.Domain.Core;

namespace Broker.Domain;

public class RateValue : EntityBase
{
    public CurrencyCodeType SourceCurrencyCodeType { get; set; }
    public CurrencyCodeType TargetCurrencyCodeType { get; set; }
    public decimal Value { get; set; }

    public Guid RateId { get; set; }
    public virtual Rate Rate { get; set; }
}