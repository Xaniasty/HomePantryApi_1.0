using System;
using System.Collections.Generic;

namespace HomePantryApi_1._0.Models;

public class Productsinshoplist
{
    public int ProductId { get; set; }

    public int ShoplistId { get; set; }

    public string ProductName { get; set; } = null!;

    public int? Quantity { get; set; } = 1;

    public bool IsLiquid { get; set; } = false;

    public decimal? Weight { get; set; } = 0.01m;

    public string? Description { get; set; }

    public bool InPackage { get; set; } = false;

    public DateOnly? DataZakupu { get; set; }

    public DateOnly? DataWaznosci { get; set; }

    public decimal? Cena { get; set; }

    public string? Rodzaj { get; set; }

    public virtual Shoplist? Shoplist { get; set; }
}
