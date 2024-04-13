using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TH_Lap3.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace TH_Lap3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var users = _userManager.Users.ToList(); // Lấy danh sách tất cả người dùng
            
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.Role = roles.FirstOrDefault();
            }
            return View(users);
        }
    }
}
