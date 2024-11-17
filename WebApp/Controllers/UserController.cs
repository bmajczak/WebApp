using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services.Interfaces;


namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly AppDbContext _dbContext;
        public UserController(IUserService service, AppDbContext dbContext)
        {
            _service = service;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _service.GetUsersAsync();
            return View(users);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(User user)
        {
            await _service.AddUserAsync(user);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(string id)
        {
            var user = await _service.GetUserAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(User user, IFormFile profilePicture)
        {
            if (profilePicture != null && profilePicture.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await profilePicture.CopyToAsync(memoryStream);
                user.ProfilePicture = memoryStream.ToArray();
            }
            await _service.UpdateUserAsync(user);
            return RedirectToAction("UserProfile", "User", new { id = user.Id });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _service.GetUserAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(User user)
        {
            await _service.DeleteUserAsync(user.Id);
            await HttpContext.SignOutAsync("Identity.Application");
            Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UserProfile(string id)
        {
            var user = await _dbContext.Users
            .Include(u => u.Posts)
            .FirstOrDefaultAsync(u => u.Id == id);

            return View(user);
        }
    }
}
