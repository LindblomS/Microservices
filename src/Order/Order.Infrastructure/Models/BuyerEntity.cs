﻿namespace Ordering.Infrastructure.Models;

using System;
using System.Collections.Generic;

public class BuyerEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<CardEntity> Cards { get; set; }
}
