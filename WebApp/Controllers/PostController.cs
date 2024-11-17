using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _dbContext;
        public PostController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var post = new Post { UserId = userId };

            return View(post);
        }
        [HttpPost]
        public ActionResult Add(Post post)
        {
            post.CreatedDate = DateTime.Now;
            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();
            return RedirectToAction("UserProfile", "User", new { id = post.UserId });
        }
        public async Task<IActionResult> Details(int id)
        {
            var post = await _dbContext.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
        public ActionResult Update(int id)
        {
            var post = _dbContext.Posts.FirstOrDefault(p => id == p.Id);
            return View(post);
        }
        [HttpPost]
        public ActionResult Update(Post post)
        {
            post.CreatedDate = DateTime.Now;
            _dbContext.Posts.Update(post);
            _dbContext.SaveChanges();
            return RedirectToAction("UserProfile", "User", new { id = post.UserId });
        }
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(p => id == p.Id);
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Post post, string userId)
        {
            if (post == null || string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            _dbContext.Posts.Remove(post);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("UserProfile", "User", new { id = userId });
        }


    }
}
