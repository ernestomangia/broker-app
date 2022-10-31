using Broker.Common;

namespace Broker.Application.Models;

public class BestRevenueModel
{
    public DateTime BuyDate { get; set; }
    public DateTime SellDate { get; set; }
    public CurrencyCodeType Tool { get; set; }
    public decimal Revenue { get; set; }
    public ICollection<RateModel> Rates { get; set; }
}