using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SocialMediaApp.DataAccess.Entity;
using System.IdentityModel.Tokens.Jwt;

namespace SocialMediaApp.BusinessLogic.Services.AuthenticationService
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IConfiguration _configuration;

		public AuthenticationService(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public string GenerateToken(User user)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
				new Claim(ClaimTypes.Role, user.Role.Name)

			};

			SymmetricSecurityKey key =
				new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:SecurityKey").Value!));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

			var token = new JwtSecurityToken(
				issuer: "Backend",
				audience: "Frontend",
				claims: claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: creds
				);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);
			return jwt;
		}
	}
}
