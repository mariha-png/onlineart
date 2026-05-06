using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Art_Gallery.Models;
using Online_Art_Gallery.Models.Data;
using System.Diagnostics;

namespace Online_Art_Gallery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Product()
        {
            var products = _context.products.ToList();
            return View(products);
        }
        public IActionResult Detail()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
       [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult SubmitContact(ContactFormViewModel model)
{
    if (ModelState.IsValid)
    {
        _context.ContactFormsViewModel.Add(model);
        _context.SaveChanges();

        TempData["Success"] = "Your message has been submitted successfully!";
        return RedirectToAction("Contact");
    }

    return View("Contact", model);
}


        // ✅ Fixed
        public IActionResult Register()
        {
            return Redirect("/Identity/Account/Register");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
