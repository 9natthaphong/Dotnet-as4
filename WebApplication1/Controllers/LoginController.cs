using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost("LoginAdmin")]
        public IActionResult LoginAdmin([FromBody] LoginModel login)
        {
            if (login.Username == "admin" && login.Password == "password")
            {
                var token = GenerateJwtToken("Admin");
                return Ok(new { token });
            }
            return Unauthorized();
        }

        [HttpPost("LoginUser")]
        public IActionResult LoginUser([FromBody] LoginModel login)
        {
            if (login.Username == "user" && login.Password == "password")
            {
                var token = GenerateJwtToken("User");
                return Ok(new { token });
            }
            return Unauthorized();
        }

        private string GenerateJwtToken(string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this_is_a_longer_secret_key_1234567890"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: "yourissuer",
                audience: "youraudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
