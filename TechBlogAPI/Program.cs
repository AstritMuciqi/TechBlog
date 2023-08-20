using Domain.Interfaces;
using Infrastructure.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechBlogApp.Domain.Models;
using TechBlogApp.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection2");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Opcioni opsional për të lejuar emrat e pronave të përputhen me emrat e propertive në klasat e modelit të shfletimit
                    options.JsonSerializerOptions.WriteIndented = true; // Opcioni opsional për të formatuar JSON-in në mënyrë të lexueshme
                });

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseCors(options => options
            .WithOrigins(new[] { "http://localhost:3000", "http://localhost:5075", "http://localhost:8080", "http://localhost:4200", "http://localhost:8000" })
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());

//builder.Services.AddControllers().AddNewtonsoftJson(x =>
//                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
