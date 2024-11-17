using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _dbContext;

        public UserProfileController(IUserService service, AppDbContext dbContext)
        {
            _userService = service;
            _dbContext = dbContext;
        }

        public async Task<ActionResult> Index(string id)
        {

            var user = await _userService.GetUserAsync(id);


            if (user == null)
            {
                return NotFound();
            }


            return View(user);
        }
        public async Task<IActionResult> Update(string id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(User user)
        {
            await _userService.UpdateUserAsync(user);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetUserAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(User user)
        {
            await _userService.DeleteUserAsync(user.Id);
            return RedirectToAction("Index");
        }
    }

}
