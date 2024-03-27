using Kontravers.GoodJob.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Kontravers.GoodJob.Auth.Data;
using Kontravers.GoodJob.Data;
using Kontravers.GoodJob.Domain;
using EmailSender = Kontravers.GoodJob.Auth.EmailSender;
using IEmailSender = Microsoft.AspNetCore.Identity.UI.Services.IEmailSender;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;
var services = builder.Services;

services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = isDevelopment
        ? "Host=localhost;Database=goodjob;Username=postgres;Password=Password1!;Timezone=UTC"
        : "Host=postgres;Database=goodjob;Username=postgres;Password=Password1!;Timezone=UTC";

    options.UseNpgsql(connectionString, x =>
        x.MigrationsHistoryTable("__EFMigrationsHistory", "auth"));
});

services.AddDatabaseDeveloperPageExceptionFilter();

services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

services.ConfigureApplicationCookie(cookie =>
{
    cookie.LoginPath = "/Identity/Account/Login";
});

services
    .AddScoped<Kontravers.GoodJob.Domain.IEmailSender, Kontravers.GoodJob.Infra.Shared.EmailSender>()
    .AddScoped<IClock, Clock>()
    .AddScoped<IEmailSender, EmailSender>()
    .AddAuthentication();

services
    .AddIdentityServer(options =>
    {
        options.Authentication.CookieLifetime = TimeSpan.FromHours(1);
        options.Authentication.CookieSlidingExpiration = true;
    })
    .AddInMemoryClients(Clients.GetClients())
    .AddInMemoryApiResources(Resources.GetApiResources())
    .AddInMemoryIdentityResources(Resources.GetIdentityResources())
    .AddInMemoryApiScopes(Scopes.GetApiScopes())
    .AddAspNetIdentity<IdentityUser>()
    .AddDeveloperSigningCredential();

services.AddControllersWithViews();
services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (isDevelopment)
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

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    Secure = CookieSecurePolicy.Always,
});

app.UseIdentityServer();
app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    await MigrationsHelper.RunDbMigrationsAsync<ApplicationDbContext>(scope);
}

await app.RunAsync();