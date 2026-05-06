using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Online_Art_Gallery.Models.Data;


public class UserController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _context;

    public UserController(UserManager<AppUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Details()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login", "Account");

        return View(user);
    }

    [Authorize(Roles = "User")]
    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login", "Account");

        return View(user);
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AppUser model, IFormFile profileImage)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login", "Account");

        user.Name = model.Name;
        user.Email = model.Email;
        user.UserName = model.Email;
        user.Country = model.Country;
        user.City = model.City;

        if (profileImage != null && profileImage.Length > 0)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
            var fullPath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await profileImage.CopyToAsync(stream);
            }

            user.ProfileImage = "/images/" + fileName;
        }

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
            return RedirectToAction("Details");

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
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

    //public IActionResult Cart()
    //{
    //    string sessionKey = "Cart";
    //    var cartData = HttpContext.Session.GetString(sessionKey);
    //    List<Product> cart = new();

    //    if (!string.IsNullOrEmpty(cartData))
    //    {
    //        cart = JsonConvert.DeserializeObject<List<Product>>(cartData);
    //    }

    //    return View(cart);
    //}

    public IActionResult Checkout()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }
    //[HttpGet]
    public IActionResult Cart()
    {
        string username = User.Identity.IsAuthenticated ? User.Identity.Name : HttpContext.Session.Id;
        string sessionKey = $"Cart_{username}";

        var cartData = HttpContext.Session.GetString(sessionKey);
        List<CartItem> cart = new();

        if (!string.IsNullOrEmpty(cartData))
        {
            cart = JsonConvert.DeserializeObject<List<CartItem>>(cartData);
        }

        return View(cart); // Make sure Cart.cshtml accepts List<CartItem>
    }

    [HttpPost]
    public IActionResult AddToCart(int productId, int quantity)
    {
        string username = User.Identity.IsAuthenticated ? User.Identity.Name : HttpContext.Session.Id;
        string sessionKey = $"Cart_{username}";

        var cartData = HttpContext.Session.GetString(sessionKey);
        List<CartItem> cart = !string.IsNullOrEmpty(cartData)
            ? JsonConvert.DeserializeObject<List<CartItem>>(cartData)
            : new List<CartItem>();

        var product = _context.products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            return NotFound("Product not found.");
        }

        var existingItem = cart.FirstOrDefault(c => c.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = quantity
            });
        }

        HttpContext.Session.SetString(sessionKey, JsonConvert.SerializeObject(cart));

        return RedirectToAction("Cart"); // After adding, redirect to cart page
    }
    [HttpPost]
    public IActionResult RemoveCartItem(int index)
    {
        string sessionKey = $"Cart_{User.Identity.Name}";
        var cartData = HttpContext.Session.GetString(sessionKey);
        if (!string.IsNullOrEmpty(cartData))
        {
            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartData);
            if (index >= 0 && index < cart.Count)
            {
                cart.RemoveAt(index);
                HttpContext.Session.SetString(sessionKey, JsonConvert.SerializeObject(cart));
            }
        }

        return RedirectToAction("Cart");
    }
    [HttpPost]
    public IActionResult UpdateCartQuantity([FromBody] CartUpdateModel update)
    {
        string sessionKey = $"Cart_{User.Identity.Name}";
        var cartData = HttpContext.Session.GetString(sessionKey);
        if (cartData != null)
        {
            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartData);
            if (update.Index >= 0 && update.Index < cart.Count)
            {
                cart[update.Index].Quantity = update.Quantity;
                HttpContext.Session.SetString(sessionKey, JsonConvert.SerializeObject(cart));
            }
        }

        return Ok();
    }

    public class CartUpdateModel
    {
        public int Index { get; set; }
        public int Quantity { get; set; }
    }
    //[HttpPost]
    //public IActionResult PlaceOrder(string name, string phone, string address)
    //{
    //    var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
    //    if (cart == null || cart.Count == 0)
    //    {
    //        return RedirectToAction("Cart");
    //    }

    //    var order = new OrderSummaryViewModel
    //    {
    //        Name = name,
    //        Phone = phone,
    //        Address = address,
    //        CartItems = cart
    //    };

    //    // Save this order to database or pass it to admin logic
    //    TempData["OrderSummary"] = JsonConvert.SerializeObject(order);

    //    return RedirectToAction("OrderConfirmation");
    //}
    //[HttpGet]
    //public IActionResult PlaceOrder()
    //{
    //    return View();
    //}
    [HttpPost]
    [HttpPost]
    public IActionResult PlaceOrder(string Name, string Phone, string Address)
    {
        string username = User.Identity.IsAuthenticated ? User.Identity.Name : HttpContext.Session.Id;
        string sessionKey = $"Cart_{username}";

        var cartData = HttpContext.Session.GetString(sessionKey);
        if (string.IsNullOrEmpty(cartData))
            return RedirectToAction("Cart");

        var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartData);

        var order = new UserOrder
        {
            Name = Name,
            Contact = Phone,
            Address = Address,
            Items = cart.Select(c => new OrderItem
            {
                ProductName = c.Name,
                Quantity = c.Quantity,
                TotalPrice = c.TotalPrice
            }).ToList()
        };

        _context.UserOrders.Add(order);
        _context.SaveChanges();

        HttpContext.Session.Remove(sessionKey); // Remove correct cart session

        return RedirectToAction("OrderSuccess");
    }

    public IActionResult OrderSuccess()
    {
        return View();
    }

    public IActionResult ThankYou()
    {
        return View();
    }


 


}
