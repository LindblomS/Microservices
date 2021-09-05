namespace Identity.API
{
    using Identity.API.Configurations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Services.Identity.Infrastructure;
    using System.Net.Http.Headers;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityContext>(c => c.UseSqlServer(Configuration.GetValue<string>("ConnectionString")));
            services.AddSingleton<TokenFactory>();
            services.AddIdentity<IdentityUser, IdentityRole>(c =>
            {
                c.Password.RequiredLength = 4;
                c.Password.RequireDigit = false;
                c.Password.RequireNonAlphanumeric = false;
                c.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(c =>
            {
                c.Cookie.Name = "IdentityServer.Cookie";
                c.LoginPath = "/Account/Login";
                c.LogoutPath = "/Account/Logout";
            });

            services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>()
                .AddInMemoryClients(Clients.Get())
                .AddInMemoryIdentityResources(Resources.GetIdentityResources())
                .AddInMemoryApiResources(Resources.GetApiResources())
                .AddInMemoryApiScopes(Resources.GetApiScopes())
                .AddDeveloperSigningCredential();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
