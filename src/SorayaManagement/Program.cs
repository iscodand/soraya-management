using SorayaManagement.Infrastructure.Data;
using SorayaManagement.Infrastructure.Identity;
using SorayaManagement.Infrastructure.Identity.Contracts;
using SorayaManagement.Services;
using SorayaManagement.Application;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Configure Newtonsoft.Json to Ignore Loops
JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
};

// Dependency Injection
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
builder.Services.AddScoped<ISessionService, SessionService>();

// Setting up ApplicationLayer
builder.Services.AddApplicationServices();

// Setting up Database
builder.Services.AddDatabaseContext(builder.Configuration);

// Setting up Identity
builder.Services.AddIdentitySetup();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseSession();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
