using Domain.Interfaces;
using Infrastructure.Interface;
using TechBlogApp.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechBlogApp.Domain.Models;
using TechBlogApp.Persistence;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(x =>
               x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddControllersWithViews();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<JwtService>();

builder.Services.AddCors();

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//builder.Services.AddIdentityCore<AppUser>(opt => {
//    opt.User.RequireUniqueEmail = true;
//    opt.Password.RequireNonAlphanumeric = false;
//})
//                .AddRoles<IdentityRole>()
//                .AddDefaultTokenProviders()
//                .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(10));


builder.Services.AddSession();

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();



builder.Services.AddRazorPages();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("Admin"));
    options.AddPolicy("RequireCreatorRole", policy =>
        policy.RequireRole("Creator"));
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404 || context.Response.StatusCode == 401)
    {
        context.Response.Redirect("/Home/NotFound"); // Redirect to your 404 page
    }
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Article}/{action=Index}/{id?}");

AppDbInitializer.SeedUsersAndRolesAsync(app).Wait();

app.Run();


