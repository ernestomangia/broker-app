using Broker.Domain.Core;

namespace Broker.Domain;

public class Rate : EntityBase
{
    public DateTime Date { get; set; }
    public virtual ICollection<RateValue> Values { get; set; }
}