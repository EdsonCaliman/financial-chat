using FinancialChat.Model;
using FinancialChat.Parameters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinancialChat.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly JwtParameters _jwtParameters;

        public TokenService(JwtParameters jwtParameters)
        {
            _jwtParameters = jwtParameters;
        }

        public string GenerateToken(Login login)
        {
            var _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtParameters.Key));
            var _issuer = _jwtParameters.Issuer;
            var _audience = _jwtParameters.Audience;

            var signinCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: signinCredentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }
    }
}