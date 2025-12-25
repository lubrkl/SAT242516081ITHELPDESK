using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SAT242516081.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // DÜZELTİLEN KISIM BURASI: İsmini "PerformLogin" yaptık
        [HttpPost("PerformLogin")]
        public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
        {
            // Basit doğrulama
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return Redirect("/Account/Login?error=EksikBilgi");
            }

            // Identity ile giriş denemesi
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (result.Succeeded)
            {
                // Başarılıysa Dashboard'a git
                return Redirect("/dashboard");
            }

            // Başarısızsa Login sayfasına hata koduyla dön
            return Redirect("/Account/Login?error=GecersizGiris");
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Account/Login");
        }
    }
}