using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using Web.Context;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _db;
        public HomeController(ApplicationContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            return View(_db.StackDrinks.ToList());
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
    }
}
