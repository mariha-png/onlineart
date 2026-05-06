
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Online_Art_Gallery;
using Online_Art_Gallery.Models.Data;

namespace Online_Art_Gallery.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<AppUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _webHostEnvironment=webHostEnvironment;
        }


        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin)
            {
                return Unauthorized();
            }

            return View(user); // pass AppUser to the view
        }
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin)
            {
                return Unauthorized();
            }

            return View(user); // ✅ Send current AppUser to Details.cshtml
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AppUser model, IFormFile profileImage)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            user.Name = model.Name;
            user.StreetAddress = model.StreetAddress;
            user.PostalCode = model.PostalCode;
            user.Country = model.Country;
            user.City = model.City;

            if (profileImage != null && profileImage.Length > 0)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                Directory.CreateDirectory(folder);
                var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await profileImage.CopyToAsync(stream);

                user.ProfileImage = "/images/" + fileName;
            }

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return RedirectToAction("Details");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
        //public async Task<IActionResult> UsersList()
        //{
        //    // Saare users fetch karo
        //    var users = await _userManager.Users.ToListAsync();

        //    // Sirf un users ko filter karo jin ka role "User" hai
        //    var usersInUserRole = new List<AppUser>();
        //    foreach (var user in users)
        //    {
        //        if (await _userManager.IsInRoleAsync(user, "User"))
        //        {
        //            usersInUserRole.Add(user);
        //        }
        //    }

        //    return View(usersInUserRole);
        //}
        public async Task<IActionResult> UserDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return RedirectToAction("UsersList");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("UsersList");
        }

        [HttpGet]
        public IActionResult ContactSubmissions()
        {
            var contacts = _context.Set<ContactFormViewModel>().OrderByDescending(c => c.SubmittedAt).ToList();
            return View(contacts);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMessage(int id)
        {
            var message = _context.ContactFormsViewModel.FirstOrDefault(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            _context.ContactFormsViewModel.Remove(message);
            _context.SaveChanges();

            TempData["Success"] = "Message deleted successfully!";
            return RedirectToAction("ContactMessages");
        }
        public async Task<IActionResult> UsersList()
        {
            var users = _userManager.Users.ToList();
            var userRoleList = new List<IdentityUser>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("User"))
                {
                    userRoleList.Add(user);
                }
            }

            return View(userRoleList);
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }


        [HttpPost]
public async Task<IActionResult> AddProduct(Product model)
{
    if (ModelState.IsValid)
    {
        if (model.ImageFile != null)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageFile.CopyToAsync(fileStream);
            }

            model.ImageUrl = "/uploads/" + uniqueFileName;
        }

        _context.products.Add(model);
        await _context.SaveChangesAsync();
        return RedirectToAction("ProductList");
    }

    return View(model);
}

        public IActionResult ProductList()
        {
            var products = _context.products.ToList();
            return View(products);
        }
        //public IActionResult OrderConfirmation()
        //{
        //    var orderData = TempData["OrderSummary"]?.ToString();
        //    if (orderData == null)
        //        return RedirectToAction("Cart");

        //    var model = JsonConvert.DeserializeObject<OrderSummaryViewModel>(orderData);
        //    return View(model);
        //}
        //public IActionResult OrderList()
        //{
        //    var orders = _context.Orders.Include(o => o.Items).ToList();
        //    return View(orders);
        //}
        public IActionResult Orders()
        {
            var orders = _context.UserOrders
                .Include(o => o.Items)
                .ToList();

            return View(orders);
        }


    }
}
