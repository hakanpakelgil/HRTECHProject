using ApplicationCore.Entities;
using ApplicationCore.Enums;
using HRTechProject.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NToastNotify;
using IdentityDenemeHRTech.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews()
    .AddNToastNotifyToastr(new ToastrOptions()
    {
        PositionClass = ToastPositions.TopRight,
        TimeOut = 3000,
        ProgressBar = true,
    });
    

var app = builder.Build();

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

app.UseNToastNotify();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Personel}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


    if (!await roleManager.RoleExistsAsync("SirketYoneticisi"))
    {
        await roleManager.CreateAsync(new IdentityRole("SirketYoneticisi"));
    }
    if (!await roleManager.RoleExistsAsync("Calisan"))
    {
        await roleManager.CreateAsync(new IdentityRole("Calisan"));
    }
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    if (!await userManager.Users.AnyAsync(x => x.UserName == "admin@example.com"))
    {
        var adminUser = new ApplicationUser()
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            EmailConfirmed = true,
            Ad = "Admin",
            FotografYolu = "/images/admin.jpg",
            Soyad = "User",
            TcNo = "12345678901",
            IseGirisTarihi = DateTime.Now.AddYears(-1), // 1 y�l �nce
            IstenCikisTarihi = null, // Hen�z i�ten ��kmad�
            DepartmanId=1,
            
            Aktiflik = true, // Hesap aktif
            MeslekId = 3, // �rnek bir meslek ID'si
            AdresId = 1, // �rnek bir adres ID'si
            Telefon = "5555555555",
            Maas = 5000.00m,
            Cinsiyet = Cinsiyet.Erkek, // �rnek bir cinsiyet
            Harcamalar = new List<Harcama>(), // Bo� bir harcama listesi
            Izinler = new List<Izin>(), // Bo� bir izin listesi
            Avanslar = new List<Avans>() // Bo� bir avans listesi,
            


        };
        await userManager.CreateAsync(adminUser, "Ankara1.");
        await userManager.AddToRoleAsync(adminUser, "SirketYoneticisi");
    }
}

    app.Run();
