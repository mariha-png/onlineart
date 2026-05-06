using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Online_Art_Gallery.Models.Data;
using Microsoft.AspNetCore.Http;
using FluentAssertions.Common;


var builder = WebApplication.CreateBuilder(args);

// ------------------------------
// 🔧 Configuration
// ------------------------------
var config = builder.Configuration;

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

// Register Identity for AppUser (your custom class)
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Optional: Email sender
builder.Services.AddSingleton<IEmailSender, EmailSender>();

// Cookies for Identity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// MVC, Razor, and HttpContext
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();
app.UseSession();
// In Startup.cs or Program.cs
builder.Services.AddSession();
app.UseSession();

// ------------------------------
// 🧱 Middleware pipeline
// ------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();            // ✅ Enable session support
app.UseAuthentication();     // ✅ Identity auth
app.UseAuthorization();

// Map Razor Pages
app.MapRazorPages();
app.UseSession();

// ------------------------------
// 🔁 Routing
// ------------------------------
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin" });

app.MapControllerRoute(
    name: "user",
    pattern: "User/{action=Index}/{id?}",
    defaults: new { controller = "User" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
