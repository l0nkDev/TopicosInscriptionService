using Microsoft.AspNetCore.Identity;

namespace CareerApi.Models
{
    public class LoginRequest
    {
        required public string Login { get; set; }
        required public string Password { get; set; }
    }

    public class LoginResponse
    {
        required public string Login { get; set; }
        required public string Token { get; set; }
    }
}
