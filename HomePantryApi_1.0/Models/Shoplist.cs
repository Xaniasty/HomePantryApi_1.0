using System;
using System.Collections.Generic;

namespace HomePantryApi_1._0.Models;

public class Shoplist
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string ShoplistName { get; set; } = null!;

    public DateTime? DataUtworzenia { get; set; }

    public DateTime? DataAktualizacji { get; set; }

    public string? Opis { get; set; }

    public virtual ICollection<Productsinshoplist> Productsinshoplists { get; set; } = new List<Productsinshoplist>();

    public virtual User? User { get; set; }
}
