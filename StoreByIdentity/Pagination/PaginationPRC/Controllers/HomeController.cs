using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PaginationPRC.Models;
using System.Diagnostics;

namespace PaginationPRC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static List<Item> data = Enumerable.Range(1, 50).Select(i =>new Item 
        {
            Id = i,
            Name=$"item{i}"
        }).ToList();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int page=1)
        {
            int pagesize = 10;
            int totalItems=data.Count; 
            int totalPages= (int)Math.Ceiling((double)totalItems / pagesize);
            var items = data.OrderBy(i => i.Id)
                .Skip((page - 1) * pagesize).Take(pagesize).ToList();
            var model = new ItemViewModel {
                Items = items  ,
                CurrentPage = page, 
                TotalPages = totalPages,
            };


            return View(model);
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
