using Shopping.WebApp.Options;
using Shopping.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddTransient<IBasketService, BasketService>();
builder.Services.AddTransient<ICatalogService, CatalogService>();
builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection(nameof(ApiOptions)));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
