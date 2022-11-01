namespace Broker.Application.Models;

public class BestRevenueRequestModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal MoneyUsd { get; set; }
}