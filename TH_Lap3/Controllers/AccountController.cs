using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TH_Lap3.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AccountController(UserManager<IdentityUser> userManager) { _userManager = userManager; }
        public async Task<IActionResult> VerifyUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return Json(true); // Tên người dùng có sẵn
            }
            else
            {
                return Json($"Tên người dùng '{username}' đã tồn tại."); // Tên người dùng đã tồn tại
            }
        }
    }
}
