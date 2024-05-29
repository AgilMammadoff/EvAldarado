using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EvAldarado.Models;
using System.Threading.Tasks;

namespace EvAldarado.Areas.Admin.Controllers
{
    [Area("AdminIdare")]
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<Users> _userManager;

        public UserManagementController(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users;
            return View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.LockoutEnd = DateTimeOffset.MaxValue;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.LockoutEnd = null;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}
