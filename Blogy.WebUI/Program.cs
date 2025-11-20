using Blogy.Business.Extensions;
using Blogy.DataAccess.Extensions;
using Blogy.WebUI.Filters;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Kestrel ayarlarý (HTTP 431 hatasý çözümü)
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestHeadersTotalSize = 64 * 1024; // 64KB (varsayýlan 32KB)
    options.Limits.MaxRequestLineSize = 16 * 1024; // 16KB (varsayýlan 8KB)
});

// Add services to the container.

builder.Services.AddServicesExt(builder.Configuration);  //Service registration Business - Configuration parametresi eklendi
builder.Services.AddRepositoriesExt(builder.Configuration); //  Service registration Data Access


builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ValidationExceptionFilter>();
});

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Login/Index";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Users}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
