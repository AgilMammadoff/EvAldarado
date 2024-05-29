using EvAldarado.DAL;
using EvAldarado.Extension;
using EvAldarado.Extensions;
using EvAldarado.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Authentication configuration (uncomment if needed)
// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
// })
//     .AddCookie()
//     .AddGoogle(GoogleDefaults.AuthenticationScheme, option =>
//     {
//         option.ClientId = builder.Configuration.GetSection("Googlekeys:ClientId").Value;
//         option.ClientSecret = builder.Configuration.GetSection("Googlekeys:ClientSecret").Value;
//     });

// Add services to the container
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Ensure the identity services are added properly
builder.Services.AddIdentity<Users, AppRole>()
    .AddEntityFrameworkStores<AppDBContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(2);
});

builder.Services.AddScoped<IEmailService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new EmailService(configuration["EmailSettings:Host"],
                            configuration.GetValue<int>("EmailSettings:Port"),
                            configuration.GetValue<bool>("EmailSettings:EnableSSL"),
                            configuration["EmailSettings:Email"],
                            configuration["EmailSettings:Password"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Value;
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "AdminIdare",
    pattern: "{area:exists}/{controller=DashBoard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
