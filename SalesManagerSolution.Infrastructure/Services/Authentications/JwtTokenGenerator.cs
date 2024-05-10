using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SalesManagerSolution.Core.Interfaces.Authentications;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SalesManagerSolution.Infrastructure.Services.Authentications
{
	public class JwtTokenGenerator : IJwtTokenGenerator
	{
		
		private readonly JwtSettings _jwtSettings;

		public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
		{
			_jwtSettings = jwtOptions.Value;
		}

		/// <summary>
		/// Method generate Token
		/// </summary>
		/// <param name="id"></param>
		/// <param name="userName"></param>
		/// <returns></returns>
		public string GenerateToken(int id, string userName)
		{
			// generate singingCredentials
			var signingCredentials = new SigningCredentials
									 (
										 new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
										 SecurityAlgorithms.HmacSha256
									 );

			// build claims for user
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
				new Claim(JwtRegisteredClaimNames.GivenName, userName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};


			//build sercurity token
			var sercurityToken = new JwtSecurityToken(
													   issuer: _jwtSettings.Issuer,
													   expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
													   audience: _jwtSettings.Audience,
													   claims: claims,
													   signingCredentials: signingCredentials
													 );

			// return token
			return new JwtSecurityTokenHandler().WriteToken(sercurityToken);
		}
	}
}
