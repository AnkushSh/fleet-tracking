using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using fleet_tracking.Models;
using QweDbContext = fleet_tracking.Data.QweDbContext;
using ApplicationUserModel = fleet_tracking.Models.ApplicationUser;
using fleet_tracking.Data;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QweDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<QweDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();


builder.Services.AddAuthentication();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();
var app = builder.Build();

// Initialize roles.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUserModel>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        RoleInitializer.InitializeAsync(userManager, roleManager).Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing roles.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => 
    { 
        endpoints.MapControllers();
        endpoints.MapRazorPages();
    });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();