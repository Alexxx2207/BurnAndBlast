using Ignite.Data.Seeding;
using Ignite.Data;
using Ignite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Ignite.Services.Fitnesses;
using Ignite.Services.Users;
using Ignite.Services.Events;
using Ignite.Services.Classes;
using Ignite.Services.Products;
using Ignite.Services.Subscriptions;
using Ignite.Services.CartProducts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.AreaViewLocationFormats.Clear();
    options.AreaViewLocationFormats.Add("Areas/{2}/Views/{1}/{0}.cshtml");
    options.AreaViewLocationFormats.Add("Areas/{2}/Views/Shared/{0}.cshtml");
    options.AreaViewLocationFormats.Add("Areas/{2}/Views/Shared/{0}.cshtml");
});

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IFitnessService, FitnessService>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<IEventsService, EventsService>();
builder.Services.AddTransient<IClassesService, ClassesService>();
builder.Services.AddTransient<IProductsService, ProductsService>();
builder.Services.AddTransient<ISubscriptionsService, SubscriptionsService>();
builder.Services.AddTransient<ICartProductsService, CartProductsService>();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "Administration",
    areaName: "Administration",
    pattern: "Administration/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();
