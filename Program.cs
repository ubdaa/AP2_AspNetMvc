using MedManager.Data;
using MedManager.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

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
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Error/AccessDenied";
});

// Ajouter support pour proxy inversé
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
context.Database.EnsureCreated();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Error/Index");
    app.UseStatusCodePagesWithRedirects("/Error/Index");
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseForwardedHeaders();
app.UsePathBase("/"); // S'assurer que le chemin de base est correct
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}"
);

// Nouveau service pour assurer la création des rôles au démarrage de l'application
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Technicien", "Utilisateur", "Visiteur" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

// Nouveau service pour assurer la création d'un compte admin au démarrage de l'application
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Doctor>>();
    
    var email = "admin@medmanager.com";
    var password = "Admin123!";
    
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var newAdmin = new Doctor
        {
            UserName = "Admin",
            Email = email,
            FirstName = "Admin",
            LastName = "Admin",
            Address = "Admin",
            Faculty = "Admin",
            Specialty = "Admin"
        };
        
        await userManager.CreateAsync(newAdmin, password);
        await userManager.AddToRoleAsync(newAdmin, "Admin");
    }
    
    // pour chaque utilisateur existant dans la base de données, s'il ne possède pas de rôle, on lui attribue le rôle "Docteur"
    var users = userManager.Users.ToList();

    foreach (var user in users)
    {
        if (!await userManager.IsInRoleAsync(user, "Utilisateur") && !await userManager.IsInRoleAsync(user, "Admin"))
            await userManager.AddToRoleAsync(user, "Utilisateur");
    }
}

app.Run();