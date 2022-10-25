﻿using Broker.Domain.Core;

namespace Broker.Domain;

public class Rate : EntityBase
{
    public DateTime Date { get; set; }

    public double Rub { get; set; }

    public double Eur { get; set; }

    public double Gbp { get; set; }

    public double Jpy { get; set; }
}