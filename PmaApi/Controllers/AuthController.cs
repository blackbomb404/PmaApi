using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pma.Models.DTOs;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly string _jwtKey;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;
    
    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
        _jwtKey = _configuration["Jwt:Key"]!;
        _jwtIssuer = _configuration["Jwt:Issuer"]!;
        _jwtAudience = _configuration["Jwt:Audience"]!;
    }
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLogin user)
    {
        if (user.Username == "admin" && user.Password == "password")
        {
            var token = GenerateJwt(user.Username);
            return Ok(new { token });
        }
        return Unauthorized();
    }

    private string GenerateJwt(string username)
    {
        // setting the data that will be part of the payload
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // key -> SSK -> SC -> JST -> JSTH.writeToken(token)
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtIssuer,
            audience: _jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}