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
    [StringLength(255)]
    public string Login { get; set; } = null!;

    [Required]
    [MinLength(8, ErrorMessage = "Hasło musi mieć co najmniej 8 znaków.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$", ErrorMessage = "Hasło musi zawierać wielkie i małe litery, cyfry oraz znaki specjalne.")]
    public string Password { get; set; } = null!;
    public virtual ICollection<Granary> Granaries { get; set; } = new List<Granary>();

    public virtual ICollection<Shoplist> Shoplists { get; set; } = new List<Shoplist>();
}
