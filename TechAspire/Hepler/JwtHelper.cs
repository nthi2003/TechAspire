using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TechAspire.Hepler
{
	public static class JwtHelper
	{
		private static readonly string SecretKey = "quocthidev@gmailcom-lenhedaotaodedcfullstack"; 
		
		public static string GenerateToken(string userId, string username)
		{
			var claims = new[]
			{
			new Claim(JwtRegisteredClaimNames.Sub, userId),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new Claim("username", username)
		};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: "quocthi",
				audience: "quocthi",
				claims: claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		
		public static ClaimsPrincipal ValidateToken(string token)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
			var tokenHandler = new JwtSecurityTokenHandler();

			try
			{
				var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidIssuer = "quocthi",
					ValidAudience = "quocthi",
					ValidateLifetime = true,
					IssuerSigningKey = key
				}, out SecurityToken validatedToken);

				return principal;
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
