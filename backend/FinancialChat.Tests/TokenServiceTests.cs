using FinancialChat.Authentication;
using FinancialChat.Model;
using FinancialChat.Parameters;
using FluentAssertions;

namespace FinancialChat.Tests
{
    public class TokenServiceTests
    {
        private readonly ITokenService _tokenService;
        private readonly JwtParameters _jwtParameters;

        public TokenServiceTests()
        {
            _jwtParameters = new JwtParameters 
            { 
                Key = "abracadabra#simsalabim@2021",
                Audience = "audience",
                Issuer = "issuer"
            };
            _tokenService = new TokenService(_jwtParameters);
        }

        [Fact]
        public void ShoudReturnValidToken()
        {
            var login = new Login
            {
                Email = "test@email.com",
                Password = "password"
            };

            var result = _tokenService.GenerateToken(login);

            result.Should().NotBeNull();
        }
    }
}