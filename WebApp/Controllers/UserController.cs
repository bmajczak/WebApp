using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }
        
        public IActionResult Index()
        {
            var u = _service.GetUsers();
            return View(u);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(User user)
        {
            _service.AddUser(user);
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            var u = _service.GetUser(id);
            return View(u);
        }
        [HttpPost]
        public IActionResult Update(User user)
        {
            _service.UpdateUser(user);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var u = _service.GetUser(id);
            return View(u);
        }
        [HttpPost]
        public IActionResult Delete(User user)
        {
            _service.DeleteUser(user);
            return RedirectToAction("Index");
        }
    }
}
