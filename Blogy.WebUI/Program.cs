using Blogy.Business.Extensions;
using Blogy.DataAccess.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddServicesExt();  //Service registration Business
builder.Services.AddRepositoriesExt(builder.Configuration); //  Service registration Data Access


builder.Services.AddControllersWithViews();

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
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Register}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
