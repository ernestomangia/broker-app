using Broker.Application.Core.Models;

namespace Broker.Application.Models;

public class RateModel : ModelBase
{
    public DateTime Date { get; set; }

    public double Rub { get; set; }

    public double Eur { get; set; }

    public double Gbp { get; set; }

    public double Jpy { get; set; }
}