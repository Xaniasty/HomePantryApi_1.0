using System.ComponentModel.DataAnnotations;

namespace HomePantryApi_1._0.Models
{
    public class LoginRequest
    {
        public string EmailOrLogin { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}

