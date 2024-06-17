using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WhatsappDemoAPIs.DTOs;
using WhatsappDemoAPIs.Models;
using WhatsappDemoAPIs.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WhatsappDemoAPIs.Controllers;

[ApiController]
[Route("api/[controller]")]

public class LoginController : ControllerBase
{
    private IConfiguration _config;
    private readonly IAuthRepository _auth;
    
    private readonly IUserRepository _user;

    public LoginController(IConfiguration config, IAuthRepository auth, IUserRepository user)
    {
        _config = config;
        _auth = auth;
        _user = user;
    }

    private string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // const string Name = nameof(Name);
        const string UserId = "id";

        var claims = new[]
        {
            // new Claim(ClaimTypes.NameIdentifier, user.Phone),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(UserId, user.Id.ToString())
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(60 * 48),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    [AllowAnonymous]
    [HttpPost("login")]

    public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
    {
        IActionResult response = Unauthorized();
        var isAuthenticated = await _auth.CheckAuthentication(userLogin.Phone);
        if(isAuthenticated)
        {
            var user = await _user.GetUserByPhone(userLogin.Phone);
            if(user != null)
            {
                var token = GenerateToken(user);
                response = Ok(new { token = token});
            }
            else
            {
                response = Unauthorized();
            }
        }
        return response;
    }

}