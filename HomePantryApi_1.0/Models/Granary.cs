using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomePantryApi_1._0.Models;

public class Granary
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [MaxLength(20)]
    public string GranaryName { get; set; } = null!;

    public DateTime? DataUtworzenia { get; set; }

    public DateTime? DataAktualizacji { get; set; }

    public string? Opis { get; set; }

    public virtual ICollection<Productsingranary> Productsingranaries { get; set; } = new List<Productsingranary>();

    public virtual User? User { get; set; }
}
