using FinancialChat.Model;

namespace FinancialChat.Authentication
{
    public interface ITokenService
    {
        string GenerateToken(Login login);
    }
}