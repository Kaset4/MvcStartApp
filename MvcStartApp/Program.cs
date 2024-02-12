using Microsoft.EntityFrameworkCore;
using MvcStartApp.Middlewares;
using MvcStartApp.Models.Db;
using MvcStartApp.Models.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BlogContext>(options => options.UseSqlServer(connection));
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<LoggingMiddleware>();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
