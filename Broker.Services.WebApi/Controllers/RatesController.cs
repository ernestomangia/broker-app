using Broker.Application.Abstractions;
using Broker.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Services.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RatesController : ControllerBase
{
    private readonly IRateService _rateService;

    public RatesController(IRateService rateService)
    {
        _rateService = rateService;
    }

    [HttpGet("best")]
    public async Task<BestRevenueModel> FindBestRevenue(
        [FromQuery] BestRevenueRequestModel model)
    {
        return await _rateService.FindBestRevenue(model);
    }
}