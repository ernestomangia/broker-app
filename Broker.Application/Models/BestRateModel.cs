namespace Broker.Application.Models;

public class BestRateModel
{
    public DateTime BuyDate { get; set; }

    public DateTime SellDate { get; set; }

    public string Tool { get; set; }

    public double Revenue { get; set; }

    public ICollection<RateModel> Rates { get; set; }
}