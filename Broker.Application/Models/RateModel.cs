using Broker.Application.Core.Models;

namespace Broker.Application.Models;

public class RateModel : ModelBase
{
    public DateTime Date { get; set; }

    public decimal Rub { get; set; }

    public decimal Eur { get; set; }

    public decimal Gbp { get; set; }

    public decimal Jpy { get; set; }
}