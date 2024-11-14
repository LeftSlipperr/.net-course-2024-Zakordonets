using BankSystem.App.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] AuthData authData)
    {
        // Пример простой проверки логина и пароля
        if (authData.Login == "admin" && authData.Password == "password")
        {
            // Генерация токена (упрощенный пример)
            var token = GenerateToken(authData.Login);

            return Ok(new AuthResponse
            {
                Name = "Admin User",
                AvatarUrl = "https://example.com/avatar.png",
                Token = token
            });
        }

        return Unauthorized(new { Message = "Invalid login or password" });
    }

    private string GenerateToken(string login)
    {
        // Простая генерация токена (в реальных проектах используйте JWT)
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        return token;
    }
}