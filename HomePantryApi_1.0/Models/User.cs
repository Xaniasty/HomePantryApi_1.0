using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomePantryApi_1._0.Models;

[Table("Users")]
public class User
{
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = null!;
    [Required]
    public string Login { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    public virtual ICollection<Granary> Granaries { get; set; } = new List<Granary>();

    public virtual ICollection<Shoplist> Shoplists { get; set; } = new List<Shoplist>();
}
