using MedManager.Data;
using MedManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var serverVersion = new MySqlServerVersion(new Version(10, 4, 28));

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion)
);

builder.Services.AddIdentity<Doctor, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;

    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error/Index");
    app.UseStatusCodePagesWithRedirects("/Error/Index"); 
}

var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
context.Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}"
    );

app.Run();
