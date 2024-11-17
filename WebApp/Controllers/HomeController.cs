using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var latestPosts = _context.Posts
               .OrderByDescending(p => p.CreatedDate)
               .Take(3)                               
               .ToList();

        return View(latestPosts);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult TestConnection()
    {
        try
        {
            _context.Database.OpenConnection();
            return Content("Połączenie z bazą danych działa!");
        }
        catch (Exception ex)
        {
            return Content($"Błąd połączenia: {ex.Message}");
        }
        finally
        {
            _context.Database.CloseConnection();
        }
    }

}

