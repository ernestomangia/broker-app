﻿using Broker.Application.Abstractions;
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

    [HttpGet]
    public async Task<ICollection<RateModel>> FindAll()
    {
        return await _rateService.FindAll();
    }

    [HttpGet("best")]
    public async Task<BestRateModel> FindBest(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] int moneyUsd)
    {
        return await _rateService.FindBest();
    }
}