using Management.WebApp.Options;
using Management.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient<ITypeService, TypeService>();
builder.Services.AddTransient<ICatalogService, CatalogService>();
builder.Services.AddTransient<OrderingService>();
builder.Services.AddHttpClient();
builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection(nameof(ApiOptions)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
