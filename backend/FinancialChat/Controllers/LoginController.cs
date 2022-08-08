using FinancialChat.Authentication;
using FinancialChat.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinancialChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenService _tokenService;

        public LoginController(SignInManager<IdentityUser> signInManager, ITokenService tokenService)
        {
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var identityResult = await _signInManager.PasswordSignInAsync(login.Email, login.Password, true, false);

            if (identityResult.Succeeded)
            {
                var token = _tokenService.GenerateToken(login);
                return Ok(new { Token = token });
            }

            return BadRequest();
        }
    }
}