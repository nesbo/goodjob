using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Kontravers.GoodJob.Auth.Data;
using Kontravers.GoodJob.Data;

var builder = WebApplication.CreateBuilder(args);
var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = isDevelopment
        ? "Host=localhost;Database=goodjob;Username=postgres;Password=Password1!;Timezone=UTC"
        : "Host=postgres;Database=goodjob;Username=postgres;Password=Password1!;Timezone=UTC";

    options.UseNpgsql(connectionString, x =>
        x.MigrationsHistoryTable("__EFMigrationsHistory", "auth"));
});
    

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer();

builder.Services.AddControllersWithViews();

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

app.UseAuthorization();
app.UseIdentityServer();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    await MigrationsHelper.RunDbMigrationsAsync<ApplicationDbContext>(scope);
}

await app.RunAsync();