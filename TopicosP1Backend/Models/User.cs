using Microsoft.AspNetCore.Identity;

namespace CareerApi.Models
{
    public class User
    {
        public long Id { get; set; }
        required public string Login { get; set; }
        required public string Token { get; set; }
        required public string PasswordHash { get; set; }
        required public char Role { get; set; } //S: Estudiante, T: Docente, A: Administrativo, C: CPD
    }
}
