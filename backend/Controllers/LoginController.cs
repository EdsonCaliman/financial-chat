using FinancialChat.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinancialChat.Controllers
{
    [Route("/login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var identityResult = await _signInManager.PasswordSignInAsync(login.Email, login.Password, true, false);

            if (identityResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}